using KuhnyaSerialSts.Models;

class Program
{
    static void Main(string[] args)
    {
    Dish soup1 = new Dish(1, "Суп1", "Картофель, морковь", "250г", 150.0, Category.СУПЫ, 15, new string[] { "вегетарианское" });
    Dish soup2 = new Dish(2, "Суп2", "Говядина, лук," , "300г", 200, Category.СУПЫ, 20, new string[] { "" });
    Dish drink1 = new Dish(3, "Кока Кола", "Газированная вода, сахар", "200мл", 50.0, Category.НАПИТКИ, 0, new string[] { "безалкогольное" });
    Dish desert1 = new Dish(4, "Мороженое", "Молоко, сахар", "100г", 100.0, Category.ДЕСЕРТЫ, 0, new string[] { "детское" });

    Console.WriteLine("1) Блюда Созданы (+)");

    List<Dish> menu = new List<Dish> {soup1, soup2, drink1, desert1};

    Console.WriteLine("\n2) Вывод меню:");
    foreach (var dish in menu)
    {
        dish.PrintDish();
        Console.WriteLine(); 
    }
    Console.WriteLine("(+)");

    Order order = new Order(1,10,1);
    order.AddDish(soup1, soup1, drink1, desert1);
    Console.WriteLine("\n3) Заказ создан (+)");
    order.PrintInformation();

    Console.WriteLine("\n4) Печать Чека : ");
    order.CheckPojaluista();
    Console.WriteLine("(0)");

    Console.WriteLine("\n5) Количество заказов выполненых официантом:");
    int waiterCounter = CountOfClosedOrders(order, 1);
    Console.WriteLine($"Количество заказов выполненых официантом 1: + {waiterCounter}");
    Console.WriteLine("(ПУСТОЙ ВЫВОД)");

    Console.WriteLine("\n6) Подсчет Суммы закрытых заказов:");
    double totalCost = costOfClosedOrders(new List<Order> {order});
    Console.WriteLine($"Сумма закрытых заказов: + {totalCost}");
    Console.WriteLine("(0)");

    Console.WriteLine("\n7) Статистика по каждому блюду:");
    Dictionary<string, int> statistics = DishStatistics(new List<Order> {order});
    if (statistics.Count == 0){
        Console.WriteLine("Статистика по блюдам пуста");
    }
    else{
        foreach (var stat in statistics){
            Console.WriteLine($"Блюдо: {stat.Key}, количество: {stat.Value}");
        }
    }

    Console.WriteLine("(ПУСТОЙ ВЫВОД)");

    Console.WriteLine("\n8) Закрытие Чека:");
    order.CloseOrder();
    Console.WriteLine("Чек закрыт (+)");

    Console.WriteLine("\n9) Изменение Заказа:");
    Dish drink2 = new Dish(5, "Напиток2", "Яблочный сок", "200мл", 60.0, Category.НАПИТКИ, 5, new string[] { "веганское" });
    bool change = order.EditOrder(new List<Dish> {soup1, drink2});
    if (!change){
        Console.WriteLine("Заказ закрыт, изменение невозможно");
    }
    else{
        Console.WriteLine("Заказ изменен");
    }
    Console.WriteLine("(x)");

    Console.WriteLine("\n10) Печать Чека : ");
    order.CheckPojaluista();
    Console.WriteLine("(+)");

    Console.WriteLine("\n10) Повтор повторения пунктов 5,6,7 с обновленными данными:");

    Console.WriteLine("\n5) Количество заказов выполненых официантом:");
    int waiterCounterNew = CountOfClosedOrders(order, 1);
    Console.WriteLine($"Количество заказов выполненых официантом 1:  {waiterCounterNew}");
    Console.WriteLine("(+)");

    Console.WriteLine("\n6) Подсчет Суммы закрытых заказов:");
    double totalCostNew = costOfClosedOrders(new List<Order> {order});
    Console.WriteLine($"Сумма закрытых заказов:  {totalCostNew}");
    Console.WriteLine("(+)");
    
    Console.WriteLine("\n7) Статистика по каждому блюду:");
    Dictionary<string, int> statisticsNew = DishStatistics(new List<Order> {order});
    if (statisticsNew.Count == 0){
        Console.WriteLine("Статистика по блюдам пуста");
    }
    else{
        foreach (var stat in statisticsNew){
            Console.WriteLine($"Блюдо: {stat.Key}, количество: {stat.Value}");
        }
    }

}
    
    //ДОП МЕТОДЫ ДЛЯ ФУ. ТЕСТА
    static int CountOfClosedOrders(Order order, int idWaiter){
        if (order.WaiterId == idWaiter && order.TimeOfServe != null ){
            return 1;
        }
        return 0; 
    }
    static double costOfClosedOrders(List<Order> orders)
        {
            return orders.Where(z => z.TimeOfServe != null).Sum(z => z.TotalCost);
        }
    static Dictionary<string, int> DishStatistics(List<Order> orders){

        var statistics = new Dictionary<string, int>();
        foreach (var order in orders.Where(z => z.TimeOfServe != null)){
            foreach (var dish in order.Dishes){
                if (statistics.ContainsKey(dish.Name)){
                    statistics[dish.Name]++;
                }
                else{
                    statistics.Add(dish.Name, 1);
                }
            }
        }
        return statistics;
//
        }
}
