using CsvHelper.Configuration;

namespace TicketBooking.Infrastructure.Dtos;

public record UserCsvDto
{
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public sealed class UserCsvDtoMap : ClassMap<UserCsvDto>
{
    public UserCsvDtoMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.FirstName).Name("FirstName");
        Map(m => m.LastName).Name("LastName");
        Map(m => m.Username).Name("Username");
        Map(m => m.Email).Name("Email");
    }
}

