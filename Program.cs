using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WorkflowService>();

var app = builder.Build();

var workflowService = app.Services.GetRequiredService<WorkflowService>();

app.MapPost("/workflow", workflowService.CreateDefinition);
app.MapGet("/workflow/{id}", workflowService.GetDefinition);
app.MapPost("/workflow/{id}/start", workflowService.StartInstance);
app.MapPost("/workflow/{instanceId}/execute/{actionId}", workflowService.ExecuteAction);
app.MapGet("/workflow/instance/{id}", workflowService.GetInstance);

app.Run();
