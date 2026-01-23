using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Domain.Repositories;
using TicketBooking.Domain.Services;
using Xunit;

namespace TicketBookingTests.Domain.Services
{
    public class BookingServiceTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly BookingService _bookingService;

        public BookingServiceTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _bookingService = new BookingService(_bookingRepositoryMock.Object);
        }

        [Fact]
        public void GetAllBookings_ReturnsAllBookings()
        {
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, State = BookingState.Active },
                new Booking { Id = 2, State = BookingState.Completed }
            };
            _bookingRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(bookings);

            var result = _bookingService.GetAllBookings().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(BookingState.Active, result[0].State);
        }

        [Fact]
        public void GetBookingById_BookingExists_ReturnsBooking()
        {
            var booking = new Booking { Id = 1, State = BookingState.Active };
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);

            var result = _bookingService.GetBookingById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetBookingById_BookingDoesNotExist_ReturnsNull()
        {
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Booking)null!);
            var result = _bookingService.GetBookingById(99);
            Assert.Null(result);
        }

        [Fact]
        public void AddBooking_ValidBooking_AddsAndReturnsBooking()
        {
            var booking = new Booking { Id = 0, State = BookingState.Active };
            var addedBooking = booking with { Id = 1 };
            _bookingRepositoryMock.Setup(r => r.AddAsync(booking)).ReturnsAsync(addedBooking);

            var result = _bookingService.AddBooking(booking);

            Assert.Equal(1, result.Id);
            Assert.Equal(BookingState.Active, result.State);
        }

        [Fact]
        public void AddBooking_NullBooking_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _bookingService.AddBooking(null));
        }

        [Fact]
        public void CancelBooking_ExistingBooking_UpdatesStateToCancelled()
        {
            var booking = new Booking { Id = 1, State = BookingState.Active };
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(r => r.UpdateAsync(It.Is<Booking>(b => b.Id == 1 && b.State == BookingState.Cancelled))).ReturnsAsync(booking with { State = BookingState.Cancelled });

            _bookingService.CancelBooking(1);
            _bookingRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Booking>(b => b.State == BookingState.Cancelled)), Times.Once);
        }

        [Fact]
        public void CompleteBooking_ExistingBooking_UpdatesStateToCompleted()
        {
            var booking = new Booking { Id = 1, State = BookingState.Active };
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(r => r.UpdateAsync(It.Is<Booking>(b => b.Id == 1 && b.State == BookingState.Completed))).ReturnsAsync(booking with { State = BookingState.Completed });

            _bookingService.CompleteBooking(1);
            _bookingRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Booking>(b => b.State == BookingState.Completed)), Times.Once);
        }

        [Fact]
        public void ModifyBooking_ExistingBooking_UpdatesBooking()
        {
            var booking = new Booking { Id = 1, State = BookingState.Active };
            var modifiedBooking = booking with { State = BookingState.Completed };
            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(r => r.UpdateAsync(modifiedBooking)).ReturnsAsync(modifiedBooking);

            _bookingService.ModifyBooking(1, modifiedBooking);
            _bookingRepositoryMock.Verify(r => r.UpdateAsync(modifiedBooking), Times.Once);
        }

        [Fact]
        public void GetBookingsByUserId_ReturnsBookingsForUser()
        {
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, User = new EntityReference<long> { Id = 1 }, State = BookingState.Active },
                new Booking { Id = 2, User = new EntityReference<long> { Id = 2 }, State = BookingState.Completed }
            };
            _bookingRepositoryMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(bookings.Where(b => b.User.Id == 1));

            var result = _bookingService.GetBookingsByUserId(1).ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].User.Id);
        }
    }
}
