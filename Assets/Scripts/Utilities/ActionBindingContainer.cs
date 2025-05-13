using System.Collections.Generic;

public class ActionBindingContainer
{
    public Dictionary<string, string> bindings = new Dictionary<string, string>();

    public void AddBinding(string action, string displayString)
    {
        bindings[action] = displayString;
    }

    public string GetBinding(string action)
    {
        return bindings.TryGetValue(action, out var result) ? result : "N/A";
    }
}
