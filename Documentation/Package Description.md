# Observable Variables — Package Description

Package: `com.fixer33.observable-variables` · Version `1.2.0` · Unity `6000.0`+ · License MIT
Install from Git URL: `https://github.com/Fixer33/ObservableVariablesPackage.git`

Written from the package contents only (code, asmdefs, `package.json`, README, CHANGELOG, samples, tests). Anything inferred rather than read directly is marked **(assumed)**; points settled by the author are marked **(confirmed)**. Unclear points are listed in `Open Questions.md` next to this file.

## 1. Purpose and audience

The package gives a project **global, key-addressed observable values** without singletons or ScriptableObject assets. Any `enum` value is a key; `Variables.Get<T>(key)` returns (and creates on first use) a shared variable object of type `T`. Every variable raises a `ValueChanged` event when its `Value` is assigned, so consumers can react instead of polling in `Update`.

The README positions it as a lighter alternative to the ScriptableObject Architecture Pattern (SOAP): no assets to create, no installer, no inspector references to wire; only enums for keys.

Audience: Unity gameplay/UI programmers who need to share state (score, health, current target, player transform, ...) between unrelated components. The observable types can also be used on their own as `[SerializeField]` fields.

## 2. Architecture and main components

```
Runtime (asmdef com.fixer33.observable-variables, all platforms)
  Variables (static registry: Dictionary<Enum, ObservableVariableBase>)
  Contract/
    ObservableVariableBase            non-generic base, GetValue(): object
    ObservableVariableBase<T>         Value, ValueChanged, implicit operator T
    IObservableVariableEventHolder    (+ generic <TVariableType>) stored/removed callbacks
  BasicTypes/  ObservableBool, ObservableFloat, ObservableInt, ObservableString
  Unity/       ObservableBehaviour, ObservableGameObject, ObservableTransform,
               ObservableVector2, ObservableVector3

Editor (asmdef com.fixer33.observable-variables.Editor, Editor only)
  GUI/VariablesWindow                 "Tools/Fixer33/Variables window" (UI Toolkit)
  GUI/ObservableVariableElements/     per-type UI Toolkit rows + factory + override hook
  Drawers/                            IMGUI PropertyDrawers for serialized fields
  GUI/Markup and stylesheets/         VariablesWindow.uxml/.uss, ObservableVariableElement.uss
```

### Runtime flow

- `Variables.Get<T>(Enum key)` looks up `key` in a private static dictionary. If missing, it creates `new T()` and stores it. If the stored variable is not a `T`, it throws `System.ComponentModel.InvalidEnumArgumentException("This key is already used for another type")`.
- Keys are `System.Enum` values, so two different enum types with the same numeric value are different keys **(assumed, from standard `Enum.Equals` semantics)**.
- The dictionary is never cleared at runtime; it lives for the lifetime of the AppDomain **(assumed)**. Only `Variables.Tests_Clear()` (compiled under `UNITY_INCLUDE_TESTS`) clears it.
- `ObservableVariableBase<T>.Value` setter, in order:
  1. if the previous value implemented `IObservableVariableEventHolder`, calls its `OnDataRemovedObservableVariable(this)`;
  2. stores the new value;
  3. if the new value implements `IObservableVariableEventHolder`, calls its `OnDataStoredInObservableVariable(this)`;
  4. invokes `ValueChanged(newValue)`.
  The setter now has an equality check: it skips events when the value is unchanged. This is the **confirmed** intended behaviour; the fix is in source and has not been verified in Unity.
- Each concrete type defines an implicit conversion **from** the raw value (e.g. `ObservableInt x = 5;`), which creates a **new** variable instance. The base class defines an implicit conversion **to** `T` (`int i = myObservableInt;`).

### Editor flow

