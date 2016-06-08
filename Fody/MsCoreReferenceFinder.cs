using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public class MsCoreReferenceFinder
{
    public IAssemblyResolver AssemblyResolver;
    public TypeDefinition StringDefinition;
    public TypeDefinition StringComparisonDefinition;

    public void Execute()
    {
        var coreTypes = new List<TypeDefinition>();
        AppendTypes("mscorlib", coreTypes);
        if (null == Type.GetType("Mono.Runtime"))
        {
            AppendTypes("System.Runtime", coreTypes);
        }

        StringDefinition = coreTypes.First(x => x.Name == "String");
        StringComparisonDefinition = coreTypes.First(x => x.Name == "StringComparison");
    }

    void AppendTypes(string name, List<TypeDefinition> coreTypes)
    {
        var definition = AssemblyResolver.Resolve(name);
        if (definition != null)
        {
            coreTypes.AddRange(definition.MainModule.Types);
        }
    }

}