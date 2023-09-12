namespace crawler.robots_txt;

public interface IParseHandler {
    public bool AllowedByRobots(Stream stream);
    public void HandleKeyValuePair(string key, string value, int lineNumber);
}