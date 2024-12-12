using System;
using System.Collections.Generic;

namespace KuhnyaSerialSts.Models{

    public enum Category{
        СУПЫ,
        НАПИТКИ,
        ДЕСЕРТЫ
    }
    public class Dish{
        public int Id {get; set;}
        public string Name {get; set;} 
        public string Composition {get; set;}
        public string Weight {get; set;}
        
        public double Cost {get; set;}
        public Category Category { get; set; }
        public int TimeToCook { get; set;}
        public string[] Type { get; set; }

        //статическое поле, хранящее количество блюд
        public static int DishCounter { get; private set;}

        public Dish(int id, string name, string composition, string weight, double cost, Category category, int timeToCook, string[] type){
            Id = id;
            Name = name;
            Composition = composition;
            Weight = weight;
            Cost = cost;
            Category = category;
            TimeToCook = timeToCook;
            Type = type;
            DishCounter++;
        }
        public void EditDish(string newName, string newComposition, string newWeight, double newCost, Category newCategory, int newTimeToCook, string[] newType){
            Name = newName;
            Composition = newComposition;
            Weight = newWeight;
            Cost = newCost;
            Category = newCategory;
            TimeToCook = newTimeToCook;
            Type = newType;
        }
        public void PrintDish(){
            Console.WriteLine("=====================");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Cost: {Cost:C}");
            Console.WriteLine($"Category: {Category}");
            Console.WriteLine($"Type: {string.Join(", ", Type)}");
            Console.WriteLine("=====================");
        }
        // static void PrintDishForTest(){
        //     Console.WriteLine("=====================");
        //     Console.WriteLine($"Name: {Name}");
        //     Console.WriteLine($"Cost: {Cost:C}");
        //     Console.WriteLine($"Category: {Category}");
        //     Console.WriteLine($"Type: {string.Join(", ", Type)}");
        //     Console.WriteLine("=====================");
        // }
            public void DeleteDish(){
            DishCounter--;
        }
        }
        
        
    }
    

//