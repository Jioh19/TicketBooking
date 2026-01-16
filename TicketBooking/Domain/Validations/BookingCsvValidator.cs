using System.Globalization;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Infrastructure.Dtos;

namespace TicketBooking.Domain.Validations;

public static class BookingCsvValidator
{
    public static IEnumerable<string> Validate(BookingCsvDto booking)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(booking.Id))
            errors.Add("Id is required.");
        else if (!long.TryParse(booking.Id, NumberStyles.Any, CultureInfo.InvariantCulture, out var id))
            errors.Add("Id must be castable as a valid number.");
        else if (id < 0)
            errors.Add("Id must be greater than or equal to zero.");

        if (string.IsNullOrWhiteSpace(booking.FlightId))
            errors.Add("FlightId is required.");
        else if (!long.TryParse(booking.FlightId, NumberStyles.Any, CultureInfo.InvariantCulture, out var flightId))
            errors.Add("FlightId must be castable as a valid number.");
        else if (flightId < 0)
            errors.Add("FlightId must be greater than or equal to zero.");

        if (string.IsNullOrWhiteSpace(booking.UserId))
            errors.Add("UserId is required.");
        else if (!long.TryParse(booking.UserId, NumberStyles.Any, CultureInfo.InvariantCulture, out var userId))
            errors.Add("UserId must be castable as a valid number.");
        else if (userId < 0)
            errors.Add("UserId must be greater than or equal to zero.");

        if (string.IsNullOrWhiteSpace(booking.State))
            errors.Add("State is required.");
        else if (!Enum.TryParse(typeof(BookingState), booking.State, out _))
            errors.Add("State must be a valid BookingState value.");

        return errors;
    }
}

