// #define UNITY_EDITOR

using PropertyGenerators;

namespace PropertyGenerators.Sample;

// This code will not compile until you build the project with the Source Generators

public partial class SampleEntity
{
    [GenerateReadOnly]
    private int _id;
    
    [GenerateEditorProperty]
    private string? _name = "Sample";
    
    [GenerateEditorWritable]
    private string? _nameAsd = "Asd";
    
    [GenerateReadOnly] [GenerateEditorWritable]
    private string? _nameAsdf = "Asdf";
}

public partial class SampleTEntity<T>
{
    [GenerateReadOnly]
    private int _id;
    
    [GenerateEditorProperty]
    private string? _name = "Sample";
    
    [GenerateEditorWritable]
    private string? _nameAsd = "Asd";
    
    [GenerateReadOnly] [GenerateEditorProperty]
    private string? _nameAsdf = "Asdf";
}