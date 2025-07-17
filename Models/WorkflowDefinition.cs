namespace WorkflowEngine.Models;
public class WorkflowDefinition {
    public string Id { get; set; }
    public Dictionary<string, State> States { get; set; }
    public Dictionary<string, ActionDef> Actions { get; set; }
}
