using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sakuno.KanColle.Amatsukaze.Game.Events;
using Sakuno.KanColle.Amatsukaze.Game.Models;

namespace Sakuno.KanColle.Amatsukaze.Game.Test
{
    [TestClass]
    public class MasterDataTest
    {
        private static MasterDataUpdate parseResult;
        [ClassInitialize]
        public static void LoadData(TestContext context)
        {
            var provider = new UnitTestProvider();
            var gameListener = new GameListener(provider);

            gameListener.MasterDataUpdated.Received += u => parseResult = u;

            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(typeof(MasterDataTest), "Data.masterdata.json"))
                provider.Push("api_start2", DateTimeOffset.Now, string.Empty, stream);
        }
        [TestMethod]
        public void TestDataLoading()
        {
            Assert.IsNotNull(parseResult);
        }
        [TestMethod]
        public void TestShipInfoFieldMap()
        {
            var mutsuki = parseResult.ShipInfos.First();
            Assert.AreEqual(mutsuki.Id, 1);
            Assert.AreEqual(mutsuki.Name, "����");
            Assert.AreEqual(mutsuki.Phonetic, "��Ĥ�");
            Assert.IsFalse(mutsuki.IsAbyssal);
            Assert.IsTrue(string.IsNullOrEmpty(mutsuki.AbyssalClass));
            Assert.AreEqual(mutsuki.TypeId, 2);
            Assert.AreEqual(mutsuki.ClassId, 28);
            Assert.AreEqual(mutsuki.UpgradeConsumption.Bullet, 100);
            Assert.AreEqual(mutsuki.UpgradeTo, 254);
            Assert.AreEqual(mutsuki.UpgradeLevel, 20);
            Assert.AreEqual(mutsuki.Speed, ShipSpeed.Fast);
            Assert.AreEqual(mutsuki.FireRange, FireRange.Short);
            Assert.AreEqual(mutsuki.SlotCount, 2);
            Assert.AreEqual(mutsuki.ConstructionTime, TimeSpan.FromMinutes(18));
            Assert.AreEqual(mutsuki.DismantleAcquirement.Fuel, 1);
            CollectionAssert.AreEqual(mutsuki.PowerupWorth.ToArray(), new[] { 1, 1, 0, 0 });

            var fuso = parseResult.ShipInfos.Single(x => x.Id == 26);
            CollectionAssert.AreEqual(fuso.Aircraft.ToArray(), new[] { 3, 3, 3, 3, 0 });
        }
        [TestMethod]
        public void TestAbyssal()
        {
            var i = parseResult.ShipInfos.Single(x => x.Id == 1514);
            Assert.IsTrue(i.IsAbyssal);
            Assert.AreEqual(i.AbyssalClass, "elite");
            Assert.IsTrue(string.IsNullOrEmpty(i.Phonetic));
            Assert.IsNull(i.Introduction);
        }
        [TestMethod]
        public void TestArrayToMaterials()
        {
            var mustuki = parseResult.ShipInfos.First();
            Assert.AreEqual(mustuki.DismantleAcquirement.Fuel, 1);
            Assert.AreEqual(mustuki.DismantleAcquirement.Bullet, 1);
            Assert.AreEqual(mustuki.DismantleAcquirement.Steel, 4);
        }
        [TestMethod]
        public void TestArrayToMordenize()
        {
            var mutsuki = parseResult.ShipInfos.First();
            Assert.AreEqual(mutsuki.HP.Min, 13);
            Assert.AreEqual(mutsuki.HP.Current, 13);
            Assert.AreEqual(mutsuki.HP.Max, 24);
            Assert.AreEqual(mutsuki.Armor.Max, 18);
            Assert.AreEqual(mutsuki.Firepower.Max, 29);
            Assert.AreEqual(mutsuki.Torpedo.Max, 59);
            Assert.AreEqual(mutsuki.AntiAir.Max, 29);
            Assert.AreEqual(mutsuki.Luck.Max, 49);
        }
        [TestMethod]
        public void TestBrEater()
        {
            foreach (var s in parseResult.ShipInfos)
                if (!s.IsAbyssal)
                    Assert.IsFalse(s.Introduction.Contains("<br>"));
        }
    }
}
