using System;
using System.Collections.Generic;
namespace Warehousing.Models{
public class WareHouse{
    public int WareHouseId {get; set; }
    public string Type {get; set; }
    public double WareHouseVolume {get; set; }
    public string Address {get; set; }
    public List<Tovar> Tovars {get; set; }


    public WareHouse(int wareHouseId, string type, double wareHouseVolume, string address, List<Tovar> tovars){
        WareHouseId = wareHouseId;
        Type = type;
        WareHouseVolume = wareHouseVolume;
        Address = address;
        Tovars = new List<Tovar>();
    }
    
    public void WareHouseEdit(string type, double wareHouseVolume, string address)
    {
        Type = type;
        WareHouseVolume = wareHouseVolume;
        Address = address;
    }

    public void WareHouseInfo()
    {
        Console.WriteLine("Id: " + WareHouseId);
        Console.WriteLine("Тип: " + Type);
        Console.WriteLine("Объем склада: " + WareHouseVolume);
        Console.WriteLine("Адрес: " + Address);
        Console.WriteLine("Товары на складе: ");
        Console.WriteLine($"Количество товаров: {Tovars.Count}");
    }

    public void AddTovar(Tovar tovar)
    {
        Tovars.Add(tovar);
    }

    public void RemoveTovar(Tovar tovar)
    {
        if (Tovars.Contains(tovar)){
            Tovars.Remove(tovar);
            WareHouseVolume += tovar.Amount;
            Console.WriteLine($"Товар {tovar.Name} удален со склада {WareHouseId}");
        }
        else {
            Console.WriteLine($"Товар {tovar.Name} не найден на складе {WareHouseId}");
        }
    }   
}
}