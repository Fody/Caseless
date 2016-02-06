using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class StaticEqualsConverter : IEqualityConverter
{
    // if UseOperatorForOrdinal has value, it means we are using StringComparsion.Ordinal
    public bool? UseOperatorForOrdinal { get; set; }
    MethodReference reference;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        if (UseOperatorForOrdinal.HasValue)
        {
            reference = ModuleDefinition.ImportReference(methods.First(x => x.Name == "op_Equality" && x.Parameters.Matches("String", "String")));
        }
        else
        {
            reference = ModuleDefinition.ImportReference(methods.First(x => x.IsStatic && x.Name == "Equals" && x.Parameters.Matches("String", "String", "StringComparison")));
        }
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (method.Name != "Equals")
        {
            yield break;
        }

        if (!method.Parameters.Matches("String", "String"))
        {
            yield break;
        }

        if (!UseOperatorForOrdinal.HasValue)
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
        }
        yield return Instruction.Create(OpCodes.Call, reference);
    }
}
