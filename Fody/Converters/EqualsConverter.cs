using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class EqualsConverter:IConverter
{
    MethodReference reference;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        reference = ModuleDefinition.ImportReference(methods.First(x => x.Name == "Equals" && x.Parameters.Matches("String", "StringComparison")));
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (method.Name != "Equals")
        {
            yield break;
        }

        var parameters = method.Parameters;
        if (parameters.Matches("String"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Callvirt, reference); 
        }

    }
}