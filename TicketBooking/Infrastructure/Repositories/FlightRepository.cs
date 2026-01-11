using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using TicketBooking.Domain.Repositories;
using TicketBooking.Infrastructure.Dtos;
using TicketBooking.Infrastructure.Mappers;
using TicketBooking.Infrastructure.Utils;
using DomainFlight = TicketBooking.Domain.Models.Flight;

namespace TicketBooking.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly string _csvFilePath = PathParser.GetPath("flight_data.csv");
    private readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        Encoding = Encoding.UTF8
    };

    public async Task<IEnumerable<DomainFlight>> GetAllAsync()
    {
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, _csvConfig);
        var records = await Task.FromResult(csv.GetRecords<FlightCsvDto>().ToList());
        var validFlights = new List<DomainFlight>();

        var line = 2;
        foreach (var record in records)
        {
            try
            {
                var result = FlightMapper.ToDomain(record, line - 2);
                validFlights.Add(result);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Invalid record on line {line}: {e.Message}");
            }
            ++line;
        }
        return validFlights;
    }

    public Task<DomainFlight?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<DomainFlight> AddAsync(DomainFlight entity)
    {
        throw new NotImplementedException();
    }

    public Task<DomainFlight?> UpdateAsync(DomainFlight entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }
}