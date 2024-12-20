using System;

public class StringSorter{
    
    public static void SortStrings(string[] strings, ComparisonDelegate comparison){
        {   
            Array.Sort(strings, new Comparison<string>(comparison));

        }


        

    }


}