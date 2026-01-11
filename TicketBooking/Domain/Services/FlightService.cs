using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
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

    public async Task<IEnumerable<Flight>> GetFlightsByParametersAsync(
        string? origin = null,
        string? destination = null,
        FlightClass? flightClass = null)
    {
        var flights = await _flightRepository.GetAllAsync();
        return flights.Where(f =>
            (origin == null || string.Equals(f.DepartureCountry, origin, StringComparison.OrdinalIgnoreCase)) &&
            (destination == null || string.Equals(f.DestinationCountry, destination, StringComparison.OrdinalIgnoreCase)) &&
            (flightClass == null || f.FlightClass == flightClass)
        );
    }

    public async Task<IEnumerable<string>> GetAllOriginsAsync()
    {
        var flights = await _flightRepository.GetAllAsync();
        return flights.Where(f => !string.IsNullOrWhiteSpace(f.DepartureCountry)).Select(f => f.DepartureCountry).Distinct();
    }

    public async Task<IEnumerable<string>> GetAllDestinationsAsync()
    {
        var flights = await _flightRepository.GetAllAsync();
        return flights.Where(f => !string.IsNullOrWhiteSpace(f.DestinationCountry)).Select(f => f.DestinationCountry).Distinct();
    }
}