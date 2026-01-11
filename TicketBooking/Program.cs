// See https://aka.ms/new-console-template for more information

using TicketBooking.Domain.Models;
using TicketBooking.Domain.Services;
using TicketBooking.Infrastructure.Repositories;

var repo = new FlightRepository();
var serv = new FlightService(repo);

var results = await serv.GetFlightsAsync();

Console.WriteLine("Result");
serv.GetFlightsByParametersAsync("usa", null, FlightClass.FirstClass).Result.ToList().ForEach(Console.WriteLine);
Console.WriteLine("Origin");
serv.GetAllOriginsAsync().Result.ToList().ForEach(Console.WriteLine);
Console.WriteLine("Destination");
serv.GetAllDestinationsAsync().Result.ToList().ForEach(Console.WriteLine);
