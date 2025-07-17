# Workflow Engine (State-Machine API)

This project implements a minimal yet extensible backend service for managing configurable workflows using state machines. 

##  Features

- Define workflows as a set of states and actions (transitions)
- Start workflow instances from any definition
- Execute actions to transition between states with validation
- Retrieve definitions, current state, and execution history of instances
- In-memory storage for simplicity (no external DB required)
- Built using .NET 8 Minimal API

##  Tech Stack

- Language: C#
- Framework: .NET 8 

##  Project Structure

```
WorkflowEngine/
├── Models/             # Data models: State, ActionDef, etc.
├── Services/           # Core logic for handling workflow runtime
├── Program.cs          # API entry point and route definitions
├── WorkflowEngine.csproj
└── README.md
```

##  Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/2004Snehal/Workflow_EngineC.git
cd WorkflowEngine
```

### 2. Build and Run

```bash
dotnet build
dotnet run
```

By default, the server runs at `http://localhost:5113`.


### 3. Example Workflow Flow

1. `POST /workflow` → create workflow definition
2. `POST /workflow/{id}/start` → start an instance
3. `POST /workflow/{instanceId}/execute/{actionId}` → move between states
4. `GET /workflow/instance/{id}` → check current state & history

##  Sample Workflow

```json
{
  "id": "onboarding",
  "states": {
    "start": { "id": "start", "isInitial": true, "isFinal": false, "enabled": true },
    "hr_review": { "id": "hr_review", "isInitial": false, "isFinal": false, "enabled": true },
    "complete": { "id": "complete", "isInitial": false, "isFinal": true, "enabled": true }
  },
  "actions": {
    "submit_form": {
      "id": "submit_form",
      "enabled": true,
      "fromStates": ["start"],
      "toState": "hr_review"
    },
    "approve": {
      "id": "approve",
      "enabled": true,
      "fromStates": ["hr_review"],
      "toState": "complete"
    }
  }
}
```

##  Assumptions & Notes

- All data is stored in-memory for simplicity and easy testing.
- Each workflow definition must have **exactly one initial state**.
- Transitions are only allowed if the action is valid from the current state.

