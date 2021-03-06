﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace UnitTest.Rollbar.Telemetry
{
    using global::Rollbar.Telemetry;
    using dto = global::Rollbar.DTOs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory(nameof(TelemetryCollectorFixture))]
    public class TelemetryCollectorFixture
    {
        [TestInitialize]
        public void SetupFixture()
        {
            //SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [TestCleanup]
        public void TearDownFixture()
        {

        }

        public IReadOnlyCollection<dto.Telemetry> GenerateTelemetryItems()
        {
            List<dto.Telemetry> result = new List<dto.Telemetry>();

            dto.Telemetry telemetry = null;

            telemetry = new dto.Telemetry(
                dto.TelemetrySource.Client
                , dto.TelemetryLevel.Critical
                , new dto.LogTelemetry("test")
                );
            result.Add(telemetry);

            telemetry = new dto.Telemetry(
                dto.TelemetrySource.Client
                , dto.TelemetryLevel.Critical
                , new dto.ErrorTelemetry(new Exception("Test exception"))
                );
            result.Add(telemetry);

            telemetry = new dto.Telemetry(
                dto.TelemetrySource.Client
                , dto.TelemetryLevel.Critical
                , new dto.DomTelemetry("TestElement")
                );
            result.Add(telemetry);

            telemetry = new dto.Telemetry(
                dto.TelemetrySource.Client
                , dto.TelemetryLevel.Critical
                , new dto.NavigationTelemetry("Here", "There")
                );
            result.Add(telemetry);

            telemetry = new dto.Telemetry(
                dto.TelemetrySource.Client
                , dto.TelemetryLevel.Critical
                , new dto.NetworkTelemetry("GET", "api/users", DateTime.Now, null, 200)
                );
            result.Add(telemetry);

            var custom = new Dictionary<string, object>
            {
                { "key1", "firstValue" },
                { "key2", "secondValue" },
            };
            telemetry = new dto.Telemetry(
                dto.TelemetrySource.Client
                , dto.TelemetryLevel.Critical
                , new dto.ManualTelemetry(custom)
                );
            result.Add(telemetry);

            return result;
        }

        [TestMethod]
        public void TestTelemetryEnabling()
        {
            TelemetryCollector.Instance.FlushQueue();
            Assert.IsFalse(TelemetryCollector.Instance.IsAutocollecting);

            var config = TelemetryCollector.Instance.Config.Reconfigure(new TelemetryConfig(false, 10));

            Assert.IsFalse(config.TelemetryEnabled);
            Assert.AreEqual(0, TelemetryCollector.Instance.GetItemsCount());

            var telemetryItems = GenerateTelemetryItems();
            Assert.IsTrue(telemetryItems.Count > 0);

            foreach (var item in telemetryItems)
            {
                TelemetryCollector.Instance.Capture(item);
            }
            Assert.AreEqual(0, TelemetryCollector.Instance.GetItemsCount());

            config.Reconfigure(new TelemetryConfig(true, telemetryItems.Count));
            Assert.IsTrue(config.TelemetryEnabled);
            Assert.AreEqual(0, TelemetryCollector.Instance.GetItemsCount());

            foreach (var item in telemetryItems)
            {
                TelemetryCollector.Instance.Capture(item);
            }
            Assert.IsTrue(TelemetryCollector.Instance.GetItemsCount() > 0);
            Assert.AreEqual(telemetryItems.Count, TelemetryCollector.Instance.GetItemsCount());
        }

        [TestMethod]
        public void TestTelemetryFixedQueueDepth()
        {
            TelemetryCollector.Instance.FlushQueue();
            Assert.IsFalse(TelemetryCollector.Instance.IsAutocollecting);

            var config = TelemetryCollector.Instance.Config;
            var config1 = new TelemetryConfig(true, 10);
            config.Reconfigure(config1);

            List<dto.Telemetry> telemetryItems = new List<dto.Telemetry>(2 * config.TelemetryQueueDepth);
            while(telemetryItems.Count <= config.TelemetryQueueDepth)
            {
                telemetryItems.AddRange(GenerateTelemetryItems());
            }

            Assert.IsTrue(config.TelemetryEnabled);
            Assert.AreEqual(0, TelemetryCollector.Instance.GetItemsCount());
            Assert.IsTrue(telemetryItems.Count > config.TelemetryQueueDepth);
            foreach (var item in telemetryItems)
            {
                TelemetryCollector.Instance.Capture(item);
            }
            Assert.AreEqual(config.TelemetryQueueDepth, TelemetryCollector.Instance.GetItemsCount());
            int oldCount = TelemetryCollector.Instance.GetItemsCount();

            TelemetryCollector.Instance.FlushQueue();
            Assert.AreEqual(0, TelemetryCollector.Instance.GetItemsCount());
            var config2 = new TelemetryConfig(true, config1.TelemetryQueueDepth / 2);
            config.Reconfigure(config2);
            foreach (var item in telemetryItems)
            {
                TelemetryCollector.Instance.Capture(item);
            }
            Assert.AreEqual(config.TelemetryQueueDepth, TelemetryCollector.Instance.GetItemsCount());
            Assert.IsTrue(oldCount > TelemetryCollector.Instance.GetItemsCount());
        }

        //NOTE: keep this one disabled until we decide on what telemetry to auto collect and
        //      implement the auto collection...
        //[TestMethod]
        //[Timeout(150000)]
        //public void BasicTest()
        //{
        //    Assert.IsFalse(TelemetryCollector.Instance.IsAutocollecting);

        //    var config = TelemetryCollector.Instance.Config;

        //    Thread.Sleep(TimeSpan.FromMilliseconds(config.TelemetryCollectionInterval.TotalMilliseconds * config.TelemetryQueueDepth));
        //    Assert.IsTrue(!config.TelemetryEnabled);
        //    Assert.AreEqual(0, TelemetryCollector.Instance.TelemetryQueue.GetItemsCount());
        //    foreach (var item in TelemetryCollector.Instance.TelemetryQueue.GetQueueContent())
        //    {
        //        Console.WriteLine(item);
        //    }

        //    TelemetryCollector.Instance.StopAutocollection(true);
        //    Assert.IsTrue(!TelemetryCollector.Instance.IsAutocollecting);
        //    config.TelemetryEnabled = true;
        //    config.TelemetryCollectionInterval = TimeSpan.FromSeconds(1);
        //    config.TelemetryQueueDepth = 20;
        //    TelemetryCollector.Instance.StartAutocollection();
        //    Assert.IsTrue(TelemetryCollector.Instance.IsAutocollecting);

        //    Thread.Sleep(TimeSpan.FromMilliseconds(config.TelemetryCollectionInterval.TotalMilliseconds * config.TelemetryQueueDepth *2));
        //    Assert.AreEqual(config.TelemetryQueueDepth, TelemetryCollector.Instance.TelemetryQueue.GetItemsCount());
        //    foreach (var item in TelemetryCollector.Instance.TelemetryQueue.GetQueueContent())
        //    {
        //        Console.WriteLine(item);
        //    }


        //}

    }
}
