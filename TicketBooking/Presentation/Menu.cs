using TicketBooking.Domain.Services;

namespace TicketBooking.Presentation;

public class Menu
{
    private readonly UserMenu _userMenu;
    private readonly AdminMenu _adminMenu;

    public Menu(UserService userService)
    {
        _adminMenu = new AdminMenu(userService);
        _userMenu = new UserMenu(userService);
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Airport Ticket Booking System ===");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Admin");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ShowUserMenu();
                    break;
                case "2":
                    ShowAdminMenu();
                    break;
                case "0":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ShowUserMenu()
    {
        _userMenu.Show();
    }

    private void ShowAdminMenu()
    {
        _adminMenu.Show();
    }
}