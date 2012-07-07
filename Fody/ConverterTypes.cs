using System    ;
using System.Collections.Generic;
using System.Linq;

public static class ConverterTypes
{
    static ConverterTypes()
    {
        Types = typeof(ConverterTypes)
            .Assembly
            .GetTypes()
            .Where(IsConverter)
            .ToList();
    }

    static bool IsConverter(Type x)
    {
        return x.IsClass && typeof(IConverter).IsAssignableFrom(x);
    }

    public static List<Type> Types;
}