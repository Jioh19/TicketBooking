using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Domain.Models;

public record Booking
{
    [Required]
    public long Id { get; init; }
    [Required]
    public EntityReference<long> Flight { get; init; } = EntityReference<long>.Empty;
    [Required] 
    public EntityReference<long> User { get; init; } = EntityReference<long>.Empty;
}