using System;
using System.Text;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

public static class StringExtensions
{
    public static StringComparison DefaultComparison = StringComparison.OrdinalIgnoreCase;

    public static string ReplaceCaseless(this string str, string oldValue, string newValue)
    {
        var builder = new StringBuilder();

        var previousIndex = 0;
        var index = str.IndexOf(oldValue, DefaultComparison);
        while (index != -1)
        {
            builder.Append(str.Substring(previousIndex, index - previousIndex));
            builder.Append(newValue);
            index += oldValue.Length;

            previousIndex = index;
            index = str.IndexOf(oldValue, index, DefaultComparison);
        }

        builder.Append(str.Substring(previousIndex));

        return builder.ToString();
    }
}