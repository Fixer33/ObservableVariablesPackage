# Observable Variables — Open Questions

Points that could not be settled from the package contents. Each item states what was found and why it is ambiguous. See `Package Description.md` for the full description. Original numbering is kept; answered items are listed in the "Answered" section at the end.

## Runtime behaviour

2. **Registry lifetime.** The `Variables` dictionary is static and only cleared by `Tests_Clear()` (tests only). With Enter Play Mode Options and domain reload disabled, variables and their subscribers would survive between Play sessions. Is a public reset/remove API (or an automatic reset on entering Play Mode) wanted?
3. **No way to remove a key.** There is no `Remove`/`Clear` outside tests. Is that by design (variables live for the whole session)?
4. **Exception type.** A key reused with a different variable type throws `InvalidEnumArgumentException`. This type normally means "invalid enum value"; is it chosen deliberately (tests rely on it) or would `InvalidOperationException`/`ArgumentException` be preferred?
5. **Implicit conversion from raw value creates a new instance.** `ObservableInt x = 5;` replaces the variable reference, silently dropping subscribers if done on a variable obtained from `Variables`. Is this conversion meant only for field initializers (as in the "Using as a field" sample)?
6. **Thread safety.** Neither `Variables` nor `Value` is synchronised. Assumed main-thread only; confirm.

## Serialization and Inspector

8. **Drawers are read-only and show nothing in Edit Mode.** Intended (runtime-only inspection), or should values be editable in the Inspector?
10. **No `ObservableString` drawer.** Every other shipped type has one. Omission or intentional?
11. **Generic drawer attribute.** `ObservableVariableDrawer<T>` is abstract but has `[CustomPropertyDrawer(typeof(ObservableVariableBase<>))]`, which cannot take effect. Leftover, or meant to provide a fallback drawer for custom types?
12. **Namespace inconsistency.** Bool/float/int drawers are in `ObservableVariables.Drawers.BasicTypes`; all other editor code is under `ObservableVariables.Editor.*`. Rename planned? (Changing it is breaking only for code that references these classes directly.)

## Variables window

13. **Subscription leak.** Editable rows unsubscribe from `ValueChanged` only in a finalizer, and the window rebuilds all rows every 380 `Update` calls. Since the variable references the row via the delegate, old rows are probably never collected and keep handling events. Known issue?
14. **Refresh interval.** The 380-frame refresh is hard-coded. Should it be configurable or event-driven (e.g. on new key registration)?
15. **Edit-mode browsing.** The window refuses to show anything outside Play Mode. Variables can exist in Edit Mode (any editor code can call `Variables.Get`). Intended limitation?
16. **UXML stylesheet path.** `VariablesWindow.uxml` references `Assets/Packages/com.fixer33.observable-variables/Editor/Markup%20and%20stylesheets/VariablesWindow.uss` (no `GUI/` folder, and an `Assets/` path that only exists when developing the package embedded). It probably resolves by GUID and the window also adds the USS manually. Should the UXML reference be fixed or removed?
17. **`PACKAGES_DEV` define.** The editor code switches asset paths to `Assets/Packages/...` when `PACKAGES_DEV` is defined. This is an undocumented convention for developing the package under `Assets`. Should it be documented in the README, or replaced by a path lookup (e.g. via `PackageInfo`)?

## Samples

18. **Duplicate types across samples.** "Custom variable without drawer" and "Custom variable with drawer" both declare `Sample.CustomData`, `Sample.ObservableCustomData`, `Sample.ExampleKeys`, `Sample.ExampleBehaviour1/2` without asmdefs. Importing both likely causes compile errors. Intended to be imported one at a time?
19. **Leftover debug log.** `ObservableVariableOverrideInstaller.Install` logs `"123"`. Remove?
20. **Override installed from runtime code.** The "with drawer" sample registers the editor override from a `#if UNITY_EDITOR` block in a runtime script via `RuntimeInitializeOnLoadMethod`, so it is only active after entering Play Mode. Is that the recommended pattern, or should overrides be registered from an editor assembly (e.g. `[InitializeOnLoad]`)?
21. **"Using as a field" omits `ObservableString`.** Intentional?

## Tests

22. **Empty runtime test assembly.** `Tests/Runtime` has only an asmdef. Planned tests, or can it be removed?
23. **Ineffective assertion.** `VariablesDictionary.CreateNewValueVariableAndSetValue` uses `Assert.NotNull(variable.Value == targetVal)`, which always passes. Should be `Assert.IsTrue` / `Assert.AreEqual`?
24. **Coverage gaps.** No tests for `ObservableBehaviour`, `ObservableTransform`, the editor factory/overrides, or the type-mismatch case with shortcuts. Wanted?

## Packaging and documentation

25. **`com.unity.test-framework` as a hard dependency.** Declared in `package.json` (1.4.6) although only test assemblies need it. Keep (so tests run out of the box) or drop?
26. **CHANGELOG inconsistency.** 1.0.1 says "Initial implementation of variable retrieval shortcut methods" while 1.2.0 lists the same shortcut methods as added. Which is accurate?
27. **README generic parameter type.** The README API table lists `T` as `ObservableVariableType`; the code requires `ObservableVariableBase, new()`. Update the README?
28. **Minimum Unity version.** `package.json` says `6000.0`; the code uses C# features (default interface members, records) that also work on 2021.2+ **(assumed)**. Is 6000.0 a deliberate floor?

## Answered

1. **No equality check in `Value` setter.** The setter should skip `ValueChanged` and the event-holder callbacks when the value is unchanged. Fixed in source; not yet verified in Unity.
7. **Is the value serialized?** No, by design. `_value` stays private and is not serialized. As a result, field initializers such as `[SerializeField] ObservableInt _observableInt = 3;` in the "Using as a field" sample are not stored as serialized data (assumed).
9. **`ObservableBoolDrawer` targets `ObservableInt`.** Yes, it is a typo. The attribute now targets `ObservableBool` (fixed in source; not yet verified in Unity).
