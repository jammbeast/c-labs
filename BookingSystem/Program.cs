using BookingSystem.Models;

class Program
{
    static void Main(string[] args)
    {
        var tables = new List<Table>
        {
            Table.CreateTable(2, "У выхода", 2),
            Table.CreateTable(3, "В глубине", 6)
        };
        
        Console.WriteLine("Тест 1: Стол +1");
        var table1 = Table.CreateTable(1, "у окна", 4);
        table1.PrintTableInfo();
        Console.WriteLine("+ стол +1 (1)");

        Console.WriteLine("Тест 2: Бронь +1 (Макс, 12:00 - 16:00)");
        
        var booking1 = Booking.CreateBooking(1, "Макс", "0000", "12:00", "16:00", "День рождения", 1);
        table1.BookTable(booking1.StartTime, booking1.EndTime, $"{booking1.ClientName} (ID: {booking1.ClientId}, Телефон: {booking1.ClientPhone})"); 
        Console.WriteLine("+ бронь (Макс, 12:00 -16:00, 0000");

        Console.WriteLine("Тест 3: Вывод информации о столе");
        table1.PrintTableInfo();
        
        Console.WriteLine("Тест 4: Бронь (Анна, 13:00 - 14:00)");
        var booking2 = Booking.CreateBooking(2, "Анна", "50000", "13:00", "14:00", "Встреча", 2);

        if (!table1.BookTable(booking2.StartTime, booking2.EndTime, $"{booking2.ClientName} (ID: {booking2.ClientId}, Телефон: {booking2.ClientPhone})"))
        {
            Console.WriteLine("x Бронь (Анна, 13:00 - 14:00, 50000) (Ошибка Время занято)");
        }
        else 
        {
            Console.WriteLine("+ Бронь (Анна, 13:00 - 14:00, 50000)");
        }

        Console.WriteLine("Тест 5: Бронь (Анна, 16:00 - 17:00)");
        if (!table1.BookTable(booking2.StartTime, booking2.EndTime, $"{booking2.ClientName} (ID: {booking2.ClientId}, Телефон: {booking2.ClientPhone})"))
        {
            Console.WriteLine("x Бронь (Анна, 16:00 - 17:00, 50000) (Ошибка Время занято)");
        }
        else 
        {
            Console.WriteLine("+ Бронь (Анна, 16:00 - 17:00, 50000)");
        }

        Console.WriteLine("Тест 6: Вывод информации о столе ");
        table1.PrintTableInfo();

        Console.WriteLine("Тест 7: Редактирование стола (1)");
        if (table1.Schedule.ContainsKey("12:00")){
            table1.UpdateTableInfo("у Выхода", 6);
            Console.WriteLine("x Редактирование стола (1) (Ошибка: есть активные брони)");
        }
        Console.WriteLine("Тест 8: Поиск бронирования (Макс, 0000)");

        var bookings = new List<Booking> { booking1, booking2 };
        var foundBooking = Booking.FindBooking(bookings, "Макс", "0000");
        if (foundBooking != null)
        {
            foundBooking.PrintBookingInfo();
        }
        else
        {
            Console.WriteLine("0 Бронирование не найдено.");

    }
}}