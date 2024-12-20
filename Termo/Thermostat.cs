using System;
using System.Threading;

public class Thermostat
    {
        public event TemperatureEventHandler OverHeat;
        public event TemperatureEventHandler OverCool;


         private double _currentTemperature;
        private double _minTemperature;
        private double _maxTemperature;
        private Random _random = new Random();

        public Thermostat(double minTemp, double maxTemp)
        {
            _minTemperature = minTemp;
            _maxTemperature = maxTemp;
            // Инициализируем текущую температуру в середине диапазона
            _currentTemperature = (minTemp + maxTemp) / 2;
        }

        public void Start(){
            while(true){
                double change = _random.NextDouble() * 2 - 1;
                _currentTemperature += change;
                Console.WriteLine($"Текущая температура: {_currentTemperature}");

                if(_currentTemperature > _maxTemperature){
                    OverHeat?.Invoke("Температура превысила максимальное значение");
                }
                if(_currentTemperature < _minTemperature){
                    OverCool?.Invoke("Температура опустилась ниже минимального значения");
                }
                Thread.Sleep(1000);
            }

        }
    }