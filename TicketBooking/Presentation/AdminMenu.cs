using TicketBooking.Domain.Services;

namespace TicketBooking.Presentation;

public class AdminMenu
{
    private readonly UserService _userService;

    public AdminMenu(UserService userService)
    {
        _userService = userService;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Admin Menu ===");
            Console.WriteLine("1. View All Users");
            Console.WriteLine("2. View All Bookings");
            Console.WriteLine("3. Import Flights from CSV");
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
                case "3":
                    ImportFlights();
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
        throw new NotImplementedException();
    }
    
    private void ImportFlights()
    {
        throw new NotImplementedException();
    }
}