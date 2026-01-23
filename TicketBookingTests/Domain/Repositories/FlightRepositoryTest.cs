using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Infrastructure.Repositories;
using Xunit;

namespace TicketBookingTests.Domain.Repositories
{
    public class FlightRepositoryTest : IDisposable
    {
        private readonly string _testCsvPath;
        private readonly FlightRepository _flightRepository;

        public FlightRepositoryTest()
        {
            _testCsvPath = Path.Combine(Path.GetTempPath(), $"flight_data_{Guid.NewGuid()}.csv");
            // Write CSV header for FlightCsvDto with Id
            File.WriteAllText(_testCsvPath, "Id,Price,DepartureCountry,DestinationCountry,DepartureDate,DepartureAirport,DestinationAirport,FlightClass\n");
            _flightRepository = new FlightRepositoryForTest(_testCsvPath);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenNoFlights()
        {
            var flights = (await _flightRepository.GetAllAsync()).ToList();
            Assert.Empty(flights);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFlights_WhenFlightsExist()
        {
            // Add a flight row manually to the CSV
            var line = "1,199.99,USA,UK,2026-01-25,JFK,LHR,Economy";
            File.AppendAllText(_testCsvPath, line + "\n");
            var flights = (await _flightRepository.GetAllAsync()).ToList();
            Assert.Single(flights);
            Assert.Equal("USA", flights[0].DepartureCountry);
            Assert.Equal("UK", flights[0].DestinationCountry);
            Assert.Equal(FlightClass.Economy, flights[0].FlightClass);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var flight = await _flightRepository.GetByIdAsync(999);
            Assert.Null(flight);
        }

        public void Dispose()
        {
            if (File.Exists(_testCsvPath))
                File.Delete(_testCsvPath);
        }

        // Helper: override FlightRepository to inject test path
        private class FlightRepositoryForTest : FlightRepository
        {
            public FlightRepositoryForTest(string path)
            {
                typeof(FlightRepository)
                    .GetField("_csvFilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(this, path);
            }
        }
    }
}
