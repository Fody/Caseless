using System;

public class TemplateClass
{
    public int CompareTo()
    {
        var x = "a";
        var y = "A";
        return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
    }

    public int CompareStatic()
    {
        var x = "a";
        var y = "A";
        return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
    }

    public bool EndsWith()
    {
        var x = "a";
        var y = "A";
        return x.EndsWith(y, StringComparison.OrdinalIgnoreCase);
    }

    public bool Contains()
    {
        var x = "a";
        var y = "A";
        return x.IndexOf(y, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public bool Equals()
    {
        var x = "a";
        var y = "A";
        return x.Equals(y, StringComparison.OrdinalIgnoreCase);
    }

    public bool EqualsStatic()
    {
        var x = "a";
        var y = "A";
        return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
    }

    public int IndexOf_StartIndex()
    {
        var x = "da";
        var y = "A";
        return x.IndexOf(y, 1, StringComparison.OrdinalIgnoreCase);
    }

    public int IndexOf_StartIndexCount()
    {
        var x = "da";
        var y = "A";
        return x.IndexOf(y, 1, 1, StringComparison.OrdinalIgnoreCase);
    }

    public int IndexOf()
    {
        var x = "a";
        var y = "A";
        return x.IndexOf(y, StringComparison.OrdinalIgnoreCase);
    }

    public int LastIndexOf()
    {
        var x = "a";
        var y = "A";
        return x.LastIndexOf(y, StringComparison.OrdinalIgnoreCase);
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
        return x.StartsWith(y, StringComparison.OrdinalIgnoreCase);
    }

    public bool OpEquals()
    {
        var x = "a";
        var y = "A";
        return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
    }

    public bool OpNotEquals()
    {
        var x = "a";
        var y = "A";
        return !string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
    }
}