## This is an add-in for  [Fody](https://github.com/Fody/Fody) 

![Icon](https://raw.github.com/Fody/Caseless/master/Icons/package_icon.png)

Change string comparisons to be case insensitive.

[Introduction to Fody](https://github.com/Fody/Fody/wiki/SampleUsage)

## Nuget

Nuget package http://nuget.org/packages/Caseless.Fody 

To Install from the Nuget Package Manager Console 
    
    PM> Install-Package Caseless.Fody

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

## Converted Methods

The following string methods get converted to their StringComparison equivalents.

 * Equality http://msdn.microsoft.com/en-us/library/system.string.op_equality
 * Inequality http://msdn.microsoft.com/en-us/library/system.string.op_inequality
 * Equals(String) http://msdn.microsoft.com/en-us/library/858x0yyx
 * Equals(String, String) http://msdn.microsoft.com/en-us/library/1hkt4325
 * Compare(String,String) http://msdn.microsoft.com/en-us/library/84787k22
 * CompareTo(String) http://msdn.microsoft.com/en-us/library/35f0x18w
 * EndsWith(String) http://msdn.microsoft.com/en-us/library/2333wewz
 * Contains(String) http://msdn.microsoft.com/en-us/library/dy85x1sa
 * IndexOf(String) http://msdn.microsoft.com/en-us/library/k8b1470s
 * IndexOf(String, Int) http://msdn.microsoft.com/en-us/library/7cct0x33
 * IndexOf(String, Int, Int) http://msdn.microsoft.com/en-us/library/d93tkzah
 * LastIndexOf(String) http://msdn.microsoft.com/en-us/library/1wdsy8fy
 * LastIndexOf(String, Int) http://msdn.microsoft.com/en-us/library/bc3z4t9d
 * LastIndexOf(String, Int, Int) http://msdn.microsoft.com/en-us/library/d0z3tk9t
 * StartsWith(String) http://msdn.microsoft.com/en-us/library/baketfxw

## What about `String.Replace`

This is because there is no overload for a case insensitive replace in the .net framework. 

Here is an extension method to achieve it manually. Take from [this StackOverflow answer](http://stackoverflow.com/a/244933/53158)
 
```
public static class StringExtensions
{
    public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
    {
        var sb = new StringBuilder();

        var previousIndex = 0;
        var index = str.IndexOf(oldValue, comparison);
        while (index != -1)
        {
            sb.Append(str.Substring(previousIndex, index - previousIndex));
            sb.Append(newValue);
            index += oldValue.Length;

            previousIndex = index;
            index = str.IndexOf(oldValue, index, comparison);
        }
        sb.Append(str.Substring(previousIndex));

        return sb.ToString();
    }
}
```

## Default String Comparison

If your don't want to use StringComparison.OrdinalIgnoreCase then you can configure the addin inside the FodyWeavers.xml file.

For example if you want to use StringComparison.InvariantCultureIgnoreCase then add the following to FodyWeavers.xml.

    <?xml version="1.0" encoding="utf-8" ?>
    <Weavers>
        <Caseless StringComparison="InvariantCultureIgnoreCase"/>
    </Weavers>

## Icon 

<a href="http://thenounproject.com/noun/aardvark/#icon-No6982" target="_blank">Aardvark</a> designed by <a href="http://thenounproject.com/nmac" target="_blank">N J MacNeil</a> from The Noun Project
