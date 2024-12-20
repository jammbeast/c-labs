using System;
using System.Collections.Generic;
using System.Globalization;

public class StringFilterer
{
    public static string[] FilterStrings(string[] strings, FilterDelegate filter)
    {
        List<string> result = new List<string>();
        foreach (var str in strings)
        {
            if (filter(str))
            {
                result.Add(str);
            }
        }
        return result.ToArray();
    }
}