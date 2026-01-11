using TicketBooking.Domain.Models;

namespace TicketBooking.Domain.Services;

public interface IFlightService
{
    Task<IEnumerable<Flight>> GetFlightsAsync();
    
    Task<Flight?> GetFlightByIdAsync(long id);
}