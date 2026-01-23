using Moq;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Domain.Repositories;
using TicketBooking.Domain.Services;

namespace TicketBookingTests.Domain.Services
{
    public class FlightServiceTest
    {
        private readonly Mock<IFlightRepository> _flightRepositoryMock;
        private readonly FlightService _flightService;

        public FlightServiceTest()
        {
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _flightService = new FlightService(_flightRepositoryMock.Object);
        }

        [Fact]
        public async Task GetFlightsAsync_ReturnsAllFlights()
        {
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DepartureCountry = "USA", DestinationCountry = "UK", FlightClass = FlightClass.Economy },
                new Flight { Id = 2, DepartureCountry = "USA", DestinationCountry = "France", FlightClass = FlightClass.Business }
            };
            _flightRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(flights);

            var result = (await _flightService.GetFlightsAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("UK", result[0].DestinationCountry);
        }

        [Fact]
        public async Task GetFlightByIdAsync_FlightExists_ReturnsFlight()
        {
            var flight = new Flight { Id = 1, DepartureCountry = "USA", DestinationCountry = "UK", FlightClass = FlightClass.Economy };
            _flightRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(flight);

            var result = await _flightService.GetFlightByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("UK", result.DestinationCountry);
        }

        [Fact]
        public async Task GetFlightByIdAsync_FlightDoesNotExist_ThrowsException()
        {
            _flightRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Flight)null!);
            await Assert.ThrowsAsync<Exception>(() => _flightService.GetFlightByIdAsync(99));
        }

        [Fact]
        public async Task GetFlightsByParametersAsync_FiltersCorrectly()
        {
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DepartureCountry = "USA", DestinationCountry = "UK", FlightClass = FlightClass.Economy },
                new Flight { Id = 2, DepartureCountry = "USA", DestinationCountry = "France", FlightClass = FlightClass.Business },
                new Flight { Id = 3, DepartureCountry = "Canada", DestinationCountry = "UK", FlightClass = FlightClass.Economy }
            };
            _flightRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(flights);

            var result = (await _flightService.GetFlightsByParametersAsync("USA", "UK", FlightClass.Economy)).ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public async Task GetAllOriginsAsync_ReturnsDistinctOrigins()
        {
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DepartureCountry = "USA", DestinationCountry = "UK", FlightClass = FlightClass.Economy },
                new Flight { Id = 2, DepartureCountry = "USA", DestinationCountry = "France", FlightClass = FlightClass.Business },
                new Flight { Id = 3, DepartureCountry = "Canada", DestinationCountry = "UK", FlightClass = FlightClass.Economy }
            };
            _flightRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(flights);

            var result = (await _flightService.GetAllOriginsAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains("USA", result);
            Assert.Contains("Canada", result);
        }

        [Fact]
        public async Task GetAllDestinationsAsync_ReturnsDistinctDestinations()
        {
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DepartureCountry = "USA", DestinationCountry = "UK", FlightClass = FlightClass.Economy },
                new Flight { Id = 2, DepartureCountry = "USA", DestinationCountry = "France", FlightClass = FlightClass.Business },
                new Flight { Id = 3, DepartureCountry = "Canada", DestinationCountry = "UK", FlightClass = FlightClass.Economy }
            };
            _flightRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(flights);

            var result = (await _flightService.GetAllDestinationsAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains("UK", result);
            Assert.Contains("France", result);
        }
    }
}
