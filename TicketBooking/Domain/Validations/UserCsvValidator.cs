using System.Globalization;
using System.Text.RegularExpressions;
using TicketBooking.Infrastructure.Dtos;

namespace TicketBooking.Domain.Validations;

public static class UserCsvValidator
{
    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static IEnumerable<string> Validate(UserCsvDto user)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(user.Id))
            errors.Add("Id is required.");
        else if (!long.TryParse(user.Id, NumberStyles.Any, CultureInfo.InvariantCulture, out var id))
            errors.Add("Id must be castable as a valid number.");
        else if (id < 0)
            errors.Add("Id must be greater than or equal to zero.");

        if (string.IsNullOrWhiteSpace(user.FirstName))
            errors.Add("First name is required.");

        if (string.IsNullOrWhiteSpace(user.LastName))
            errors.Add("Last name is required.");

        if (string.IsNullOrWhiteSpace(user.Username))
            errors.Add("Username is required.");

        if (string.IsNullOrWhiteSpace(user.Email))
            errors.Add("Email is required.");
        else if (!EmailRegex.IsMatch(user.Email))
            errors.Add("Email must be a valid email address.");

        return errors;
    }
}

