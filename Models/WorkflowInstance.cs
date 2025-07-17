namespace WorkflowEngine.Models;
public class WorkflowInstance {
    public string Id { get; set; }
    public string DefinitionId { get; set; }
    public string CurrentStateId { get; set; }
    public List<Transition> History { get; set; }
}
