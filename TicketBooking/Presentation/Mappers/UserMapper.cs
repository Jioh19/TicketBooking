using TicketBooking.Domain.Models;
using TicketBooking.Presentation.DTOs;

namespace TicketBooking.Presentation.Mappers;

public static class UserMapper
{
    public static User ToDomain(UserDto dto)
    {
        return new User
        {
            Id = 0, // ID will be assigned by UserService
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Username = dto.Username,
            Email = dto.Email
        };
    }

    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email
        };
    }
}
