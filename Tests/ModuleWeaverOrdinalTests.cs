using System;
using System.Xml.Linq;
using Xunit;
using Fody;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

public class ModuleWeaverOrdinalTests
{
    static dynamic targetClass;

    static ModuleWeaverOrdinalTests()
    {
        var weavingTask = new ModuleWeaver
        {
            Config = XElement.Parse(@"<Caseless StringComparison=""ordinal""/>"),
        };

        var testResult = weavingTask.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll",
            assemblyName: $"{nameof(ModuleWeaverOrdinalTests)}AssemblyToProcess");
        targetClass = testResult.GetInstance("TargetClass");
    }

    [Fact]
    public void OpEquals()
    {
        Assert.False(targetClass.OpEquals());
    }

    [Fact]
    public void OpEqualsWithNull()
    {
        Assert.False(targetClass.OpEqualsWithNull());
    }

    [Fact]
    public void OpNotEquals()
    {
        Assert.True(targetClass.OpNotEquals());
    }

    [Fact]
    public void OpNotEqualsWithNull()
    {
        Assert.True(targetClass.OpNotEqualsWithNull());
    }

    [Fact]
    public void Equal()
    {
        Assert.False(targetClass.Equals());
    }

    [Fact]
    public void EqualsCallOnNull()
    {
        Assert.Throws<NullReferenceException>(() => targetClass.EqualsCallOnNull());
    }

    [Fact]
    public void EqualsStatic()
    {
        Assert.False(targetClass.EqualsStatic());
    }

    [Fact]
    public void EqualsStaticWithNull()
    {
        Assert.False(targetClass.EqualsStaticWithNull());
    }
}