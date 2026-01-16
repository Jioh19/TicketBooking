using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Domain.Validations;
using TicketBooking.Infrastructure.Dtos;

namespace TicketBooking.Infrastructure.Mappers;

public static class BookingCsvMapper
{
    public static Booking ToDomain(BookingCsvDto dto)
    {
        var errors = BookingCsvValidator.Validate(dto).ToList();
        if (errors.Count != 0)
        {
            throw new ValidationException(string.Join("; ", errors));
        }

        var id = long.Parse(dto.Id, NumberStyles.Any, CultureInfo.InvariantCulture);
        var flightId = long.Parse(dto.FlightId, NumberStyles.Any, CultureInfo.InvariantCulture);
        var userId = long.Parse(dto.UserId, NumberStyles.Any, CultureInfo.InvariantCulture);
        var state = Enum.Parse<BookingState>(dto.State);

        var booking = new Booking
        {
            Id = id,
            Flight = new EntityReference<long> { Id = flightId },
            User = new EntityReference<long> { Id = userId },
            State = state
        };
        return booking;
    }

    public static BookingCsvDto ToDto(Booking booking)
    {
        return new BookingCsvDto
        {
            Id = booking.Id.ToString(CultureInfo.InvariantCulture),
            FlightId = booking.Flight.Id.ToString(CultureInfo.InvariantCulture),
            UserId = booking.User.Id.ToString(CultureInfo.InvariantCulture),
            State = booking.State.ToString()
        };
    }
}

