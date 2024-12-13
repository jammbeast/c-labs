using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework; using NUnit.Framework.Legacy;

using Warehousing.Models;


namespace Test5Lab
{
 [TestFixture]
 public class LogistikaTest
 {
    private WareHouse obshiyWareHouse;
    private WareHouse holodilniyWareHouse;
    private WareHouse sortirovochniyWareHouse;
    private WareHouse utilizationWareHouse;
    private List<WareHouse> wareHouses;
    private List<Tovar> tovars;
    public TestContext TestContext { get; set; }

    [SetUp]
    public void Setup(){
        obshiyWareHouse = new WareHouse(1, "Общий", 1000, "Общий", new List<Tovar>());
        holodilniyWareHouse = new WareHouse(2, "Холодильник", 2000, "Холодильник", new List<Tovar>());
        sortirovochniyWareHouse = new WareHouse(3, "Сортировочный", 800, "Сортировочный", new List<Tovar>());
        utilizationWareHouse = new WareHouse(4, "Утилизация", 300, "Утилизация", new List<Tovar>());

        wareHouses = new List<WareHouse>{obshiyWareHouse, holodilniyWareHouse, sortirovochniyWareHouse, utilizationWareHouse};
    
        tovars = new List<Tovar>{
            new Tovar(1,101, "Товар A", 50, 1000, 40),
            new Tovar(2, 102, "Товар B", 30, 800, 20),
            new Tovar(3, 103, "Товар C", 20, 500, -5)
        };
        }

    
    /// Проверка метода TovarShipment
    /// Вариант 1: Все товары имееют срок > 30 дней.
    /// Вариант 2: Все товары имееют срок < 30 дней.
    /// Вариант 3: Сроки товаров разные.


    [Test]
    public void TestPostavkaTovarov()
{
    // Вариант 1
    var postavka1 = new List<Tovar>
    {
        new Tovar (4, 104, "Товар D", 60, 1200, 35),
        new Tovar (5, 105, "Товар E", 70, 1500, 40),
    };

    using (StringWriter sw = new StringWriter())
    {
        Console.SetOut(sw);
        Delivery.TovarShipment(postavka1, wareHouses);
        string logOutput = sw.ToString().Trim();

        TestContext.WriteLine("Выполнение теста TestPostavkaTovarov 1 Вариант:");
        TestContext.WriteLine(logOutput);
        foreach(var tovar in postavka1)
        {
            bool isAdded = obshiyWareHouse.Tovars.Contains(tovar);
            TestContext.WriteLine($"Товар {tovar.Name} добавлен на склад ID {obshiyWareHouse.WareHouseId}: {isAdded}");
        }

        ClassicAssert.IsTrue(obshiyWareHouse.Tovars.Contains(postavka1[0]));
        ClassicAssert.IsTrue(obshiyWareHouse.Tovars.Contains(postavka1[1]));
        ClassicAssert.IsTrue(logOutput.Contains("Поставка товаров направлена на склад ID 1"));
    }

    //Вариант 2: все товары <30 дней
    var postavka2 = new List<Tovar>
        {
            new Tovar (6, 106, "Товар F", 40, 900, 10),
            new Tovar (7, 107, "Товар G", 50, 1100, 5),
        };

        using (StringWriter sw = new StringWriter()){
            Console.SetOut(sw);
            Delivery.TovarShipment(postavka2, wareHouses);
            string logOutput = sw.ToString().Trim();
            TestContext.WriteLine("Выполнение теста TestPostavkaTovarov 2 Вариант:");
            TestContext.WriteLine(logOutput);
             foreach(var tovar in postavka2)
        {
            bool isAdded = holodilniyWareHouse.Tovars.Contains(tovar);
            TestContext.WriteLine($"Товар {tovar.Name} добавлен на склад ID {obshiyWareHouse.WareHouseId}: {isAdded}");
        }

            ClassicAssert.IsTrue(holodilniyWareHouse.Tovars.Contains(postavka2[0]));
            ClassicAssert.IsTrue(holodilniyWareHouse.Tovars.Contains(postavka2[1]));
            ClassicAssert.IsTrue(logOutput.Contains("Поставка товаров направлена на склад ID 2"));
    }

 
    // Вариант 3: сроки товаров разные
    var postavka3 = new List<Tovar>
            {
                new Tovar(8, 108, "Товар H", 20, 600, 40),
                new Tovar(9, 109, "Товар I", 25, 700, -5) // Просроченный
            };

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Delivery.TovarShipment(postavka3, wareHouses);
                string logOutput = sw.ToString().Trim();
                TestContext.WriteLine("Выполнение теста TestPostavkaTovarov 3 Вариант:");
                TestContext.WriteLine(logOutput);
foreach(var tovar in postavka3)
        {
            bool isAdded = sortirovochniyWareHouse.Tovars.Contains(tovar);
            TestContext.WriteLine($"Товар {tovar.Name} добавлен на склад ID {sortirovochniyWareHouse.WareHouseId}: {isAdded}");
        }
                ClassicAssert.IsTrue(sortirovochniyWareHouse.Tovars.Contains(postavka3[0]));
                ClassicAssert.IsTrue(sortirovochniyWareHouse.Tovars.Contains(postavka3[1]));
                ClassicAssert.IsTrue(logOutput.Contains("Поставка товаров направлена на склад ID 3"));
            }
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }

   
        /// Тестирование метода оптимизации перемещения товаров между складами.
        

