using TicketBooking.Domain.Models;

namespace TicketBooking.Domain.Services;

public interface IFlightService
{
    Task<IEnumerable<Flight>> GetFlightsAsync();
    
    Task<Flight?> GetFlightByIdAsync(long id);
    
    Task<IEnumerable<Flight>> GetFlightsByParametersAsync(string? origin, string? destination, FlightClass? flightClass);
    
    Task<IEnumerable<string>> GetAllOriginsAsync();
    
    Task<IEnumerable<string>> GetAllDestinationsAsync();
}