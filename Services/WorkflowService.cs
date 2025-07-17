using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public class WorkflowService {
    private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
    private readonly Dictionary<string, WorkflowInstance> _instances = new();

    public IResult CreateDefinition([FromBody] WorkflowDefinition def) {
        if (_definitions.ContainsKey(def.Id))
            return Results.BadRequest("Duplicate workflow ID.");
        if (def.States.Values.Count(s => s.IsInitial) != 1)
            return Results.BadRequest("Exactly one initial state required.");
        _definitions[def.Id] = def;
        return Results.Ok("Created");
    }

    public IResult GetDefinition(string id) =>
        _definitions.TryGetValue(id, out var def) ? Results.Ok(def) : Results.NotFound();

    public IResult StartInstance(string id) {
        if (!_definitions.TryGetValue(id, out var def)) return Results.NotFound("Workflow not found.");
        var init = def.States.Values.First(s => s.IsInitial);
        var instance = new WorkflowInstance {
            Id = Guid.NewGuid().ToString(),
            DefinitionId = id,
            CurrentStateId = init.Id,
            History = new()
        };
        _instances[instance.Id] = instance;
        return Results.Ok(instance);
    }

    public IResult ExecuteAction(string instanceId, string actionId) {
        if (!_instances.TryGetValue(instanceId, out var inst))
            return Results.NotFound("Instance not found.");
        var def = _definitions[inst.DefinitionId];
        if (!def.Actions.TryGetValue(actionId, out var action))
            return Results.BadRequest("Action invalid.");
        if (!action.Enabled || !action.FromStates.Contains(inst.CurrentStateId))
            return Results.BadRequest("Transition not allowed.");
        if (!def.States.ContainsKey(action.ToState))
            return Results.BadRequest("Target state missing.");

        inst.CurrentStateId = action.ToState;
        inst.History.Add(new Transition { ActionId = actionId, Timestamp = DateTime.UtcNow });
        return Results.Ok(inst);
    }

    public IResult GetInstance(string id) =>
        _instances.TryGetValue(id, out var inst) ? Results.Ok(inst) : Results.NotFound();
}
