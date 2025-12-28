# Unity Project Source Ruleset

This document defines how we structure and write code in this Unity project.

The goals:
- Keep **game logic** engine-agnostic, testable, and reusable.
- Keep **Unity code** focused on presentation, input, and scene wiring.
- Use **ScriptableObjects** mainly as data/definition assets, not as global mutable state.
- Support a clean growth path for a 2D, choice-driven RPG with battle systems.

---

## 1. High-Level Architecture

We organize code into three conceptual layers:

1. **Core**  
   - Pure C# logic and data.
   - No Unity APIs (`MonoBehaviour`, `GameObject`, `Transform`, TMP, Input System, etc.).  
   - Contains shared game concepts: stats, unit definitions, skills, effects, balance, dialogue structures, etc.

2. **Features**  
   - Game-specific systems: Battle, Dialogue, Movement practices, etc.
   - Split conceptually into:
     - **Logic**: pure C# feature logic (no Unity types).
     - **Unity/UI**: MonoBehaviours, scene controllers, UI views, Input System usage, ScriptableObject assets.

3. **Tests**  
   - EditMode tests that target **Core** and **Feature Logic** assemblies.
   - No dependence on Unity scene objects.

We build **2D games** moving on the **X/Y plane** (Z is used only for sorting/layering when needed).  
Input is handled exclusively via the **new Input System**.

---

## 2. Folder Structure

Base layout under `Assets/_Project`:

```text
Assets/
  _Project/
    Core/
      Project.Core.asmdef
      Battle/
      Dialogue/
      Stats/
      Effects/
      _Practice/
        ...
    Features/
      Project.Features.asmdef
      _Practice/
        ArcadeMove/
          Logic/
          Unity/
          Data/
      Battle/
        Logic/
        Unity/
        UI/
        Data/
          UnitDefinitions/
          Skills/
          Config/
    Tests/
      EditMode/
        EditMode.asmdef
```

**Rules:**

- All **engine-agnostic** code goes under `Core/`.
- All **feature-specific** code goes under `Features/<FeatureName>/`.
  - `Logic/` for pure C#.
  - `Unity/` for components, scene controllers, and Input System code.
  - `UI/` when you want to keep view/UI scripts separate from Unity scene controllers.
  - `Data/` for ScriptableObject asset instances (not scripts).
- Tests live under `Tests/EditMode/` and target Core + Feature logic.

---

## 3. Assemblies (asmdef) and Dependencies

We use asmdefs to keep dependencies explicit and compilations fast.

### 3.1 Project.Core

- File: `Assets/_Project/Core/Project.Core.asmdef`
- Namespace root: `Project.Core`
- Contains:
  - All pure C# definitions and rules (no Unity types).
- Must **not** reference Unity-specific assemblies unless absolutely necessary.

### 3.2 Project.Features

- File: `Assets/_Project/Features/Project.Features.asmdef`
- Namespace root: `Project.Features`
- Contains:
  - All feature code (Logic, Unity, UI).
- References (typical):
  - `Project.Core`
  - `UnityEngine`
  - `Unity.InputSystem` (for input-related scripts)
  - `Unity.TextMeshPro` (for UI text)
- Feature-specific namespaces live under this assembly:
  - `Project.Features.Battle`
  - `Project.Features.Battle.Unity`
  - `Project.Features.Battle.UI`
  - `Project.Features._Practice.ArcadeMove`, etc.

### 3.3 EditMode (Tests)

- File: `Assets/_Project/Tests/EditMode/EditMode.asmdef`
- Namespace root: `Project.Tests`
- References:
  - `Project.Core`
  - `Project.Features`
  - `TestAssemblies` (optional Unity reference for NUnit, etc.)
- Platforms: `Editor` only.
- Contains only test code.

**Rule:**  
- **Core** must never depend on **Features** or Unity.  
- **Features** can depend on **Core** and Unity.  
- **Tests** depend on Core and Features but are not used in runtime builds.

---

## 4. Namespaces and Naming Conventions

### 4.1 Namespaces

- Core:
  - `Project.Core.Battle`
  - `Project.Core.Stats`
  - `Project.Core.Dialogue`, etc.
- Feature Logic:
  - `Project.Features.Battle`
  - `Project.Features._Practice.ArcadeMove`
- Feature Unity:
  - `Project.Features.Battle.Unity`
  - `Project.Features._Practice.ArcadeMove.Unity`
- Feature UI:
  - `Project.Features.Battle.UI`
- Tests:
  - `Project.Tests.Battle`
  - `Project.Tests._Practice.ArcadeMove`

### 4.2 C# Naming

- **Classes / structs / enums / interfaces**: `PascalCase`
- **Properties**: `PascalCase`
- **Methods**: `PascalCase`
- **Private fields**: `_camelCase`
- **Parameters & locals**: `camelCase`

Examples:

```csharp
public sealed class BattleUnit
{
    private readonly UnitDefinition _definition;
    private int _currentHp;

    public int CurrentHp => _currentHp;

    public void TakeDamage(int damageAmount) { ... }
}
```

---

## 5. ScriptableObjects

We use ScriptableObjects primarily as **data/definition assets**.

### 5.1 Allowed Uses

- Static/shared configuration and design data:
  - Unit definitions (`UnitDefinitionAsset`).
  - Skill definitions (`SkillDefinitionAsset`).
  - Balance/config (`GameBalanceAsset`).
  - Dialogue data wrappers, if needed.
