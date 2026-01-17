using TicketBooking.Domain.Models;

namespace TicketBooking.Domain.Services;

public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(long id);
    User? GetUserByUsername(string username);
    User AddUser(User user);
}