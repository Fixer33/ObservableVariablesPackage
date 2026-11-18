# Observable Variables — Usage Guide

Package `com.fixer33.observable-variables` · version `1.2.0` · Unity `6000.0`+ · MIT

This guide explains how to install and use the package. Everything here was checked against the package's code, samples and README. Anything inferred rather than read directly is marked **(assumed)**. For architecture details see `Package Description.md`; for unresolved points see `Open Questions.md` (both next to this file).

---

## 1. Installation

### Option A: through the Fixer33 Package Installer (preferred)

1. Install the installer package first, from its Git URL: `https://github.com/Fixer33/PackageInstaller.git` (*Window > Package Manager > + > Install package from git URL...*).
2. Open **Tools > Fixer33 > Package Installer**.
3. In the **Tools** group, select **Observable Variables** and press **Install**.

The installer entry for this package is:

```json
{
  "PackageName": "Observable Variables",
  "PackageId": "com.fixer33.observable-variables",
  "PackageUrl": "https://github.com/Fixer33/ObservableVariablesPackage.git"
}
```

The entry declares no `Dependencies`, so nothing else is installed with it.

### Option B: Git URL directly

*Window > Package Manager > + > Install package from git URL...* and paste:

```
https://github.com/Fixer33/ObservableVariablesPackage.git
```

To pin a version, append the tag: `https://github.com/Fixer33/ObservableVariablesPackage.git#1.2.0` (the repository has a `1.2.0` tag).

### What gets installed

| Assembly | Platforms | Contents |
|---|---|---|
| `com.fixer33.observable-variables` | all | `Variables`, the variable types, `IObservableVariableEventHolder` |
| `com.fixer33.observable-variables.Editor` | Editor only | Variables window, row elements, Inspector drawers |

Both assemblies are auto-referenced, so scripts in the default `Assembly-CSharp` can use them without setup. If your code lives in its own asmdef, add a reference to `com.fixer33.observable-variables` (and to `com.fixer33.observable-variables.Editor` for editor extensions).

`package.json` declares one dependency, `com.unity.test-framework` `1.4.6`, which UPM installs automatically. It is only needed by the package's tests.

### Samples

Package Manager > Observable Variables > **Samples** tab:

| Sample | Shows |
|---|---|
| Using as a field | Observable types as `[SerializeField]` fields on a MonoBehaviour (`ExampleMonoBehaviour`). Has its own asmdef. |
| Custom variable without drawer | A custom type `ObservableCustomData` used through `Variables.Get<T>`. |
| Custom variable with drawer | Same, plus an editable row for it in the Variables window. |

Import only one of the two "Custom variable" samples: both declare the same types in namespace `Sample` without an asmdef, so importing both will probably cause duplicate-type compile errors **(assumed)**.

---

## 2. Minimal setup

There is nothing to configure: no assets, no installer, no scene objects. You need only an enum for keys.

```csharp
public enum GameKeys
{
    Score,
    PlayerName,
    IsPaused,
}
```

Any `enum` works, and you can have as many key enums as you like. Values from different enum types are different keys even if their numeric values match **(assumed, from standard `Enum` equality)**.

---

## 3. Using the API

Namespaces:

```csharp
using ObservableVariables;               // Variables
using ObservableVariables.BasicTypes;    // ObservableBool, ObservableFloat, ObservableInt, ObservableString
using ObservableVariables.Unity;         // ObservableBehaviour, ObservableGameObject, ObservableTransform, ObservableVector2, ObservableVector3
using ObservableVariables.Contract;      // ObservableVariableBase<T>, IObservableVariableEventHolder
```

### 3.1 Get a global variable

`Variables.Get<T>(Enum key)` returns the variable registered under `key`. If none exists yet, it creates `new T()` (default value) and registers it. Every later call with the same key returns the **same instance**.

```csharp
ObservableInt score = Variables.Get<ObservableInt>(GameKeys.Score);
```

`T` must derive from `ObservableVariableBase` and have a public parameterless constructor (`where T : ObservableVariableBase, new()`).

Shortcut methods exist for the shipped types:

| Shortcut | Returns |
|---|---|
| `Variables.GetFloat(key)` | `ObservableFloat` |
| `Variables.GetInt(key)` | `ObservableInt` |
| `Variables.GetBool(key)` | `ObservableBool` |
| `Variables.GetString(key)` | `ObservableString` |
| `Variables.GetGameObject(key)` | `ObservableGameObject` |
| `Variables.GetBehaviour(key)` | `ObservableBehaviour` |
| `Variables.GetTransform(key)` | `ObservableTransform` |
| `Variables.GetVector2(key)` | `ObservableVector2` |
| `Variables.GetVector3(key)` | `ObservableVector3` |

