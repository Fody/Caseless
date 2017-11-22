using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class ModuleWeaverOperatorTests
{
    dynamic targetClass;
    string afterAssemblyPath;
    string beforeAssemblyPath;

    public ModuleWeaverOperatorTests()
    {
        beforeAssemblyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "AssemblyToProcess.dll");
        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "4.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        using (var assemblyResolver = new MockAssemblyResolver())
        {
            var readerParameters = new ReaderParameters
            {
                AssemblyResolver = assemblyResolver
            };

            using (var moduleDefinition = ModuleDefinition.ReadModule(beforeAssemblyPath, readerParameters))
            {
                var weavingTask = new ModuleWeaver
                {
                    Config = XElement.Parse(@"<Caseless StringComparison=""operator""/>"),
                    ModuleDefinition = moduleDefinition,
                };

                weavingTask.Execute();
                moduleDefinition.Assembly.Name.Name += "ForOperator";
                moduleDefinition.Write(afterAssemblyPath);
            }
            var assembly = Assembly.LoadFrom(afterAssemblyPath);
            var type = assembly.GetType("TargetClass", true);
            targetClass = Activator.CreateInstance(type);
        }
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
        Assert.IsFalse(targetClass.EqualsCallOnNull());
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

    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
}