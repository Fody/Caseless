using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

public class OpNotEqualsConverter : IEqualityConverter
{
    // if UseOperatorForOrdinal has value, it means we are using StringComparison.Ordinal
    public bool? UseOperatorForOrdinal { get; set; }
    MethodReference reference;
    public ModuleWeaver ModuleWeaver { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        if (UseOperatorForOrdinal.HasValue)
        {
            return;
        }

        var method = ModuleWeaver
            .StringDefinition
            .Methods
            .First(_ => _.Name == "Equals" &&
                        _.Parameters.Matches("String", "String", "StringComparison"));
        reference = ModuleDefinition.ImportReference(method);
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (UseOperatorForOrdinal.HasValue)
        {
            yield break;
        }

        if (method.Name != "op_Inequality")
        {
            yield break;
        }

        if (!method.Parameters.Matches("String", "String"))
        {
            yield break;
        }

        yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
        yield return Instruction.Create(OpCodes.Call, reference);
        yield return Instruction.Create(OpCodes.Ldc_I4_0);
        yield return Instruction.Create(OpCodes.Ceq);
    }
}