using TicketBooking.Domain.Models;

namespace TicketBooking.Domain.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByUserIdAsync(long userId);
    Task<IEnumerable<Booking>> GetByFlightIdAsync(long flightId);
}

