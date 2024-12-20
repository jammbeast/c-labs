using System;
using System.Globalization;

public class Program{

    public static void Main(){

        string[] strings = {"apple", "sponge", "bob", "sleep", "prekol", "немец", "виноград", "амэриканец", "китаец", "японец"};

        Console.WriteLine("Выберите способ фильтрации");
        Console.WriteLine("1. Длина строки больше n");
        Console.WriteLine("2. Строка начинается с буквы");
        Console.WriteLine("3. Строка содержит n гласных букв или более");

        var choice = Console.ReadLine();

        FilterDelegate? filter = null; //nulable делегат

        switch (choice)
        {
            case "1":
                Console.WriteLine("Введите длину строки");
                if (int.TryParse(Console.ReadLine(), out var length))
                {
                    filter = StringFilters.LengthGreaterThan(length); // делегат принимает длину строки и передает в лямбда выражение LengthGreaterThan
                }
                else
                {
                    Console.WriteLine("Неверный ввод длины");
                    return;
                }
                break;
            case "2":
                Console.WriteLine("Введите начальную букву:");
                var letterInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(letterInput) && letterInput.Length == 1)
                {
                    char letter = letterInput[0];
                    filter = StringFilters.StartWithLetter(letter); // делегат принимает букву и передает в лямбда выражение StartWithLetter
                }
                else
                {
                    Console.WriteLine("Неверный ввод буквы");
                    return;
                }
                break;
            case "3":
                Console.WriteLine("Введите количество гласных букв");
                if (int.TryParse(Console.ReadLine(), out var count))
                { 
                    filter = StringFilters.ContainsAtLeastVowels(count); // делегат принимает количество гласных букв и передает в лямбда выражение ContainsAtLeastVowels
                }
                else
                {
                    Console.WriteLine("Неверный ввод количества");
                    return;
                }
                break;
            default:
                Console.WriteLine("Неверный выбор");
                return;
        }

    if (filter != null)
        {
            string[] result = StringFilterer.FilterStrings(strings, filter); // передаем массив строк и делегат в метод FilterStrings

            Console.WriteLine("Результат фильтрации:");
            foreach (var str in result)
            {
                Console.WriteLine(str);

    }

}
    }
}
