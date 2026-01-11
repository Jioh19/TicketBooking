using System.Text.RegularExpressions;

namespace TicketBooking.Domain.Validations;

public static class UserValidator
{
    public static (bool IsValid, string Error) ValidateFirstName(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "First name is required.");
        if (input.Length < 2)
            return (false, "First name must be at least 2 characters.");
        return (true, string.Empty);
    }

    public static (bool IsValid, string Error) ValidateLastName(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "Last name is required.");
        if (input.Length < 2)
            return (false, "Last name must be at least 2 characters.");
        return (true, string.Empty);
    }

    public static (bool IsValid, string Error) ValidateUsername(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "Username is required.");
        if (input.Length < 3)
            return (false, "Username must be at least 3 characters.");
        return (true, string.Empty);
    }

    public static (bool IsValid, string Error) ValidateEmail(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "Email is required.");
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(input, emailPattern))
            return (false, "Email format is invalid.");
        return (true, string.Empty);
    }
}

