using System;

namespace BookingSystem.Models
{
    public class Booking
    {
        public int ClientId { get; set; }          // Уникальный ID клиента
        public string ClientName { get; set; }    // Имя клиента
        public string ClientPhone { get; set; }   // Номер телефона клиента
        public string StartTime { get; set; }     // Время начала брони
        public string EndTime { get; set; }       // Время окончания брони
        public string Comment { get; set; }       // Комментарий
        public int TableId { get; set; }          // Назначенный стол

        // Метод для создания бронирования
        public static Booking CreateBooking(int clientId, string name, string phone, string start, string end, string comment, int tableId)
        {
            return new Booking
            {
                ClientId = clientId,
                ClientName = name,
                ClientPhone = phone,
                StartTime = start,
                EndTime = end,
                Comment = comment,
                TableId = tableId
            };
        }

        // изменение бронирования
        public void UpdateBooking(string start, string end, string comment)
        {
            StartTime = start;
            EndTime = end;
            Comment = comment;
            Console.WriteLine("Бронирование обновлено.");
        }

        // отмена бронирования
        public void CancelBooking()
        {
            Console.WriteLine($"Бронирование клиента {ClientName} отменено.");
            StartTime = null;
            EndTime = null;
            Comment = null;
            TableId = -1; //убираю привязку к столу
        }

        // вывод информации о бронировании
        public void PrintBookingInfo()
        {
            Console.WriteLine($"Клиент: {ClientName} (ID: {ClientId}, Телефон: {ClientPhone})");
            Console.WriteLine($"Время: {StartTime} - {EndTime}");
            Console.WriteLine($"Стол: {TableId}");
            Console.WriteLine($"Комментарий: {Comment}");
        }


        // поиск бронирования для тестов
        public static Booking FindBooking(List<Booking> bookings, string ClientName, string PhoneSuffix){
            return bookings.Find(bookings =>
            bookings.ClientName.Equals(ClientName, StringComparison.OrdinalIgnoreCase) && bookings.ClientPhone.EndsWith(PhoneSuffix));
            
    }
}
}