- Small helper methods for converting to Core types:

  ```csharp
  public UnitDefinition ToDefinition() => new UnitDefinition(...);
  ```

- Optional: a few event channels (e.g., `GameEvent` assets) for cross-scene signals, used sparingly.

### 5.2 Disallowed / Discouraged Uses

- Do **not** use ScriptableObjects as generic “variable” containers holding runtime state like:
  - Current HP
  - Current position
  - Cooldowns
  - Player inventory at runtime
- Do **not** put complex game rules in ScriptableObjects:
  - Rules should live in Core/Feature logic classes.

### 5.3 Asset Placement

- ScriptableObject **scripts** (classes) live under `Features/<Feature>/Unity/`.
- ScriptableObject **instances** (`.asset` files) live under `Features/<Feature>/Data/`:
  - `Assets/_Project/Features/Battle/Data/UnitDefinitions/*.asset`
  - `Assets/_Project/Features/Battle/Data/Skills/*.asset`
  - `Assets/_Project/Features/Battle/Data/Config/*.asset`

---

## 6. Input and 2D Movement

### 6.1 Input

- We use **only the new Input System** (`UnityEngine.InputSystem`).
- Old API (`UnityEngine.Input`) is not allowed in gameplay code.
- If project setting is *Input System only*, avoid `Input.GetKeyDown` and use:
  - `InputActionReference` for action-based input, or
  - `Keyboard.current`, `Gamepad.current`, etc., for direct polling in small practice scripts.

### 6.2 2D Movement

- All movement is on the **X/Y plane**:
  - `transform.position.x` and `.y` are modified.
  - `.z` is kept constant or used only for sorting layers.
- Core/Logic uses `System.Numerics.Vector2` for movement input and delta when possible.
- Unity layer converts between `UnityEngine.Vector2` and `System.Numerics.Vector2` at the boundary.

Example:

```csharp
// Core
public Vector2 GetDelta(Vector2 input, float dt);

// Unity
Vector2 input = _moveAction.action.ReadValue<Vector2>();
var coreInput = new System.Numerics.Vector2(input.x, input.y);
var coreDelta = _movementLogic.GetDelta(coreInput, Time.deltaTime);
var pos = transform.position;
pos.x += coreDelta.X;
pos.y += coreDelta.Y;
transform.position = pos;
```

---

## 7. Practice Flows (1–4)

These practices are examples of how to use this structure.

### Practice 1 – Player Movement

- **Core**:
  - `PlayerStats` (move speed).
  - `PlayerMovementLogic` (input + dt → delta Vector2).
- **Features/Unity**:
  - `PlayerMover` MonoBehaviour uses Input System, calls `PlayerMovementLogic`, moves transform.
- **Tests**:
  - EditMode tests for `PlayerMovementLogic.GetDelta`.

### Practice 2 – HP System

- **Core**:
  - `UnitDefinition` (id, name, MaxHp, Attack).
- **Feature Logic**:
  - `BattleUnit` manages `CurrentHp`, `TakeDamage`, `Heal`, `IsDead`.
- **Feature Unity**:
  - `UnitDefinitionAsset` (ScriptableObject wrapper).
  - `BattleUnitView` holds a `BattleUnit` and exposes methods like `ApplyDamage`, `ApplyHeal`.

### Practice 3 – Skills

- **Core**:
  - `SkillDefinition` (id, name, description, power, mpCost).
- **Feature Logic**:
  - `SkillLogic` calculates damage based on skill and units.
- **Feature Unity**:
  - `SkillDefinitionAsset` (ScriptableObject for skills).
  - `SkillButton` (UI) uses a `SkillDefinitionAsset` + `BattleUnitView`s (attacker/target) to call `SkillLogic`.

### Practice 4 – Balance Config

- **Core**:
  - `GameBalance` (crit chance, crit multiplier, global damage multiplier, exp multiplier).
- **Feature Logic**:
  - `SkillLogic` takes a `GameBalance` instance and uses it for crit and scaling.
- **Feature Unity**:
  - `GameBalanceAsset` (ScriptableObject for balance/config).
  - `SkillButton` references a `GameBalanceAsset`, converts it to `GameBalance`, and passes it to `SkillLogic`.

---

## 8. General Coding Rules

1. **Keep Core clean**  
   - No UnityEngine types or Editor APIs.
   - No direct references to MonoBehaviours or ScriptableObjects.

2. **Unity as a thin layer**  
   - MonoBehaviours should:
     - Read input.
     - Call Core/Logic classes.
     - Update scene objects (transforms, UI, animations).
   - Avoid putting heavy game rules into MonoBehaviours.

3. **Prefer constructor injection in Core/Logic**  
   - Pass dependencies via constructors or method parameters.
   - Avoid global static state for gameplay logic.

4. **Use ScriptableObjects as design-time data sources**  
   - Convert them to Core definitions at runtime (`ToDefinition()`, `ToBalance()`, etc.).

5. **Write tests for Core and Logic where it matters**  
   - Movement calculation, HP rules, damage formulas, crit logic, turn order, etc.
   - Keep these free of Unity dependencies so EditMode tests are simple.

---

This ruleset should be treated as the default for all new gameplay code.  
If we deviate, we should have a clear reason (and ideally document it).