### 3.2 Read and write the value

Read and write through `Value`. Each assignment raises `ValueChanged`.

```csharp
Variables.GetInt(GameKeys.Score).Value += 10;
Variables.GetString(GameKeys.PlayerName).Value = "Player 1";

int current = Variables.GetInt(GameKeys.Score).Value;
int alsoCurrent = Variables.GetInt(GameKeys.Score); // implicit conversion to int
```

`GetValue()` returns the value as `object` (useful for code that handles variables of unknown type).

### 3.3 React to changes

`ValueChanged` is an event of type `ObservableVariableBase<T>.ValueChangedDelegate` (`void (T newValue)`). Subscribe when the component becomes active and unsubscribe when it goes away; the variable lives for the whole session and keeps references to its subscribers.

```csharp
using ObservableVariables;
using ObservableVariables.BasicTypes;
using UnityEngine;

public class ScoreLabel : MonoBehaviour
{
    private ObservableInt _score;

    private void OnEnable()
    {
        _score = Variables.GetInt(GameKeys.Score);
        _score.ValueChanged += OnScoreChanged;
        OnScoreChanged(_score.Value); // show the current value immediately
    }

    private void OnDisable()
    {
        _score.ValueChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int newScore)
    {
        Debug.Log($"Score: {newScore}");
    }
}
```

The event is not raised on subscription; read `Value` yourself to get the starting state.

### 3.4 Use observable types as fields

The observable types are `[Serializable]` and can be used as fields without the global registry (sample "Using as a field"):

```csharp
using ObservableVariables.BasicTypes;
using ObservableVariables.Unity;
using UnityEngine;

public class ExampleMonoBehaviour : MonoBehaviour
{
    [SerializeField] private ObservableInt _observableInt = 3;
    [SerializeField] private ObservableGameObject _observableGameObject;
    [SerializeField] private ObservableVector3 _currentPosition;

    private void Start() => _observableGameObject.ValueChanged += OnGameObjectChanged;
    private void OnDestroy() => _observableGameObject.ValueChanged -= OnGameObjectChanged;
    private void Update() => _currentPosition.Value = transform.position;

    private void OnGameObjectChanged(GameObject newValue) => Debug.Log($"New game object selected: {newValue}");
}
```

The stored value is **not serialized** (by design). The Inspector shows the field name only, and in Play Mode it also shows the current value as read-only text. Initial values such as `= 3` are therefore probably not kept after Unity deserializes the component **(assumed)**; set start values in code (`Awake`/`Start`) if you depend on them.

### 3.5 Get notified when an object is stored in a variable

If the value type implements `IObservableVariableEventHolder<TVariableType>`, the variable calls it when the object is assigned to, or replaced in, the variable:

```csharp
using ObservableVariables.Contract;

public class MyData : IObservableVariableEventHolder<ObservableMyData>
{
    public void OnDataStoredInObservableVariable(ObservableMyData variable)
    {
        // this instance was just assigned to variable.Value
    }

    public void OnDataRemovedObservableVariable(ObservableMyData variable)
    {
        // this instance was just replaced by another value
    }
}

public class ObservableMyData : ObservableVariableBase<MyData>
{
    public ObservableMyData() : base() { }
}
```

Order inside the `Value` setter: removed callback on the old value, store the new value, stored callback on the new value, then `ValueChanged`. The typed callbacks are only called when the variable is of type `TVariableType`. You can implement the non-generic `IObservableVariableEventHolder` instead to receive `ObservableVariableBase`.

A value passed to a constructor (for example `new ObservableMyData(data)`, if your type has one) does **not** trigger `OnDataStoredInObservableVariable`; only the `Value` setter does.

---

## 4. Editor windows and menu paths

| Menu path | Window | Notes |
|---|---|---|
| **Tools > Fixer33 > Variables window** | "Variables window" | Browse and edit the global variables in Play Mode. |

How it works:

- **Outside Play Mode** it shows "Can't browse variables in editor mode".
- **In Play Mode** the header shows one variable type at a time (for example `ObservableInt`). Press **<** / **>** to cycle through all non-abstract subclasses of `ObservableVariableBase` found in the loaded assemblies, including your own types.
- Each registered key of that type is one row, labelled `EnumTypeName.EnumValue` (for example `GameKeys.Score`).
- The list is rebuilt about every 380 editor updates, so newly registered keys appear after a short delay.
- Rows for the nine shipped types are editable:

