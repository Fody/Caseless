using Mono.Cecil;
using Mono.Collections.Generic;

public static class CecilExtensions
{
    public static bool Matches(this Collection<ParameterDefinition> parameters, params string[] matches)
    {
        if (parameters.Count != matches.Length)
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
}