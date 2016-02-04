using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class EqualsConverter : IConverter
{
    bool isOrdinal;
    MethodReference reference;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        isOrdinal = ((int)MsCoreReferenceFinder.StringComparisonDefinition
           .Fields.Single(f => f.Name == "Ordinal").Constant) == StringComparisonConstant;

        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        if (isOrdinal)
        {
            reference = ModuleDefinition.ImportReference(methods.First(x => x.Name == "op_Equality" && x.Parameters.Matches("String", "String")));
        }
        else
        {
            reference = ModuleDefinition.ImportReference(methods.First(x => x.Name == "Equals" && x.Parameters.Matches("String", "StringComparison")));
        }
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (method.Name != "Equals")
        {
            yield break;
        }

        if (!method.Parameters.Matches("String"))
        {
            yield break;
        }

        if (isOrdinal)
        {
            yield return Instruction.Create(OpCodes.Call, reference);
        }
        else
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Callvirt, reference);
        }
    }
}