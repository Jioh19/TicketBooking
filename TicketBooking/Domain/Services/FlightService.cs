using TicketBooking.Domain.Models;
using TicketBooking.Domain.Repositories;

namespace TicketBooking.Domain.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;

    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    public async Task<IEnumerable<Flight>> GetFlightsAsync()
    {
        return await _flightRepository.GetAllAsync();
    }

    public async Task<Flight?> GetFlightByIdAsync(long id)
    {
        var flight = await _flightRepository.GetByIdAsync(id);
        if (flight is null)
        {
            throw new Exception($"Flight with id {id} not found");
        }
        return flight;
    }
}