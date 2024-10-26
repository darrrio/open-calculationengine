# OpenCalculationEngine

[![.NET](https://github.com/darrrio/open-calculationengine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/darrrio/open-calculationengine/actions/workflows/dotnet.yml)

## Description

OpenBilling is a .NET project designed to handle customizable billing calculation.

The operations are defined with an excel-like language.

## Installation

```bash
git clone https://github.com/darrrio/open-calculationengine.git
cd open-calculationengine
dotnet restore
dotnet build
```
## Usage

### Syntax for `expression` and `inputFile`

The `CalculateExpression` method in the `EngineController` class requires two parameters: `expression` and `inputFile`.

#### `expression` Parameter
- **Type**: `string`
- **Description**: The formula or expression to be evaluated.
- **Format**: The expression should be a string that can include operators and functions recognized by the `ExpressionNode` class.
- **Examples**:
    - `"$OP.SUM(1;2;3)"`
    - `"$OP.MULTIPLY(4;5)"`
    - `"$FN.TARIFF()"`
    - `More to be added...`

#### `inputFile` Parameter
- **Type**: `Hashtable`
- **Description**: A hashtable containing the base data used for the calculation.
- **Format**: The input file should be a JSON object that can be serialized into a `JsonObject`. The keys should be strings, and the values can be nested JSON objects, arrays, or primitive types (e.g., string, number).
- **Example**:
  ```json
  {
    "key1": "value1",
    "key2": {
      "nestedKey1": "nestedValue1",
      "nestedKey2": 123
    },
    "key3": [
      {
        "arrayKey1": "arrayValue1"
      },
      {
        "arrayKey2": "arrayValue2"
      }
    ]
  }
    ```
### Available Operators and Functions
#### Operators
- **Addition**: `$OP.SUM`
- **Multiplication**: `$OP.MULTIPLY`
- **Division**: `$OP.DIVIDE`

#### Functions
- **GetTariff()**: `$FN.TARIFF`

### Naming Convention in JSON Object and Expression
- **Format**: The keys in the JSON object should be named according to the following convention:
- **Operators**: The key should be named `$OP.<OPERATOR_NAME>`.
- **Functions**: The key should be named `$FN.<FUNCTION_NAME>`.
- **Operators with Parameters**: Can be used only 2 parameters. The parameters should be separated by a semicolon (`;`).
- **Functions with Parameters**: Can be used only 1 or 2 parameters. The parameters should be separated by a semicolon (`;`) even with only 1 parameter. eg. `$FN.TARIFF($PRM.PARAM1;)` or `$FN.TARIFF($PRM.PARAM1;$PRM.PARAM2;)`

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
