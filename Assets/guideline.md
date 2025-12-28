# Unity Project Architecture – Practical Guidelines

This document explains how to work day-to-day with the Core / Features / Unity / SO / Data / Tests structure.

It complements `ruleset.md` with concrete workflows and examples.

---

## 1. When Adding New Gameplay Logic

Example: new system like inventory, status effects, or turn order.

1. **Start in Core**  
   - Create a folder under `Assets/_Project/Core/<SystemName>/`.
   - Define pure C# types:
     - Data classes (definitions, value objects).
     - Rule/logic classes (calculators, state machines).
   - No Unity types.

2. **Add Feature Logic if it’s game-specific**  
   - If this system is specific to one feature (e.g., Battle):
     - Add classes under `Assets/_Project/Features/Battle/Logic/`.
     - These reference your Core types and orchestrate them for this game.

3. **Expose through Unity later**  
   - Only after logic is stable, create `Unity` and `View` scripts to connect it to scenes and UI.

---

## 2. When Creating a New ScriptableObject Type

Example: a new type of definition or config object.

1. **Create the Core data type (if needed)**  
   - Location: `Assets/_Project/Core/...`
   - Example: `StatusEffectDefinition`, `ItemDefinition`, `DialogueNode`.
   - Keep it pure C#.

2. **Create the ScriptableObject script**  
   - Location: `Assets/_Project/Features/<Feature>/SO/`
   - Example:
     - `ItemDefinitionAsset.cs`
     - `StatusEffectAsset.cs`
   - Include serialized fields and a method like `ToDefinition()` that returns the Core type.

3. **Create asset instances**  
   - Right-click in Project window → Create → your menu item.
   - Save assets in `Assets/_Project/Features/<Feature>/Data/...`:
     - e.g. `Data/Items`, `Data/StatusEffects`, `Data/DialogueDatabases`.

4. **Use assets from Unity code**  
   - `Unity`/`View` scripts reference the SO assets via `[SerializeField]`.
   - At runtime, convert to Core with `ToDefinition()` / `ToBalance()` etc., and pass into Logic classes.

---

## 3. When Building a New Feature

Example: new feature `WorldMap`.

1. **Create feature folders**

   ```text
   Assets/_Project/Features/WorldMap/
     Logic/
     Unity/
     View/
     SO/
     Data/
   ```

2. **Logic** (`Features/WorldMap/Logic`)  
   - Implement feature-specific logic without Unity types:
     - e.g. `WorldMapGraph`, `Pathfinder`, `LocationState`.

3. **SO** (`Features/WorldMap/SO`)  
   - ScriptableObject scripts defining data:
     - `WorldLocationAsset`, `WorldMapConfigAsset`, etc.

4. **Data** (`Features/WorldMap/Data`)  
   - `.asset` instances for locations, configs, etc.

5. **Unity** (`Features/WorldMap/Unity`)  
   - Scene controller MonoBehaviours:
     - `WorldMapController`, `WorldMapInputHandler`.

6. **View** (`Features/WorldMap/View`)  
   - Visual components:
     - `WorldMapNodeView`, `WorldMapLineView`, HUD elements.

7. **Tests** (optional but encouraged)  
   - Add tests under `Assets/_Project/Tests/EditMode/WorldMap` for logic classes.

---

## 4. Dialogue Layer – Example Responsibilities

Using your current Dialogue setup as a reference:

- **Core/Dialogue**
  - `DialogueId`, `DialogueState`, `DialogueTypes`, `DialogueMachine`.
  - Owns the rules of how dialogue flows (next line, choices, branching).

- **Features/Dialogue/Logic**
  - `DialogueRunner`:
    - Coordinates a running dialogue session using `DialogueMachine`.
  - `DialogueSession`:
    - Holds current state for an active conversation for this game.

- **Features/Dialogue/SO**
  - `CharacterDefinitionAsset`: speaker name, portrait, color, etc.
  - `BackgroundDefinitionAsset`: background sprite, ambience id.
  - `DialogueScriptAsset`: lines and choices for one conversation.
  - `DialogueDatabaseAsset`: list or lookup of many scripts.
  - `AudioLibraryAsset`: mapping from ids to SFX/BGM clips.

- **Features/Dialogue/Data**
  - Instances of assets above: specific characters, scenes, conversations.

- **Features/Dialogue/Unity**
  - `DialogueController`:
    - Hooks up the active `DialogueSession` to input and the `DialogueView`.
    - Loads assets from `DialogueDatabaseAsset` for the current scene/event.

- **Features/Dialogue/View**
  - `DialogueView`:
    - References TMP text, portraits, background images, choice buttons.
    - Has methods like `ShowLine(...)`, `ShowChoices(...)`, `SetSpeaker(...)`.
    - Contains only presentation logic.

---

## 5. Battle Layer – Example Responsibilities

Mirroring the practices you’ve started:

- **Core/Battle**
  - `UnitDefinition`: static data for a unit.
  - `SkillDefinition`: static skill data.
  - `GameBalance`: global tuning (crit, multipliers, etc.).

- **Features/Battle/Logic**
  - `BattleUnit`: runtime HP and stats, `TakeDamage`, `Heal`, `IsDead`.
  - `SkillLogic`: skill damage calculations using `GameBalance`.
  - Later: `TurnController`, `BattleState`, `ActionQueue`, etc.

