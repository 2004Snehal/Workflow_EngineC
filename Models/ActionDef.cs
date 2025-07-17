namespace WorkflowEngine.Models;
public class ActionDef {
    public string Id { get; set; }
    public bool Enabled { get; set; }
    public List<string> FromStates { get; set; }
    public string ToState { get; set; }
}
