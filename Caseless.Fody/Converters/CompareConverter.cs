using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

public class CompareConverter : IConverter
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
            .First(_ => _.Name == "Compare" &&
                        _.Parameters.Matches("String", "String", "StringComparison"));
        reference = ModuleDefinition.ImportReference(method);
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (method.Name == "Compare" && method.Parameters.Matches("String", "String"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Call, reference);
        }

        if (method.Name == "CompareTo" && method.Parameters.Matches("String"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Call, reference);
        }
    }
}