        [Test]
        public void TestOptimizedDelivery(){
            var tovar1 = tovars[0]; //дни до окончания срока годности > 30
            var tovar2 = tovars[1]; //дни до окончания срока годности < 30
            var tovar3 = tovars[2]; //просрочен

            sortirovochniyWareHouse.AddTovar(tovar1);
            sortirovochniyWareHouse.AddTovar(tovar2);
            sortirovochniyWareHouse.AddTovar(tovar3);

            using (StringWriter sw = new StringWriter()){
                Console.SetOut(sw);
                Delivery.OptimizationOfTOvarsDelivery(wareHouses);
                string logOutput = sw.ToString().Trim();

                TestContext.WriteLine("Выполнение теста TestOptimizedDelivery:");
                TestContext.WriteLine(logOutput);

                // Проверяем, что товары переместились правильно
                ClassicAssert.IsFalse(sortirovochniyWareHouse.Tovars.Contains(tovar1), "Товар 1 должен быть удалён из сортировочного склада.");
                ClassicAssert.IsFalse(sortirovochniyWareHouse.Tovars.Contains(tovar2), "Товар 2 должен быть удалён из сортировочного склада.");
                ClassicAssert.IsFalse(sortirovochniyWareHouse.Tovars.Contains(tovar3), "Товар 3 должен быть удалён из сортировочного склада.");

                ClassicAssert.IsTrue(obshiyWareHouse.Tovars.Contains(tovar1), "Товар 1 должен быть добавлен в общий склад.");
                ClassicAssert.IsTrue(holodilniyWareHouse.Tovars.Contains(tovar2), "Товар 2 должен быть добавлен в холодильник.");
                ClassicAssert.IsTrue(utilizationWareHouse.Tovars.Contains(tovar3), "Товар 3 должен быть добавлен в утилизацию.");

        }

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
}

    /// Тестирование метода переброски товаров с одного склада на другой.
    [Test]
    public void TestDelivery(){
        // Переброска с холодного на общий.
        var tovarHolodniy = new Tovar(10, 110, "Товар J", 50, 1000, 25);
        holodilniyWareHouse.AddTovar(tovarHolodniy);

        using (StringWriter sw = new StringWriter()){
             Console.SetOut(sw);
             
             
             
            
                Delivery.TovarDelivery(new List<Tovar> { tovarHolodniy }, holodilniyWareHouse, obshiyWareHouse);
                
                string logOutput = sw.ToString().Trim();
                 TestContext.WriteLine("Выполнение теста TestOptimizedDelivery:");
                 TestContext.WriteLine(logOutput);
             
                ClassicAssert.IsFalse(holodilniyWareHouse.Tovars.Contains(tovarHolodniy), "Товар должен быть удалён из холодильника.");
                ClassicAssert.IsTrue(obshiyWareHouse.Tovars.Contains(tovarHolodniy), "Товар должен быть добавлен в общий склад.");
               ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarHolodniy.Name}, объем: {tovarHolodniy.Amount}, откуда: {holodilniyWareHouse.WareHouseId} отправлен на склад {obshiyWareHouse.WareHouseId}"));
    }   
    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

