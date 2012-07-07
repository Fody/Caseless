using System.Linq;
using Mono.Cecil;

public class MsCoreReferenceFinder
{
   public IAssemblyResolver AssemblyResolver;
    public TypeDefinition StringDefinition;
    public TypeDefinition StringComparisonDefinition;


    public void Execute()
    {
        var msCoreLibDefinition = AssemblyResolver.Resolve("mscorlib");
        var msCoreTypes = msCoreLibDefinition.MainModule.Types;

        var objectDefinition = msCoreTypes.FirstOrDefault(x => x.Name == "Object");
        if (objectDefinition == null)
        {
            ExecuteWinRT();
            return;
        }
        StringDefinition = msCoreTypes.First(x => x.Name == "String");
        StringComparisonDefinition = msCoreTypes.First(x => x.Name == "StringComparison");
    }



    public void ExecuteWinRT()
    {
        var systemRuntime = AssemblyResolver.Resolve("System.Runtime");
        StringDefinition = systemRuntime
            .MainModule
            .Types
            .First(x => x.Name == "String");
    }

}