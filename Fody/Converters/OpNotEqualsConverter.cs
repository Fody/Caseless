using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class OpNotEqualsConverter : IConverter
{
    bool isOrdinal;
    MethodReference reference;
    public MsCoreReferenceFinder MsCoreReferenceFinder { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public int StringComparisonConstant { get; set; }

    public void Init()
    {
        isOrdinal = ((int)MsCoreReferenceFinder.StringComparisonDefinition
            .Fields.Single(f => f.Name == "Ordinal").Constant) == StringComparisonConstant;

        if (isOrdinal)
        {
            return;
        }

        var methods = MsCoreReferenceFinder.StringDefinition.Methods;
        reference = ModuleDefinition.ImportReference(methods.First(x => x.Name == "Equals" && x.Parameters.Matches("String", "String", "StringComparison")));
    }

    public IEnumerable<Instruction> Convert(MethodReference method)
    {
        if (isOrdinal)
        {
            yield break;
        }

        if (method.Name != "op_Inequality")
        {
            yield break;
        }

        if (!method.Parameters.Matches("String", "String"))
        {
            yield break;
        }

        yield return Instruction.Create(OpCodes.Ldc_I4, StringComparisonConstant);
        yield return Instruction.Create(OpCodes.Call, reference); 
        yield return Instruction.Create(OpCodes.Ldc_I4_0); 
        yield return Instruction.Create(OpCodes.Ceq); 
    }
}