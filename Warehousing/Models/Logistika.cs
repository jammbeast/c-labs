namespace Warehousing.Models{
public static class Delivery{
    public static void TovarShipment(List<Tovar> shipment, List<WareHouse> skladList)
    {
        string WareHouseType = "";
        if (shipment.TrueForAll(t => t.DaysToExpire >= 30))
        {
            WareHouseType = "Общий";
        }
            else if (shipment.TrueForAll(t => t.DaysToExpire < 30))
        {
            WareHouseType = "Холодильник";
        }
        else
        {
            WareHouseType = "Сортировочный";
        }

        foreach (var wareHouse in skladList){
            if (wareHouse.Type == WareHouseType){
                double shipmentVolume = 0;
                foreach (var tovar in shipment){
                    shipmentVolume += tovar.Amount;
                }

                if (wareHouse.WareHouseVolume >= shipmentVolume){
                    foreach (var tovar in shipment)
                    {
                        wareHouse.AddTovar(tovar);
                    }

                    Console.WriteLine("Поставка товаров направлена на склад ID {0}", wareHouse.WareHouseId);

                    foreach (var tovar in shipment){
                        Console.WriteLine($"Товар: {tovar.Name}, объем: {tovar.Amount}, откуда поставка: {tovar.IdOfProvider}, куда: {wareHouse.WareHouseId}");    
                    }
                    return;
                }
            }

        }
        Console.WriteLine("Нет свободных складов для текущей поставки");
    }

    public static void OptimizationOfTOvarsDelivery(List<WareHouse> wareHouses){
            foreach (var wareHouse in wareHouses){
                if (wareHouse.Type == "Сортировочный"){
                    List<Tovar> ToRemove = new List<Tovar>();
                    foreach (var tovar in wareHouse.Tovars)
                    {
                        string typeOfWareHouseDestination;
                        if (tovar.DaysToExpire < 0)
                        {
                            typeOfWareHouseDestination = "Утилизация";
                        }
                        else
                        {
                            typeOfWareHouseDestination = tovar.DaysToExpire >= 30 ? "Общий" : "Холодильник";
                        }
                        var wareHouseDestination = wareHouses.Find(w => w.Type == typeOfWareHouseDestination && w.WareHouseVolume >= tovar.Amount);
                        if (wareHouseDestination != null){
                            wareHouseDestination.AddTovar(tovar);
                            ToRemove.Add(tovar);
                            Console.WriteLine($"Товар: {tovar.Name}, объем: {tovar.Amount}, откуда: {wareHouse.WareHouseId} отправлен на склад {wareHouseDestination.WareHouseId}");

                        }
                    }
                    foreach (var tovar in ToRemove){
                        wareHouse.RemoveTovar(tovar);
                    }
                    Console.WriteLine("Оптимизация перемещения товаров завершена.");
                }
            }
    }
    public static void TovarDelivery(List<Tovar> tovars, WareHouse from, WareHouse destination){
        foreach (var tovar in tovars){
                if (from.Tovars.Contains(tovar) && destination.WareHouseVolume >= tovar.Amount){
                    from.RemoveTovar(tovar);
                    destination.AddTovar(tovar);
                    Console.WriteLine($"Товар: {tovar.Name}, объем: {tovar.Amount}, откуда: {from.WareHouseId} отправлен на склад {destination.WareHouseId}");
                }
    }
}
    public static void Utilization(List<WareHouse> wareHouses, WareHouse utilizationWareHouse){
        foreach (var wareHouse in wareHouses){
            List<Tovar> ExpiredTovars = new List<Tovar>();
            foreach (var tovar in wareHouse.Tovars){
                if (tovar.DaysToExpire < 0){
                    ExpiredTovars.Add(tovar);
                }
            }
            foreach (var tovar in ExpiredTovars){
                wareHouse.RemoveTovar(tovar);
                utilizationWareHouse.AddTovar(tovar);
                Console.WriteLine($"Товар: {tovar.Name}, объем: {tovar.Amount}, откуда: {wareHouse.WareHouseId} утилизирован, куда {utilizationWareHouse.WareHouseId}");
        }

    } 

}

    public static void Analisis(List<WareHouse> wareHouses) //самый крутой метод imo 
{
    foreach (var wareHouse in wareHouses)
    {
        bool violation = false;
        List<string> violationMessages = new List<string>();

        foreach (var tovar in wareHouse.Tovars)
        {
            string targetWarehouseType = string.Empty;

            if (tovar.DaysToExpire < 0)
            {
                // Товары с истекшим сроком годности должны перемещаться на склад утилизации
                targetWarehouseType = "Утилизация";
            }
            else if (wareHouse.Type == "Общий" && tovar.DaysToExpire < 30 && tovar.DaysToExpire >= 0)
            {
                // Холодные товары на общем складе должны перемещаться на холодильный склад
                targetWarehouseType = "Холодильник";
            }
            else if (wareHouse.Type == "Холодильник" && tovar.DaysToExpire >= 30)
            {
                // Товары с большим сроком годности на холодильном складе могут требовать перемещения на общий склад
                targetWarehouseType = "Общий";
            }
            else if (wareHouse.Type == "Сортировочный" && tovar.DaysToExpire >= 30)
            {
                // Нарушения на сортировочном складе требуют перемещения на склад утилизации
                targetWarehouseType = "Общий";
            }
            else if  (wareHouse.Type == "Сортировочный" && tovar.DaysToExpire < 0)
            {
                // Нарушения на сортировочном складе требуют перемещения на склад утилизации
                targetWarehouseType = "Утилизация";
            }
            else if (wareHouse.Type == "Сортировочный" && tovar.DaysToExpire < 30 && tovar.DaysToExpire >= 0)
            {
                // Нарушения на сортировочном складе требуют перемещения на холодильный склад
                targetWarehouseType = "Холодильник";
            }

            if (!string.IsNullOrEmpty(targetWarehouseType))
            {
                violation = true;
                
                var targetWarehouse = wareHouses.FirstOrDefault(w => w.Type == targetWarehouseType);
                if (targetWarehouse != null)
                {
                    violationMessages.Add($"Товар \"{tovar.Name}\" (ID: {tovar.Id}) на складе \"{wareHouse.Type}\" (ID: {wareHouse.WareHouseId}) должен быть перемещён на склад \"{targetWarehouse.Type}\" (ID: {targetWarehouse.WareHouseId}).");
                }
                else
                {
                    violationMessages.Add($"Товар \"{tovar.Name}\" (ID: {tovar.Id}) на складе \"{wareHouse.Type}\" (ID: {wareHouse.WareHouseId}) требует перемещения, но целевой склад \"{targetWarehouseType}\" не найден.");
                }
            }
        }

        if (violation)
        {
            Console.WriteLine($"Склад ID: {wareHouse.WareHouseId} ({wareHouse.Type}): Нарушения есть");
            foreach (var message in violationMessages)
            {
                Console.WriteLine(message);
            }
        }
        else
        {
            Console.WriteLine($"Склад ID: {wareHouse.WareHouseId} ({wareHouse.Type}): Нарушений нет");
        }
    }
}

    public static void TotalCost(WareHouse wareHouse){
        double totalCost = 0;
        foreach (var tovar in wareHouse.Tovars){
            totalCost += tovar.Price;
        }
        Console.WriteLine($"Общая стоимость товаров на складе {wareHouse.WareHouseId}: {totalCost}");
    }
}}