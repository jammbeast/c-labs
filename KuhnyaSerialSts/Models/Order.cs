using System;
using System.Collections.Generic;

namespace KuhnyaSerialSts.Models{

    class Order{

        public int Id {get; set;}
        public int TableId {get; set;}
        public List<Dish> Dishes {get; set;}
        public string comment {get; set;}
        public DateTime TimeOfOrder {get; set;}
        public int WaiterId {get; set;}
        public DateTime? TimeOfServe {get; set;}
        public double TotalCost {get; set;}

        public static int OrderCounter {get; private set;}


    public Order(int id, int tableId, int waiterId){
        Id = id;
        TableId = tableId;
        Dishes = new List<Dish>();
        TimeOfOrder = DateTime.Now;
        WaiterId = waiterId;
        TotalCost = 0.0;
        OrderCounter++;
    }

    public void AddDish(params Dish[] newDishes){
        foreach (var dish in newDishes){
            Dishes.Add(dish);
            TotalCost += dish.Cost;
        }
    }
    public bool EditOrder( List<Dish> newDishes){
        if (TimeOfServe != null){
            return false;
        }

        Dishes = newDishes;
        TotalCost = Dishes.Sum(b => b.Cost);
        return true;
    }

    public void PrintInformation(){
        Console.WriteLine($"Id заказа: {Id}");
        Console.WriteLine($"id столика : {TableId}");
        Console.WriteLine($"Id Официанта: {WaiterId}");
        Console.WriteLine($"Время принятия заказа: {TimeOfOrder}");
        Console.WriteLine($"Комментарий: {comment}");
        Console.WriteLine($"Блюда в заказе:");
        foreach (var dish in Dishes){
            Console.WriteLine($"Название: {dish.Name}, цена: {dish.Cost} руб.");
        }
        Console.WriteLine($"Итоговая стоимость: {TotalCost} руб.");
    }
    public void CloseOrder (){
        if (Dishes == null || Dishes.Count == 0){
            Console.WriteLine("Заказ не содержит блюд");
            return;
        }
        int maxCookTIme =Dishes.Max(dish => dish.TimeToCook);

        TimeOfServe = TimeOfOrder.AddMinutes(maxCookTIme);
    }

    public void CheckPojaluista(){
        if (TimeOfServe == null){
            Console.WriteLine("Заказ еще закрыт");
            return;
        }
        Console.WriteLine("************************************************");
        Console.WriteLine($"Столик:, {TableId}");
        Console.WriteLine($"Официант: {WaiterId}");
        Console.WriteLine($"Время обслуживания : с {TimeOfOrder} по {TimeOfServe}");
        
        var Category = Dishes.GroupBy(b => b.Category);
        foreach (var category in Category){
            Console.WriteLine($"Категория: {category.Key}");
            var categoryCost = 0.0;
            var categoryDishes = category.GroupBy(b => b.Id);
            foreach (var group in categoryDishes){
                var counter = group.Count();
                var dish = group.First();
                var costOfDishes = counter * dish.Cost;
                Console.WriteLine($"Название: {dish.Name}\t{counter} * {dish.Cost}  = {costOfDishes} руб");
                categoryCost += costOfDishes;
            }
            Console.WriteLine($"Итого по категории: {categoryCost} руб.");
        }
        Console.WriteLine($"\n\t\tИтог счета: {TotalCost} руб.");
        Console.WriteLine("************************************************");
    }
//
    }
    }