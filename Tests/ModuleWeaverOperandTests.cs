using System;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;

public class ModuleWeaverOperandTests
{
    dynamic targetClass;

    public ModuleWeaverOperandTests()
    {
        var weavingTask = new ModuleWeaver();

        var testResult = weavingTask.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll",
            beforeExecuteCallback: AddConditionalBranchLong,
            assemblyName: $"{nameof(ModuleWeaverOperandTests)}AssemblyToProcess");
        var type = testResult.Assembly.GetType("TargetClass", true);
        targetClass = Activator.CreateInstance(type);
    }

    static void AddConditionalBranchLong(ModuleDefinition module)
    {
        var type = module.Types.Single(t => t.Name == "TargetClass");
        var method = new MethodDefinition("ConditionalBranchLong", MethodAttributes.Public, module.TypeSystem.Boolean);
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
}