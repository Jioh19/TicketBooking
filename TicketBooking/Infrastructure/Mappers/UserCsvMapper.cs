using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Validations;
using TicketBooking.Infrastructure.Dtos;

namespace TicketBooking.Infrastructure.Mappers;

public static class UserCsvMapper
{
    public static User ToDomain(UserCsvDto dto)
    {
        var errors = UserCsvValidator.Validate(dto).ToList();
        if (errors.Count != 0)
        {
            throw new ValidationException(string.Join("; ", errors));
        }

        var id = long.Parse(dto.Id, NumberStyles.Any, CultureInfo.InvariantCulture);

        var user = new User
        {
            Id = id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Username = dto.Username,
            Email = dto.Email
        };
        return user;
    }

    public static UserCsvDto ToDto(User user)
    {
        return new UserCsvDto
        {
            Id = user.Id.ToString(CultureInfo.InvariantCulture),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email
        };
    }
}

