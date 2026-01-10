using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Repositories;
using TicketBooking.Infrastructure.Utils;

namespace TicketBooking.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly string _csvFilePath = PathParser.GetPath("flight_data.csv");
    private readonly CsvConfiguration _csvConfig;

    public FlightRepository()
    {
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8
        };
    }
    
    public async Task<IReadOnlyCollection<Flight>> GetAllAsync()
    {
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