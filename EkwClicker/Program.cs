using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EkwClicker.Algorithms;
using EkwClicker.Datasource;
using EkwClicker.Datasource.Repositories;
using EkwClicker.Models;
using EkwClicker.Seeker;
using Serilog;

// ReSharper disable ClassNeverInstantiated.Global

namespace EkwClicker
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger()
                .ForContext<Program>();
            
            var input = ReadProgramInput(args);
            
            var inputLines = input.ToString().Split(Environment.NewLine);
            foreach(var inputLine in inputLines)
            {
                logger.Information(inputLine);
            }

            await using var connection = DbAccess.Exists(input.DatabaseFile)
                ? await DbAccess.Connect(input.DatabaseFile)
                : await DbAccess.Create(input.DatabaseFile);

            var repository = new BooksRepository(connection);

            if (input.NumberFrom.HasValue && input.NumberTo.HasValue)
            {
                var numberFrom = new BookNumber(input.CourtCode, input.NumberFrom.Value.ToString("D8"));
                var numberTo = new BookNumber(input.CourtCode, input.NumberTo.Value.ToString("D8"));
                
                var seeder = new DatasourceSeeder(repository);
                await seeder.SeedAsync(numberFrom, numberTo);
            }
            
            var explorer = new BooksExplorer(logger, repository);
            
            await explorer.Open();
            await explorer.Explore();
        }
        
        private static ProgramInput ReadProgramInput(IReadOnlyList<string> args)
        {
            switch (args.Count)
            {
                case 1 when args[0].Contains(".json"):
                    return new ProgramInput().ReadFromJson(args[0]);
                case 0:
                {
                    var availableDatabases = DbAccess.GetAvailableDatabases();
                    if (availableDatabases.Count == 0)
                        Console.WriteLine("No available databases");
                    else
                    {
                        Console.WriteLine("Available databases:");
                        Console.WriteLine(string.Join(Environment.NewLine, availableDatabases));
                    }
                
                    return new ProgramInput().ReadFromConsole();
                }
                default:
                    return new ProgramInput().ReadFromArgs(args);
            }
        }
    }
}