using System.Globalization;
using TicketBooking.Infrastructure.Dtos;

namespace TicketBooking.Domain.Validations
{
    public static class FlightValidator
    {
        public static IEnumerable<string> Validate(FlightCsvDto flight)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(flight.Price))
                errors.Add($"Price is required.");
            else if (!decimal.TryParse(flight.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                errors.Add($"Price must be castable as decimal.");
            else if (price <= 0)
                errors.Add($"Price must be greater than zero.");
            
            if (string.IsNullOrWhiteSpace(flight.DepartureCountry))
                errors.Add($"Departure country is required.");
            
            if (string.IsNullOrWhiteSpace(flight.DestinationCountry))
                errors.Add($"Destination country is required.");
            
            if (string.IsNullOrWhiteSpace(flight.DepartureDate))
                errors.Add($"Departure date is required.");
            else if (!DateTime.TryParse(flight.DepartureDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var departureDate))
                errors.Add($"Departure date must be castable as a valid date.");
            else if (departureDate < DateTime.Today)
                errors.Add($"Departure date must be today or in the future.");
            
            if (string.IsNullOrWhiteSpace(flight.DepartureAirport))
                errors.Add($"Departure airport is required.");
            
            if (string.IsNullOrWhiteSpace(flight.DestinationAirport))
                errors.Add($"Destination airport is required.");
            
            if (string.IsNullOrWhiteSpace(flight.FlightClass))
                errors.Add($"Flight class is required.");
            else if (!Enum.TryParse(typeof(TicketBooking.Domain.Models.FlightClass), flight.FlightClass, out _))
                errors.Add($"Flight class must be a valid value.");

            return errors;
        }
    }
}
