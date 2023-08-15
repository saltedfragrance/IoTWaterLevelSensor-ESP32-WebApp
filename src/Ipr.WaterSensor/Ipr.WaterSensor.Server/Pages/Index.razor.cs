﻿using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Infrastructure.Data;
using Ipr.WaterSensor.Server.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text;
using static MudBlazor.Colors;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public MQTTService MQTTService { get; set; } = default!;
        [Inject]
        protected IDbContextFactory<WaterSensorDbContext> DbContextFactory { get; set; } = default!;
        public WaterLevel CurrentWaterLevel { get; set; }
        public double CurrentWaterPercentage { get; set; }
        public TankStatistics TankStatistics { get; set; }
        public WaterTank CurrentWaterTank { get; set; } = default!;
        public FireBeetle FireBeetleDevice { get; set; } = default!;

        private async Task Initialize()
        {
            if (!MQTTService.ClientStarted)
            {
                await MQTTService.StartClient();
                MQTTService.MqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    var measurement = Encoding.Default.GetString(e.ApplicationMessage.Payload);
                    if (e.ApplicationMessage.Topic == MQTTService.topicMainTank)
                    {
                        UpdateWaterTankLevel(measurement);
                    }

                    if (e.ApplicationMessage.Topic == MQTTService.topicBatteryLevel)
                    {
                        UpdateBatteryLevel(measurement);
                    }

                    if (e.ApplicationMessage.Topic == MQTTService.topicIntervalReceive)
                    {
                        if (measurement != CurrentWaterTank.CurrentUpdateIntervalMicroSeconds.ToString())
                        {
                            PublishNewInterval(CurrentWaterTank.CurrentUpdateIntervalMicroSeconds);
                        }
                    }
                    return Task.CompletedTask;
                };
            }
            await GetData();
        }
        private async Task GetData()
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                CurrentWaterTank = await context.WaterTanks.Include(tank => tank.WaterLevels).FirstOrDefaultAsync();
                FireBeetleDevice = await context.FireBeetleDevice.FirstOrDefaultAsync();
                TankStatistics = await context.TankStatistics.Where(stat => stat.WaterTankId == CurrentWaterTank.Id).FirstOrDefaultAsync();
            }
            UpdateWaterPercentage();
        }

        private async Task SaveInterval()
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                var toUpdate = context.WaterTanks.FirstOrDefault(tank => tank.Id == CurrentWaterTank.Id);
                toUpdate.CurrentUpdateIntervalMicroSeconds = CurrentWaterTank.CurrentUpdateIntervalMicroSeconds;
                await context.SaveChangesAsync();
            }
            await GetData();
        }

        private async Task PublishNewInterval(double newInterval)
        {
            string intervalMicroseconds = newInterval.ToString();

            byte[] bytes = Encoding.UTF8.GetBytes(intervalMicroseconds);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(MQTTService.topicIntervalSend)
                .WithPayloadSegment(bytes)
            .Build();

            await MQTTService.MqttClient.PublishAsync(applicationMessage, CancellationToken.None);
        }


        private void UpdateWaterPercentage()
        {
            CurrentWaterLevel = CurrentWaterTank.WaterLevels.OrderByDescending(level => level.DateTimeMeasured).First();
            CurrentWaterPercentage = (Math.Round(CurrentWaterLevel.Percentage, 2));
        }
        private async Task UpdateWaterTankLevel(string measuredValue)
        {
            var newVolume = CurrentWaterTank.Radius * ((CurrentWaterTank.Height + 60) - Convert.ToDouble(measuredValue));
            var newPercentage = (newVolume / CurrentWaterTank.Volume) * 100;

            if (newPercentage > 100) newPercentage = 100;
            else if (newPercentage < 0) newPercentage = 0;

            var newWaterLevel = new WaterLevel
            {
                DateTimeMeasured = DateTime.Now,
                Id = Guid.NewGuid(),
                Percentage = newPercentage,
                WaterTankId = CurrentWaterTank.Id
            };

            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                await context.AddAsync(newWaterLevel);
                await context.SaveChangesAsync();
            }
            UpdateWaterPercentage();
            await UpdateStatistics(newPercentage);
        }

        private async Task UpdateBatteryLevel(string measuredValue)
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                context.FireBeetleDevice.First().BatteryPercentage = Convert.ToDouble(measuredValue);
                context.FireBeetleDevice.First().DateTimeMeasured = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }
        private string GetWaterLevelPixels(double percentage)
        {
            var pixels = (210 - (percentage * 2) - 35).ToString() + "px";
            return pixels;
        }

        private async Task UpdateStatistics(double newPercentage)
        {
            if (TankStatistics == null)
            {
                TankStatistics = new TankStatistics
                {
                    Id = Guid.NewGuid(),
                    Month = DateTime.Now.Month,
                    WaterTankId = CurrentWaterTank.Id,
                    Year = DateTime.Now.Year
                };

                using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
                {
                    await context.AddAsync(TankStatistics);
                    await context.SaveChangesAsync();
                }
            }

            if (CurrentWaterTank.WaterLevels.Count > 0)
            {
                var previousWaterLevel = CurrentWaterTank.WaterLevels.Where(level => level.DateTimeMeasured.Day == (DateTime.Now.Day - 1)).FirstOrDefault();
                if (previousWaterLevel != null)
                {
                    var previousPercentage = previousWaterLevel.Percentage;
                    if (previousPercentage < newPercentage)
                    {
                        var litersConsumed = (newPercentage - previousPercentage) * 100;

                        using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
                        {
                            TankStatistics.TotalWaterConsumed += litersConsumed;
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private double GetCurrentWaterTankUpdateIntervalMinutes()
        {
            return (CurrentWaterTank.CurrentUpdateIntervalMicroSeconds / 60000000);
        }
        private async Task SetCurrentWaterTankUpdateIntervalMinutes(double newInterval)
        {
            CurrentWaterTank.CurrentUpdateIntervalMicroSeconds = ((newInterval * 60000000));
        }
    }
}
