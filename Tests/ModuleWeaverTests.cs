using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class ModuleWeaverTests
{
    string beforeAssemblyPath;
    Assembly assembly;
    dynamic targetClass;
    string afterAssemblyPath;

    public ModuleWeaverTests()
    {
        beforeAssemblyPath = @"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll";
#if (!DEBUG)
       beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(afterAssemblyPath);

        var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
            };

        weavingTask.Execute();
        moduleDefinition.Write(afterAssemblyPath);
        assembly = Assembly.LoadFrom(afterAssemblyPath);
        var type = assembly.GetType("TargetClass", true);
        targetClass = Activator.CreateInstance(type);
    }


    [Test]
    public void CompareTo()
    {
        Assert.AreEqual(0,targetClass.CompareTo());
    }
    [Test]
    public void CompareStatic()
    {
        Assert.AreEqual(0,targetClass.CompareStatic());
    }

    [Test]
    public void Contains()
    {
        Assert.IsTrue(targetClass.Contains());
    }
    [Test]
    public void IndexOf()
    {
        Assert.AreEqual(0,targetClass.IndexOf());
    }
    [Test]
    public void IndexOf_StartIndex()
    {
        Assert.AreEqual(1, targetClass.IndexOf_StartIndex());
    }
    [Test]
    public void IndexOf_StartIndexCount()
    {
        Assert.AreEqual(1, targetClass.IndexOf_StartIndexCount());
    }

    [Test]
    public void LastIndexOf()
    {
        Assert.AreEqual(0,targetClass.LastIndexOf());
    }

    [Test]
    public void OpEquals()
    {
        Assert.IsTrue(targetClass.OpEquals());
    }
    [Test]
    public void OpNotEquals()
    {
        Assert.IsFalse(targetClass.OpNotEquals());
    }
    [Test]
    public void StartsWith()
    {
        Assert.IsTrue(targetClass.StartsWith());
    }
    [Test]
    public void EndsWith()
    {
        Assert.IsTrue(targetClass.EndsWith());
    }
    [Test]
    public void Equals()
    {
        Assert.IsTrue(targetClass.Equals());
    }
    [Test]
    public void EqualsStatic()
    {
        Assert.IsTrue(targetClass.EqualsStatic());
    }


#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
#endif

}