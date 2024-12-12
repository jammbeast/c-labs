using System;
using System.Collections.Generic;

namespace BookingSystem.Models
{
    public class Table
    {
       public int Id { get; set; }                // id стола
        public string Location { get; set; }       // Расположение стола
        public int Seats { get; set; }             // Количество мест
        public Dictionary<string, string> Schedule { get; set; } = new Dictionary<string, string>(); // Расписание


        public void UpdateTableInfo(string location, int seats)
        {
            if (Schedule.Count == 0)
            {
                Location = location;
                Seats = seats;
                Console.WriteLine("Информация о столе обновлена.");
            }
            else
            {
                Console.WriteLine("Нельзя изменить стол, так как есть активные брони.");
            }
        }

        public void PrintTableInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Расположение: {Location}");
            Console.WriteLine($"Количество мест: {Seats}");
            Console.WriteLine("Расписание:");
            
            DateTime startTime = DateTime.Today.AddHours(9); // Начало дня бронирования
            DateTime endTime = DateTime.Today.AddHours(18); // Конец дня бронирования

            for (var time = startTime; time < endTime; time = time.AddHours(1))
            {
                string timeSlot = time.ToString("HH:mm") + "-" + time.AddHours(1).ToString("HH:mm");
                if (Schedule.ContainsKey(timeSlot))
                {
                    Console.WriteLine($"{timeSlot} -------------------{Schedule[timeSlot]}");
                }
                else
                {
                    Console.WriteLine($"{timeSlot} -----------------------------------------------------");
                }
            }
        }
        // Создание стола
        public static Table CreateTable(int id, string location, int seats)
        {
            return new Table
            {
                Id = id,
                Location = location,
                Seats = seats
            };
        }
        // добавление бронирования в расписание от класса Booking 
        public bool BookTable(string startTime, string endTime, string clientInfo)
        {
            DateTime start = DateTime.Parse(startTime);
            DateTime end = DateTime.Parse(endTime);

            if (end <= start)
            {
                Console.WriteLine("Ошибка: Время окончания брони должно быть позже времени начала.");
                return false;
            }

            for (var time = start; time < end; time = time.AddHours(1))
            {
                string timeSlot = time.ToString("HH:mm") + "-" + time.AddHours(1).ToString("HH:mm");
                if (Schedule.ContainsKey(timeSlot))
                {
                    Console.WriteLine($"Ошибка: Время {timeSlot} уже занято.");
                    return false;
                }
            }

            //// для обратка брони положительна
            for (var time = start; time < end; time = time.AddHours(1))
            {
                string timeSlot = time.ToString("HH:mm") + "-" + time.AddHours(1).ToString("HH:mm");
                Schedule[timeSlot] = clientInfo;
            }

            Console.WriteLine($"Стол {Id} успешно забронирован с {startTime} до {endTime} для {clientInfo}.");
            return true;
        }

        // метод для отмены бронирования от класса Booking
        public void CancelBooking(string startTime, string endTime)
        {
            DateTime start = DateTime.Parse(startTime);
            DateTime end = DateTime.Parse(endTime);

            for (var time = start; time < end; time = time.AddHours(1))
            {
                string timeSlot = time.ToString("HH:mm") + "-" + time.AddHours(1).ToString("HH:mm");
                Schedule.Remove(timeSlot);
            }

            Console.WriteLine($"Бронирование на столе {Id} отменено.");
        }

        // Метод для поиска свободных столов
        public static List<Table> GetAvailableTables(List<Table> tables, string location, int minSeats)
        {
            List<Table> availableTables = new List<Table>();
            foreach (var table in tables)
            {
                if (table.Location == location && table.Seats >= minSeats)
                {
                    availableTables.Add(table);
                }
            }
            return availableTables;
        }
    }
}