- `VariablesWindow` (internal `EditorWindow`) loads its UXML/USS from `Packages/com.fixer33.observable-variables/Editor/GUI/Markup and stylesheets/` (prefixed with `Assets/` when the scripting define `PACKAGES_DEV` is set, for developing the package embedded under `Assets`).
- It reflects over all loaded assemblies for non-abstract subclasses of `ObservableVariableBase` and lets the user cycle through them with `<` / `>`. For the current type it calls `Variables.Editor_GetAllKeysForType(type)` and `Variables.Editor_GetExisting(key)`, and builds one row per key via `ObservableVariableElementFactory.Create(variable, key)`. The row label is `EnumTypeName.EnumValue`.
- Outside Play Mode it shows "Can't browse variables in editor mode". In Play Mode it rebuilds the list every 380 editor `Update` calls.
- `ObservableVariableElementFactory.Create` first asks registered `IObservableVariableElementFactoryOverride`s (`IsApplicableTo` → `CreateInputField`), then falls back to a built-in `switch` over the nine shipped types, then to `ObservableVariableDefaultElement` (read-only label showing `GetValue().ToString()` or `"null"`).
- Editable rows derive from `ObservableVariableElementBase<T1, T2, T3>` (variable type, value type, input `VisualElement` type). They subscribe to `ValueChanged` to update the field, and write field changes back to `Variable.Value` when the value differs.
- PropertyDrawers (`ObservableVariableDrawer<T>` and subclasses) draw serialized observable fields in the Inspector: label only in Edit Mode; label plus the current value as read-only text in Play Mode.

## 3. Public API surface

### Namespace `ObservableVariables`
`static class Variables`
- `T Get<T>(Enum key) where T : ObservableVariableBase, new()`
- Shortcuts: `GetFloat`, `GetInt`, `GetBool`, `GetString`, `GetGameObject`, `GetBehaviour`, `GetTransform`, `GetVector2`, `GetVector3` (each `(Enum key)`).
- `#if UNITY_EDITOR`: `Enum[] Editor_GetAllKeysForType(Type type)`, `ObservableVariableBase Editor_GetExisting(Enum key)` (returns null if absent).
- `#if UNITY_INCLUDE_TESTS`: `void Tests_Clear()`.

### Namespace `ObservableVariables.Contract`
- `abstract class ObservableVariableBase` — `abstract object GetValue()`.
- `[Serializable] abstract class ObservableVariableBase<T> : ObservableVariableBase`
  - `delegate void ValueChangedDelegate(T newValue)`
  - `event ValueChangedDelegate ValueChanged`
  - `T Value { get; set; }`
  - `sealed override object GetValue()`
  - `static implicit operator T(ObservableVariableBase<T> val)`
  - protected constructors `()` and `(T value)`.
- `interface IObservableVariableEventHolder` — `OnDataStoredInObservableVariable(ObservableVariableBase)`, `OnDataRemovedObservableVariable(ObservableVariableBase)`.
- `interface IObservableVariableEventHolder<in TVariableType> : IObservableVariableEventHolder` (`TVariableType : ObservableVariableBase`) — typed versions of both callbacks; default interface implementations forward the non-generic calls when the variable is a `TVariableType`.

### Namespace `ObservableVariables.BasicTypes`
`ObservableBool`, `ObservableFloat`, `ObservableInt`, `ObservableString` — `[Serializable]`, constructors `()` and `(value)`, implicit conversion from the raw type.

### Namespace `ObservableVariables.Unity`
`ObservableBehaviour` (`Behaviour`), `ObservableGameObject`, `ObservableTransform`, `ObservableVector2`, `ObservableVector3` — same shape as the basic types.

### Editor API (assembly `com.fixer33.observable-variables.Editor`)
- `ObservableVariables.Editor.GUI.ObservableVariableElements`
  - `static class ObservableVariableElementFactory` — `Create(ObservableVariableBase, Enum)`, `AddOverride`, `RemoveOverride`, `HasOverride`.
  - `interface IObservableVariableElementFactoryOverride` — `bool IsApplicableTo(ObservableVariableBase)`, `ObservableVariableElementBase CreateInputField(ObservableVariableBase, StyleSheet, string variableName)`.
  - `abstract class ObservableVariableElementBase : VisualElement` and `abstract class ObservableVariableElementBase<T1, T2, T3>` with abstract `Initialize`, `GetInputFieldValue`, `SetInputFieldValue`; protected `Variable`, `InputField`.
  - `ObservableVariableDefaultElement`.
  - Built-in rows: `ObservableBoolElement`, `ObservableFloatElement`, `ObservableIntElement`, `ObservableStringElement` (`.BasicTypes`), `ObservableBehaviourElement`, `ObservableGameObjectElement`, `ObservableTransformElement`, `ObservableVector2Element`, `ObservableVector3Element` (`.Unity`).
