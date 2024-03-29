using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

public class IndexOfConverter : IConverter
{
    MethodReference referenceString;
    MethodReference referenceStringInt;
    MethodReference referenceStringIntInt;
    public ModuleWeaver ModuleWeaver { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = ModuleWeaver.StringDefinition.Methods;
        referenceString = ModuleDefinition.ImportReference(
            methods.First(_ => _.Name == "IndexOf" &&
                               _.Parameters.Matches("String", "StringComparison")));
        referenceStringInt = ModuleDefinition.ImportReference(
            methods.First(_ => _.Name == "IndexOf" &&
                               _.Parameters.Matches("String", "Int32", "StringComparison")));
        referenceStringIntInt = ModuleDefinition.ImportReference(
            methods.First(_ => _.Name == "IndexOf" &&
                               _.Parameters.Matches("String", "Int32", "Int32", "StringComparison")));
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (method.Name != "IndexOf")
        {
            yield break;
        }

        if (method.Parameters.Matches("String"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Callvirt, referenceString);
            yield break;
        }

        if (method.Parameters.Matches("String", "Int32"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Callvirt, referenceStringInt);
            yield break;
        }

        if (method.Parameters.Matches("String", "Int32", "Int32"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Callvirt, referenceStringIntInt);
        }
    }
}