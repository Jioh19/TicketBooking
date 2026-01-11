using System.Collections.Generic;
using System.Linq;
using TicketBooking.Domain.Models;

namespace TicketBooking.Domain.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = [];

    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }

    public User? GetUserById(long id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new Exception($"User with id {id} not found");
        }
        return user;
    }

    public User? GetUserByUsername(string username)
    {
        var user = _users.FirstOrDefault(u => u.Username == username);
        if (user is null)
        {
            throw new Exception($"User with username {username} not found");
        }
        return user;
    }
    
    public User AddUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        _users.Add(user);
        return user;
    }
}