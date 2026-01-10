// See https://aka.ms/new-console-template for more information

using TicketBooking.Domain.Models;
using TicketBooking.Infrastructure.Repositories;

var repo = new FlightRepository();

var results = await repo.GetAllAsync();

Console.WriteLine("Result");

results.Where(res => res.FlightClass == FlightClass.Business).ToList().ForEach(Console.WriteLine);
