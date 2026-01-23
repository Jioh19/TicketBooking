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
    public class BookingRepositoryTest : IDisposable
    {
        private readonly string _testCsvPath;
        private readonly BookingRepository _bookingRepository;

        public BookingRepositoryTest()
        {
            _testCsvPath = Path.Combine(Path.GetTempPath(), $"booking_data_{Guid.NewGuid()}.csv");
            File.WriteAllText(_testCsvPath, "Id,FlightId,UserId,State\n");
            _bookingRepository = new BookingRepositoryForTest(_testCsvPath);
        }

        [Fact]
        public async Task AddAndGetBooking_WorksCorrectly()
        {
            var booking = new Booking
            {
                Id = 0,
                Flight = new EntityReference<long> { Id = 1 },
                User = new EntityReference<long> { Id = 2 },
                State = BookingState.Active
            };
            var added = await _bookingRepository.AddAsync(booking);
            Assert.True(added.Id > 0);
            var fetched = await _bookingRepository.GetByIdAsync(added.Id);
            Assert.NotNull(fetched);
            Assert.Equal(1, fetched.Flight.Id);
            Assert.Equal(2, fetched.User.Id);
        }

        [Fact]
        public async Task UpdateBooking_WorksCorrectly()
        {
            var booking = new Booking
            {
                Id = 0,
                Flight = new EntityReference<long> { Id = 1 },
                User = new EntityReference<long> { Id = 2 },
                State = BookingState.Active
            };
            var added = await _bookingRepository.AddAsync(booking);
            var updated = added with { State = BookingState.Completed };
            var result = await _bookingRepository.UpdateAsync(updated);
            Assert.NotNull(result);
            Assert.Equal(BookingState.Completed, result.State);
            var fetched = await _bookingRepository.GetByIdAsync(added.Id);
            Assert.Equal(BookingState.Completed, fetched.State);
        }

        [Fact]
        public async Task DeleteBooking_WorksCorrectly()
        {
            var booking = new Booking
            {
                Id = 0,
                Flight = new EntityReference<long> { Id = 1 },
                User = new EntityReference<long> { Id = 2 },
                State = BookingState.Active
            };
            var added = await _bookingRepository.AddAsync(booking);
            await _bookingRepository.DeleteAsync(added.Id);
            var fetched = await _bookingRepository.GetByIdAsync(added.Id);
            Assert.Null(fetched);
        }

        [Fact]
        public async Task GetAllBookings_ReturnsAll()
        {
            var booking1 = new Booking
            {
                Id = 0,
                Flight = new EntityReference<long> { Id = 1 },
                User = new EntityReference<long> { Id = 2 },
                State = BookingState.Active
            };
            var booking2 = new Booking
            {
                Id = 0,
                Flight = new EntityReference<long> { Id = 3 },
                User = new EntityReference<long> { Id = 4 },
                State = BookingState.Completed
            };
            await _bookingRepository.AddAsync(booking1);
            await _bookingRepository.AddAsync(booking2);
            var all = (await _bookingRepository.GetAllAsync()).ToList();
            Assert.True(all.Count >= 2);
            Assert.Contains(all, b => b.Flight.Id == 1 && b.User.Id == 2);
            Assert.Contains(all, b => b.Flight.Id == 3 && b.User.Id == 4);
        }

        public void Dispose()
        {
            if (File.Exists(_testCsvPath))
                File.Delete(_testCsvPath);
        }

        // Helper: override BookingRepository to inject test path
        private class BookingRepositoryForTest : BookingRepository
        {
            public BookingRepositoryForTest(string path)
            {
                typeof(BookingRepository)
                    .GetField("_csvFilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(this, path);
            }
        }
    }
}
