using System.ComponentModel.DataAnnotations;
using TicketBooking.Domain.Models.Enums;

namespace TicketBooking.Domain.Models;

public record Flight
{
    [Required]
    public long Id { get; init; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; init; }
    [Required]
    public string DepartureCountry { get; init; } = string.Empty;
    [Required]
    public string DestinationCountry { get; init; } = string.Empty;
    [Required]
    [CustomValidation(typeof(Flight), nameof(ValidateDepartureDate))]
    public DateTime DepartureDate { get; init; }
    [Required]
    public string DepartureAirport { get; init; } = string.Empty;
    [Required]
    public string DestinationAirport { get; init; } = string.Empty;
    [Required]
    public FlightClass FlightClass { get; init; }

    public static ValidationResult? ValidateDepartureDate(DateTime date, ValidationContext context)
    {
        return date < DateTime.Today
            ? new ValidationResult("Departure date must be today or in the future.")
            : ValidationResult.Success;
    }
}