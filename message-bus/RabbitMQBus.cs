using System.Text;
using System.Text.Json.Nodes;
using message_bus.Commands;
using message_bus.Events;
using message_bus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace message_bus;

public sealed class RabbitMQBus : IMessageBus
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;
    public RabbitMQBus(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _connectionFactory = new ConnectionFactory { Uri = new Uri("amqp://admin:admin@localhost:5672") };   // { Uri = new Uri(configuration["rmqConnection"]) };
        _serviceScopeFactory = serviceScopeFactory;
        _handlers = new Dictionary<string, List<Type>>();
        _eventTypes = new List<Type>();
    }
    public void Publish<T>(T @event) where T : Event
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        var eventName = @event.GetType().Name;
        channel.QueueDeclare(eventName, false, false, false, null);
        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("", eventName, null, body);
    }

    public void Subscribe<T, TH>() where T : Event where TH : IEventHandler
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);
        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName, new List<Type>());
        }

        if (_handlers[eventName].Any(s => s.GetType() == handlerType))
        {
            throw new ArgumentException(
                $"Handler type {handlerType.Name} is already registered for {eventName}", nameof(handlerType)
            );
        }

        _handlers[eventName].Add(handlerType);
        StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : Event
    {
        var connection = _connectionFactory.CreateConnection();
        var channel = connection.CreateModel();

        var eventName = typeof(T).Name;
        channel.QueueDeclare(eventName, false, false, false, null);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += ConsumerReceived;
        channel.BasicConsume(eventName, autoAck: true, consumer);
    }

    private async Task ConsumerReceived(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.Span);
        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            
        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (_handlers.ContainsKey(eventName))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var subscriptions = _handlers[eventName];
            foreach (var subscription in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(subscription);
                if (handler == null) { continue; }
                var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                var @event = JsonConvert.DeserializeObject(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }
        }
    }
}