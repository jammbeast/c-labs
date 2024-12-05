using system;
using system.Collections.Generic;

namespace KuhnyaSerialSts.Models{
    public class Dish{
        int id {get; set;}
        string name {get; set;} 
        string description {get; set;}
        string weight {get; set;}
        
        double price {get; set;}
        public enum Category
        {
            Напиток,
            Салат,
            Холодная закуска,
            Горячая закуска,
            Суп,
            Основное блюдо,
            Десерт
        }
        int timeToCook {get; set;}
        string[] types { get; set; }
        
        Category category { get; set; }



        public void CreateDish(in int id, in string name, in string description, in string weight, in double price, in int timeToCook, params string[] types, in Category category){
            this.id = id;
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.price = price;
            this.timeToCook = timeToCook;
            this.types = types;
            this.category = category;
            this.types = types;

    }

        public void EditDish(in int id, in string name, in string description, in string weight, in double price, in int timeToCook, params string[] types, in Category category){
            this.id = id;
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.price = price;
            this.timeToCook = timeToCook;
            this.types = types;
            this.category = category;
            this.types = types;
        }

        public void DisplayDish(){
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Название: {name}");
            Console.WriteLine($"Описание: {description}");
            Console.WriteLine($"Вес: {weight}");
            Console.WriteLine($"Цена: {price}");
             Console.WriteLine($"Категория: {category}");
            Console.WriteLine($"Время приготовления: {timeToCook}");
            Console.WriteLine($"Типы: {string.Join(", ", types)}");
           
        }

        public void DeleteDish(){
            id = 0;
            name = "";
            description = "";
            weight = "";
            price = 0;
            timeToCook = 0;
            types = null;
            category = 0;
        }
    }
}
