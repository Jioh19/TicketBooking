namespace TicketBooking.Domain.Models;

public class EntityReference<T> where T : struct 
{
    public T Id { get; init; }
    public string? Name { get; init; } = string.Empty;

    public static EntityReference<T> Empty => new()
    {
        Id = default(T),
        Name = string.Empty
    };

    public override bool Equals(object? obj) =>
        obj is EntityReference<T> reference && Id.Equals(reference.Id);

    public override int GetHashCode() =>
        Id.GetHashCode() + Name!.GetHashCode();
}