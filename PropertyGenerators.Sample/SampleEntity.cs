// #define UNITY_EDITOR

using PropertyGenerators;
using PropertyGenerators.Sample1;

namespace PropertyGenerators.Sample1
{
    public struct TestStruct1
    {
        public string str;
    }
}

namespace PropertyGenerators.Sample
{
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
    
    public partial class SampleEntity2
    {
        [GenerateReadOnly]
        private TestStruct1 _id;

        [GenerateEditorProperty]
        private TestStruct1? _name;

        [GenerateEditorWritable]
        private TestStruct1? _nameAsd;

        [GenerateReadOnly] [GenerateEditorProperty]
        private TestStruct1? _nameAsdf;
    }
}

