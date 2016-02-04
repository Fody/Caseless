using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

public interface IConverter
{
    MsCoreReferenceFinder MsCoreReferenceFinder { set; }
    ModuleDefinition ModuleDefinition { get; set; }
    int StringComparisonConstant { get; set; }
    void Init();
    IEnumerable<Instruction> Convert(MethodReference method);
}
