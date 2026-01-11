using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Domain.Models;

public record User
{
    [Required] 
    public long Id { get; init; } 
    [Required]
    public string FirstName { get; init; } = string.Empty;
    [Required]
    public string LastName { get; init; } = string.Empty;
    [Required]
    public string Username { get; init; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
}