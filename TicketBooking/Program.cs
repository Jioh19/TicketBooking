// See https://aka.ms/new-console-template for more information

using TicketBooking.Domain.Models;
using TicketBooking.Domain.Models.Enums;
using TicketBooking.Domain.Services;
using TicketBooking.Infrastructure.Repositories;

var flightRepo = new FlightRepository();
var flightServ = new FlightService(flightRepo);
var userServ = new UserService();
var bookingServ = new BookingService();

var results = await flightServ.GetFlightsAsync();

Console.WriteLine("Result");
flightServ.GetFlightsByParametersAsync("usa", null, FlightClass.FirstClass).Result.ToList().ForEach(Console.WriteLine);
Console.WriteLine("Origin");
flightServ.GetAllOriginsAsync().Result.ToList().ForEach(Console.WriteLine);
Console.WriteLine("Destination");
flightServ.GetAllDestinationsAsync().Result.ToList().ForEach(Console.WriteLine);
var flight = flightServ.GetFlightByIdAsync(10).Result;

Console.WriteLine("Users");
var user = new User
{
    Id = 13,
    FirstName = "John",
    LastName = "Doe",
    Username = "joedoe",
    Email = "jow@doe.com"
};
userServ.AddUser(user);
userServ.GetAllUsers().ToList().ForEach(Console.WriteLine);

Console.WriteLine("Booking");
if (flight is not null)
{
    var booking = new Booking
    {
        Id = 1,
        Flight = new EntityReference<long> { Id = flight.Id },
        User = new EntityReference<long> { Id = user.Id, Name = user.Username },
        State = BookingState.Active
    };
    bookingServ.AddBooking(booking);
}
bookingServ.GetAllBookings().ToList().ForEach(Console.WriteLine);
