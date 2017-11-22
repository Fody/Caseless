using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public TypeDefinition StringDefinition;
    public TypeDefinition StringComparisonDefinition;

    public void FindCoreReferences()
    {
        var coreTypes = new List<TypeDefinition>();
        AddAssemblyIfExists("mscorlib", coreTypes);
        AddAssemblyIfExists("System", coreTypes);
        AddAssemblyIfExists("System.Runtime", coreTypes);
        AddAssemblyIfExists("System.Core", coreTypes);
        AddAssemblyIfExists("netstandard", coreTypes);

        StringDefinition = coreTypes.First(x => x.Name == "String");
        StringComparisonDefinition = coreTypes.First(x => x.Name == "StringComparison");
    }

    void AddAssemblyIfExists(string name, List<TypeDefinition> types)
    {
        try
        {
            var msCoreLibDefinition = ModuleDefinition.AssemblyResolver.Resolve(new AssemblyNameReference(name, null));

            if (msCoreLibDefinition != null)
            {
                types.AddRange(msCoreLibDefinition.MainModule.Types);
            }
        }
        catch (AssemblyResolutionException)
        {
            LogInfo($"Failed to resolve '{name}'. So skipping its types.");
        }
    }
}