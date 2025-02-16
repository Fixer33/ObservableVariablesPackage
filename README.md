
# Observable variables for Unity Engine

This custom Unity package is designed to provide a simple API to simplify sharing global variables throughout your project without creating infinite Singleton objects.

It also implements Observable pattern by sharing ValueChanged event to avoid constant reading the value from Update loop.


## Authors

- [@Fixer33](https://github.com/Fixer33)


## Installation

Install with upm

```bash
  1. Open Unity Package Manager (Window/Package Manager)
  2. Navigate to top-left corner of the window and press the plus (+) icon
  3. Choose "Install package from git URL..."
  4. Paste url: https://github.com/Fixer33/ObservableVariablesPackage.git
  5. Press "Install"
```

## Usage/Examples

There are 3 samples that are recommended to check before starting to work with the package.
To install these samples, you need to open "Samples" tab of the installed package in UPM.

![Samples Tab Screenshot](https://prnt.sc/gcCGhjbfNXgY)

## Running Tests

There are editor tests included using Unity's package "com.unity.test-framework"
To run these tests:

```bash
  1. Navigate to Window/General/Test Runner
  2. Run tests by any of two ways:
    2a. Press "Run All"
    2b. RMB on specific test and choose "Run"
```


## API Reference

#### Get global variable

```http
  Variables.Get<T>(Enum variableKey)
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `T` | `ObservableVariableType` | The type of a variable you want to recieve |
| `variableKey` | `Enum` | Any enum value that serves as a key for a variable |


