using System;
using System.Xml.Linq;
using Fody;
using Xunit;
#pragma warning disable 618

public class ModuleWeaverOperatorTests
{
    dynamic targetClass;

    public ModuleWeaverOperatorTests()
    {
        var weavingTask = new ModuleWeaver
        {
            Config = XElement.Parse(@"<Caseless StringComparison=""operator""/>"),
        };

        var testResult = weavingTask.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll",
            assemblyName: $"{nameof(ModuleWeaverOperatorTests)}AssemblyToProcess");
        var type = testResult.Assembly.GetType("TargetClass", true);
        targetClass = Activator.CreateInstance(type);
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
        Assert.False(targetClass.EqualsCallOnNull());
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