using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class IndexOfConverter:IConverter
{
    MethodReference referenceString;
    MethodReference referenceStringInt;
    MethodReference referenceStringIntInt;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        referenceString = ModuleDefinition.ImportReference(methods.First(x => x.Name == "IndexOf" && x.Parameters.Matches("String", "StringComparison")));
        referenceStringInt = ModuleDefinition.ImportReference(methods.First(x => x.Name == "IndexOf" && x.Parameters.Matches("String", "Int32", "StringComparison")));
        referenceStringIntInt = ModuleDefinition.ImportReference(methods.First(x => x.Name == "IndexOf" && x.Parameters.Matches("String", "Int32", "Int32", "StringComparison")));
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
        if (method.Parameters.Matches("String","Int32"))
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