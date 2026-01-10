namespace TicketBooking.Domain.Models;

public record Flight
{
    public decimal Price { get; init; }
    public string DepartureCountry { get; init; } = string.Empty;
    public string DestinationCountry { get; init; } = string.Empty;
    public DateTime DepartureDate { get; init; }
    public string DepartureAirport { get; init; } = string.Empty;
    public string DestinationAirport { get; init; } = string.Empty;
    public FlightClass FlightClass { get; init; }
}