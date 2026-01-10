// See https://aka.ms/new-console-template for more information

using TicketBooking.Infrastructure.Repositories;

var repo = new FlightRepository();

var results = await repo.GetAllAsync();

Console.WriteLine("Result");

results.ToList().ForEach(Console.WriteLine);