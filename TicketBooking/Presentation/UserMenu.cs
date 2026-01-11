using TicketBooking.Domain.Validations;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Services;
using TicketBooking.Presentation.DTOs;
using TicketBooking.Presentation.Mappers;

namespace TicketBooking.Presentation;

public class UserMenu
{
    private readonly UserService _userService;
    public UserMenu(UserService userService)
    {
        _userService = userService;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== User Menu ===");
            Console.WriteLine("1. Book a Flight");
            Console.WriteLine("2. Search for Available Flights");
            Console.WriteLine("3. Manage Bookings");
            Console.WriteLine("4. Add User");
            Console.WriteLine("5. Edit User");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Select an option: ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    BookFlight();
                    break;
                case "2":
                    SearchFlights();
                    break;
                case "3":
                    ManageBookings();
                    break;
                case "4":
                    AddUser();
                    break;
                case "5":
                    EditUser();
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

    private void BookFlight()
    {
        throw new NotImplementedException();
    }

    private void SearchFlights()
    {
        throw new NotImplementedException();
    }

    private void ManageBookings()
    {
        throw new NotImplementedException();
    }

    private void AddUser()
    {
        Console.Clear();
        Console.WriteLine("=== Add User ===");
        string firstName;
        do
        {
            Console.Write("First Name: ");
            firstName = Console.ReadLine() ?? "";
            var (valid, error) = UserValidator.ValidateFirstName(firstName);
            if (!valid) Console.WriteLine(error);
        } while (!UserValidator.ValidateFirstName(firstName).IsValid);

        string lastName;
        do
        {
            Console.Write("Last Name: ");
            lastName = Console.ReadLine() ?? "";
            var (valid, error) = UserValidator.ValidateLastName(lastName);
            if (!valid) Console.WriteLine(error);
        } while (!UserValidator.ValidateLastName(lastName).IsValid);

        string username;
        do
        {
            Console.Write("Username: ");
            username = Console.ReadLine() ?? "";
            var (valid, error) = UserValidator.ValidateUsername(username);
            if (!valid) Console.WriteLine(error);
        } while (!UserValidator.ValidateUsername(username).IsValid);

        string email;
        do
        {
            Console.Write("Email: ");
            email = Console.ReadLine() ?? "";
            var (valid, error) = UserValidator.ValidateEmail(email);
            if (!valid) Console.WriteLine(error);
        } while (!UserValidator.ValidateEmail(email).IsValid);

        var userDto = new UserDto
        {
            FirstName = firstName,
            LastName = lastName,
            Username = username,
            Email = email
        };
        var user = _userService.AddUser(UserMapper.ToDomain(userDto));
        Console.WriteLine($"User created: {user}");
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    private void EditUser()
    {
        throw new NotImplementedException();
    }
}