using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Repositories;

namespace TicketBooking.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly string _csvFilePath;
    private readonly CsvConfiguration _csvConfig;

    private const string DefaultCsvFilePath = "../../../csv/flight_data.csv";

    public FlightRepository() : this(DefaultCsvFilePath) { }

    public FlightRepository(string csvFilePath)
    {
        _csvFilePath = csvFilePath;
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8
        };
    }

    public async Task<IReadOnlyCollection<Flight>> GetAllAsync()
    {
        Console.WriteLine($"Reading from {_csvFilePath}");
        if (!File.Exists(_csvFilePath))
            return [];
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, _csvConfig);
        var records = csv.GetRecords<Flight>().ToList();
        return await Task.FromResult(records);
    }

    public Task<Flight?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<Flight> AddAsync(Flight entity)
    {
        throw new NotImplementedException();
    }

    public Task<Flight?> UpdateAsync(Flight entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }
}