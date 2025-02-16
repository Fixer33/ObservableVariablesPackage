
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

To get a variable by the key use static method Get<T> of a class "Variables" passing the type of the variable and any enum key that you want to be associated with this variable.

You can see all global variables that exists at the time in editor window by opening it "Tools/Fixer33/Variables window". Here you can change viewed type of the variable by pressing buttons "<" and ">". Note that you can change the values of the variables in this window.

If you have created your own variable type, you will only see the current value. To be able to change it from the window, you need to check the sample "Custom variable with drawer".

There are 3 samples that are recommended to check before starting to work with the package.
To install these samples, you need to open "Samples" tab of the installed package in UPM.

![SamplesTab](https://github.com/user-attachments/assets/4a8eaac3-8075-404e-8170-322db8b7bd08)


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


