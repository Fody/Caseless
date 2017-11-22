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
        AssemblyDefinition msCoreLibDefinition;
        try
        {
            msCoreLibDefinition = ModuleDefinition.AssemblyResolver.Resolve(new AssemblyNameReference(name, null));
        }
        catch (AssemblyResolutionException)
        {
            LogInfo($"Failed to resolve '{name}'. So skipping its types.");
            return;
        }
        if (msCoreLibDefinition == null)
        {
            return;
        }
        var module = msCoreLibDefinition.MainModule;
        types.AddRange(ResolveExportedTypes(module));
        types.AddRange(module.Types);
    }

    static IEnumerable<TypeDefinition> ResolveExportedTypes(ModuleDefinition module)
    {
        return module.ExportedTypes
            .Select(exportedType => exportedType.Resolve())
            .Where(typeDefinition => typeDefinition != null);
    }
}