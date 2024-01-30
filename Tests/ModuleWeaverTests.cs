using System;
using Fody;
using Xunit;

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

    [Fact]
    public void CompareTo()
    {
        Assert.Equal(0, targetClass.CompareTo());
    }

    [Fact]
    public void CompareStatic()
    {
        Assert.Equal(0, targetClass.CompareStatic());
    }

    [Fact]
    public void CompareStaticWithNull()
    {
        Assert.Equal(-1, targetClass.CompareStaticWithNull());
    }

    [Fact]
    public void Contains()
    {
        Assert.True(targetClass.Contains());
    }

    [Fact]
    public void IndexOf()
    {
        Assert.Equal(0, targetClass.IndexOf());
    }

    [Fact]
    public void IndexOf_StartIndex()
    {
        Assert.Equal(1, targetClass.IndexOf_StartIndex());
    }

    [Fact]
    public void IndexOf_StartIndexCount()
    {
        Assert.Equal(1, targetClass.IndexOf_StartIndexCount());
    }

    [Fact]
    public void LastIndexOf()
    {
        Assert.Equal(0, targetClass.LastIndexOf());
    }

    [Fact]
    public void OpEquals()
    {
        Assert.True(targetClass.OpEquals());
    }

    [Fact]
    public void OpEqualsWithNull()
    {
        Assert.False(targetClass.OpEqualsWithNull());
    }

    [Fact]
    public void OpNotEquals()
    {
        Assert.False(targetClass.OpNotEquals());
    }

    [Fact]
    public void OpNotEqualsWithNull()
    {
        Assert.True(targetClass.OpNotEqualsWithNull());
    }

    [Fact]
    public void StartsWith()
    {
        Assert.True(targetClass.StartsWith());
    }

    [Fact]
    public void EndsWith()
    {
        Assert.True(targetClass.EndsWith());
    }

    [Fact]
    public void Equal()
    {
        Assert.True(targetClass.Equals());
    }

    [Fact]
    public void EqualsCallOnNull()
    {
        Assert.Throws<NullReferenceException>(() => targetClass.EqualsCallOnNull());
    }

    [Fact]
    public void EqualsStatic()
    {
        Assert.True(targetClass.EqualsStatic());
    }

    [Fact]
    public void EqualsStaticWithNull()
    {
        Assert.False(targetClass.EqualsStaticWithNull());
    }

    [Fact]
    public void Conditional()
    {
        Assert.True(targetClass.ConditionalBranch());
    }
}