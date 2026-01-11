using TicketBooking.Domain.Models;

namespace TicketBooking.Domain.Services;

public class UserService : IUserService
{
    private long _userIdCounter = 1;
    private readonly List<User> _users = [];

    public UserService()
    {
        var user = new User
        {
            Id = 0,
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@mail.com"
        };
        AddUser(user);
        user = new User()
        {
            Id = 0,
            FirstName = "Jane",
            LastName = "Toe",
            Username = "janetoe",
            Email = "jane@mail.com"
        };
        AddUser(user);
    }
    
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
        if (user == null) throw new ArgumentNullException(nameof(user));
        var newUser = user with { Id = _userIdCounter++ };
        _users.Add(newUser);
        return newUser;
    }
}