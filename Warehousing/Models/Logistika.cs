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
                    wareHouse.Tovars.AddRange(shipment);

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
                if (wareHouse.Type == "сортировочный"){
                    List<Tovar> ToRemove = new List<Tovar>();
                    foreach (var tovar in wareHouse.Tovars)
                    {
                        string typeOfWareHouseDestination = tovar.DaysToExpire >= 30 ? "Общий" : "Холодильник"; 
                        var wareHouseDestination = wareHouses.Find(w =>w.Type == typeOfWareHouseDestination && w.WareHouseVolume >= tovar.Amount);
                        if (wareHouseDestination != null){
                            wareHouseDestination.AddTovar(tovar);
                            ToRemove.Add(tovar);
                            Console.WriteLine($"Товар: {tovar.Name}, объем: {tovar.Amount}, откуда: {wareHouse.WareHouseId} отправлен на склад {wareHouseDestination.WareHouseId}");

                        }
                    }
                    foreach (var tovar in ToRemove){
                        wareHouse.RemoveTovar(tovar);
                    }
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
                if (tovar.DaysToExpire == 0){
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

    public static void Analisis(List<WareHouse> wareHouses){
        foreach (var wareHouse in wareHouses){
            bool violation = false;
            foreach (var tovar in wareHouse.Tovars){
                if (tovar.DaysToExpire < 0){
                    violation = true;
                    Console.WriteLine($"Склад ID {wareHouse.WareHouseId}: Нарушения есть");
                Console.WriteLine("Необходимо провести перемещение товаров с истекшим сроком годности на склад утилизации.");
                break;
                }
            }
            if (!violation){
                Console.WriteLine($"Склад ID {wareHouse.WareHouseId}: Нарушений нет");
            }
    }
}

    public static void TotalCost(WareHouse wareHouse){
        double totalCost = 0;
        foreach (var tovar in wareHouse.Tovars){
            totalCost += tovar.Price;
        }
    }
}}