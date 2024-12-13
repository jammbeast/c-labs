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
                 TestContext.WriteLine("Выполнение теста переброски с холодного на общиий:");
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
                 TestContext.WriteLine("Выполнение теста переброски с холодного на сортировочный");
                 TestContext.WriteLine(logOutput);
             
                ClassicAssert.IsFalse(holodilniyWareHouse.Tovars.Contains(tovarHolodniy2), "Товар должен быть удалён из холодильника.");
                ClassicAssert.IsTrue(sortirovochniyWareHouse.Tovars.Contains(tovarHolodniy2), "Товар должен быть добавлен в сортировочный склад.");
               ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarHolodniy2.Name}, объем: {tovarHolodniy.Amount}, откуда: {holodilniyWareHouse.WareHouseId} отправлен на склад {sortirovochniyWareHouse.WareHouseId}"));
    }   
    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    // Переброска с холодного на сортировочный.
     var tovarObisniy = new Tovar(12, 110, "Товар L", 50, 1000, -25);
        obshiyWareHouse.AddTovar(tovarObisniy);

        using (StringWriter sw = new StringWriter()){
             Console.SetOut(sw);
             
             
             
            
                Delivery.TovarDelivery(new List<Tovar> { tovarObisniy }, obshiyWareHouse, sortirovochniyWareHouse);
                
                string logOutput = sw.ToString().Trim();
                 TestContext.WriteLine("Выполнение теста переброски с общего на сортировочный");
                 TestContext.WriteLine(logOutput);
             
                ClassicAssert.IsFalse(obshiyWareHouse.Tovars.Contains(tovarObisniy), "Товар должен быть удалён из холодильника.");
                ClassicAssert.IsTrue(sortirovochniyWareHouse.Tovars.Contains(tovarObisniy), "Товар должен быть добавлен в сортировочный склад.");
               ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarObisniy.Name}, объем: {tovarObisniy.Amount}, откуда: {obshiyWareHouse.WareHouseId} отправлен на склад {sortirovochniyWareHouse.WareHouseId}"));
    }   
    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

    // Переброска с общего на холодильник.
    var tovarObisniy2 = new Tovar(13, 112, "Товар M", 50, 1000, 25);
    obshiyWareHouse.AddTovar(tovarObisniy2);

    using (StringWriter sw = new StringWriter()){
        Console.SetOut(sw);
        Delivery.TovarDelivery(new List<Tovar> { tovarObisniy2 }, obshiyWareHouse, holodilniyWareHouse);
        string logOutput = sw.ToString().Trim();
        TestContext.WriteLine("Выполнение теста переброски с общего на холодный");
        TestContext.WriteLine(logOutput);
        ClassicAssert.IsFalse(obshiyWareHouse.Tovars.Contains(tovarObisniy2), "Товар должен быть удалён из общего склада.");
        ClassicAssert.IsTrue(holodilniyWareHouse.Tovars.Contains(tovarObisniy2), "Товар должен быть добавлен в холодильник.");
        ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarObisniy2.Name}, объем: {tovarObisniy2.Amount}, откуда: {obshiyWareHouse.WareHouseId} отправлен на склад {holodilniyWareHouse.WareHouseId}"));
    }
    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

    // Переброска с общего на сортировочный.
    var tovarObisniy3 = new Tovar(14, 113, "Товар N", 50, 1000, 25);
    obshiyWareHouse.AddTovar(tovarObisniy3);
    
    using (StringWriter sw = new StringWriter()){
        Console.SetOut(sw);
        Delivery.TovarDelivery(new List<Tovar> { tovarObisniy3 }, obshiyWareHouse, sortirovochniyWareHouse);
        string logOutput = sw.ToString().Trim();
        TestContext.WriteLine("Выполнение теста переброски с общего на сортировочный:");
        TestContext.WriteLine(logOutput);
        ClassicAssert.IsFalse(obshiyWareHouse.Tovars.Contains(tovarObisniy3), "Товар должен быть удалён из общего склада.");
        ClassicAssert.IsTrue(sortirovochniyWareHouse.Tovars.Contains(tovarObisniy3), "Товар должен быть добавлен в сортировочный склад.");
        ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovarObisniy3.Name}, объем: {tovarObisniy3.Amount}, откуда: {obshiyWareHouse.WareHouseId} отправлен на склад {sortirovochniyWareHouse.WareHouseId}"));
    }
 }

 /// Тестирование метода анализа складской сети.
        [Test]
        public void TestAnalizSkladskoySeti()
        {
            // Добавляем товары с различными условиями
            var tovar1 = new Tovar(15, 115, "Товар O", 60, 1300, 10); // Холодный товар на общем складе
            var tovar2 = new Tovar(16, 116, "Товар P", 35, 900, 40);  // Общий товар на холодном складе
            var tovar3 = new Tovar(17, 117, "Товар Q", 45, 1000, 40); // Общий товар на сортировочном складе
            var tovar4 = new Tovar(18, 118, "Товар R", 20, 600, -2);  // Просроченный товар

            obshiyWareHouse.AddTovar(tovar1);
            holodilniyWareHouse.AddTovar(tovar2);
            sortirovochniyWareHouse.AddTovar(tovar3);
            sortirovochniyWareHouse.AddTovar(tovar4);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Delivery.Analisis(wareHouses);
                string logOutput = sw.ToString().Trim();
                TestContext.WriteLine("Выполнение теста Анализа:");
                TestContext.WriteLine(logOutput);
                // Проверяем вывод логов
                ClassicAssert.IsTrue(logOutput.Contains($"Товар \"{tovar1.Name}\" (ID: {tovar1.Id}) на складе \"{obshiyWareHouse.Type}\" (ID: {obshiyWareHouse.WareHouseId}) должен быть перемещён на склад \"{holodilniyWareHouse.Type}\" (ID: {holodilniyWareHouse.WareHouseId})."));
               ClassicAssert.IsTrue(logOutput.Contains($"Товар \"{tovar2.Name}\" (ID: {tovar2.Id}) на складе \"{holodilniyWareHouse.Type}\" (ID: {holodilniyWareHouse.WareHouseId}) должен быть перемещён на склад \"{obshiyWareHouse.Type}\" (ID: {obshiyWareHouse.WareHouseId})."));
                ClassicAssert.IsTrue(logOutput.Contains($"Товар \"{tovar3.Name}\" (ID: {tovar3.Id}) на складе \"{sortirovochniyWareHouse.Type}\" (ID: {sortirovochniyWareHouse.WareHouseId}) должен быть перемещён на склад \"{obshiyWareHouse.Type}\" (ID: {obshiyWareHouse.WareHouseId})."));
                ClassicAssert.IsTrue(logOutput.Contains($"Товар \"{tovar4.Name}\" (ID: {tovar4.Id}) на складе \"{sortirovochniyWareHouse.Type}\" (ID: {sortirovochniyWareHouse.WareHouseId}) должен быть перемещён на склад \"{utilizationWareHouse.Type}\" (ID: {utilizationWareHouse.WareHouseId})."));
            }
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        /// <summary>
        /// Тестирование метода перемещения товаров с истекшим сроком годности.
        /// </summary>
        /// 

        [Test]
        public void TestUtilization()
        {
            // Добавляем товары с истекшим сроком годности
            var tovar1 = new Tovar(19, 119, "Товар S", 60, 1300, -10);
            var tovar2 = new Tovar(20, 120, "Товар T", 35, 900, -5);

            obshiyWareHouse.AddTovar(tovar1);
            holodilniyWareHouse.AddTovar(tovar2);
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
            
            Delivery.Utilization(wareHouses, utilizationWareHouse);
            string logOutput = sw.ToString().Trim();
            TestContext.WriteLine("Выполнение теста утилизации:");
            TestContext.WriteLine(logOutput);

            ClassicAssert.IsTrue(utilizationWareHouse.Tovars.Contains(tovar1));
            ClassicAssert.IsTrue(utilizationWareHouse.Tovars.Contains(tovar2));
            ClassicAssert.IsFalse(obshiyWareHouse.Tovars.Contains(tovar1));
            ClassicAssert.IsFalse(holodilniyWareHouse.Tovars.Contains(tovar2));
            ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovar1.Name}, объем: {tovar1.Amount}, откуда: {obshiyWareHouse.WareHouseId} утилизирован, куда {utilizationWareHouse.WareHouseId}"));
            ClassicAssert.IsTrue(logOutput.Contains($"Товар: {tovar2.Name}, объем: {tovar2.Amount}, откуда: {holodilniyWareHouse.WareHouseId} утилизирован, куда {utilizationWareHouse.WareHouseId}"));
            
        
        }
 }
 public void TestOptimizedDelivery1(){
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
[Test]
        public void totalSum()
        {
            // Добавляем товары на склад
            var HouseOfTolik1 = new Tovar(21, 121, "Товар U", 40, 900, 20);
            var HouseOfTolik2 = new Tovar(22, 122, "Товар V", 60, 1100, 30);
            obshiyWareHouse.AddTovar(HouseOfTolik1);
            obshiyWareHouse.AddTovar(HouseOfTolik2);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Delivery.TotalCost(obshiyWareHouse);
                string logOutput = sw.ToString().Trim();
                TestContext.WriteLine("Выполнение теста TestOptimizedDelivery:");
                TestContext.WriteLine(logOutput);

                double expectedTotal = HouseOfTolik1.Price + HouseOfTolik2.Price;
                ClassicAssert.IsTrue(logOutput.Contains($"Общая стоимость товаров на складе {obshiyWareHouse.WareHouseId}: {expectedTotal}"));
            }
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}

//7:09 PM 13/12/2024
//Time taken to complete the lab: 7 hours


//Очень крутая лаба так интересно было писать тесты и разбираться в коде и вообще во всем. Спасибо за лабу. 

//Самое главное что Я понял, что нужно прописывать 8 if else для работоспобоности метода Анализа.


//я понял что нужно писать тесты для каждого метода и проверять все возможные варианты.
//понял что нужно писать тесты для каждого метода и проверять все возможные варианты.
//что нужно писать тесты для каждого метода и проверять все возможные варианты.
//нужно писать тесты для каждого метода и проверять все возможные варианты.
//писать тесты для каждого метода и проверять все возможные варианты.
//тесты для каждого метода и проверять все возможные варианты.
//для каждого метода и проверять все возможные варианты.
//каждого метода и проверять все возможные варианты.
//метода и проверять все возможные варианты.
//и проверять все возможные варианты.
//проверять все возможные варианты.
//все возможные варианты.
//возможные варианты.
//варианты.
