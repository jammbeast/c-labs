using System.Linq;
using System.Globalization;

public class StringFilters{

    public static FilterDelegate LengthGreaterThan(int length){
        return str => str.Length > length;
    }
    public static FilterDelegate StartWithLetter(char letter)
    {
        return str => !string.IsNullOrEmpty(str) && string.Compare(str[0].ToString(), letter.ToString(), CultureInfo.CurrentCulture, CompareOptions.IgnoreCase) == 0;
    }
    // не знаю, тут gpt даже не справился чтобы кусок кода был рабочим для русского языка, для этого добавил в массив строк английские слова
    public static FilterDelegate ContainsAtLeastVowels(int count){
        return str => str.Count(c => "аоуыэеёюяАОУЫЭЕЁЮЯ".Contains(c)) >= count;
    }

}