using System;
using System.Collections.Generic;
namespace Warehousing.Models{
public class WareHouse{
    public int WareHouseId {get; private set; }
    public string Type {get; private set; }
    public double WareHouseVolume {get; private set; }
    public string Address {get; private set; }
    
    public IReadOnlyList<Tovar> Tovars => _tovars.AsReadOnly();
    private List<Tovar> _tovars;


    public WareHouse(int wareHouseId, string type, double wareHouseVolume, string address, List<Tovar> tovars){
        WareHouseId = wareHouseId;
        Type = type;
        WareHouseVolume = wareHouseVolume;
        Address = address;
        _tovars = new List<Tovar>();
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
        if (WareHouseVolume >= tovar.Amount)
        {
            _tovars.Add(tovar);
            WareHouseVolume -= tovar.Amount;
        }
        else
        {
            Console.WriteLine($"Недостаточно места на складе {WareHouseId} для добавления товара {tovar.Name}");
            return;
        }
    }

    public void RemoveTovar(Tovar tovar)
    {
        if (Tovars.Contains(tovar)){
            _tovars.Remove(tovar);
            WareHouseVolume += tovar.Amount;
            Console.WriteLine($"Товар {tovar.Name} удален со склада {WareHouseId}");
        }
        else {
            Console.WriteLine($"Товар {tovar.Name} не найден на складе {WareHouseId}");
        }
    }   
}
}