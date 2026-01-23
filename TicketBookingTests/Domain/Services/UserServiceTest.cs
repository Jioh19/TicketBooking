using Moq;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Repositories;
using TicketBooking.Domain.Services;

namespace TicketBookingTests.Domain.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public void GetAllUsers_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@example.com" },
                new User { Id = 2, FirstName = "Jane", LastName = "Smith", Username = "janesmith", Email = "jane@example.com" }
            };
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = _userService.GetAllUsers().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("johndoe", result[0].Username);
            Assert.Equal("janesmith", result[1].Username);
        }

        [Fact]
        public void GetUserById_UserExists_ReturnsUser()
        {
            var user = new User { Id = 1, FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@example.com" };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            var result = _userService.GetUserById(1);

            Assert.NotNull(result);
            Assert.Equal("johndoe", result.Username);
        }

        [Fact]
        public void GetUserById_UserDoesNotExist_ThrowsException()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User)null!);
            Assert.Throws<Exception>(() => _userService.GetUserById(99));
        }

        [Fact]
        public void GetUserByUsername_UserExists_ReturnsUser()
        {
            var user = new User { Id = 1, FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@example.com" };
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("johndoe")).ReturnsAsync(user);

            var result = _userService.GetUserByUsername("johndoe");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetUserByUsername_UserDoesNotExist_ThrowsException()
        {
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("nouser")).ReturnsAsync((User)null!);
            Assert.Throws<Exception>(() => _userService.GetUserByUsername("nouser"));
        }

        [Fact]
        public void AddUser_ValidUser_AddsAndReturnsUserWithNextId()
        {
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@example.com" }
            };
            var newUser = new User { Id = 0, FirstName = "Jane", LastName = "Smith", Username = "janesmith", Email = "jane@example.com" };
            var addedUser = newUser with { Id = 2 };

            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _userRepositoryMock.Setup(r => r.AddAsync(It.Is<User>(u => u.Username == "janesmith" && u.Id == 2))).ReturnsAsync(addedUser);

            var result = _userService.AddUser(newUser);

            Assert.Equal(2, result.Id);
            Assert.Equal("janesmith", result.Username);
        }

        [Fact]
        public void AddUser_NullUser_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _userService.AddUser(null!));
        }
    }
}