using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public static class SeedData
{
    public static async Task SeedAsync(TournamentApiContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.TournamentDetails.AnyAsync())
            return;

        try
        {
            var tournamentDetails = GenerateTournamentDetails(4);
            context.AddRange(tournamentDetails);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private static List<TournamentDetails> GenerateTournamentDetails(int nrOfTournaments)
    {
        var faker = new Faker<TournamentDetails>("sv").Rules((f, t) =>
        {
            t.Title = $"{f.Address.City()} Open";
            t.StartDate = f.Date.Past(f.Random.Int(1, 40));
        });
            
        var tournaments = faker.Generate(nrOfTournaments);
        foreach(var tournament in tournaments)
            tournament.Games = GenerateGames(7, tournament);

        return tournaments;  
    }

    private static List<Game> GenerateGames(int nrOfGames, TournamentDetails tournament)
    {
        var faker = new Faker<Game>("sv").Rules((f, g) =>
        {
            g.Title = $"{f.Name.FullName()} VS {f.Name.FullName()}";
            g.Time = tournament.StartDate.AddHours(f.Random.Int(1, 72));
            g.TournamentDetails = tournament;
        });
        return faker.Generate(nrOfGames);
    }
}
