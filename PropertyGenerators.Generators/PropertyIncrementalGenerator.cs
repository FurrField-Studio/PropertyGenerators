using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PropertyGenerators.Generators;

[Generator]
public class PropertyIncrementalGenerator : IIncrementalGenerator
{
    private static string[] _attributes =
    [
        "GenerateReadOnly",
        "GenerateEditorWritable",
        "GenerateEditorProperty"
    ];
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var fieldDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (node, token) =>
            {
                if (!(node is FieldDeclarationSyntax f)) return false;
                if (f.AttributeLists.Count == 0) return false;

                foreach (var attributeList in f.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (_attributes.Contains(attribute.Name.ToString()))
                        {
                            return true;
                        }
                    }
                }

                return false;
            },
            transform: static (ctx, token) => (FieldDeclarationSyntax)ctx.Node);
        
        var compilationAndFields = context.CompilationProvider.Combine(fieldDeclarations.Collect());
        
        context.RegisterSourceOutput(compilationAndFields, static (spc, source) =>
        {
            var (compilation, fields) = source;

            foreach (var fieldDeclaration in fields)
            {
                var semanticModel = compilation.GetSemanticModel(fieldDeclaration.SyntaxTree);
                foreach (var variable in fieldDeclaration.Declaration.Variables)
                {
                    IFieldSymbol? fieldSymbol = semanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
                    if (fieldSymbol == null)
                        continue;

                    var attributeNames = fieldSymbol.GetAttributes().Select(ad => ad.AttributeClass?.ToDisplayString());

                    bool containsGenerateReadOnlyProperty = false;
                    bool containsGenerateEditorWritableProperty = false;
                    bool containsGenerateEditorPropertyProperty = false;
                    
                    foreach (var an in attributeNames)
                    {
                        if (an == "PropertyGenerators.GenerateReadOnlyAttribute")
                        {
                            containsGenerateReadOnlyProperty = true;
                        }
                        
                        if (an == "PropertyGenerators.GenerateEditorWritableAttribute")
                        {
                            containsGenerateEditorWritableProperty = true;
                        }
                        
                        if (an == "PropertyGenerators.GenerateEditorPropertyAttribute")
                        {
                            containsGenerateEditorPropertyProperty = true;
                        }
                    }
                    
                    if (!containsGenerateReadOnlyProperty && !containsGenerateEditorWritableProperty && !containsGenerateEditorPropertyProperty)
                        continue;
                    
                    var sourceData = GenerateSource(fieldSymbol, containsGenerateReadOnlyProperty, containsGenerateEditorWritableProperty, containsGenerateEditorPropertyProperty);
                    
                    spc.AddSource(sourceData.hint, SourceText.From(sourceData.source, Encoding.UTF8));
                }
            }
        });
    }
    
    private static (string source, string hint) GenerateSource(IFieldSymbol fieldSymbol, 
        bool containsGenerateReadOnlyProperty,
        bool containsGenerateEditorWritableProperty,
        bool containsGenerateEditorPropertyProperty)
    {
        var containingType = fieldSymbol.ContainingType;
        var namespaceName = containingType.ContainingNamespace.IsGlobalNamespace ? "" : containingType.ContainingNamespace.ToDisplayString();
        var className = containingType.Name;
        var fieldName = fieldSymbol.Name;
        
        string genericParameters = "";
        if (containingType is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
        {
            genericParameters = "<" + string.Join(", ", namedTypeSymbol.TypeParameters.Select(tp => tp.Name)) + ">";
        }
        
        var propertyName = fieldName;
        if (propertyName[0] == '_')
        {
            propertyName = propertyName.Substring(1);
        }

        propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
        
        if (containsGenerateReadOnlyProperty && containsGenerateEditorPropertyProperty)
        {
            containsGenerateEditorWritableProperty = true;
            containsGenerateEditorPropertyProperty = false;
        }

        if (containsGenerateReadOnlyProperty && containsGenerateEditorWritableProperty)
        {
            var propertySource = $@"
namespace {namespaceName}
{{
    public partial class {className}{genericParameters}
    {{
        #if UNITY_EDITOR
        public {fieldSymbol.Type.ToDisplayString()} {propertyName}
        {{
            get => {fieldName};
            set => {fieldName} = value;
        }}
        #else
        public {fieldSymbol.Type.ToDisplayString()} {propertyName} => {fieldName};
        #endif
    }}
}}
";
            var hintName = $"{className}_{fieldName}_GenerateHybridProperty.g.cs";

            return (propertySource, hintName);
        } else if (containsGenerateEditorPropertyProperty)
        {
            var propertySource = $@"
#if UNITY_EDITOR
namespace {namespaceName}
{{
    public partial class {className}{genericParameters}
    {{
        public {fieldSymbol.Type.ToDisplayString()} {propertyName}
        {{
            get => {fieldName};
            set => {fieldName} = value;
        }}
    }}
}}
#endif
";
            var hintName = $"{className}_{fieldName}_GenerateEditorProperty.g.cs";
            
            return (propertySource, hintName);
        } else if (containsGenerateEditorWritableProperty)
        {
            var propertySource = $@"
#if UNITY_EDITOR
namespace {namespaceName}
{{
    public partial class {className}{genericParameters}
    {{
        public {fieldSymbol.Type.ToDisplayString()} {propertyName} {{ set => {fieldName} = value; }}
    }}
}}
#endif
";
            var hintName = $"{className}_{fieldName}_GenerateEditorWritableProperty.g.cs";
            
            return (propertySource, hintName);
        } else
        {
            var propertySource = $@"
namespace {namespaceName}
{{
    public partial class {className}{genericParameters}
    {{
        public {fieldSymbol.Type.ToDisplayString()} {propertyName} => {fieldName};
    }}
}}
";
            var hintName = $"{className}_{fieldName}_GenerateReadOnlyProperty.g.cs";
            
            return (propertySource, hintName);
        }
    }
}