| Type | Input field |
|---|---|
| `ObservableBool` | `Toggle` |
| `ObservableFloat` | `FloatField` |
| `ObservableInt` | `IntegerField` |
| `ObservableString` | `TextField` |
| `ObservableBehaviour` | `ObjectField` |
| `ObservableGameObject` | `ObjectField` |
| `ObservableTransform` | `ObjectField` |
| `ObservableVector2` | `Vector2Field` |
| `ObservableVector3` | `Vector3Field` |

  Editing a field writes to `Value` (raising `ValueChanged`). Changes made from code update the field.
- Custom types show a read-only label with `ToString()` of the value (or `null`) unless you register an override (section 6).

**Inspector drawers** exist for serialized fields of `ObservableFloat`, `ObservableInt`, `ObservableBehaviour`, `ObservableGameObject`, `ObservableTransform`, `ObservableVector2`, `ObservableVector3`. There is no drawer for `ObservableString`, and `ObservableBool` now gets one (the typo was fixed in source; not yet verified in Unity, see section 7).

---

## 5. Configuration and scripting defines

There are no settings assets, Project Settings pages or preferences. The window's refresh interval (380 updates) is hard-coded.

Scripting defines read by the package:

| Define | Set by | Effect |
|---|---|---|
| `UNITY_EDITOR` | Unity | Enables `Variables.Editor_GetAllKeysForType(Type)` and `Variables.Editor_GetExisting(Enum)`, used by the Variables window. Do not call them from runtime code. |
| `UNITY_INCLUDE_TESTS` | Unity (test assemblies) | Enables `Variables.Tests_Clear()`, which empties the registry. Intended for tests only. |
| `PACKAGES_DEV` | you, optional | Only for developing the package itself embedded under `Assets/Packages/com.fixer33.observable-variables`. The editor then loads its UXML/USS from `Assets/Packages/...` instead of `Packages/...`. Do **not** set it in a normal project: the Variables window would fail to find its layout and stylesheets **(assumed)**. |

---

## 6. Extension points

| Goal | What to do |
|---|---|
| New variable type | Subclass `ObservableVariableBase<T>` with a public parameterless constructor. Add `[Serializable]` if it will be a serialized field. Optionally add `public static implicit operator YourType(T val) => new(val);`. |
| Editable row in the Variables window | Implement `IObservableVariableElementFactoryOverride` and register it with `ObservableVariableElementFactory.AddOverride(...)`. The row usually derives from `ObservableVariableElementBase<TVariable, TValue, TInputField>`. Remove with `RemoveOverride`; check with `HasOverride`. Overrides are checked before the built-in types, so you can also replace a shipped row. |
| Inspector drawer | Subclass `ObservableVariableDrawer<T>` (namespace `ObservableVariables.Editor.Drawers`), implement `protected override string GetValueString(T val)`, and add `[CustomPropertyDrawer(typeof(YourType))]`. |
| Lifecycle callbacks on stored objects | Implement `IObservableVariableEventHolder<TVariableType>` on the value type (section 3.5). |

### Worked example: an `ObservableColor` with a window row and a drawer

**Runtime type** (any runtime folder):

```csharp
using System;
using ObservableVariables.Contract;
using UnityEngine;

[Serializable]
public class ObservableColor : ObservableVariableBase<Color>
{
    public ObservableColor() : base() { }
    public ObservableColor(Color value) : base(value) { }

    public static implicit operator ObservableColor(Color val) => new(val);
}
```

Use it like any shipped type:

```csharp
Variables.Get<ObservableColor>(GameKeys.TeamColor).Value = Color.red;
```

(`GameKeys.TeamColor` is a key you add to your own enum.)

**Variables window row** (in an `Editor` folder, or an editor-only asmdef that references `com.fixer33.observable-variables` and `com.fixer33.observable-variables.Editor`):

