using System;

namespace PropertyGenerators
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateReadOnlyAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateEditorWritableAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateEditorPropertyAttribute : Attribute
    {
    }
}