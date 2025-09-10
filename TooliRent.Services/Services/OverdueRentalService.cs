using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Services.Services.Interfaces;

public class OverdueRentalService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OverdueRentalService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var now = DateTime.UtcNow;

                var overdueBookings = await db.Rentals
                    .Include(r => r.Customer)
                        .ThenInclude(c => c.User)
                    .Include(r => r.Tool)
                    .Where(r => r.EndDate <= now
                                && r.Status != RentalStatus.Overdue
                                && r.Status != RentalStatus.Returned
                                && (r.Status == RentalStatus.Pending || r.Status == RentalStatus.Confirmed || r.Status == RentalStatus.PickedUp))
                    .ToListAsync(stoppingToken);

                foreach (var rental in overdueBookings)
                {
                    rental.Status = RentalStatus.Overdue;
                    rental.ModifiedAt = now;

                    // Skicka e-post om kund finns
                    var user = rental.Customer?.User;
                    if (user?.Email != null)
                    {
                        string subject = "Din bokning har gått över tiden";
                        string body;

                        if (rental.Status == RentalStatus.Confirmed)
                        {
                            body = $"Hej {user.FirstName},\n\n" +
                                   $"Din bokning för verktyget '{rental.Tool?.Name}' skulle ha hämtats senast {rental.EndDate:u} " +
                                   "men den har ännu inte hämtats. Bokningen är nu markerad som försenad.";
                        }
                        else if (rental.Status == RentalStatus.Pending)
                        {
                            body = $"Hej {user.FirstName},\n\n" +
                                   $"Din bokning för verktyget '{rental.Tool?.Name}' har inte betalats i tid." +
                                   "Bokningen är nu markerad som försenad.";
                        }
                        else // PickedUp
                        {
                            body = $"Hej {user.FirstName},\n\n" +
                                   $"Din bokning för verktyget '{rental.Tool?.Name}' skulle ha återlämnats senast {rental.EndDate:u} " +
                                   "men den har ännu inte återlämnats. Bokningen är nu markerad som försenad.";
                        }

                        await emailService.SendEmailAsync(user.Email, subject, body);
                        Console.WriteLine($"Skickade mail till {user.Email}");
                    }
                }

                if (overdueBookings.Count > 0)
                    await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel i OverdueRentalService: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
