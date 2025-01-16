# <img src="/package_icon.png" height="30px"> Caseless.Fody

[![NuGet Status](https://badge.fury.io/nu/caseless.fody.svg)](https://www.nuget.org/packages/Caseless.Fody/)

Change string comparisons to be case insensitive.

**See [Milestones](../../milestones?state=closed) for release notes.**


### This is an add-in for [Fody](https://github.com/Fody/Home/)

**It is expected that all developers using Fody [become a Patron on OpenCollective](https://opencollective.com/fody/contribute/patron-3059). [See Licensing/Patron FAQ](https://github.com/Fody/Home/blob/master/pages/licensing-patron-faq.md) for more information.**


## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).


### NuGet installation

Install the [Caseless.Fody NuGet package](https://nuget.org/packages/Caseless.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package Caseless.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<Caseless/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/usage.md#add-fodyweaversxml)

```xml
<Weavers>
  <Caseless/>
</Weavers>
```


## Your Code

```csharp
public bool Foo()
{
    var x = "a";
    var y = "A";
    return x == y;
}
```


## What gets compiled

```csharp
public bool Foo()
{
    var x = "a";
    var y = "A";
    return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
}
```


## Converted Methods

The following string methods get converted to their StringComparison equivalents.

 * `Equality` http://msdn.microsoft.com/en-us/library/system.string.op_equality
 * `Inequality` http://msdn.microsoft.com/en-us/library/system.string.op_inequality
 * `Equals(String)` http://msdn.microsoft.com/en-us/library/858x0yyx
 * `Equals(String, String)` http://msdn.microsoft.com/en-us/library/1hkt4325
 * `Compare(String,String)` http://msdn.microsoft.com/en-us/library/84787k22
 * `CompareTo(String)` http://msdn.microsoft.com/en-us/library/35f0x18w
 * `EndsWith(String)` http://msdn.microsoft.com/en-us/library/2333wewz
 * `Contains(String)` http://msdn.microsoft.com/en-us/library/dy85x1sa
 * `IndexOf(String)` http://msdn.microsoft.com/en-us/library/k8b1470s
 * `IndexOf(String, Int)` http://msdn.microsoft.com/en-us/library/7cct0x33
 * `IndexOf(String, Int, Int)` http://msdn.microsoft.com/en-us/library/d93tkzah
 * `LastIndexOf(String)` http://msdn.microsoft.com/en-us/library/1wdsy8fy
 * `LastIndexOf(String, Int)` http://msdn.microsoft.com/en-us/library/bc3z4t9d
 * `LastIndexOf(String, Int, Int)` http://msdn.microsoft.com/en-us/library/d0z3tk9t
 * `StartsWith(String)` http://msdn.microsoft.com/en-us/library/baketfxw


## What about `String.Replace`

This is because there is no overload for a case insensitive replace in the .net framework.

Here is an extension method to achieve it manually. Taken from [this StackOverflow answer](http://stackoverflow.com/a/244933/53158)

```csharp
public static class StringExtensions
{
    public static StringComparison DefaultComparison = StringComparison.OrdinalIgnoreCase;
    public static string ReplaceCaseless(this string str, string oldValue, string newValue)
    {
        var sb = new StringBuilder();

        var previousIndex = 0;
        var index = str.IndexOf(oldValue, DefaultComparison);
        while (index != -1)
        {
            sb.Append(str.Substring(previousIndex, index - previousIndex));
            sb.Append(newValue);
            index += oldValue.Length;

            previousIndex = index;
            index = str.IndexOf(oldValue, index, DefaultComparison);
        }
        sb.Append(str.Substring(previousIndex));

        return sb.ToString();
    }
}
```


## Default String Comparison

The default string comparison can be configured in the FodyWeavers.xml file.

For example to use `StringComparison.InvariantCultureIgnoreCase` add the following to FodyWeavers.xml.

```xml
<Weavers>
    <Caseless StringComparison="InvariantCultureIgnoreCase"/>
</Weavers>
```


## Icon

<a href="https://thenounproject.com/noun/aardvark/#icon-No6982">Aardvark</a> designed by <a href="https://thenounproject.com/nmac">N J MacNeil</a> from [The Noun Project](https://thenounproject.com).