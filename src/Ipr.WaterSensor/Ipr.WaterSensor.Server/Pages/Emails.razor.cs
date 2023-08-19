using Ipr.WaterSensor.Core.Entities;
using Ipr.WaterSensor.Core.Enums;
using Ipr.WaterSensor.Infrastructure.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Ipr.WaterSensor.Server.Pages
{
    public partial class Emails : ComponentBase
    {
        [Inject]
        protected IDbContextFactory<WaterSensorDbContext> DbContextFactory { get; set; } = default!;
        public Person NewPerson { get; set; }
        public List<Person> People { get; set; }
        private async Task GetData()
        {
            using (WaterSensorDbContext context = await DbContextFactory.CreateDbContextAsync())
            {
                People = context.People.Include(p => p.SubscribedEmails).ToList();
            }
        }

        private async Task SavePerson()
        {
            using (WaterSensorDbContext context = await DbContextFactory.CreateDbContextAsync())
            {
                NewPerson.Id = Guid.NewGuid();
                NewPerson.SubscribedEmails = new List<AlarmEmail>
                {
                    new AlarmEmail {  AlarmType = EmailTypes.Batterij, IsEnabled = false, PersonId = NewPerson.Id, Id = Guid.NewGuid() },
                    new AlarmEmail {  AlarmType = EmailTypes.Waterniveau, IsEnabled = false, PersonId = NewPerson.Id, Id = Guid.NewGuid() }
                };
                context.Add(NewPerson);
                await context.SaveChangesAsync();
            }
            await GetData();
        }

        private async Task SavePreferences(string emailType, bool enabled, Guid personId)
        {
            using (WaterSensorDbContext context = await DbContextFactory.CreateDbContextAsync())
            {
                var toUpdate = context.People.Where(p => p.Id == personId).Include(x => x.SubscribedEmails).First();
                if(enabled)
                {
                    toUpdate.SubscribedEmails.Where(p => p.AlarmType.ToString() == emailType).First().IsEnabled = true;
                }
                else
                {
                    toUpdate.SubscribedEmails.Where(p => p.AlarmType.ToString() == emailType).First().IsEnabled = false;
                }
                context.Update(toUpdate);
                await context.SaveChangesAsync();
                await GetData();
            }
        }
    }
}
