using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;
using Xunit.Abstractions;

public class ModuleWeaverOperandTests :
    XunitLoggingBase
{
    static dynamic targetClass;

    static ModuleWeaverOperandTests()
    {
        var moduleWeaver = new ModuleWeaver();

        var testResult = moduleWeaver.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll",
            beforeExecuteCallback: definition => AddConditionalBranchLong(definition, moduleWeaver),
            assemblyName: $"{nameof(ModuleWeaverOperandTests)}AssemblyToProcess");
        targetClass = testResult.GetInstance("TargetClass");
    }

    static void AddConditionalBranchLong(ModuleDefinition module, ModuleWeaver moduleWeaver)
    {
        var type = module.Types.Single(t => t.Name == "TargetClass");
        var method = new MethodDefinition("ConditionalBranchLong", MethodAttributes.Public, moduleWeaver.TypeSystem.BooleanReference);
        var body = method.Body;
        body.InitLocals = true;
        var il = body.GetILProcessor();

        // This is the key: a call which will be replaced, targeted by a branch
        var call = il.Create(OpCodes.Callvirt, module.ImportReference(typeof(string).GetMethod("StartsWith", new[] {typeof(string)})));
        var branch = il.Create(OpCodes.Brtrue, call);

        il.Append(il.Create(OpCodes.Ldstr, "foo"));
        il.Append(il.Create(OpCodes.Ldstr, "F"));
        il.Append(il.Create(OpCodes.Dup));
        il.Append(branch);
        il.Append(il.Create(OpCodes.Pop));
        il.Append(il.Create(OpCodes.Ldstr, "G"));
        il.Append(call);
        il.Append(il.Create(OpCodes.Ret));

        type.Methods.Add(method);
    }

    [Fact]
    public void Conditional()
    {
        Assert.True(targetClass.ConditionalBranchLong());
    }

    public ModuleWeaverOperandTests(ITestOutputHelper output) :
        base(output)
    {
    }
}