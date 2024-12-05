using System;
using System.Collections.Generic;

namespace BookingSystem.Models
{
    public class Table
    {
        public int Id { get; set; }                // Уникальный идентификатор стола
        public string Location { get; set; }       // Расположение стола
        public int Seats { get; set; }             // Количество мест
        public Dictionary<string, string> Schedule { get; set; } = new Dictionary<string, string>(); // Расписание занятости по часам

        // Изменение информации стола;
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

        // Вывод информа о столе
        public void PrintTableInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Расположение: {Location}");
            Console.WriteLine($"Количество мест: {Seats}");
            Console.WriteLine("Расписание:");
            foreach (var slot in Schedule)
            {
                Console.WriteLine($"{slot.Key} - {slot.Value}");
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
            if (!Schedule.ContainsKey(startTime))
            {
                Schedule[startTime] = clientInfo;
                Schedule[endTime] = clientInfo;
                Console.WriteLine($"Стол {Id} успешно забронирован с {startTime} до {endTime} для {clientInfo}.");
                return true;
            }
            Console.WriteLine($"Ошибка: Время {startTime} уже занято.");
            return false;
        }

        // Метод для отмены бронирования от класса Booking
        public void CancelBooking(string startTime, string endTime)
        {
            Schedule.Remove(startTime);
            Schedule.Remove(endTime);
            Console.WriteLine($"Бронирование на столе {Id} отменено.");
        }

        // Метод для поиска свободных столов
        
}
}