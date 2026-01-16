// See https://aka.ms/new-console-template for more information

using TicketBooking.Domain.Services;
using TicketBooking.Infrastructure.Repositories;
using TicketBooking.Presentation;

var flightRepo = new FlightRepository();
var flightService = new FlightService(flightRepo);
var bookingRepo = new BookingRepository();
var bookingService = new BookingService(bookingRepo);
var userRepo = new UserRepository();
var userService = new UserService(userRepo);
//Test.RunTest();
var menu = new Menu(userService, bookingService, flightService); menu.Show();