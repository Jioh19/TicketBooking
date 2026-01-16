using CsvHelper.Configuration;

namespace TicketBooking.Infrastructure.Dtos;

public record BookingCsvDto
{
    public string Id { get; init; } = string.Empty;
    public string FlightId { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
}

public sealed class BookingCsvDtoMap : ClassMap<BookingCsvDto>
{
    public BookingCsvDtoMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.FlightId).Name("FlightId");
        Map(m => m.UserId).Name("UserId");
        Map(m => m.State).Name("State");
    }
}

