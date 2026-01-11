using TicketBooking.Domain.Validations;
using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Domain.Services;
using TicketBooking.Presentation.DTOs;
using TicketBooking.Presentation.Mappers;

namespace TicketBooking.Presentation;

public class UserMenu
{
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly IFlightService _flightService;
    
    public UserMenu(IUserService userService, IBookingService bookingService, IFlightService flightService)
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
            Console.WriteLine("=== User Menu ===");
            Console.WriteLine("1. Book a Flight");
            Console.WriteLine("2. Search for Available Flights");
            Console.WriteLine("3. Manage Bookings");
            Console.WriteLine("4. Add User");
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
        Console.Clear();
        Console.WriteLine("=== Book a Flight ===");
        Console.WriteLine("Enter search parameters (leave blank to skip):");
        Console.Write("Departure Country: ");
        var departureCountry = Console.ReadLine();
        Console.Write("Destination Country: ");
        var destinationCountry = Console.ReadLine();
        Console.Write("Class (Economy, Business, FirstClass): ");
        var classInput = Console.ReadLine();
        FlightClass? flightClass = null;
        if (!string.IsNullOrWhiteSpace(classInput) && Enum.TryParse<FlightClass>(classInput, true, out var fc))
            flightClass = fc;

        var flights = _flightService.GetFlightsByParametersAsync(
            departureCountry,
            destinationCountry,
            flightClass
        ).Result.ToList();

        if (flights.Count == 0)
        {
            Console.WriteLine("No flights found matching the criteria.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("=== Available Flights ===");
        foreach (var flight in flights)
        {
            Console.WriteLine($"Id: {flight.Id}, {flight.DepartureCountry} -> {flight.DestinationCountry}, Date: {flight.DepartureDate:yyyy-MM-dd}, Class: {flight.FlightClass}, Price: {flight.Price:C}");
        }

        long flightId;
        Flight? selectedFlight = null;
        do
        {
            Console.Write("Enter Flight Id to book: ");
            var flightIdInput = Console.ReadLine();
            if (!long.TryParse(flightIdInput, out flightId))
            {
                Console.WriteLine("Invalid Flight Id. Try again.");
                continue;
            }
            selectedFlight = flights.FirstOrDefault(f => f.Id == flightId);
            if (selectedFlight == null)
            {
                Console.WriteLine("Flight not found. Try again.");
            }
        } while (selectedFlight == null);
        
        Console.WriteLine("=== Select User ===");
        var users = _userService.GetAllUsers().ToList();
        if (users.Count == 0)
        {
            Console.WriteLine("No users found. Please add a user first.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }
        users.ForEach(Console.WriteLine);
        long userId;
        User? selectedUser = null;
        do
        {
            Console.Write("Enter User Id to book for: ");
            var userIdInput = Console.ReadLine();
            if (!long.TryParse(userIdInput, out userId))
            {
                Console.WriteLine("Invalid User Id. Try again.");
                continue;
            }
            selectedUser = users.FirstOrDefault(u => u.Id == userId);
            if (selectedUser == null)
            {
                Console.WriteLine("User not found. Try again.");
            }
        } while (selectedUser == null);
        
        var booking = new Booking
        {
            Id = 0,
            Flight = new EntityReference<long> { Id = selectedFlight.Id },
            User = new EntityReference<long> { Id = selectedUser.Id, Name = selectedUser.FirstName + " " + selectedUser.LastName },
            State = BookingState.Active
        };
        booking = _bookingService.AddBooking(booking);
        Console.WriteLine($"Booking created! Id: {booking.Id}\n" +
                          $" {_flightService.GetFlightByIdAsync(selectedFlight.Id).Result}\n" +
                          $" {_userService.GetUserById(selectedUser.Id)}");
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    private void SearchFlights()
    {
        Console.Clear();
        Console.WriteLine("=== Search for Available Flights ===");
        Console.WriteLine("Enter search parameters (leave blank to skip):");

        Console.Write("Departure Country: ");
        var departureCountry = Console.ReadLine();
        Console.Write("Destination Country: ");
        var destinationCountry = Console.ReadLine();
        Console.Write("Class (Economy, Business, FirstClass): ");
        var classInput = Console.ReadLine();
        FlightClass? flightClass = null;
        if (!string.IsNullOrWhiteSpace(classInput) && Enum.TryParse<FlightClass>(classInput, true, out var fc))
            flightClass = fc;

        var results = _flightService.GetFlightsByParametersAsync(
            departureCountry,
            destinationCountry,
            flightClass
        ).Result;

        var flights = results.ToList();
        if (flights.Count == 0)
        {
            Console.WriteLine("No flights found matching the criteria.");
        }
        else
        {
            Console.WriteLine("=== Matching Flights ===");
            flights.ForEach(Console.WriteLine);
        }
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    private void ManageBookings()
    {
        Console.Clear();
        Console.WriteLine("=== Manage Bookings ===");
        // Select user
        var users = _userService.GetAllUsers().ToList();
        if (users.Count == 0)
        {
            Console.WriteLine("No users found. Please add a user first.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }
        users.ForEach(Console.WriteLine);
        long userId;
        User? selectedUser = null;
        do
        {
            Console.Write("Enter User Id to manage bookings for: ");
            var userIdInput = Console.ReadLine();
            if (!long.TryParse(userIdInput, out userId))
            {
                Console.WriteLine("Invalid User Id. Try again.");
                continue;
            }
            selectedUser = users.FirstOrDefault(u => u.Id == userId);
            if (selectedUser == null)
            {
                Console.WriteLine("User not found. Try again.");
            }
        } while (selectedUser == null);

        var bookings = _bookingService.GetBookingsByUserId(selectedUser.Id).ToList();
        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings found for this user.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("=== Bookings ===");
        foreach (var booking in bookings)
        {
            Console.WriteLine($"Id: {booking.Id}, Flight: {booking.Flight.Id}, State: {booking.State}");
        }
        long bookingId;
        Booking? selectedBooking = null;
        do
        {
            Console.Write("Enter Booking Id to manage (or 0 to return): ");
            var bookingIdInput = Console.ReadLine();
            if (!long.TryParse(bookingIdInput, out bookingId) || bookingId < 0)
            {
                Console.WriteLine("Invalid Booking Id. Try again.");
                continue;
            }
            if (bookingId == 0) return;
            selectedBooking = bookings.FirstOrDefault(b => b.Id == bookingId);
            if (selectedBooking == null)
            {
                Console.WriteLine("Booking not found. Try again.");
            }
        } while (selectedBooking == null);

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Booking Id: {selectedBooking.Id}\nFlight: {selectedBooking.Flight.Id}\nUser: {selectedBooking.User.Id}\nCurrent State: {selectedBooking.State}");
            Console.WriteLine("Change State:");
            foreach (var state in Enum.GetValues(typeof(BookingState)))
            {
                Console.WriteLine($"{(int)state}. {state}");
            }
            Console.WriteLine("0. Return");
            Console.Write("Select new state: ");
            var action = Console.ReadLine();
            if (action == "0") return;
            if (int.TryParse(action, out int stateValue) && Enum.IsDefined(typeof(BookingState), stateValue))
            {
                var newState = (BookingState)stateValue;
                if (newState == selectedBooking.State)
                {
                    Console.WriteLine("Booking is already in this state.");
                }
                else
                {
                    _bookingService.ModifyBooking(selectedBooking.Id, selectedBooking with { State = newState });
                    Console.WriteLine($"Booking state changed to {newState}.");
                }
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Invalid state. Press any key to try again...");
                Console.ReadKey();
            }
        }
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
}