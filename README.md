# Project P0015 - Source Structure Documentation

This document provides an overview of the Unity project structure and development guidelines.

---

## Project Overview

This structure follows a clean architecture pattern, separating pure C# logic from Unity-specific implementations.

---

## Directory Structure

```
Assets/_Project/
├── Core/                          # Engine-agnostic pure C# code
│   ├── Dialogue/                  # Dialogue system core logic
│   │   ├── DialogueId.cs          # Identifier struct
│   │   ├── DialogueMachine.cs     # State machine for dialogue playback
│   │   ├── DialogueState.cs       # Runtime state container
│   │   └── DialogueTypes.cs       # DTOs, enums, and node types
│   └── Project.Core.asmdef        # Assembly definition (no Unity references)
│
├── Features/                      # Unity-specific implementations
│   ├── Dialogue/
│   │   ├── Data/                  # ScriptableObject asset instances
│   │   ├── Logic/                 # Feature-level logic
│   │   │   ├── DialogueRunner.cs  # Wraps DialogueMachine
│   │   │   └── DialogueSession.cs # Session data container
│   │   ├── SO/                    # ScriptableObject definitions
│   │   │   ├── CharacterDefinitionAsset.cs
│   │   │   ├── DialogueDatabaseAsset.cs
│   │   │   ├── DialogueScriptAsset.cs
│   │   │   └── ...
│   │   ├── Unity/                 # MonoBehaviour controllers
│   │   │   └── DialogueController.cs
│   │   └── View/                  # UI/View components
│   │       └── DialogueView.cs
│   └── Project.Features.asmdef    # References Project.Core + Unity libs
│
└── Editor/                        # Editor-only code
    ├── ManagedRef/                # Custom property drawers
    └── Project.Editor.asmdef
```

---

## Architecture Patterns

### 1. Core vs Features Separation

| Layer | Purpose | Dependencies |
|-------|---------|--------------|
| **Core** | Pure C# logic, reusable, testable | None (no Unity) |
| **Features** | Unity integration, MonoBehaviours, ScriptableObjects | Core + Unity |
| **Editor** | Editor tools and inspectors | Features + Unity Editor |

> **IMPORTANT**: The `Project.Core.asmdef` has `"noEngineReferences": true`, meaning Core code **cannot** use any Unity APIs. This ensures logic remains portable and unit-testable.

### 2. MVC-like Pattern for Features

Each feature follows a consistent internal structure:

```
Features/[FeatureName]/
├── Data/       # Asset instances (.asset files)
├── Logic/      # Feature-specific logic classes
├── SO/         # ScriptableObject class definitions
├── Unity/      # MonoBehaviour controllers
└── View/       # UI/View components
```

### 3. Layer Responsibilities

| Folder | Responsibility | Example |
|--------|---------------|---------|
| `SO/` | Data definitions, asset schemas | `CharacterDefinitionAsset` |
| `Data/` | Actual asset instances | `00.1.CharacterDefinitionAsset.asset` |
| `Logic/` | Feature-level business logic | `DialogueRunner`, `DialogueSession` |
| `Unity/` | MonoBehaviour orchestration, input handling | `DialogueController` |
| `View/` | UI presentation, visual feedback | `DialogueView` |

---

## Namespace Conventions

```csharp
// Core layer
namespace Project.Core.Dialogue { }

// Features layer
namespace Project.Features.Dialogue.Logic { }
namespace Project.Features.Dialogue.SO { }
namespace Project.Features.Dialogue.Unity { }
namespace Project.Features.Dialogue.View { }
```

**Pattern**: `Project.[Layer].[Feature].[SubCategory]`

---

## Coding Conventions

### Naming Standards

| Type | Convention | Example |
|------|------------|---------|
| Fields (private, serialized) | `_camelCase` | `[SerializeField] private TMP_Text _speakerText;` |
| Fields (private, non-serialized) | `_camelCase` | `private Coroutine _typingRoutine;` |
| Properties | `PascalCase` | `public bool IsTyping => ...` |
| Methods | `PascalCase` | `public void ShowLine(...)` |
| Classes/Structs | `PascalCase` | `DialogueController`, `DialogueLine` |
| Enums | `PascalCase` | `DialoguePhase`, `PortraitSlot` |
| Events | `PascalCase` | `public event Action TypewriterFinished;` |

