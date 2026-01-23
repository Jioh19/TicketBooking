using TicketBooking.Domain.Models;
using TicketBooking.Infrastructure.Repositories;

namespace TicketBookingTests.Domain.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly string _testCsvPath;
        private readonly UserRepository _userRepository;

        public UserRepositoryTest()
        {
            // Use a unique temp file for each test run
            _testCsvPath = Path.Combine(Path.GetTempPath(), $"user_data_{Guid.NewGuid()}.csv");
            File.WriteAllText(_testCsvPath, "Id,FirstName,LastName,Username,Email\n"); // CSV header
            // Patch PathParser.GetPath to return our temp file (if needed)
            Environment.SetEnvironmentVariable("USER_CSV_PATH", _testCsvPath);
            _userRepository = new UserRepositoryForTest(_testCsvPath);
        }

        [Fact]
        public async Task AddAndGetUser_WorksCorrectly()
        {
            var user = new User { Id = 0, FirstName = "Test", LastName = "User", Username = "testuser", Email = "test@example.com" };
            var added = await _userRepository.AddAsync(user);
            Assert.True(added.Id > 0);
            var fetched = await _userRepository.GetByIdAsync(added.Id);
            Assert.NotNull(fetched);
            Assert.Equal("testuser", fetched.Username);
        }

        [Fact]
        public async Task UpdateUser_WorksCorrectly()
        {
            var user = new User { Id = 0, FirstName = "Test", LastName = "User", Username = "testuser", Email = "test@example.com" };
            var added = await _userRepository.AddAsync(user);
            var updated = added with { FirstName = "Updated" };
            var result = await _userRepository.UpdateAsync(updated);
            Assert.NotNull(result);
            Assert.Equal("Updated", result.FirstName);
            var fetched = await _userRepository.GetByIdAsync(added.Id);
            if (fetched != null) Assert.Equal("Updated", fetched.FirstName);
        }

        [Fact]
        public async Task DeleteUser_WorksCorrectly()
        {
            var user = new User { Id = 0, FirstName = "Test", LastName = "User", Username = "testuser", Email = "test@example.com" };
            var added = await _userRepository.AddAsync(user);
            await _userRepository.DeleteAsync(added.Id);
            var fetched = await _userRepository.GetByIdAsync(added.Id);
            Assert.Null(fetched);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAll()
        {
            var user1 = new User { Id = 0, FirstName = "A", LastName = "B", Username = "a", Email = "a@example.com" };
            var user2 = new User { Id = 0, FirstName = "C", LastName = "D", Username = "c", Email = "c@example.com" };
            await _userRepository.AddAsync(user1);
            await _userRepository.AddAsync(user2);
            var all = (await _userRepository.GetAllAsync()).ToList();
            Assert.True(all.Count >= 2);
            Assert.Contains(all, u => u.Username == "a");
            Assert.Contains(all, u => u.Username == "c");
        }

        public void Dispose()
        {
            if (File.Exists(_testCsvPath))
                File.Delete(_testCsvPath);
        }

        // Helper: override UserRepository to inject test path
        private class UserRepositoryForTest : UserRepository
        {
            public UserRepositoryForTest(string path)
            {
                typeof(UserRepository)
                    .GetField("_csvFilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(this, path);
            }
        }
    }
}