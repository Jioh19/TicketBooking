using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using TicketBooking.Domain.Repositories;
using TicketBooking.Infrastructure.Dtos;
using TicketBooking.Infrastructure.Mappers;
using TicketBooking.Infrastructure.Utils;
using DomainBooking = TicketBooking.Domain.Models.Booking;

namespace TicketBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly string _csvFilePath = PathParser.GetPath("booking_data.csv");
    private readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        Encoding = Encoding.UTF8
    };

    public async Task<IEnumerable<DomainBooking>> GetAllAsync()
    {
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, _csvConfig);
        var records = await Task.FromResult(csv.GetRecords<BookingCsvDto>().ToList());
        var validBookings = new List<DomainBooking>();

        var line = 2;
        foreach (var record in records)
        {
            try
            {
                var result = BookingCsvMapper.ToDomain(record);
                validBookings.Add(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid booking record on line {line}: {e.Message}");
            }
            ++line;
        }
        return validBookings;
    }

    public async Task<DomainBooking?> GetByIdAsync(long id)
    {
        var bookings = await GetAllAsync();
        return bookings.FirstOrDefault(b => b.Id == id);
    }

    public async Task<IEnumerable<DomainBooking>> GetByUserIdAsync(long userId)
    {
        var bookings = await GetAllAsync();
        return bookings.Where(b => b.User.Id == userId);
    }

    public async Task<IEnumerable<DomainBooking>> GetByFlightIdAsync(long flightId)
    {
        var bookings = await GetAllAsync();
        return bookings.Where(b => b.Flight.Id == flightId);
    }

    public async Task<DomainBooking> AddAsync(DomainBooking entity)
    {
        var bookings = (await GetAllAsync()).ToList();
        
        // Generate new ID
        var nextId = bookings.Any() ? bookings.Max(b => b.Id) + 1 : 1;
        
        var newBooking = entity with { Id = nextId };
        bookings.Add(newBooking);
        await SaveAllAsync(bookings);
        return newBooking;
    }

    public async Task<DomainBooking?> UpdateAsync(DomainBooking entity)
    {
        var bookings = (await GetAllAsync()).ToList();
        var existingBooking = bookings.FirstOrDefault(b => b.Id == entity.Id);
        
        if (existingBooking == null)
        {
            return null;
        }

        bookings.Remove(existingBooking);
        bookings.Add(entity);
        await SaveAllAsync(bookings);
        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        var bookings = (await GetAllAsync()).ToList();
        var booking = bookings.FirstOrDefault(b => b.Id == id);
        
        if (booking != null)
        {
            bookings.Remove(booking);
            await SaveAllAsync(bookings);
        }
    }

    private async Task SaveAllAsync(IEnumerable<DomainBooking> bookings)
    {
        var directory = Path.GetDirectoryName(_csvFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var writer = new StreamWriter(_csvFilePath, false, Encoding.UTF8);
        using var csv = new CsvWriter(writer, _csvConfig);
        
        csv.Context.RegisterClassMap<BookingCsvDtoMap>();
        var dtos = bookings.Select(BookingCsvMapper.ToDto);
        await csv.WriteRecordsAsync(dtos);
    }
}

