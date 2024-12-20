using System.Linq;

public class StringComparison{
    public static int compareByLength(string x, string y){
        return x.Length.CompareTo(y.Length);
    
    }

    public static int CompareByAlphabet(string x, string y){
        return string.Compare(x, y);
    }

    public static int CompareByVowelCount(string x, string y){
        int countX = x.Count(c => "аоуыэеёюяАОУЫЭЕЁЮЯ".Contains(c));
        int countY = y.Count(c => "аоуыэеёюяАОУЫЭЕЁЮЯ".Contains(c));
        return countX.CompareTo(countY);
        
        }

}
