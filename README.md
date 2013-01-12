## This is an add-in for  [Fody](https://github.com/Fody/Fody) 

Change string comparisons to be case insensitive.

[Introduction to Fody](https://github.com/Fody/Fody/wiki/SampleUsage)

## Nuget

Nuget package http://nuget.org/packages/Caseless.Fody 

## Your Code

    public bool Foo()
    {
        var x = "a";
        var y = "A";
        return x == y;
    }

## What gets compiled

    public bool Foo()
    {
        var x = "a";
        var y = "A";
        return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
    }

Other string comparison methods also get converted. See [ConvertedMethods](https://github.com/Fody/Caseless/wiki/ConvertedMethods)

To change the [StringComparison](http://msdn.microsoft.com/en-us/library/system.stringcomparison.aspx) used by default see [DefaultStringComparison](https://github.com/Fody/Caseless/wiki/DefaultStringComparison)
