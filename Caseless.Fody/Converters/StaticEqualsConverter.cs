using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

public class StaticEqualsConverter : IEqualityConverter
{
    // if UseOperatorForOrdinal has value, it means we are using StringComparison.Ordinal
    public bool? UseOperatorForOrdinal { get; set; }
    MethodReference reference;
    public ModuleWeaver ModuleWeaver { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = ModuleWeaver.StringDefinition.Methods;
        if (UseOperatorForOrdinal.HasValue)
        {
            var method = methods.First(_ => _.Name == "op_Equality" &&
                                            _.Parameters.Matches("String", "String"));
            reference = ModuleDefinition.ImportReference(method);
        }
        else
        {
            var method = methods.First(_ => _.IsStatic && _.Name == "Equals" &&
                                            _.Parameters.Matches("String", "String", "StringComparison"));
            reference = ModuleDefinition.ImportReference(method);
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