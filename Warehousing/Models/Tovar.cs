using System;
namespace Warehousing.Models{
public class Tovar
{
    public int Id {get; set; }
    public int IdOfProvider {get; set; }

    public string Name {get; set; }

    public double Amount {get; set; }

    public double Price {get; set; }

    public int DaysToExpire {get; set; }



    public Tovar(int id, int idOfProvider, string name, double amount, double price, int daysToExpire){
        Id = id;
        IdOfProvider = idOfProvider;
        Name = name;
        Amount = amount;
        Price = price;
        DaysToExpire = daysToExpire;

    }

    public void TovarEdit(int idOfProvider, string name, double amount, double price, int daysToExpire)
    {
        IdOfProvider = idOfProvider;
        Name = name;
        Amount = amount;
        Price = price;
        DaysToExpire = daysToExpire;

    }

    public void TovarInfo()
    {
        Console.WriteLine("Id: " + Id);
        Console.WriteLine("Id поставщика: " + IdOfProvider);
        Console.WriteLine("Навзание: " + Name);
        Console.WriteLine("Объем: " + Amount);
        Console.WriteLine("Цена: " + Price);
        Console.WriteLine("Дней до окончания срока годности: " + DaysToExpire);
    }
    public void DeleteTovar()
    {
        Id = 0;
        IdOfProvider = 0;
        Name = "";
        Amount = 0;
        Price = 0;
        DaysToExpire = 0;
    }
}

}