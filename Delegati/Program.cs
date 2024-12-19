using System;

class Program
{
    // Определяем делегат для сравнения двух строк
    delegate int StringComparer(string s1, string s2);

    static void Main(string[] args)
    {
        string[] strings = { "Еврей", "Серб", "Немец", "Француз", "Русский", "Узбек" };

        Console.WriteLine("Выберите способ сортировки:");
        Console.WriteLine("1 - длина строки");
        Console.WriteLine("2 - алфавит");
        Console.WriteLine("3 - По количеству гласных букв в строке");
        int choice = int.Parse(Console.ReadLine());

        StringComparer comparer = null;

        switch (choice)
        {
            case 1:
                comparer = CompareByLength;
                break;
            case 2:
                comparer = CompareAlphabetically;
                break;
            case 3:
                comparer = CompareByVowelCount;
                break;
            default:
                Console.WriteLine("Некорректный выбор");
                return;
        }

        // Сортируем массив с использованием выбранного делегата
        Array.Sort(strings, (s1, s2) => comparer(s1, s2));

        Console.WriteLine("\nОтсортированный массив:");
        foreach (string s in strings)
        {
            Console.WriteLine(s);
        }
    }

    // Метод для сравнения по длине строки
    static int CompareByLength(string s1, string s2)
    {
        return s1.Length.CompareTo(s2.Length);
    }

    // Метод для алфавитного сравнения
    static int CompareAlphabetically(string s1, string s2)
    {
        return s1.CompareTo(s2);
    }

    // Метод для сравнения по количеству гласных букв
    static int CompareByVowelCount(string s1, string s2)
    {
        int vowels1 = CountVowels(s1);
        int vowels2 = CountVowels(s2);
        return vowels1.CompareTo(vowels2);
    }

    // Метод для подсчета гласных букв в строке
    static int CountVowels(string s)
    {
        int count = 0;
        string vowels = "аеёиоуыэюяAEЁИОУЫЭЮЯ";
        foreach (char c in s)
        {
            if (vowels.Contains(c))
            {
                count++;
            }
        }
        return count;
    }
}