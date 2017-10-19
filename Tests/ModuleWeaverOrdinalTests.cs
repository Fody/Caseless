using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class ModuleWeaverOrdinalTests
{
    dynamic targetClass;
    string afterAssemblyPath;
    string beforeAssemblyPath;

    public ModuleWeaverOrdinalTests()
    {
        beforeAssemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\AssemblyToProcess\bin\Debug\net452\AssemblyToProcess.dll"));
#if (!DEBUG)
       beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
#if (!DEBUG)
        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "3.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        using (var moduleDefinition = ModuleDefinition.ReadModule(beforeAssemblyPath))
        {
            var weavingTask = new ModuleWeaver
            {
                Config = XElement.Parse(@"<Caseless StringComparison="" ordinal ""/>"),
                ModuleDefinition = moduleDefinition,
            };

            weavingTask.Execute();
            moduleDefinition.Assembly.Name.Name += "ForOrdinal";
            moduleDefinition.Write(afterAssemblyPath);
        }
        var assembly = Assembly.LoadFrom(afterAssemblyPath);
        var type = assembly.GetType("TargetClass", true);
        targetClass = Activator.CreateInstance(type);
    }

    [Test]
    public void OpEquals()
    {
        Assert.IsFalse(targetClass.OpEquals());
    }

    [Test]
    public void OpEqualsWithNull()
    {
        Assert.IsFalse(targetClass.OpEqualsWithNull());
    }

    [Test]
    public void OpNotEquals()
    {
        Assert.IsTrue(targetClass.OpNotEquals());
    }

    [Test]
    public void OpNotEqualsWithNull()
    {
        Assert.IsTrue(targetClass.OpNotEqualsWithNull());
    }

    [Test]
    public void Equals()
    {
        Assert.IsFalse(targetClass.Equals());
    }

    [Test]
    public void EqualsCallOnNull()
    {
     Assert.Throws<NullReferenceException>(() => targetClass.EqualsCallOnNull());
    }

    [Test]
    public void EqualsStatic()
    {
        Assert.IsFalse(targetClass.EqualsStatic());
    }

    [Test]
    public void EqualsStaticWithNull()
    {
        Assert.IsFalse(targetClass.EqualsStaticWithNull());
    }


#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
#endif

}