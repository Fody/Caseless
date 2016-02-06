using System;

public class TemplateClassForOrdinal
{

    public int CompareTo()
    {
        var x = "a";
        var y = "A";
        return string.Compare(x, y, StringComparison.Ordinal);
    }

    public int CompareStatic()
    {
        var x = "a";
        var y = "A";
        return string.Compare(x, y, StringComparison.Ordinal);
    }


    public bool EndsWith()
    {
        var x = "a";
        var y = "A";
        return x.EndsWith(y, StringComparison.Ordinal);
    }

    public bool Contains()
    {
        var x = "a";
        var y = "A";
        return x.IndexOf(y, StringComparison.Ordinal) >= 0;
    }

    public bool Equals()
    {
        var x = "a";
        var y = "A";
        return x.Equals(y, StringComparison.Ordinal);
    }

    public bool EqualsStatic()
    {
        var x = "a";
        var y = "A";
        return x == y;
    }

    public int IndexOf_StartIndex()
    {
        var x = "da";
        var y = "A";
        return x.IndexOf(y, 1, StringComparison.Ordinal);
    }

    public int IndexOf_StartIndexCount()
    {
        var x = "da";
        var y = "A";
        return x.IndexOf(y, 1, 1, StringComparison.Ordinal);
    }

    public int IndexOf()
    {
        var x = "a";
        var y = "A";
        return x.IndexOf(y, StringComparison.Ordinal);
    }

    public int LastIndexOf()
    {
        var x = "a";
        var y = "A";
        return x.LastIndexOf(y, StringComparison.Ordinal);
    }

    public string Replace()
    {
        var x = "a";
        var y = "A";
        return  x.Replace(y, "b");
    }

    public bool StartsWith()
    {
        var x = "a";
        var y = "A";
        return x.StartsWith(y, StringComparison.Ordinal);
    }
    
    public bool OpEquals()
    {
        var x = "a";
        var y = "A";
        return x == y;
    }

    public bool OpNotEquals()
    {
        var x = "a";
        var y = "A";
        return x != y;
    }
}