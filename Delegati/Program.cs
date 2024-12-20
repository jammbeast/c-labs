using System;

public class Program{

    public static void Main(){
        string[] strings = new string[] {"еврей", "турок", "серб", "русский", "француз", "немец", "виноград", "амэриканец", "китаец", "японец"};
        

        Console.WriteLine("способ сортировки:");

        Console.WriteLine("1. По длине строки");
        Console.WriteLine("2. По алфавиту");
        Console.WriteLine("3. По количеству гласных букв");
        var choice = Console.ReadLine();

        ComparisonDelegate comparison = null;


        switch(choice){
            case "1":
                comparison = StringComparison.compareByLength;
                break;
            case "2":
                comparison = StringComparison.CompareByAlphabet;
                break;

            default:
                comparison = StringComparison.CompareByVowelCount;
                break;
        }
        StringSorter.SortStrings(strings, comparison);

        Console.WriteLine("Отсортированный массив:");
        foreach (var str in strings){
            Console.WriteLine(str);
        }
    }
}