```csharp
using ObservableVariables.Contract;
using ObservableVariables.Editor.GUI.ObservableVariableElements;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ObservableColorElement : ObservableVariableElementBase<ObservableColor, Color, ColorField>
{
    public ObservableColorElement(ObservableColor variable, StyleSheet styleSheet, string variableName)
        : base(variable, styleSheet, variableName) { }

    protected override void Initialize(ColorField editField, Color activeValue,
        EventCallback<ChangeEvent<Color>> onInputValueChanged)
    {
        editField.value = activeValue;
        editField.RegisterValueChangedCallback(onInputValueChanged);
    }

    protected override Color GetInputFieldValue(ColorField inputField) => inputField.value;
    protected override void SetInputFieldValue(ColorField inputField, Color value) => inputField.value = value;
}

public class ObservableColorElementOverride : IObservableVariableElementFactoryOverride
{
    public bool IsApplicableTo(ObservableVariableBase variable) => variable is ObservableColor;

    public ObservableVariableElementBase CreateInputField(ObservableVariableBase variable,
        StyleSheet mainStyleSheet, string variableName) =>
        new ObservableColorElement(variable as ObservableColor, mainStyleSheet, variableName);
}
```

**Registering the override.** The "Custom variable with drawer" sample registers its override from a runtime script inside `#if UNITY_EDITOR`, so it becomes active when entering Play Mode (the only time the window shows rows):

```csharp
#if UNITY_EDITOR
using ObservableVariables.Editor.GUI.ObservableVariableElements;
using UnityEngine;

public static class ObservableColorOverrideInstaller
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Install()
    {
        ObservableVariableElementFactory.AddOverride(new ObservableColorElementOverride());
    }
}
#endif
```

This pattern needs the script to be in an assembly that can see the Editor assembly while in the Editor (the default `Assembly-CSharp` works, as in the sample). Registering from an editor assembly with `[InitializeOnLoad]` should also work **(assumed)**; which pattern is recommended is still open (see section 9). `AddOverride` ignores an override that is already registered (by equality), so repeated registration is harmless. With a class (not a `record`) a new instance is not equal to the old one, so register exactly once per domain load **(assumed)**.

**Inspector drawer** (Editor folder or editor asmdef):

```csharp
using ObservableVariables.Editor.Drawers;
using UnityEditor;

[CustomPropertyDrawer(typeof(ObservableColor))]
public class ObservableColorDrawer : ObservableVariableDrawer<ObservableColor>
{
    protected override string GetValueString(ObservableColor val) => val.Value.ToString();
}
```

---

## 7. Common pitfalls and known issues

### Pitfalls

- **Never assign a raw value to a variable reference.** `ObservableInt score = Variables.GetInt(GameKeys.Score); score = 5;` uses the implicit conversion `int -> ObservableInt`, which creates a **new, unregistered** instance. The global variable is unchanged and nobody is notified. Always write `score.Value = 5;`.
- **One key, one type.** Calling `Get<ObservableFloat>(GameKeys.Score)` after `GetInt(GameKeys.Score)` throws `System.ComponentModel.InvalidEnumArgumentException("This key is already used for another type")`. Use separate keys per type.
- **Every assignment raises events, even with the same value.** The `Value` setter now skips events when the new value equals the current one (fixed in source; not yet verified in Unity). Before the fix, `x.Value = x.Value;` raised `ValueChanged` and the event-holder removed/stored callbacks.
- **Unsubscribe.** Variables are never removed from the registry, so they keep strong references to every subscriber. Unsubscribe in `OnDisable`/`OnDestroy`, or destroyed objects keep receiving events (and may throw `MissingReferenceException`) **(assumed)**.
- **No reset or remove API.** Outside tests, the registry is never cleared; values live until the scripting domain reloads. With *Enter Play Mode Options* and domain reload disabled, values and subscribers from the previous Play session will still be there **(assumed)**. Reset the values you care about at startup.
- **Main thread only.** Neither `Variables` nor `Value` is synchronised; use them from the Unity main thread **(assumed)**.
- **Serialized fields keep no value.** See section 3.4. Do not rely on Inspector values or field initializers.
- **The Variables window shows nothing in Edit Mode**, even if editor code has created variables.
- **Exact type matching in the window.** A variable registered as a subclass is listed under its own type, not under its base type.
- **Override registration timing.** With the sample's `RuntimeInitializeOnLoadMethod` pattern the custom row exists only after Play Mode starts.

### Known issues (in the current code)

