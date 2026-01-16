using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Domain.Repositories;

namespace TicketBooking.Domain.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public IEnumerable<Booking> GetAllBookings()
    {
        return _bookingRepository.GetAllAsync().Result;
    }

    public Booking? GetBookingById(long id)
    {
        return _bookingRepository.GetByIdAsync(id).Result;
    }

    public Booking AddBooking(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        return _bookingRepository.AddAsync(booking).Result;
    }

    public void CancelBooking(long id)
    {
        var booking = GetBookingById(id);
        if (booking != null)
        {
            var updatedBooking = booking with { State = BookingState.Cancelled };
            _bookingRepository.UpdateAsync(updatedBooking);
        }
    }

    public void CompleteBooking(long id)
    {
        var booking = GetBookingById(id);
        if (booking != null)
        {
            var updatedBooking = booking with { State = BookingState.Completed };
            _bookingRepository.UpdateAsync(updatedBooking);
        }
    }

    public void ModifyBooking(long id, Booking booking)
    {
        var existingBooking = GetBookingById(id);
        if (existingBooking != null)
        {
            _bookingRepository.UpdateAsync(booking);
        }
    }

    public IEnumerable<Booking> GetBookingsByUserId(long userId)
    {
        return _bookingRepository.GetByUserIdAsync(userId).Result;
    }
}