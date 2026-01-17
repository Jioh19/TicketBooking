using CsvHelper.Configuration;

namespace TicketBooking.Infrastructure.Dtos
{
    public record FlightCsvDto
    {
        public string Price { get; init; } = string.Empty;
        public string DepartureCountry { get; init; } = string.Empty;
        public string DestinationCountry { get; init; } = string.Empty;
        public string DepartureDate { get; init; } = string.Empty;
        public string DepartureAirport { get; init; } = string.Empty;
        public string DestinationAirport { get; init; } = string.Empty;
        public string FlightClass { get; init; } = string.Empty;
    }

    public sealed class FlightCsvDtoMap : ClassMap<FlightCsvDto>
    {
        public FlightCsvDtoMap()
        {
            Map(m => m.Price).Name("Price");
            Map(m => m.DepartureCountry).Name("DepartureCountry");
            Map(m => m.DestinationCountry).Name("DestinationCountry");
            Map(m => m.DepartureDate).Name("DepartureDate");
            Map(m => m.DepartureAirport).Name("DepartureAirport");
            Map(m => m.DestinationAirport).Name("DestinationAirport");
            Map(m => m.FlightClass).Name("FlightClass");
        }
    }
}

