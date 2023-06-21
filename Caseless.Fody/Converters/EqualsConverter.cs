using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

public class EqualsConverter : IEqualityConverter
{
    // only if UseOperatorForOrdinal is true, then it means we are forcing a.Equals(b) being converted into a == b
    public bool? UseOperatorForOrdinal { get; set; }
    MethodReference reference;
    public ModuleWeaver ModuleWeaver { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = ModuleWeaver.StringDefinition.Methods;
        if (UseOperatorForOrdinal.GetValueOrDefault())
        {
            var method = methods.First(_ => _.Name == "op_Equality" &&
                                            _.Parameters.Matches("String", "String"));
            reference = ModuleDefinition.ImportReference(method);
        }
        else
        {
            var method = methods.First(_ => _.Name == "Equals" &&
                                            _.Parameters.Matches("String", "StringComparison"));
            reference = ModuleDefinition.ImportReference(method);
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

        if (UseOperatorForOrdinal.GetValueOrDefault())
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