using TicketBooking.Domain.Models;
using TicketBooking.Domain.Repositories;

namespace TicketBooking.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAllAsync().Result;
    }

    public User? GetUserById(long id)
    {
        var user = _userRepository.GetByIdAsync(id).Result;
        if (user is null)
        {
            throw new Exception($"User with id {id} not found");
        }
        return user;
    }

    public User? GetUserByUsername(string username)
    {
        var user = _userRepository.GetByUsernameAsync(username).Result;
        if (user is null)
        {
            throw new Exception($"User with username {username} not found");
        }
        return user;
    }
    
    public User AddUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        
        // Get next ID
        var users = _userRepository.GetAllAsync().Result.ToList();
        var nextId = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        
        var newUser = user with { Id = nextId };
        return _userRepository.AddAsync(newUser).Result;
    }
}