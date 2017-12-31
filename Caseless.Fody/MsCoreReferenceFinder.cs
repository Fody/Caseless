using Mono.Cecil;

public partial class ModuleWeaver
{
    public TypeDefinition StringDefinition;
    public TypeDefinition StringComparisonDefinition;

    public void FindCoreReferences()
    {
        StringDefinition = FindType("System.String");
        StringComparisonDefinition = FindType("System.StringComparison");
    }
}