    // Переброска с холодного на сортировочный.
     var tovarHolodniy2 = new Tovar(11, 110, "Товар K", 50, 1000, 25);
        holodilniyWareHouse.AddTovar(tovarHolodniy2);

        using (StringWriter sw = new StringWriter()){
             Console.SetOut(sw);
             
             
             
            
                Delivery.TovarDelivery(new List<Tovar> { tovarHolodniy2 }, holodilniyWareHouse, sortirovochniyWareHouse);
                
                string logOutput = sw.ToString().Trim();
                 TestContext.WriteLine("Выполнение теста TestOptimizedDelivery:");
                 TestContext.WriteLine(logOutput);
             
                ClassicAssert.IsFalse(holodilniyWareHouse.Tovars.Contains(tovarHolodniy2), "Товар должен быть удалён из холодильника.");
                ClassicAssert.IsTrue(sortirovochniyWareHouse.Tovars.Contains(tovarHolodniy2), "Товар должен быть добавлен в сортировочный склад.");
               ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarHolodniy2.Name}, объем: {tovarHolodniy.Amount}, откуда: {holodilniyWareHouse.WareHouseId} отправлен на склад {sortirovochniyWareHouse.WareHouseId}"));
    }   
    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    // Переброска с холодного на сортировочный.
     var tovarObisniy = new Tovar(11, 110, "Товар L", 50, 1000, -25);
        obshiyWareHouse.AddTovar(tovarObisniy);

        using (StringWriter sw = new StringWriter()){
             Console.SetOut(sw);
             
             
             
            
                Delivery.TovarDelivery(new List<Tovar> { tovarObisniy }, obshiyWareHouse, sortirovochniyWareHouse);
                
                string logOutput = sw.ToString().Trim();
                 TestContext.WriteLine("Выполнение теста TestOptimizedDelivery:");
                 TestContext.WriteLine(logOutput);
             
                ClassicAssert.IsFalse(obshiyWareHouse.Tovars.Contains(tovarObisniy), "Товар должен быть удалён из холодильника.");
                ClassicAssert.IsTrue(sortirovochniyWareHouse.Tovars.Contains(tovarObisniy), "Товар должен быть добавлен в сортировочный склад.");
               ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarObisniy.Name}, объем: {tovarObisniy.Amount}, откуда: {obshiyWareHouse.WareHouseId} отправлен на склад {sortirovochniyWareHouse.WareHouseId}"));
    }   
    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

    // Переброска с общего на холодильник.
    var tovarObisniy2 = new Tovar(12, 112, "Товар M", 50, 1000, 25);
    obshiyWareHouse.AddTovar(tovarObisniy2);

    using (StringWriter sw = new StringWriter()){
        Console.SetOut(sw);
        Delivery.TovarDelivery(new List<Tovar> { tovarObisniy2 }, obshiyWareHouse, holodilniyWareHouse);
        string logOutput = sw.ToString().Trim();
        TestContext.WriteLine("Выполнение теста TestOptimizedDelivery:");
        TestContext.WriteLine(logOutput);
        ClassicAssert.IsFalse(obshiyWareHouse.Tovars.Contains(tovarObisniy2), "Товар должен быть удалён из общего склада.");
        ClassicAssert.IsTrue(holodilniyWareHouse.Tovars.Contains(tovarObisniy2), "Товар должен быть добавлен в холодильник.");
        ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarObisniy2.Name}, объем: {tovarObisniy2.Amount}, откуда: {obshiyWareHouse.WareHouseId} отправлен на склад {holodilniyWareHouse.WareHouseId}"));
    }
    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
 }}}