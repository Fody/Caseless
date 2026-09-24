using System;
using Fody;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

public class ModuleWeaverTests
{
    static dynamic targetClass;

    static ModuleWeaverTests()
    {
        var weaver = new ModuleWeaver();

        var testResult = weaver.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll",
            assemblyName: $"{nameof(ModuleWeaverTests)}AssemblyToProcess");
        targetClass = testResult.GetInstance("TargetClass");
    }

    [Test]
    public async Task CompareTo()
    {
        await Assert.That((int)targetClass.CompareTo()).IsEqualTo(0);
    }

    [Test]
    public async Task CompareStatic()
    {
        await Assert.That((int)targetClass.CompareStatic()).IsEqualTo(0);
    }

    [Test]
    public async Task CompareStaticWithNull()
    {
        await Assert.That((int)targetClass.CompareStaticWithNull()).IsEqualTo(-1);
    }

    [Test]
    public async Task Contains()
    {
        await Assert.That((bool)targetClass.Contains()).IsTrue();
    }

    [Test]
    public async Task IndexOf()
    {
        await Assert.That((int)targetClass.IndexOf()).IsEqualTo(0);
    }

    [Test]
    public async Task IndexOf_StartIndex()
    {
        await Assert.That((int)targetClass.IndexOf_StartIndex()).IsEqualTo(1);
    }

    [Test]
    public async Task IndexOf_StartIndexCount()
    {
        await Assert.That((int)targetClass.IndexOf_StartIndexCount()).IsEqualTo(1);
    }

    [Test]
    public async Task LastIndexOf()
    {
        await Assert.That((int)targetClass.LastIndexOf()).IsEqualTo(0);
    }

    [Test]
    public async Task OpEquals()
    {
        await Assert.That((bool)targetClass.OpEquals()).IsTrue();
    }

    [Test]
    public async Task OpEqualsWithNull()
    {
        await Assert.That((bool)targetClass.OpEqualsWithNull()).IsFalse();
    }

    [Test]
    public async Task OpNotEquals()
    {
        await Assert.That((bool)targetClass.OpNotEquals()).IsFalse();
    }

    [Test]
    public async Task OpNotEqualsWithNull()
    {
        await Assert.That((bool)targetClass.OpNotEqualsWithNull()).IsTrue();
    }

    [Test]
    public async Task StartsWith()
    {
        await Assert.That((bool)targetClass.StartsWith()).IsTrue();
    }

    [Test]
    public async Task EndsWith()
    {
        await Assert.That((bool)targetClass.EndsWith()).IsTrue();
    }

    [Test]
    public async Task Equal()
    {
        await Assert.That((bool)targetClass.Equals()).IsTrue();
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
        await Assert.That((bool)targetClass.EqualsStatic()).IsTrue();
    }

    [Test]
    public async Task EqualsStaticWithNull()
    {
        await Assert.That((bool)targetClass.EqualsStaticWithNull()).IsFalse();
    }

    [Test]
    public async Task Conditional()
    {
        await Assert.That((bool)targetClass.ConditionalBranch()).IsTrue();
    }
}