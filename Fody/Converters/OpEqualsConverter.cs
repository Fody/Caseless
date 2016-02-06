using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class OpEqualsConverter : IEqualityConverter
{
    // if UseOperatorForOrdinal has value, it means we are using StringComparison.Ordinal
    public bool? UseOperatorForOrdinal { get; set; }
    MethodReference reference;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        if (UseOperatorForOrdinal.HasValue)
        {
            return;
        }

        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        reference = ModuleDefinition.ImportReference(methods.First(x => x.Name == "Equals" && x.Parameters.Matches("String", "String", "StringComparison")));
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (UseOperatorForOrdinal.HasValue)
        {
            yield break;
        }

        if (method.Name != "op_Equality")
        {
            yield break;
        }

        if (!method.Parameters.Matches("String", "String"))
        {
            yield break;
        }

        yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
        yield return Instruction.Create(OpCodes.Call, reference);
    }
}
