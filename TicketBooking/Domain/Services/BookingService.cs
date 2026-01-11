using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;

namespace TicketBooking.Domain.Services;

public class BookingService : IBookingService
{
    private readonly List<Booking> _bookings = [];
    
    public IEnumerable<Booking> GetAllBookings()
    {
        return _bookings;
    }

    public Booking? GetBookingById(long id)
    {
        return _bookings.FirstOrDefault(b => b.Id == id);
    }

    public Booking AddBooking(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        _bookings.Add(booking);
        return booking;
    }

    public void CancelBooking(long id)
    {
        var booking = GetBookingById(id);
        if (booking != null)
        {
            booking.State = BookingState.Cancelled;
        }
    }

    public void CompleteBooking(long id)
    {
        var booking = GetBookingById(id);
        if (booking != null)
        {
            booking.State = BookingState.Completed;
        }
    }

    public void ModifyBooking(long id, Booking booking)
    {
        var index = _bookings.FindIndex(b => b.Id == id);
        if (index >= 0)
        {
            _bookings[index] = booking;
        }
    }

    public IEnumerable<Booking> GetBookingsByUserId(long userId)
    {
        return _bookings.Where(b => b.User.Id == userId);
    }
}