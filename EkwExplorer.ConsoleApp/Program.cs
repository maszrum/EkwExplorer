using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EkwExplorer.Core.Algorithms;
using EkwExplorer.ChromeScraper;
using EkwExplorer.Core;
using EkwExplorer.Core.Models;
using EkwExplorer.Persistence.Repositories;
using EkwExplorer.Persistence.SQLite;
using Serilog;
using System.Threading;

// ReSharper disable ClassNeverInstantiated.Global

namespace EkwExplorer.ConsoleApp
{
    internal class Program
    {
        private static ILogger _logger;
        
        private static async Task Main(string[] args)
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger()
                .ForContext<Program>();

            var dbConfiguration = new PersistenceConfiguration()
            {
                BookTable = "Book",
                PropertyTable = "Property"
            };
            var dbManager = new DbManager(dbConfiguration);

            ShowAvailableDatabasesInfo(dbManager);
            var input = ReadProgramInput(args);
            
            var inputLines = input.ToString().Split(Environment.NewLine);
            foreach(var inputLine in inputLines)
            {
                _logger.Information(inputLine);
            }

            await using var connection = dbManager.Exists(input.DatabaseFile)
                ? await dbManager.Connect(input.DatabaseFile)
                : await dbManager.Create(input.DatabaseFile);

            var repository = new BooksRepository(connection);

            await SeedDatabaseIfNeed(repository, input);
            
            var explorer = new BooksExplorer(_logger, repository);
            
            await explorer.Explore(CancellationToken.None);
        }

        private static ProgramInput ReadProgramInput(IReadOnlyList<string> args) =>
            args.Count switch
            {
                1 when args[0].Contains(".json") => new ProgramInput().ReadFromJson(args[0]),
                0 => new ProgramInput().ReadFromConsole(),
                _ => new ProgramInput().ReadFromArgs(args)
            };

        private static void ShowAvailableDatabasesInfo(DbManager dbManager)
        {
            var availableDatabases = dbManager.GetAvailableDatabases();
            if (availableDatabases.Count == 0)
            {
                _logger.Information("No available databases");
            }
            else
            {
                _logger.Information("Available databases:");
                _logger.Information(string.Join(Environment.NewLine, availableDatabases));
            }
        }

        private static async Task SeedDatabaseIfNeed(IBooksRepository repository, ProgramInput input)
        {
            if (input.NumberFrom.HasValue && input.NumberTo.HasValue)
            {
                var numberFrom = new BookNumber(input.CourtCode, input.NumberFrom.Value.ToString("D8"));
                var numberTo = new BookNumber(input.CourtCode, input.NumberTo.Value.ToString("D8"));

                var seeder = new DatasourceSeeder(repository);
                await seeder.SeedAsync(numberFrom, numberTo);
            }
        }
    }
}