# Property Generators

Automatically generate public properties for your private fields in Unity, saving you boilerplate code and ensuring consistency between runtime and editor behaviors.

# Table of Contents
- [Installation](#installation)
- [Attributes](#attributes)
    - [GenerateReadOnly](#generatereadonly)
	- [GenerateEditorWritable](#generateeditorwritable)
	- [GenerateEditorProperty](#generateeditorproperty)
    - [Hybrid properties](#hybrid-properties)

# Installation

- Download ``PropertyGenerators.Attributes.dll`` and ``PropertyGenerators.Generators.dll`` from Releases.
- Create ``Plugins`` folder and ``Editor`` folder inside plugins.
- Place ``PropertyGenerators.Attributes.dll`` into ``Plugins`` folder and ``PropertyGenerators.Generators.dll`` into ``Editor`` folder.
- In the ``PropertyGenerators.Generators.dll`` import settings, untick everything and add asset label named ``RoslynAnalyzer``.

# Attributes

Using any attribute requires your class to be partial.

## GenerateReadOnly

Generates getter for specific variable.

```csharp
[GenerateReadOnly]
private int _name;
```

will generate:

```csharp
public int Name => _name;
```

## GenerateEditorWritable

Generates editor-only setter for specific variable.

```csharp
[GenerateEditorProperty]
private string _name;
```

will generate:

```csharp
#if UNITY_EDITOR
public string Name { set => _name = value; }
#endif
```

## GenerateEditorProperty

Generates editor-only property for specific variable.

```csharp
[GenerateEditorProperty]
private string _name;
```

will generate:

```csharp
#if UNITY_EDITOR
public string Name
{
    get => _name;
    set => _name = value;
}
#endif
```

## Hybrid properties

Generates editor property and runtime only getter for specific variable.

```csharp
[GenerateReadOnly] [GenerateEditorWritable]
private string _name;

//or

[GenerateReadOnly] [GenerateEditorProperty]
private string _name;
```

will generate:

```csharp
#if UNITY_EDITOR
public string Name
{
    get => _name;
    set => _name = value;
}
#else
public string Name => _name;
#endif
```
