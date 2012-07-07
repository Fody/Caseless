using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class EndsWithConverter:IConverter
{
    MethodReference reference;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        reference = ModuleDefinition.Import(methods.First(x => x.Name == "EndsWith" && x.Parameters.Matches("String", "StringComparison")));
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
       if (method.Name != "EndsWith")
       {
           yield break;
        }
        
        if (!method.Parameters.Matches("String"))
        {
            yield break;
        }

        yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
        yield return Instruction.Create(OpCodes.Callvirt,  reference); 
    }
}