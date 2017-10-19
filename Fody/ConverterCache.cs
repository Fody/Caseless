using System;
using System.Collections.Generic;
using Mono.Cecil;

public class ConverterCache
{
    public MsCoreReferenceFinder MsCoreReferenceFinder;
    public ModuleDefinition ModuleDefinition;
    public DefaultStringComparisonFinder DefaultStringComparisonFinder;
    public void Execute()
    {
        Converters = new List<IConverter>();
        foreach (var type in ConverterTypes.Types)
        {
            var converter = (IConverter) Activator.CreateInstance(type);
            converter.MsCoreReferenceFinder = MsCoreReferenceFinder;
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