using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class IndexOfConverter:IConverter
{
    MethodReference reference_String;
    MethodReference reference_StringInt;
    MethodReference reference_StringIntInt;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        reference_String = ModuleDefinition.Import(methods.First(x => x.Name == "IndexOf" && x.Parameters.Matches("String", "StringComparison")));
        reference_StringInt = ModuleDefinition.Import(methods.First(x => x.Name == "IndexOf" && x.Parameters.Matches("String","Int32", "StringComparison")));
        reference_StringIntInt = ModuleDefinition.Import(methods.First(x => x.Name == "IndexOf" && x.Parameters.Matches("String","Int32", "Int32", "StringComparison")));
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
            yield return Instruction.Create(OpCodes.Callvirt, reference_String); 
            yield break;
        }
        if (method.Parameters.Matches("String","Int32"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Callvirt, reference_StringInt); 
            yield break;
        }
        if (method.Parameters.Matches("String", "Int32", "Int32"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Callvirt, reference_StringIntInt);
        }
    }
}