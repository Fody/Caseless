using System;
using System.Xml.Linq;
using Fody;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

public class ModuleWeaverOrdinalTests
{
    static dynamic targetClass;

    static ModuleWeaverOrdinalTests()
    {
        var weaver = new ModuleWeaver
        {
            Config = XElement.Parse("""<Caseless StringComparison="ordinal"/>"""),
        };

        var testResult = weaver.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll",
            assemblyName: $"{nameof(ModuleWeaverOrdinalTests)}AssemblyToProcess");
        targetClass = testResult.GetInstance("TargetClass");
    }

    [Test]
    public async Task OpEquals()
    {
        await Assert.That((bool)targetClass.OpEquals()).IsFalse();
    }

    [Test]
    public async Task OpEqualsWithNull()
    {
        await Assert.That((bool)targetClass.OpEqualsWithNull()).IsFalse();
    }

    [Test]
    public async Task OpNotEquals()
    {
        await Assert.That((bool)targetClass.OpNotEquals()).IsTrue();
    }

    [Test]
    public async Task OpNotEqualsWithNull()
    {
        await Assert.That((bool)targetClass.OpNotEqualsWithNull()).IsTrue();
    }

    [Test]
    public async Task Equal()
    {
        await Assert.That((bool)targetClass.Equals()).IsFalse();
    }

    [Test]
    public async Task EqualsCallOnNull()
    {
        Action action = () => targetClass.EqualsCallOnNull();
        await Assert.That(action).Throws<NullReferenceException>();
    }

    [Test]
    public async Task EqualsStatic()
    {
        await Assert.That((bool)targetClass.EqualsStatic()).IsFalse();
    }

    [Test]
    public async Task EqualsStaticWithNull()
    {
        await Assert.That((bool)targetClass.EqualsStaticWithNull()).IsFalse();
    }
}