using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;

public static class CecilExtensions
{

    public static List<TypeDefinition> GetAllTypeDefinitions(this ModuleDefinition moduleDefinition)
    {
        var definitions = new List<TypeDefinition>();
        //First is always module so we will skip that;
        GetTypes(moduleDefinition.Types.Skip(1), definitions);
        return definitions;
    }
    public static bool Matches(this Collection<ParameterDefinition> parameters, params string[] matches)
    {
        if(parameters.Count!= matches.Length)
        {
            return false;
        }
        for (var index = 0; index < parameters.Count; index++)
        {
            var definition = parameters[index];
            var paramName = matches[index];
            if (definition.ParameterType.Name != paramName)
            {
                return false;
            }
        }
        return true;
    }

    static void GetTypes(IEnumerable<TypeDefinition> typeDefinitions, List<TypeDefinition> definitions)
    {
        foreach (var type in typeDefinitions)
        {
            GetTypes(type.NestedTypes, definitions);
            if (type.IsInterface)
            {
                continue;
            }
            if (type.IsEnum)
            {
                continue;
            }
            definitions.Add(type);
        }
    }
}