### Class Modifiers

- Use `sealed` for classes that should not be inherited
- Use `readonly struct` for immutable value types
- Use `[Serializable]` for structs/classes that need Unity serialization

### ScriptableObject Patterns

```csharp
[CreateAssetMenu(menuName = "Project/[Category]/[AssetName]")]
public sealed class ExampleAsset : ScriptableObject
{
    // Public fields for Unity serialization
    public string Id;
    public List<SomeData> Items = new();
    
    // Lookup methods
    public SomeData GetItem(string id) { ... }
}
```

---

## Asset Naming Convention

Assets in `Data/` folders follow a numbered prefix pattern:

```
[Chapter].[Sequence].[AssetType].asset

Examples:
00.0.DialogueDatabaseAsset.asset
00.1.CharacterDefinitionAsset.asset
00.2.BackgroundDefinitionAsset.asset
```

This allows for logical ordering and grouping of related assets.

---

## Data Flow Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                     Core Layer (No Unity)                       │
│  ┌─────────────────┐  ┌─────────────┐  ┌───────────────┐        │
│  │ DialogueMachine │  │DialogueState│  │DialogueTypes  │        │
│  └────────┬────────┘  └─────────────┘  └───────────────┘        │
└───────────┼─────────────────────────────────────────────────────┘
            │ wraps
┌───────────┼─────────────────────────────────────────────────────┐
│           ▼              Features Layer                         │
│  ┌─────────────────┐                                            │
│  │ DialogueRunner  │◄─── DialogueSession                        │
│  └────────┬────────┘                                            │
│           │ events                                              │
│           ▼                                                     │
│  ┌──────────────────┐    ┌─────────────────┐                    │
│  │DialogueController│───►│  DialogueView   │                    │
│  └────────┬─────────┘    └─────────────────┘                    │
│           │                                                     │
│           ▼                                                     │
│  ┌──────────────────────┐                                       │
│  │DialogueDatabaseAsset │ (Scripts, Characters, Backgrounds)    │
│  └──────────────────────┘                                       │
└─────────────────────────────────────────────────────────────────┘
```

---

## Guidelines for Adding New Features

### Step 1: Plan the Core Logic

If your feature needs engine-agnostic logic:

1. Create folder: `Core/[FeatureName]/`
2. Define types, state, and state machine
3. Keep all code Unity-free

### Step 2: Create Feature Structure

```
Features/[FeatureName]/
├── Data/       # Create empty folder for assets
├── Logic/      # Runner/Session wrappers
├── SO/         # ScriptableObject definitions  
├── Unity/      # Controller MonoBehaviours
└── View/       # UI components
```

### Step 3: Define ScriptableObjects

1. Create definition classes in `SO/`
2. Add `[CreateAssetMenu]` attribute
3. Follow naming: `[Feature][Purpose]Asset.cs`

### Step 4: Create Controller

1. Create MonoBehaviour in `Unity/`
2. Use `[SerializeField]` for asset references
3. Handle Unity lifecycle and input
4. Delegate to Logic layer

### Step 5: Create View

1. Create MonoBehaviour in `View/`
2. Handle only visual presentation
3. Expose events for completion callbacks

---

## Event-Driven Communication

The project uses C# events for decoupled communication:

```csharp
// In Logic layer
public event Action<DialogueLine> OnLine;
public event Action<DialogueCommand> OnCommand;
public event Action OnFinished;

// In View layer
public event Action TypewriterFinished;
```

Controllers subscribe to events in `OnEnable()` and unsubscribe in `OnDisable()`.

---

## Key Files Reference

| File | Purpose |
|------|---------|
| `Core/Dialogue/DialogueMachine.cs` | Core state machine for dialogue progression |
| `Core/Dialogue/DialogueTypes.cs` | All dialogue data types (Line, Command, Wait, Nodes) |
| `Features/Dialogue/Unity/DialogueController.cs` | Main orchestrator, handles input and Unity integration |
| `Features/Dialogue/View/DialogueView.cs` | UI presentation and typewriter effect |
| `Features/Dialogue/SO/DialogueDatabaseAsset.cs` | Central registry for all dialogue assets |
