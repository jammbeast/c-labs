using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Warehousing.Models;

namespace Test5Lab
{
 [TestFixture]
 public class SkladTests
 {
    private WareHouse wareHouse;
    private Tovar tovar;
    [SetUp]
    public void Setup(){
        wareHouse = new WareHouse(1, "Общий", 1000, "Адресс Общего", new List<Tovar>());
        tovar = new Tovar(1, 101, "Товар A", 50, 1000, 40);
    }

    [Test]
    public void DobatTovarNaSklad(){
        wareHouse.AddTovar(tovar);
        wareHouse.RemoveTovar(tovar);
         Assert.That(wareHouse.Tovars.Contains(tovar), "Товар должен быть удалён со склада.");
    }
 }
}