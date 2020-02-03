using Mono.Cecil;

public partial class ModuleWeaver
{
    public TypeDefinition StringDefinition;
    public TypeDefinition StringComparisonDefinition;

    public void FindCoreReferences()
    {
        StringDefinition = FindTypeDefinition("System.String");
        StringComparisonDefinition = FindTypeDefinition("System.StringComparison");
    }
}