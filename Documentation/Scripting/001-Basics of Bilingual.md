# Basics of Bilingual
This file is aimed to give a brief overview of Bilingual's syntax. This guide assumes that you have basic knowledge of programming.

## Most Basic File
All Bilingual scripts are housed within a namespace-like wrapper called a container. This container can contain periods to separate different files or scenes.
The container is like a folder structure (and when a localization file is generated, it becomes the file structure for the script's translated content).
You can have more than one container in a file.

Containers can have zero (0) or more scripts. A script is written out similarly to a function but without access modifiers and a return type. 
The name should be followed by parenthesis and then a block of statements using curly braces.

Below is an example of a basic Bilingual file.

```cs
This.Is.My.Container 
{
    NewScript() 
    {

    }
}
```

## Basic Programming
Comments are C-like comments that start with `//`. Bilingual does not have support for multiline comments, so just prefix every line with the slashes.
Statements can only be written inside of scripts. Statements generally end with a semicolon if it doesn't contain its own block of statements. 

### Variables
Variables are created using the following syntax. Variables can be assigned to expressions that contain other variables and objects, but the types should match.
```cs
// Create a basic variable that stores a string
var myNewVar = "any value here";

// Create a global variable.
global var myGlobalVar = 5 + 5;

// Set wow to a boolean expression.
var wow = 80 < (5 * 2);
```
Global variables can be accessed from any scope and are accessible to the game. These are intended for use in save data or persistent information such as character name.
They can be set and created by the game's code and by the Bilingual script.

Numerical and boolean expressions will be folded when compiled. 
This means that arithmetic and boolean expressions that contain constants, like numbers or `true`/`false`, will be evaluated when compiled.
Expressions will be folded until there is a variable or non-constant value. 

Examples of expression folding:
- `5 + 4` will be folded into `9` instead of an addition expression.
- `(5 * 3) - 2` will be folded into `13` instead of several math expressions.
- `5 <= 0` will be folded into `false` instead of a boolean expression.
- `(5 * 3) / wow` only the `(5 * 3)` will be folded. The ending expression will be `15 / wow` because `wow` is a variable.
And so on.

Global and normal variables can be assigned a new value using the `=` operator. Types do not need to match. Compound operators are supported.
```cs
var test = 5.4; // value is 5.4
test = "wow";   // value now "wow"
test = 5 * 2;   // value now 10
test += 4;      // value now 14
test++;         // Value now 15
```

There are several basic types in Bilingual:
- Numbers: All numbers will be treated as a double-precision floating point number.
- Strings: A string of text wrapped in double quotes.
- Boolean: `true` and `false`.
- Array: `["values", 7, true]` Arrays can contain multiple types.

On a string, variable, and array you can access the items using an indexer.
```cs
var myArray = [1, 2, 3];
var firstItem = myArray[0];
var secondItem = myArray[3 - 2];

var firstLetter = "this is a string"[0];
```

### Dialogue
Dialogue statements are the heart of Bilingual. The game queries for the next line of dialogue and all statements are run until dialogue is found. 
Dialogue statements begin with the name of a character (must be one word, no spaces and it must begin with a letter) followed by a `:` then a string.
Interpolated strings are supported in dialogue statements only, they allow the integration of inline commands or variables into the dialogue string returned.
To interpolate a string, prefix the string with `$` and put inline content and expressions within curly braces. If you want a curly brace inside 
an interpolated string instead of an inline expression, you must escape it using a `\`.

```cs
Person: "This is dialogue!";
Wow: "Wow!";

var name = "Person";
// "My name is Person." is returned.
Person: $"My name is {name}.";
// "My name is Person, and I love the { character!" is returned.
Person: $"My name is {name}, and I love the \{ character!";
```

Dialogue statements can have line IDs and translation comments. Line IDs are unique to every line of dialogue. They are required when a translation file is created
and will be added. They start with a `#` and contain 8 digits. A translation comment can be added after a line ID. To create a translation comment, end the
line ID with a `:` and then type out a string. This string will be put in the translation file.

```cs
// Dialogue line with id.
Person: "Hello there.";             #00000001

// Dialogue line with line id and translation comment.
Person: "I am very sad.";           #00000002:"Very sad"
```

Inline commands and expressions can also be inserted into an interpolated string. Custom inline commands must return a string or `null`. 
The built-in `Wait(seconds)` command is the only command that will not return a value. It will be stripped from the dialogue.

```cs
// "Wait!!! Ok thanks." will be returned. A 5 second pause will happen after the exclamation marks.
Person: $"Wait!!! {Wait(5)}Ok thanks.";
```

To create plurals and ordinals, create an interpolated string and use `Ordinal` or `Plural` functions. (`Ord` and `Pl` are shorthands.)
You can specify "zero", "one", two", "few", "many", "more", or "other" strings to replace a value with.
The replacement strings will vary based on how the language specifies plurals and ordinals.

```cs
var apples = 2;
Person: $"I have {apples} {Plural(apples, one="apple", other="apples")}!"; // Returns: 'I have 2 apples!'
var one = 1;
Person: $"I have {one} {Plural(one, one="apple", other="apples")}!"; // Returns: 'I have 1 apple!'
```

When you use a `#` symbol inside a quantity string, it will replace the `#` with the value provided in the first parameter.
The following is equivalent to the example above.

```cs
var apples = 2;
Person: $"I have {Plural(apples, one="# apple", other="# apples")}!"; // Returns: 'I have 2 apples!'
var one = 1;
Person: $"I have {Plural(one, one="# apple", other="# apples")}!"; // Returns: 'I have 1 apple!'
```

To replace the number inserted by the `#` with a word, use two hashtags (`##`). This feature uses the Humanizer library and may not support all languages.
If a language is not supported, the number is used instead.

```cs
var apples = 2;
Person: $"I have {Plural(apples, one="## apple", other="## apples")}!"; // Returns: 'I have two apples!'
var one = 1;
Person: $"I have {Plural(one, one="## apple", other="## apples")}!"; // Returns: 'I have one apple!'
```

You can add a letter following the hashtag to symbolize the grammatical gender of the object to alter the gender of the word.
The letters can be "m" for masculine, "f" for feminine, and "n" for neuter. If the letter is capitalized, the number will be capitalized in title case (first letter in caps).
Adding the letter "a" after the gender letter will use the abbreviated version of that number's word. 
If a language or word does not support abbreviation or gender, they will be ignored.

```cs
var apples = 2;
Person: $"Tengo {Plural(apples, one="##f manzana", other="##f manzanas")}!"; // Returns: 'Tengo dos manzanas!'
var one = 1;
Person: $"Tengo {Plural(one, one="##f manzana", other="##f manzana")}!"; // Returns: 'Tengo una manzana!'
Person: $"Tengo {Pl(one, one="##ma lapiz", other="##m lapices")}!"; // Returns: 'Tengo un lapiz!'
```

For ordinals do the same. `##` will replace with the word version. Ordinals can also have gender.

```cs
var place = 2;
Person: $"I am {Ordinal(place, one="st", two="nd", few="rd", other="th")} place!"; // Returns: 'I am 2nd place!'
var one = 1;
Person: $"I am {Ordinal(one, one="st", two="nd", few="rd", other="th")} place!"; // Returns: 'I am 1st place!'
Person: $"I am {Ord(one, one="##", two="##", few="##", other="##")}! place"; // Returns: 'I am in first place!'
```

You can escape the `#` if you want to.

### Control Flow
There are several types of control flow in Bilingual.

If statements are written as following:
```cs
// If statement.
if (true) 
{
    Person: "This dialogue will always show.";
}
else 
{
    Person: "This dialogue will never show.";
}

// If, if else, and else.
if (4 * 3 == 1)
{
    Person: "This won't show.";
}
// You can have several else ifs.
else if (4 * 3 == 12)
{
    Person: "This will show.";
}
else 
{
    Person: "If all else fails, and it does here.";
}
```

While and do/while loops are written as follows:
```cs
while (true)
{
    Person: "Forever looping.";
}

do
{
    Person: "Say something, then loop.";
} while (true);
```

For and foreach loops are written like this:
```cs
foreach (item in [1, 2, 3])
{
    Person: $"{item}";

    if (item == 2)
    {
        // You can break.
        break;
    }
}

for (var i = 0; i <= 9; i++)
{
    if (i == 2)
    {
        // Skip 2.
        continue;
    }

    Person: $"{i}";
}
```
You can also use `break` and `continue` to get out of a loop or continue to the next iteration.

To end a script, use the `return` keyword. When run, it will end the script immediately regardless of how nested the command may be.
```cs
Test.Container
{
    TestScript()
    {
        Person: "Hi.";
        return; // end script.
        Person: "I will never run.";
    }
}
```

#### Choose Statement
The choose statement is a special type of control flow. It will prompt the player with a list of options to choose. The selected option's statements will be run.
You need at least two (2) choices for a valid choose statement.

```cs
choose "First choice."
{
    Person: "I chose the first choice.";
}
choose "Second choice."
{
    Person: "This is the second choice.";
}
```

### Run and Inject
You can run dialogue scripts from another script. To end the execution of a script and start a new one, use the `run` keyword followed by the 
path to the script. The path is the container + `.` + name of script. 

```cs
TestContainer.SubName
{
    TestScript()
    {
        Person: "Run a new script.";
        run TestContainer.SubName.SecondScript; // Jump to a new script.

        Person: "This dialogue will not run.";
    }

    SecondScript()
    {
        Person: "I am in a new place!";
    }
}
```

If you want to continue execution of a script, you can use the `inject` command. The `inject` will also give the injected script 
access to the local variables defined in the script that called `inject`.

```cs
TestContainer.SubName
{
    TestScript()
    {
        Person: "Run a new script.";
        var name = "Person";

        inject TestContainer.SubName.SecondScript; // Insert a set of commands.

        Person: "This dialogue WILL run, but only after SecondScript.";
    }

    // This script will be injected into TestScript. name comes from there.
    SecondScript()
    {
        Person: "I am in a new place!";
        Person: $"My name is {name}.";
    }
}
```

### Commands
You can call functions from Bilingual using commands. A command is written by writing out the full name then using parenthesis. You can pass in parameters
into the command. If the command is async, you will need to await it using `await` before the command. If you dont want to await, just leave out the `await`.

```cs
// Call the function called Character.Jump
Character.Jump("Person");

// Await.
await Character.JumpAsync("Person");
// or dont
Character.JumpAsync("Person");
```

You can pass in as many parameters as you like.

### Script Attributes
You can provide certain data to the game using script attributes. They are written out like C# attributes and go on top of a script.

```cs
TestContainer.SubName
{
    [MyAttribute(10, "wow", [8, 2, 2, 2])]
    TestScript()
    {
        Person: "Hi.";
    }
}
```

You can have more than one attribute.