- `ObservableVariables.Editor.Drawers` — `abstract class ObservableVariableDrawer<T> : PropertyDrawer` with abstract `string GetValueString(T)`.
- Drawers: `ObservableBoolDrawer`, `ObservableFloatDrawer`, `ObservableIntDrawer` (namespace `ObservableVariables.Drawers.BasicTypes`), `ObservableBehaviourDrawer`, `ObservableGameObjectDrawer`, `ObservableTransformDrawer`, `ObservableVector2Drawer`, `ObservableVector3Drawer` (namespace `ObservableVariables.Editor.Drawers.Unity`). There is no drawer for `ObservableString`.
- `VariablesWindow` is `internal`.

### Extension points
1. **Custom variable type:** subclass `ObservableVariableBase<T>` with a public parameterless constructor (required by `Get<T>`'s `new()` constraint). Add `[Serializable]` if it will be a serialized field. Optionally add an implicit conversion from `T`.
2. **Editable row in the Variables window:** implement `IObservableVariableElementFactoryOverride`, usually returning a subclass of `ObservableVariableElementBase<T1,T2,T3>`, and register it with `ObservableVariableElementFactory.AddOverride(...)`. Without it, a custom type shows as a read-only `ToString()` label.
3. **Inspector drawer:** subclass `ObservableVariableDrawer<T>` and add `[CustomPropertyDrawer(typeof(YourType))]`.
4. **Lifecycle hooks on stored objects:** implement `IObservableVariableEventHolder<TVariable>` on the value type.

## 4. Usage example

```csharp
using ObservableVariables;
using ObservableVariables.BasicTypes;

public enum GameKeys { Score, PlayerName }

// writer
Variables.GetInt(GameKeys.Score).Value += 10;

// reader
ObservableInt score = Variables.GetInt(GameKeys.Score);
score.ValueChanged += newScore => label.text = newScore.ToString();
// unsubscribe when done (e.g. OnDestroy)
```

Do not write `score = 5;` on a variable obtained from `Variables`: the implicit conversion creates a new, unregistered instance. Use `score.Value = 5;`.

## 5. Dependencies

- **Declared (`package.json`):** `com.unity.test-framework` `1.4.6`. Only the test assemblies use it **(assumed it could be optional)**.
- **Unity modules used:** UnityEngine core (Runtime), UnityEditor + UI Toolkit (`UnityEngine.UIElements`, `UnityEditor.UIElements`) in the Editor assembly.
- **Asmdefs:**
  - `com.fixer33.observable-variables` (root namespace `ObservableVariables`): no references, all platforms, auto-referenced.
  - `com.fixer33.observable-variables.Editor` (root namespace `ObservableVariables.Editor`): references the runtime asmdef by GUID, Editor only, auto-referenced.
  - `com.fixer33.observable-variables.Editor.Tests` and `com.fixer33.observable-variables.Tests`: reference `UnityEngine.TestRunner`, `UnityEditor.TestRunner`, the runtime asmdef and `nunit.framework.dll`; constraint `UNITY_INCLUDE_TESTS`; not auto-referenced.
  - Sample asmdef `com.fixer33.observable-variables.Example` (in "Using as a field").
- **Define symbols read by the code:** `UNITY_EDITOR`, `UNITY_INCLUDE_TESTS` (Unity built-ins), `PACKAGES_DEV` (custom; switches editor asset paths to `Assets/Packages/...` when the package is developed under `Assets`).
- **C# features:** default interface implementations, `#nullable`, `record`, target-typed `new` (needs Unity's C# 9+ compiler; fine for Unity 6).
- No dependency on other `com.fixer33.*` packages.

## 6. Editor tools and menu items

| Menu | What it does |
|---|---|
| `Tools/Fixer33/Variables window` | Opens `VariablesWindow`: in Play Mode, browse all registered global variables by type and edit values for the supported types. |

Inspector PropertyDrawers for the shipped types (see §3). No other menu items, context menus or project settings.

## 7. Samples (`Samples~`, importable from the Package Manager "Samples" tab)

| Sample | Contents |
|---|---|
| **Using as a field** | `ExampleMonoBehaviour` (namespace `ObservableVariables.Example`, own asmdef) with `[SerializeField]` observable fields of every shipped type except string; subscribes to `ObservableGameObject.ValueChanged`, writes position into an `ObservableVector3` each frame. Scene `Example.unity`. |
| **Custom variable without drawer** | `CustomData` struct, `ObservableCustomData : ObservableVariableBase<CustomData>`, `ExampleKeys` enum, `ExampleBehaviour1` (sets Key1 in `Awake`), `ExampleBehaviour2` (logs Key1 and Key2 in `Start`). Scene `Example.unity`. No asmdef (compiles into the default assembly **(assumed)**). |
| **Custom variable with drawer** | Same as above plus `ObservableVariableOverrideInstaller` (`#if UNITY_EDITOR`): registers `ObservableCustomDataElementOverrideData` via `[RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]`, which creates `ObservableCustomDataElement` (a `TextField` row). `ExampleBehaviour2` adds a `LogKey1ValueInUpdate` toggle. |

Both "Custom variable" samples declare the same types in namespace `Sample`; importing both at once will probably cause duplicate-type compile errors **(assumed)**.

## 8. Tests

Editor tests (`Tests/Editor`, namespace `ObservableVariables.EditorTests`), run from *Window > General > Test Runner*:
- `BasicTypes` — value set, `ValueChanged`, implicit cast and shortcut methods for float/int/bool/string.
- `UnityTypes` — same for Vector2/Vector3/GameObject (no tests for Behaviour/Transform).
- `VariablesDictionary` — creation, retrieval, per-key isolation, same instance for the same key, `InvalidEnumArgumentException` on type mismatch.
- `AdditionalFunctions` — `IObservableVariableEventHolder` stored/removed callbacks.

`Tests/Runtime` contains only an asmdef; there are no runtime tests.

## 9. How other projects consume it

1. Package Manager → *+* → *Install package from git URL…* → `https://github.com/Fixer33/ObservableVariablesPackage.git` (optionally `#1.2.0`, a git tag `1.2.0` exists **(assumed usable as a version pin)**).
2. Create one or more enums for keys and call `Variables.Get<T>` / the shortcuts.
3. Optionally import samples, define custom types and register Variables-window overrides from editor-only code.

## 10. Current state

- **Finished and stable.** Version 1.2.0 (CHANGELOG dated 2026-03-24) added `IObservableVariableEventHolder` and the shortcut methods. Small, complete API; no TODO comments in code.
- **Known defects / rough edges found in the code:**
  - `ObservableVariableBase<T>.Value` raises `ValueChanged` and the event-holder callbacks even when the value is unchanged; it should skip them **(confirmed)**. Fixed in source; not yet verified in Unity.
  - `ObservableBoolDrawer` is registered with `[CustomPropertyDrawer(typeof(ObservableInt))]` instead of `ObservableBool`, so `ObservableBool` fields get no custom drawer and `ObservableInt` has two drawers registered. This typo is fixed in source **(confirmed)**; not yet verified in Unity.
  - `ObservableVariableDrawer<T>` carries `[CustomPropertyDrawer(typeof(ObservableVariableBase<>))]` but is abstract, so the attribute has no effect **(assumed)**. It also reads a `_value` field via reflection into a local that is never used.
  - The bool/float/int drawers live in namespace `ObservableVariables.Drawers.BasicTypes`, unlike the other editor types (`ObservableVariables.Editor.*`).
  - `ObservableVariableElementBase<T1,T2,T3>` unsubscribes from `ValueChanged` only in a finalizer; because the variable holds the delegate, rows rebuilt every refresh likely stay subscribed and are never collected **(assumed)**.
  - The UXML `Style src` points to `.../Editor/Markup%20and%20stylesheets/VariablesWindow.uss` (missing the `GUI/` folder); the window also loads the USS by path, so styling still works **(assumed)**.
  - Sample `ObservableVariableOverrideInstaller.Install` contains a leftover `Debug.Log("123")`.
  - `VariablesDictionary.CreateNewValueVariableAndSetValue` uses `Assert.NotNull` on a `bool`, so it always passes.
  - CHANGELOG 1.0.1 says shortcut methods were introduced, while 1.2.0 lists them as added.
  - README's API table names the generic parameter type `ObservableVariableType`; the real constraint is `ObservableVariableBase, new()`.
- **By design:** `ObservableVariableBase<T>._value` is private and not serialized **(confirmed)**. Observable fields therefore carry no serialized value; the Inspector drawers only display the runtime value, read-only.
- **Unused / stubbed:** the `Tests/Runtime` assembly is empty.
