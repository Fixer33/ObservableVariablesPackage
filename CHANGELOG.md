## [1.2.1] - 2026-11-20

### Changed
- Variables window restyled; the type dropdown replaces the arrows.
- Variable value events are no longer raised when the assigned value is unchanged.

### Fixed
- `ObservableBool` property drawer used the wrong target.

### Documentation
- Added Package Description, Usage Guide and Open Questions documents, linked from the README.

## [1.2.0] - 2026-03-24

### Added
- `IObservableVariableEventHolder`: Interface for objects to receive events when they are stored in or removed from an `ObservableVariable`.
- Shortcut methods in the `Variables` class for common types: `GetFloat`, `GetInt`, `GetBool`, `GetString`, `GetGameObject`, `GetBehaviour`, `GetTransform`, `GetVector2`, and `GetVector3`.

## [1.0.1] - 2025-02-17

### Added
- Initial implementation of variable retrieval shortcut methods.

## [1.0.0] - 2025-02-13

### Added
- Initial release of the Observable Variables package.