- **Features/Battle/SO**
  - `UnitDefinitionAsset`: SO script for units.
  - `SkillDefinitionAsset`: SO script for skills.
  - `GameBalanceAsset`: SO script for balance/tuning.

- **Features/Battle/Data**
  - Unit, skill, and balance asset instances (Hero, Slime, Fireball, Normal/Hard, etc.).

- **Features/Battle/Unity**
  - `BattleUnitView`: MonoBehaviour that wraps `BattleUnit` and links to sprites/HP bars.
  - `BattleSceneController`: sets up units, skills, and balance for a battle scene.

- **Features/Battle/View**
  - `SkillButton`: UI for triggering skills.
  - HUD components: HP text/bars, turn order, etc.

---

## 6. Input System Usage

### 6.1 For Movement and Gameplay

- Use `InputActionReference` fields on MonoBehaviours for main controls.
- In `Awake` or `OnEnable`, ensure actions are enabled.
- In `Update`:
  - Read `Vector2` or button state from actions.
  - Convert to Core-friendly types if needed.

Example:

```csharp
[SerializeField] private InputActionReference _moveAction;

private void OnEnable()
{
    _moveAction.action.Enable();
}

private void OnDisable()
{
    _moveAction.action.Disable();
}

private void Update()
{
    Vector2 input = _moveAction.action.ReadValue<Vector2>();
    // pass to movement logic...
}
```

### 6.2 For Small Tools / Debug Scripts

- You may use direct polling via `Keyboard.current` or `Gamepad.current` from `UnityEngine.InputSystem`.
- Do not use `UnityEngine.Input.GetKeyDown` in gameplay code if the project is set to **Input System only**.

Example debug tester:

```csharp
using UnityEngine.InputSystem;

private void Update()
{
    var kb = Keyboard.current;
    if (kb == null) return;

    if (kb.spaceKey.wasPressedThisFrame)
    {
        // trigger test action
    }
}
```

---

## 7. Working with Tests

1. **Where to put tests**
   - Under `Assets/_Project/Tests/EditMode/`.
   - Mirror the folder structure of Core/Features where useful:
     - `Tests/EditMode/Battle`
     - `Tests/EditMode/Dialogue`
     - `Tests/EditMode/_Practice/ArcadeMove`

2. **What to test**
   - Core rules and calculations:
     - Damage formulas, crit logic, HP clamping.
     - Dialogue progression, choices, branching.
   - Feature Logic:
     - Battle turn order.
     - Movement results for given inputs.

3. **What not to test (in unit tests)**
   - Raw Unity rendering, exact layouts.
   - Animator behaviours, VFX.

4. **Naming**
   - Test namespaces: `Project.Tests.<Feature>`.
   - Test class examples:
     - `BattleUnitTests`
     - `SkillLogicTests`
     - `DialogueMachineTests`
     - `PlayerMovementLogicTests`

---

## 8. Adding to Existing Systems – Checklist

When touching existing systems (Battle, Dialogue, etc.), use this checklist:

1. **Is this change Core logic or Unity behaviour?**
   - If it’s rules/data → Core or Feature Logic.
   - If it’s input or visual → Unity or View.

2. **Do I need a ScriptableObject?**
   - If you’re adding a new type of design data → yes.
   - Add a ScriptableObject script under `SO/` and instances under `Data/`.

3. **Am I accidentally adding runtime state to a ScriptableObject?**
   - If a value changes per-frame or per-instance → keep it in a C# class or MonoBehaviour instead.

4. **Do I need a test?**
   - For non-trivial rules (damage, crit, branching, movement) → add or update EditMode tests.

5. **Did I respect dependencies?**
   - No Unity types in Core or pure Logic.
   - No Core → Features references.
   - Assemblies compile without circular dependencies.

---

## 9. Quick Reference per Layer

**Core**

- What: Reusable, engine-agnostic logic and data.
- Can use: .NET / C# only.
- Cannot use: Unity API.
- Typical files: `UnitDefinition`, `SkillDefinition`, `GameBalance`, `DialogueMachine`.

**Features – Logic**

- What: Game-specific orchestration using Core types.
- Can use: Core types, .NET.
- Avoid: Unity API where possible.
- Typical files: `BattleUnit`, `SkillLogic`, `DialogueRunner`, `DialogueSession`.

**Features – SO**

- What: ScriptableObject scripts defining data assets.
- Can use: UnityEngine (for `ScriptableObject`, references to sprites, audio, etc.).
- Typical files: `UnitDefinitionAsset`, `SkillDefinitionAsset`, `GameBalanceAsset`, `CharacterDefinitionAsset`.

**Features – Data**

- What: `.asset` instances used by the game.
- Can contain: configuration values and references (no code).

**Features – Unity**

- What: Scene controllers, input handlers, high-level glue.
- Can use: UnityEngine, InputSystem, Core + Feature Logic + SO assets.
- Typical files: `BattleSceneController`, `PlayerMover`, `DialogueController`.

**Features – View**

- What: Visual/UI components.
- Can use: UnityEngine, TextMeshPro, UI components.
- Typical files: `BattleUnitView`, `DialogueView`, `SkillButton`, HP bars, etc.

**Tests (EditMode)**

- What: Unit tests for Core and Logic.
- Can use: NUnit, Core, Features assemblies.
- Typical files: `BattleUnitTests`, `SkillLogicTests`, `DialogueMachineTests`.

---

This guideline is meant to make day-to-day development smoother:
if a script feels “confused” about where it belongs, use the responsibilities above to move it into the correct layer.
