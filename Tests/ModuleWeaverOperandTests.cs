using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;

[TestFixture]
public class ModuleWeaverOperandTests
{
    dynamic targetClass;

    public ModuleWeaverOperandTests()
    {
        var beforeAssemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\AssemblyToProcess\bin\Debug\net452\AssemblyToProcess.dll"));
#if (!DEBUG)
        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
        var afterAssemblyPath = typeof(ModuleWeaverOperandTests).Name + ".dll";

        var moduleDefinition = ModuleDefinition.ReadModule(beforeAssemblyPath);
        AddConditionalBranchLong(moduleDefinition, moduleDefinition.Types.Single(t => t.Name == "TargetClass"));

        // Offset assignment happens on write
        // Having offsets assigned prior to weaving makes tracking down bugs easier
        moduleDefinition.Write(afterAssemblyPath);

        var weavingTask = new ModuleWeaver
        {
            ModuleDefinition = moduleDefinition,
        };
        weavingTask.Execute();

        moduleDefinition.Assembly.Name.Name += "ForOperand";
        moduleDefinition.Write(afterAssemblyPath);
        var assembly = Assembly.LoadFrom(afterAssemblyPath);
        var type = assembly.GetType("TargetClass", true);
        targetClass = Activator.CreateInstance(type);
    }

    static void AddConditionalBranchLong(ModuleDefinition module, TypeDefinition type)
    {
        var method = new MethodDefinition("ConditionalBranchLong", Mono.Cecil.MethodAttributes.Public, module.TypeSystem.Boolean);
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

    [Test]
    public void Conditional()
    {
        Assert.IsTrue(targetClass.ConditionalBranchLong());
    }
}