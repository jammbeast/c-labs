using System;
class Program
    {
static void Main(string[] args)
        {
                Console.Write("мин темп");

            double minTemp = Convert.ToDouble(Console.ReadLine());

            Console.Write("макс темп: ");
            double maxTemp = Convert.ToDouble(Console.ReadLine());

            Thermostat thermostat = new Thermostat(minTemp, maxTemp);

            thermostat.OverHeat += message => Console.WriteLine(message);
            thermostat.OverCool += message => Console.WriteLine(message);

            thermostat.Start();

        }
    }