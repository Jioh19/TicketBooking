using TicketBooking.Domain.Services;

namespace TicketBooking.Presentation;

public class AdminMenu
{
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly IFlightService _flightService;

    public AdminMenu(IUserService userService, IBookingService bookingService, IFlightService flightService)
    {
        _userService = userService;
        _bookingService = bookingService;
        _flightService = flightService;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Admin Menu ===");
            Console.WriteLine("1. View All Users");
            Console.WriteLine("2. View All Bookings");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Select an option: ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ViewAllUsers();
                    break;
                case "2":
                    ViewAllBookings();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ViewAllUsers()
    {
        Console.Clear();
        var users = _userService.GetAllUsers().ToList();
        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
        }
        else
        {
            Console.WriteLine("=== All Users ===");
            users.ForEach(Console.WriteLine);
        }
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    private void ViewAllBookings()
    {
        Console.Clear();
        var bookings = _bookingService.GetAllBookings().ToList();
        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings found.");
        }
        else
        {
            Console.WriteLine("=== All Bookings ===");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"Id: {booking.Id}, State: {booking.State}\n" +
                                  $"Flight: {_flightService.GetFlightByIdAsync(booking.Flight.Id)}\n" +
                                  $"User: {_userService.GetUserById(booking.User.Id)}");
            }
        }
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

}