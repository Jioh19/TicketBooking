using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using TicketBooking.Domain.Repositories;
using TicketBooking.Infrastructure.Dtos;
using TicketBooking.Infrastructure.Mappers;
using TicketBooking.Infrastructure.Utils;
using DomainUser = TicketBooking.Domain.Models.User;

namespace TicketBooking.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _csvFilePath = PathParser.GetPath("user_data.csv");
    private readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        Encoding = Encoding.UTF8
    };

    public async Task<IEnumerable<DomainUser>> GetAllAsync()
    {
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, _csvConfig);
        var records = await Task.FromResult(csv.GetRecords<UserCsvDto>().ToList());
        var validUsers = new List<DomainUser>();

        var line = 2;
        foreach (var record in records)
        {
            try
            {
                var result = UserCsvMapper.ToDomain(record);
                validUsers.Add(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid user record on line {line}: {e.Message}");
            }
            ++line;
        }
        return validUsers;
    }

    public async Task<DomainUser?> GetByIdAsync(long id)
    {
        var users = await GetAllAsync();
        return users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<DomainUser?> GetByUsernameAsync(string username)
    {
        var users = await GetAllAsync();
        return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<DomainUser?> GetByEmailAsync(string email)
    {
        var users = await GetAllAsync();
        return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<DomainUser> AddAsync(DomainUser entity)
    {
        var users = (await GetAllAsync()).ToList();
        
        // Check if user with same username or email already exists
        if (users.Any(u => u.Username.Equals(entity.Username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"User with username '{entity.Username}' already exists.");
        }
        
        if (users.Any(u => u.Email.Equals(entity.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"User with email '{entity.Email}' already exists.");
        }

        users.Add(entity);
        await SaveAllAsync(users);
        return entity;
    }

    public async Task<DomainUser?> UpdateAsync(DomainUser entity)
    {
        var users = (await GetAllAsync()).ToList();
        var existingUser = users.FirstOrDefault(u => u.Id == entity.Id);
        
        if (existingUser == null)
        {
            return null;
        }

        // Check for duplicate username/email (excluding current user)
        if (users.Any(u => u.Id != entity.Id && u.Username.Equals(entity.Username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"User with username '{entity.Username}' already exists.");
        }
        
        if (users.Any(u => u.Id != entity.Id && u.Email.Equals(entity.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"User with email '{entity.Email}' already exists.");
        }

        users.Remove(existingUser);
        users.Add(entity);
        await SaveAllAsync(users);
        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        var users = (await GetAllAsync()).ToList();
        var user = users.FirstOrDefault(u => u.Id == id);
        
        if (user != null)
        {
            users.Remove(user);
            await SaveAllAsync(users);
        }
    }

    private async Task SaveAllAsync(IEnumerable<DomainUser> users)
    {
        // Ensure directory exists
        var directory = Path.GetDirectoryName(_csvFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var writer = new StreamWriter(_csvFilePath, false, Encoding.UTF8);
        using var csv = new CsvWriter(writer, _csvConfig);
        
        csv.Context.RegisterClassMap<UserCsvDtoMap>();
        var dtos = users.Select(UserCsvMapper.ToDto);
        await csv.WriteRecordsAsync(dtos);
    }
}

