using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class StaticEqualsConverter:IConverter
{
    MethodReference reference;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        reference = ModuleDefinition.Import(methods.First(x => x.IsStatic && x.Name == "Equals" && x.Parameters.Matches("String", "String", "StringComparison")));
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (method.Name != "Equals")
        {
            yield break;
        }

        var parameters = method.Parameters;
        if (parameters.Matches("String", "String"))
        {
            yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
            yield return Instruction.Create(OpCodes.Call, reference); 
        }

    }
}