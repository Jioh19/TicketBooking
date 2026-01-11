// See https://aka.ms/new-console-template for more information

using TicketBooking.Domain.Models;
using TicketBooking.Domain.Services;
using TicketBooking.Infrastructure.Repositories;

var repo = new FlightRepository();
var serv = new FlightService(repo);

var results = await serv.GetFlightsAsync();

Console.WriteLine("Result");

results.Where(res => res.FlightClass == FlightClass.FirstClass).ToList().ForEach(Console.WriteLine);