1. **`ObservableBoolDrawer` targeted the wrong type (fixed in source).** It was declared with `[CustomPropertyDrawer(typeof(ObservableInt))]` instead of `ObservableBool`. Result: `ObservableBool` fields get no custom drawer (Unity's default drawing is used), and two drawers are registered for `ObservableInt`. If Unity picks `ObservableBoolDrawer` for an int field, it shows "Failed to get value" in Play Mode **(assumed, which drawer wins is not defined by the package)**. Fixed in source; not yet verified in Unity.
2. **Variables window rows leak subscriptions.** Each editable row (`ObservableVariableElementBase<T1, T2, T3>`) subscribes to the variable's `ValueChanged` in its constructor and unsubscribes only in a finalizer. Because the variable holds the delegate, the finalizer likely never runs, and the window rebuilds all rows about every 380 updates. Old rows probably stay in memory and keep handling events for the rest of the Play session **(assumed)**. Closing the window does not unsubscribe them; leaving Play Mode with a domain reload clears everything **(assumed)**. Custom rows derived from that base class have the same behaviour.
3. **`Value` setter equality check (fixed in source).** Unchanged values no longer raise events; see pitfalls above.
4. **The generic drawer attribute has no effect.** `ObservableVariableDrawer<T>` carries `[CustomPropertyDrawer(typeof(ObservableVariableBase<>))]` but is abstract, so custom types do not get a fallback drawer **(assumed)**. Write a drawer per type (section 6).
5. **Namespace inconsistency.** `ObservableBoolDrawer`, `ObservableFloatDrawer` and `ObservableIntDrawer` are in `ObservableVariables.Drawers.BasicTypes`; the other drawers are in `ObservableVariables.Editor.Drawers.Unity`. Use the actual namespaces if you reference these classes.
6. **Sample leftover.** The "Custom variable with drawer" sample's `ObservableVariableOverrideInstaller.Install` logs `123` to the Console on every Play Mode start.
7. **README API table.** It names the generic constraint `ObservableVariableType`; the real constraint is `ObservableVariableBase, new()`.

---

## 8. Working with other com.fixer33.* packages

This package has **no dependency** on any other `com.fixer33.*` package, and none of them is required to use it.

| Package | Git URL | Relation |
|---|---|---|
| `com.fixer33.package-installer` | `https://github.com/Fixer33/PackageInstaller.git` | Lists this package in its **Tools** group and can install it (section 1). Editor only; no runtime link. |
| `com.fixer33.easy-events` | `https://github.com/Fixer33/EasyEventsPackage.git` | Independent. Complements this package: use observable variables for shared **state** and easy-events for one-off **events** **(assumed)**. |
| `com.fixer33.utilities` | `https://github.com/Fixer33/UtilitiesPackage.git` | Independent. It ships its own, unrelated observable value type; the two are not interchangeable **(assumed)**. |

---

## 9. Remaining open questions

These points are still open (numbers match `Open Questions.md`) and affect how you use the package:

- **#2 Registry lifetime** — will a reset/remove API or automatic reset on entering Play Mode be added (relevant with domain reload disabled)?
- **#3 No way to remove a key** — is a session-long lifetime by design?
- **#4 Exception type** — will `InvalidEnumArgumentException` for type mismatches stay, or change to `InvalidOperationException`/`ArgumentException`? Catch it by its current type for now.
- **#5 Implicit conversion from raw values** — is it meant only for field initializers?
- **#6 Thread safety** — main-thread-only use is assumed, not confirmed.
- **#8 Drawers** — will Inspector values stay read-only and hidden in Edit Mode?
- **#10 No `ObservableString` drawer** — omission or intentional?
- **#11 Generic drawer attribute** — leftover, or meant as a fallback drawer for custom types?
- **#12 Drawer namespaces** — will the bool/float/int drawers move to `ObservableVariables.Editor.*` (breaking for code that references them)?
- **#13 Window row subscription leak** — acknowledgement and fix pending (listed as known issue 2).
- **#14 Refresh interval** — will it become configurable or event-driven?
- **#15 Edit-mode browsing** — will the window support Edit Mode?
- **#16 UXML stylesheet path** — the UXML references a wrong stylesheet path; the window loads the stylesheet separately, so styling works **(assumed)**.
- **#17 `PACKAGES_DEV`** — will it stay, be documented, or be replaced by a package-path lookup?
- **#18 Duplicate types across samples** — are the two "Custom variable" samples meant to be imported one at a time?
- **#20 Override registration** — is `RuntimeInitializeOnLoadMethod` in a runtime script the recommended pattern, or should overrides be registered from an editor assembly?
- **#21 "Using as a field" omits `ObservableString`** — intentional?
- **#25 `com.unity.test-framework` dependency** — will it stay a hard dependency?
- **#28 Minimum Unity version** — is `6000.0` a deliberate floor?
