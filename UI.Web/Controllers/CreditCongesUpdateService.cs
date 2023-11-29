using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ApplicationCore.Interfaces;
using ApplicationCore.Domain;

namespace UI.Web.Controllers
{
    public class CreditCongesUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        IServiceSoldeConges ssc;

        public CreditCongesUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Update every Second
            TimeSpan bySecond = TimeSpan.FromSeconds(1);
            decimal x = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        ssc = scope.ServiceProvider.GetRequiredService<IServiceSoldeConges>();
                        var soldeCongesList = ssc.GetMany();

                        if (soldeCongesList != null)
                        {
                            var activeSoldeConges = soldeCongesList.FirstOrDefault(e => e.IsActivated);

                            if (activeSoldeConges != null)
                            {
                                x = activeSoldeConges.Nombre;
                            }
                            else
                            {
                                x = 0;
                            }
                        }
                        else
                        {
                            x = 0; // Handle the case where GetMany() returns null
                        }
                    }
                    catch (Exception ex)
                    {

                        x = 0; 
                    }

                    var se = scope.ServiceProvider.GetRequiredService<IServiceEmployees>();
                    var employees = se.GetMany().ToList();
                    
                    foreach (var employee in employees)
                    {
                        employee.CreditConges += x;
                        se.Update(employee);
                    }

                    se.Commit();
                }

                // Wait for the next month
                var now = DateTime.Now;
                var nextMonthStart = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                var byMonth = nextMonthStart - now;

                await Task.Delay(byMonth, stoppingToken);
            }
        }
    }
}
