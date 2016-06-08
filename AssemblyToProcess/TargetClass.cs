public class TargetClass
{
    public int CompareTo()
    {
        var x = "a";
        var y = "A";
        return x.CompareTo(y);
    }

    public int CompareStatic()
    {
        var x = "a";
        var y = "A";
        return string.Compare(x, y);
    }

    public int CompareStaticWithNull()
    {
        string x = null;
        var y = "A";
        return string.Compare(x, y);
    }

    public bool EndsWith()
    {
        var x = "a";
        var y = "A";
        return x.EndsWith(y);
    }

    public bool Contains()
    {
        var x = "a";
        var y = "A";
        return x.Contains(y);
    }


    public bool Equals()
    {
        var x = "a";
        var y = "A";
        return x.Equals(y);
    }

    public bool EqualsCallOnNull()
    {
        string x = null;
        var y = "A";
        return x.Equals(y);
    }

    public bool EqualsStatic()
    {
        var x = "a";
        var y = "A";
        return string.Equals(x,y);
    }

    public bool EqualsStaticWithNull()
    {
        string x = null;
        var y = "A";
        return string.Equals(x,y);
    }

    public int IndexOf()
    {
        var x = "a";
        var y = "A";
        return x.IndexOf(y);
    }
    public int IndexOf_StartIndex()
    {
        var x = "da";
        var y = "A";
        return x.IndexOf(y,1);
    }
    public int IndexOf_StartIndexCount()
    {
        var x = "da";
        var y = "A";
        return x.IndexOf(y,1,1);
    }

    public int LastIndexOf()
    {
        var x = "a";
        var y = "A";
        return x.LastIndexOf(y);
    }

    public string Replace()
    {
        var x = "a";
        var y = "A";
        return x.Replace(y, "b");
    }

    public bool StartsWith()
    {
        var x = "a";
        var y = "A";
        return x.StartsWith(y);
    }

    public bool OpEquals()
    {
        var x = "a";
        var y = "A";
        return x == y;
    }

    public bool OpEqualsWithNull()
    {
        string x = null;
        var y = "A";
        return x == y;
    }

    public bool OpNotEquals()
    {
        var x = "a";
        var y = "A";
        return x != y;
    }

    public bool OpNotEqualsWithNull()
    {
        string x = null;
        var y = "A";
        return x != y;
    }

    public bool ConditionalBranch()
    {
        string x = "foo";
        string y = "F";
        string z = "G";
        return x.StartsWith (y ?? z);
    }
}