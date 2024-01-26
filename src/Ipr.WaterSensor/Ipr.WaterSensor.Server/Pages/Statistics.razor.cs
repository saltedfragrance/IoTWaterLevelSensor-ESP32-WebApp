using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Axes.Ticks;
using ChartJs.Blazor.PieChart;
using ChartJs.Blazor.Util;
using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Infrastructure.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Animation = ChartJs.Blazor.Common.Animation;
using Color = System.Drawing.Color;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Statistics : ComponentBase
    {
        [Inject]
        protected IDbContextFactory<WaterSensorDbContext> DbContextFactory { get; set; } = default!;
        public List<WaterTank> Tanks { get; set; } = default!;
        public Guid CurrentSelectedTankId { get; set; }
        public int CurrentSelectedYear { get; set; }
        public List<TankStatistics> TankStatistics { get; set; }
        public BarConfig ChartConfig { get; set; }
        private async Task GetData()
        {
            using (WaterSensorDbContext context = DbContextFactory.CreateDbContext())
            {
                Tanks = await context.WaterTanks.ToListAsync();
                if (CurrentSelectedTankId == Guid.Empty)
                {
                    CurrentSelectedTankId = Tanks.FirstOrDefault().Id;
                }
                if (CurrentSelectedYear == 0)
                {
                    CurrentSelectedYear = DateTime.Now.Year;
                }
                TankStatistics = await context.TankStatistics.Where(stat => stat.WaterTankId == CurrentSelectedTankId && stat.Year == CurrentSelectedYear).ToListAsync();
            }
        }
        private async Task InitializeChart()
        {
            if (TankStatistics.Count > 0)
            {
                ChartConfig = new BarConfig(horizontal: true)
                {
                    Options = new BarOptions
                    { 
                        Responsive = true,
                        Title = new OptionsTitle
                        {
                            Display = true,
                            Text = "Verbruik per maand"
                        },
                        Animation = new Animation { Duration = 500, Easing = ChartJs.Blazor.Common.Enums.Easing.EaseInCubic },
                        AspectRatio = 1,
                        Legend = new Legend { Display = false },
                        Scales = new BarScales
                        {
                            XAxes = new List<CartesianAxis>
                         {
                             new LinearCartesianAxis
                             {
                                 ScaleLabel = new ScaleLabel
                                 {
                                      Display = true,
                                      LabelString = "Aantal liter",
                                      FontSize = 15
                                 },
                                 Ticks = new LinearCartesianTicks
                                 {
                                     BeginAtZero = true
                                 }
                             }
                         },
                        }
                    }
                };

                foreach (var monthNumber in TankStatistics.Select(stat => stat.Month).OrderByDescending(x => x).ToList())
                {
                    var monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(monthNumber);
                    ChartConfig.Data.Labels.Add(monthName);
                }

                var orderedData = TankStatistics.OrderByDescending(stat => stat.Month).ToList().Select(y => y.TotalWaterConsumed).ToList();

                IDataset<double> dataset = new BarDataset<double>(orderedData, horizontal: true)
                {
                    BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.DodgerBlue)),
                    BarThickness = BarThickness.Absolute(30)
                     
                };

                ChartConfig.Data.Datasets.Add(dataset);
            }
        }

        private async Task ChangeYear(bool nextYear)
        {
            if (nextYear)
            {
                CurrentSelectedYear += 1;
            }
            else
            {
                CurrentSelectedYear -= 1;
            }
            ChartConfig = null;
            TankStatistics.Clear();
            await GetData();
            await InitializeChart();
        }
    }
}
