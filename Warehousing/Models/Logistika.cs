public static class Delivery{
    public static void TovarShipment(List<Tovar> shipment, List<WareHouse> skladList)
    {
        string WareHouseType = "";
        if (Shipment.TrueForAll(t => t.DaysToExpire >= 30))
        {
            WareHouseType = "Общий";
        }
            else if (Shipment.TrueForAll(t => t.DaysToExpire < 30))
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

                    Console.WriteLine("Поставка товаров направлена на склад ID {0}", wareHouse.IdSklada);

                    foreach (var tovar in shipment){
                        Console.WriteLine($"Товар: {tovar.Name}, объем: {tovar.Amount}, откуда поставка: {tovar.IdOfProvider}, куда: {wareHouse.WareHouseId}");    
                    }
                    return;
                }
            }

        }
        Console.WriteLine("Нет свободных складов для текущей поставки");
    }

    public static void OptimizationOfTOvarsDelivery(List<WareHouse> WareHouses)
}