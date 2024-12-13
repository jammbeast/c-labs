using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkladTests
{
    [TestFixture]
    public class LogistikaTests
    {
        private Sklad skladObshiy;
        private Sklad skladKholodny;
        private Sklad skladSortirovochnyy;
        private Sklad skladUtilizatsii;
        private List<Sklad> vseSklady;
        private List<Tovar> vseTovary;

        [SetUp]
        public void Setup()
        {
            // Инициализация складов
            skladObshiy = new Sklad(1, "общий", 1000, "Адрес Общего");
            skladKholodny = new Sklad(2, "холодный", 500, "Адрес Холодного");
            skladSortirovochnyy = new Sklad(3, "сортировочный", 800, "Адрес Сортировочного");
            skladUtilizatsii = new Sklad(4, "утилизация", 300, "Адрес Утилизации");

            vseSklady = new List<Sklad> { skladObshiy, skladKholodny, skladSortirovochnyy, skladUtilizatsii };

            // Инициализация товаров
            vseTovary = new List<Tovar>
            {
                new Tovar(1, 101, "Товар A", 50, 1000, 40),
                new Tovar(2, 102, "Товар B", 30, 800, 20),
                new Tovar(3, 103, "Товар C", 20, 500, -5) // Просроченный товар
            };
        }

        /// <summary>
        /// Тестирование метода поставки товаров.
        /// Вариант 1: Все товары имеют срок > 30 дней.
        /// Вариант 2: Все товары имеют срок < 30 дней.
        /// Вариант 3: Смешанные сроки.
        /// </summary>
        [Test]
        public void TestPostavkaTovarov()
        {
            // Вариант 1: Все товары > 30 дней
            var postavka1 = new List<Tovar>
            {
                new Tovar(4, 104, "Товар D", 60, 1200, 35),
                new Tovar(5, 105, "Товар E", 70, 1500, 40)
            };

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PostavkaTovarov(postavka1, vseSklady);
                string logOutput = sw.ToString().Trim();

                Assert.IsTrue(skladObshiy.Tovary.Contains(postavka1[0]));
                Assert.IsTrue(skladObshiy.Tovary.Contains(postavka1[1]));
                Assert.IsTrue(logOutput.Contains("Поставка товаров направлена на склад ID 1"));
            }

            // Вариант 2: Все товары < 30 дней
            var postavka2 = new List<Tovar>
            {
                new Tovar(6, 106, "Товар F", 40, 900, 10),
                new Tovar(7, 107, "Товар G", 50, 1100, 5)
            };

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PostavkaTovarov(postavka2, vseSklady);
                string logOutput = sw.ToString().Trim();

                Assert.IsTrue(skladKholodny.Tovary.Contains(postavka2[0]));
                Assert.IsTrue(skladKholodny.Tovary.Contains(postavka2[1]));
                Assert.IsTrue(logOutput.Contains("Поставка товаров направлена на склад ID 2"));
            }

            // Вариант 3: Смешанные сроки
            var postavka3 = new List<Tovar>
            {
                new Tovar(8, 108, "Товар H", 20, 600, 40),
                new Tovar(9, 109, "Товар I", 25, 700, -5) // Просроченный
            };

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PostavkaTovarov(postavka3, vseSklady);
                string logOutput = sw.ToString().Trim();

                Assert.IsTrue(skladSortirovochnyy.Tovary.Contains(postavka3[0]));
                Assert.IsTrue(skladSortirovochnyy.Tovary.Contains(postavka3[1]));
                Assert.IsTrue(logOutput.Contains("Поставка товаров направлена на склад ID 3"));
            }
        }

        /// <summary>
        /// Тестирование метода оптимизации перемещения товаров между складами.
        /// </summary>
        [Test]
        public void TestOptimizatsionnoePeremeshchenieTovarov()
        {
            // Добавляем товары в сортировочный склад
            var tovar1 = vseTovary[0]; // DniDoOkonchaniyaSroka = 40
            var tovar2 = vseTovary[1]; // DniDoOkonchaniyaSroka = 20
            var tovar3 = vseTovary[2]; // DniDoOkonchaniyaSroka = -5

            skladSortirovochnyy.DobavitTovar(tovar1);
            skladSortirovochnyy.DobavitTovar(tovar2);
            skladSortirovochnyy.DobavitTovar(tovar3);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.OptimizatsionnoePeremeshchenieTovarov(vseSklady);
                string logOutput = sw.ToString().Trim();

                // Проверяем, что товары переместились правильно
                Assert.IsFalse(skladSortirovochnyy.Tovary.Contains(tovar1));
                Assert.IsFalse(skladSortirovochnyy.Tovary.Contains(tovar2));
                Assert.IsFalse(skladSortirovochnyy.Tovary.Contains(tovar3));

                Assert.IsTrue(skladObshiy.Tovary.Contains(tovar1));
                Assert.IsTrue(skladKholodny.Tovary.Contains(tovar2));
                Assert.IsTrue(skladUtilizatsii.Tovary.Contains(tovar3));

                Assert.IsTrue(logOutput.Contains($"Товар: {tovar1.Nazvanie}, объем: {tovar1.ObemEdinitsy}, откуда: Склад {skladSortirovochnyy.IdSklada}, куда: Склад {skladObshiy.IdSklada}"));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovar2.Nazvanie}, объем: {tovar2.ObemEdinitsy}, откуда: Склад {skladSortirovochnyy.IdSklada}, куда: Склад {skladKholodny.IdSklada}"));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovar3.Nazvanie}, объем: {tovar3.ObemEdinitsy}, откуда: Склад {skladSortirovochnyy.IdSklada}, куда: Склад {skladUtilizatsii.IdSklada}"));
            }
        }

        /// <summary>
        /// Тестирование метода перемещения товаров между складами.
        /// </summary>
        [Test]
        public void TestPeremestitTovary()
        {
            // Перемещение с холодного на общий
            var tovarKholodny = new Tovar(10, 110, "Товар J", 50, 1000, 25);
            skladKholodny.DobavitTovar(tovarKholodny);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PeremestitTovary(new List<Tovar> { tovarKholodny }, skladKholodny, skladObshiy);
                string logOutput = sw.ToString().Trim();

                Assert.IsFalse(skladKholodny.Tovary.Contains(tovarKholodny));
                Assert.IsTrue(skladObshiy.Tovary.Contains(tovarKholodny));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovarKholodny.Nazvanie}, объем: {tovarKholodny.ObemEdinitsy}, откуда: Склад {skladKholodny.IdSklada}, куда: Склад {skladObshiy.IdSklada}"));
            }

            // Перемещение с холодного на сортировочный
            var tovarKholodny2 = new Tovar(11, 111, "Товар K", 30, 900, 15);
            skladKholodny.DobavitTovar(tovarKholodny2);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PeremestitTovary(new List<Tovar> { tovarKholodny2 }, skladKholodny, skladSortirovochnyy);
                string logOutput = sw.ToString().Trim();

                Assert.IsFalse(skladKholodny.Tovary.Contains(tovarKholodny2));
                Assert.IsTrue(skladSortirovochnyy.Tovary.Contains(tovarKholodny2));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovarKholodny2.Nazvanie}, объем: {tovarKholodny2.ObemEdinitsy}, откуда: Склад {skladKholodny.IdSklada}, куда: Склад {skladSortirovochnyy.IdSklada}"));
            }

            // Перемещение с общего на утилизационный
            var tovarObshiy = new Tovar(12, 112, "Товар L", 40, 1100, -10);
            skladObshiy.DobavitTovar(tovarObshiy);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PeremestitTovary(new List<Tovar> { tovarObshiy }, skladObshiy, skladUtilizatsii);
                string logOutput = sw.ToString().Trim();

                Assert.IsFalse(skladObshiy.Tovary.Contains(tovarObshiy));
                Assert.IsTrue(skladUtilizatsii.Tovary.Contains(tovarObshiy));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovarObshiy.Nazvanie}, объем: {tovarObshiy.ObemEdinitsy}, откуда: Склад {skladObshiy.IdSklada}, куда: Склад {skladUtilizatsii.IdSklada}"));
            }

            // Перемещение с общего на холодный
            var tovarObshiy2 = new Tovar(13, 113, "Товар M", 20, 700, 50);
            skladObshiy.DobavitTovar(tovarObshiy2);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PeremestitTovary(new List<Tovar> { tovarObshiy2 }, skladObshiy, skladKholodny);
                string logOutput = sw.ToString().Trim();

                Assert.IsFalse(skladObshiy.Tovary.Contains(tovarObshiy2));
                Assert.IsTrue(skladKholodny.Tovary.Contains(tovarObshiy2));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovarObshiy2.Nazvanie}, объем: {tovarObshiy2.ObemEdinitsy}, откуда: Склад {skladObshiy.IdSklada}, куда: Склад {skladKholodny.IdSklada}"));
            }

            // Перемещение с общего на сортировочный
            var tovarObshiy3 = new Tovar(14, 114, "Товар N", 25, 750, 30);
            skladObshiy.DobavitTovar(tovarObshiy3);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PeremestitTovary(new List<Tovar> { tovarObshiy3 }, skladObshiy, skladSortirovochnyy);
                string logOutput = sw.ToString().Trim();

                Assert.IsFalse(skladObshiy.Tovary.Contains(tovarObshiy3));
                Assert.IsTrue(skladSortirovochnyy.Tovary.Contains(tovarObshiy3));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovarObshiy3.Nazvanie}, объем: {tovarObshiy3.ObemEdinitsy}, откуда: Склад {skladObshiy.IdSklada}, куда: Склад {skladSortirovochnyy.IdSklada}"));
            }
        }

        /// <summary>
        /// Тестирование метода анализа складской сети.
        /// </summary>
        [Test]
        public void TestAnalizSkladskoySeti()
        {
            // Добавляем товары с различными условиями
            var tovar1 = new Tovar(15, 115, "Товар O", 60, 1300, 40); // Общий товар на общем складе
            var tovar2 = new Tovar(16, 116, "Товар P", 35, 900, 10);  // Холодный товар на общем складе
            var tovar3 = new Tovar(17, 117, "Товар Q", 45, 1000, 40); // Общий товар на сортировочном складе
            var tovar4 = new Tovar(18, 118, "Товар R", 20, 600, -2);  // Просроченный товар

            skladObshiy.DobavitTovar(tovar1);
            skladObshiy.DobavitTovar(tovar2);
            skladSortirovochnyy.DobavitTovar(tovar3);
            skladSortirovochnyy.DobavitTovar(tovar4);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.AnalizSkladskoySeti(vseSklady);
                string logOutput = sw.ToString().Trim();

                // Проверяем вывод логов
                Assert.IsTrue(logOutput.Contains("Склад ID 1: Нарушения есть"));
                Assert.IsTrue(logOutput.Contains("Склад ID 3: Нарушения есть"));
                Assert.IsTrue(logOutput.Contains("Склад ID 2: Нарушений нет"));
                Assert.IsTrue(logOutput.Contains("Склад ID 4: Нарушений нет"));
            }
        }

        /// <summary>
        /// Тестирование метода перемещения товаров с истекшим сроком годности.
        /// </summary>
        [Test]
        public void TestPeremestitProsrochennyeTovary()
        {
            // Добавляем просроченные товары в разные склады
            var tovar1 = new Tovar(19, 119, "Товар S", 30, 800, -1);
            var tovar2 = new Tovar(20, 120, "Товар T", 25, 700, -3);

            skladObshiy.DobavitTovar(tovar1);
            skladSortirovochnyy.DobavitTovar(tovar2);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PeremestitProsrochennyeTovary(vseSklady, skladUtilizatsii);
                string logOutput = sw.ToString().Trim();

                // Проверяем, что товары переместились на склад утилизации
                Assert.IsFalse(skladObshiy.Tovary.Contains(tovar1));
                Assert.IsFalse(skladSortirovochnyy.Tovary.Contains(tovar2));

                Assert.IsTrue(skladUtilizatsii.Tovary.Contains(tovar1));
                Assert.IsTrue(skladUtilizatsii.Tovary.Contains(tovar2));

                Assert.IsTrue(logOutput.Contains($"Товар: {tovar1.Nazvanie}, объем: {tovar1.ObemEdinitsy}, откуда: Склад {skladObshiy.IdSklada}, куда: Склад утилизации {skladUtilizatsii.IdSklada}"));
                Assert.IsTrue(logOutput.Contains($"Товар: {tovar2.Nazvanie}, объем: {tovar2.ObemEdinitsy}, откуда: Склад {skladSortirovochnyy.IdSklada}, куда: Склад утилизации {skladUtilizatsii.IdSklada}"));
            }
        }

        /// <summary>
        /// Тестирование подсчёта стоимости товаров на складе.
        /// </summary>
        [Test]
        public void TestPodschetStoimostiTovarovNaSklade()
        {
            // Добавляем товары на склад
            var tovar1 = new Tovar(21, 121, "Товар U", 40, 900, 20);
            var tovar2 = new Tovar(22, 122, "Товар V", 60, 1100, 30);
            skladObshiy.DobavitTovar(tovar1);
            skladObshiy.DobavitTovar(tovar2);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Logistika.PodschetStoimostiTovarovNaSklade(skladObshiy);
                string logOutput = sw.ToString().Trim();

                double expectedTotal = tovar1.TsenaEdinitsy + tovar2.TsenaEdinitsy;
                Assert.IsTrue(logOutput.Contains($"Общая стоимость товаров на складе ID {skladObshiy.IdSklada}: {expectedTotal}"));
            }
        }
    }
}