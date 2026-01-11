// See https://aka.ms/new-console-template for more information

using TicketBooking.Domain.Services;
using TicketBooking.Infrastructure.Repositories;
using TicketBooking.Presentation;

var flightRepo = new FlightRepository();
var flightServ = new FlightService(flightRepo);
var userService = new UserService();
var menu = new Menu(userService);
menu.Show();