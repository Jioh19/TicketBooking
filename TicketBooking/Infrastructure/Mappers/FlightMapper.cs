using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Validations;
using TicketBooking.Infrastructure.Dtos;

namespace TicketBooking.Infrastructure.Mappers
{
    public static class FlightMapper
    {
        public static Flight ToDomain(FlightCsvDto dto, int id)
        {
            var errors = FlightValidator.Validate(dto).ToList();
            if (errors.Count is not 0)
            {
                throw new ValidationException(string.Join("; ", errors));
            }
            
            var price = decimal.Parse(dto.Price, NumberStyles.Any, CultureInfo.InvariantCulture);
            var departureDate = DateTime.Parse(dto.DepartureDate, CultureInfo.InvariantCulture);
            var flightClass = Enum.Parse<FlightClass>(dto.FlightClass);

            var flight = new Flight
            {
                Id = id,
                Price = price,
                DepartureCountry = dto.DepartureCountry,
                DestinationCountry = dto.DestinationCountry,
                DepartureDate = departureDate,
                DepartureAirport = dto.DepartureAirport,
                DestinationAirport = dto.DestinationAirport,
                FlightClass = flightClass
            };
            return flight;
        }

        public static FlightCsvDto ToDto(Flight flight)
        {
            return new FlightCsvDto
            {
                Price = flight.Price.ToString(CultureInfo.InvariantCulture),
                DepartureCountry = flight.DepartureCountry,
                DestinationCountry = flight.DestinationCountry,
                DepartureDate = flight.DepartureDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                DepartureAirport = flight.DepartureAirport,
                DestinationAirport = flight.DestinationAirport,
                FlightClass = flight.FlightClass.ToString()
            };
        }
    }
}
