using System;
using System.Collections.Generic;
using Warehousing.Models;

public class WareHouse{
    public int id WareHouseId {get ; set }
    public string Type {get ; set }
    public double WareHouseVolume {get ; set }
    public string Address {get ; set }
    public List<Tovar> Tovars {get ; set }


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
        Console.WriteLine("Id: " + Id);
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
        Tovars.Remove(tovar);
    }   
}