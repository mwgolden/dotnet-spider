using System.Text;
using System.Text.Json.Nodes;
using message_bus.Commands;
using message_bus.Events;
using message_bus.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace message_bus;

public sealed class RabbitMQBus : IMessageBus
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;
    public RabbitMQBus(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory { Uri = new Uri("amqp://admin:admin@localhost:5672") };   // { Uri = new Uri(configuration["rmqConnection"]) };
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
        throw new NotImplementedException();
    }
}