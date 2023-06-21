using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

public class StartsWithConverter : IConverter
{
    MethodReference reference;
    public ModuleWeaver ModuleWeaver { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var method = ModuleWeaver
            .StringDefinition
            .Methods
            .First(_ => _.Name == "StartsWith" &&
                        _.Parameters.Matches("String", "StringComparison"));
        reference = ModuleDefinition.ImportReference(method);
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (method.Name != "StartsWith")
        {
            yield break;
        }

        if (!method.Parameters.Matches("String"))
        {
            yield break;
        }

        yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
        yield return Instruction.Create(OpCodes.Callvirt, reference);
    }
}