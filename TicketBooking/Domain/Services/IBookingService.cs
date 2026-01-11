using TicketBooking.Domain.Models;

namespace TicketBooking.Domain.Services;

public interface IBookingService
{
    IEnumerable<Booking> GetAllBookings();
    Booking? GetBookingById(long id);
    Booking AddBooking(Booking booking);
    void CancelBooking(long id);
    void CompleteBooking(long id);
    void ModifyBooking(long id, Booking booking);
    IEnumerable<Booking> GetBookingsByUserId(long userId);
}