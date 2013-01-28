using System.Reflection;
using NUnit.Framework;

public abstract class BaseTaskTests
{
    string beforeAssemblyPath;
    Assembly assembly;
    dynamic targetClass;
    string afterAssemblyPath;

    protected BaseTaskTests(string beforeAssemblyPath)
    {
        this.beforeAssemblyPath = beforeAssemblyPath;
#if (!DEBUG)
        this.beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
        afterAssemblyPath = WeaverHelper.Weave(this.beforeAssemblyPath);
        assembly = Assembly.LoadFrom(afterAssemblyPath);
        targetClass = assembly.GetInstance("TargetClass");
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