# 📝 Rez

<!-- TOC -->
* [📝 Rez](#-rez)
  * [The Basics](#the-basics)
  * [Where did the name come from?](#where-did-the-name-come-from)
* [User Guide](#user-guide)
  * [Syntax](#syntax)
    * [Variables](#variables)
    * [Functions](#functions)
    * [Escaping](#escaping)
  * [Examples](#examples)
    * [Simple Example: Greeting](#simple-example-greeting)
    * [Example with a Function: Temperature Conversion](#example-with-a-function-temperature-conversion)
    * [Example with Nested Variables and Functions: Order Details](#example-with-nested-variables-and-functions-order-details)
* [Developer Guide](#developer-guide)
  * [Template Language Syntax](#template-language-syntax)
  * [Using Rez](#using-rez)
  * [Tips and Best Practices](#tips-and-best-practices)
  * [Sample](#sample)
* [Syntax guide](#syntax-guide)
  * [Plain text](#plain-text)
  * [Variables](#variables-1)
    * [Resolve a variable](#resolve-a-variable)
    * [Resolve nested variables](#resolve-nested-variables)
    * [When a variable is not found](#when-a-variable-is-not-found)
  * [Functions](#functions-1)
    * [Resolve a function](#resolve-a-function)
    * [Resolve a function with multiple parameters](#resolve-a-function-with-multiple-parameters)
    * [When a function is not found](#when-a-function-is-not-found)
  * [Escaping](#escaping-1)
* [Example data for this page](#example-data-for-this-page)
<!-- TOC -->

## The Basics

Rez consists of two main parts:

- Specification for a simple templating language
- A library to implement the parsing and resolving of Rez template text in .NET applications.

Rez takes heavy inspiration from [Mustache](https://mustache.github.io/), which is a popular templating language used in many web
applications.

One of the key reasons for creating Rez instead of using an existing solution was the need for terse, nestable tokens that are
still easy to read.

## Where did the name come from?

The name Rez is a play on the word "res" (short for "resolution"), which is the process of resolving a template. The name was chosen as
it is short and memorable and is not too similar to other names in the .NET ecosystem, or the domain of testing frameworks.

# User Guide

Rez is a powerful templating tool that allows you to create dynamic text with variables and functions.

## Syntax

### Variables

To insert a variable, use curly braces:

```
Input:
Hello {variableName}!

Output:
Hello World!

Variables:
variableName = World
```

Variables can also be nested and will be resolve inside-out, left-to-right:

```
Input:
Hello {{nested}}!

Output:
Hello World!

Variables:
nested = variableName
variableName = World
```

Variables can also be recursive:

> Note: There is a hard limit of 4096 recursions in a single template to prevent infinite loops

```
Input:
Hello {variableName1}!

Output:
Hello World!

Variables:
variableName1 = {variableName2}
variableName2 = World
```

### Functions

To use a function, use curly braces with parentheses:

```
Input:
Hello {greeting()}!

Output:
Hello World!

Functions:
greeting() => World
```

Some functions can also support parameters, and can be nested:

```
Input:
{greeting({name})}!

Output:
Hello World!

Functions:
greeting(name) => {hi} name!
name => World
hi => Hello
```

### Escaping

To include braces without being resolved, use a backslash:

```
Input:
Hello \{world\}!

Output:
Hello {world}!

Variables:
world = World
```

If writing templates inside .json files, you will need to escape the backslashes as well

```json
{
  "myTemplate": "\\{escapedVariable\\}"
}
```

## Examples

### Simple Example: Greeting

Template:

```
Hello, {name}! Welcome to our platform.
```

Variables:

```json
{
  "name": "John"
}
```

Resolved text:

```
Hello, John! Welcome to our platform.
```

### Example with a Function: Temperature Conversion

Template:

```
The temperature is {temperature} degrees Fahrenheit ({fahrenheitToCelsius({temperature})} degrees Celsius).
```

Variables:

```json
{
  "temperature": "68"
}
```

Functions:

```
fahrenheitToCelsius(fahrenheit) => (fahrenheit - 32) * 5 / 9
```

Resolved text:

```
The temperature is 68 degrees Fahrenheit (20 degrees Celsius).
```

### Example with Nested Variables and Functions: Order Details

Template:

```
Dear {customer:name},

Thank you for your order of {product:name} ({product:sku}). Your order number is {order:number}.

The total cost of your order is {calculateTotal({order:price},{order:tax})}.

Best regards,
{company:name}
```

Variables:

```json
{
  "customer": {
    "name": "Jane"
  },
  "product": {
    "name": "Wireless Headphones",
    "sku": "WH-123"
  },
  "order": {
    "number": "ORD-456",
    "price": "100",
    "tax": "10"
  },
  "company": {
    "name": "Electronics Store"
  }
}
```

Functions:

```
calculateTotal(price,tax) => price + tax
```

Resolved text:

```
Dear Jane,

Thank you for your order of Wireless Headphones (WH-123). Your order number is ORD-456.

The total cost of your order is $110.

Best regards,
Electronics Store
```

# Developer Guide

Rez allows you to resolve variables and functions embedded within strings, making it easier to dynamically generate text based on the given
variables and functions. This guide will help you understand how to use Rez and its template language to create powerful and flexible text
templates.

## Template Language Syntax

In Rez's template language, variables and functions are enclosed within curly braces:

- Variables: `{variableName}`
- Functions: `{functionName()}` or `{functionName(parameter)}`

To include the delimiters within the text without being resolved, you can use escape characters:

- Escaped variable: `\{escapedVariable\}`
- Escaped function: `\{escapedFunction(parameter)\}`

## Using Rez

To use Rez to resolve the variables and functions in your JSON template, you'll need to:

1. Instantiate an implementation of `IResolver` (e.g. `Resolver`).
2. Add sources for variable and function resolution, in form of `IResolverSource` implementations.
3. Call the `Resolve()` method on the `IResolver` instance, passing in the text to be resolved.

Here are some examples of how to set up and use Rez in C#

The easiest way is to directly call Resolver.Resolve() with some variables

```csharp
var resolver = new Resolver();
resolver.AddSource(new ResolverSource(new Dictionary<string, string> { { "name", "World" } }));
var input = "Hello, {name}!";

var greeting = resolver.Resolve(input);

// greeting: "Hello, World!"
```

## Tips and Best Practices

- Be mindful of the order in which sources are added to Rez. Rez will search the sources in the order they were added. If a variable or
  function is found in multiple sources, Rez will use the first one it encounters.
- Make use of custom `IResolverSource` implementations to provide additional variables and functions or to override existing ones. This can
  be useful when you want to extend the template language or provide domain-specific functionality.
- When creating your text templates, ensure that variables and functions are enclosed within curly braces and use escape characters when
  necessary.

By following these guidelines, you'll be able to effectively use Rez to create dynamic text templates that can be easily maintained and
updated.

## Sample

Let's say we have a console application that reads a configuration from a JSON file and uses that configuration to populate variables.

The text to resolve will come from user input.

> appsettings.json

```json
{
  "animal1": "fox",
  "animal2": "dog",
  "color": "brown",
  "description": "lazy"
}
```

> App code

```csharp
// Read the configuration from the JSON file
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Create a Resolver instance
var resolver = new Resolver();

// Add the config as a source for variable resolution
resolver.AddSource(new ConfigResolverSource(config));

// Get user input
Console.WriteLine("Enter a sentence:");
string input = Console.ReadLine();

// Resolve the text from the JSON template
string output = resolver.Resolve(input);

// Output the resolved text
Console.WriteLine(output);
```

If we run the application and enter the following text:

> The quick {color} {animal1} jumped over the {description} {animal2}.

The output will be:

> The quick brown fox jumped over the lazy dog.

# Syntax guide

## Plain text

Any plain text that doesn’t get resolved into variables or functions will always resolve as itself:

```
in  > here are some words
out > here are some words
```

## Variables

### Resolve a variable

Insert a variable using curly braces:

```
in  > {variable1}
out > variableValue1
```

### Resolve nested variables

Variables can be nested and will always be resolved from the innermost to the outermost, left to right:

```
in  > {variable{{number2}}
--- > {variable2}
out > variableValue2

in  > {variable1} {variable2} {variable3}
--- > variableValue1 {variable2} {variable3}
--- > variableValue1 variableValue2 {variable3}
out > variableValue1 variableValue2 variableValue3
```

### When a variable is not found

When a variable is not found, it will resolve as the input, including the double curly braces:

```
in  > {variable4}
out > {variable4}
```

## Functions

### Resolve a function

Call a function with curly braces and parentheses:

```
in  > {date()}
out > 2023-04-05
```

Functions can also accept parameters:

```
in  > {fancyFunction(ooh!)}
out > ***ooh!***
```

### Resolve a function with multiple parameters

Call a function with multiple parameters by separating them with commas:

```
in  > wo{repeatFunction(lo,2)}
out > wololo
```

Note - Whitespace is NOT ignored in function parameters, and is treated as part of the parameter:

> This usually doesn't matter for parameters that are numbers, but it's good to not get into the habit of adding whitespace to function
> parameters, as is the case with many programming languages.

```
in  > {andFunction(apple,banana)}
out > apple&banana

in  > {andFunction(apple, banana)}
out > apple& banana
```

### When a function is not found

When a function is not found, it will resolve as the input, including the double curly braces and parentheses:

```
in  > {notAFunction(ooh!)}
out > {notAFunction(ooh!)}
```

## Escaping

Use a backslash to include braces without being resolved:

```
in  > \{{variable1\}
out > {variable1}
```

This will persist through nested resolves:

```
in  > {fancyFunction(\{variable1\})}
--- > ***\{variable1\}***
out > ***{variable1}***
```

Note—This doesn't persist if the output is used as input for another resolve:

> This isn't usually something that a user writing a template would need to deal with, but if you experience unexpected behavior, contact
> the developer in charge of the project that you're writing a templates for.

```
(text is passed into 1st resolver)

in  > \{variable1\}
out > {variable1}

(out is passed into 2nd resolver)

in  > {variable1}
out > variableValue1
```

# Example data for this page

Variables used in examples:

```
variable1: variableValue1
variable2: variableValue2
variable3: variableValue3
number1:   1
number2:   2
number3:   3
```

Functions used in examples:

```
fancyFunction(input):        ***input***
andFunction(input1,input2):  input1&input2
repeatFunction(input,count): functionOutput2=[repeat input count times]
```