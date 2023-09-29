using System;
using System.Collections.Generic;
using Mono.Cecil;

public class ConverterCache
{
    public ModuleWeaver ModuleWeaver;
    public ModuleDefinition ModuleDefinition;
    public DefaultStringComparisonFinder DefaultStringComparisonFinder;
    public void Execute()
    {
        Converters = new();
        foreach (var type in ConverterTypes.Types)
        {
            var converter = (IConverter) Activator.CreateInstance(type);
            converter.ModuleWeaver = ModuleWeaver;
            converter.ModuleDefinition = ModuleDefinition;
            converter.StringComparisonConstant = DefaultStringComparisonFinder.StringComparisonConstant;
            if (converter is IEqualityConverter equality)
            {
                equality.UseOperatorForOrdinal = DefaultStringComparisonFinder.UseOperatorForOrdinal;
            }
            converter.Init();
            Converters.Add(converter);
        }
    }

    public List<IConverter> Converters;
}