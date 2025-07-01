using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class GameRepository(TournamentApiContext context) : RepositoryBase<Game>(context), IGameRepository
{
    public async Task<bool> AnyAsync(int id)
    {
        return await GetWithCondition(t => t.Id.Equals(id)).AnyAsync();
    }

    public async Task<IEnumerable<Game>> GetGamesAsync(int tournamentId, bool trackChanges = false)
    {
        return await GetWithCondition(g => g.TournamentId.Equals(tournamentId), trackChanges).ToListAsync();
    }

    public async Task<IEnumerable<Game>> GetGamesByTitleAsync(string title, bool trackChanges = false)
    {
        return await GetWithCondition(g => g.Title.ToLower().Contains(title.ToLower()), trackChanges).ToListAsync();
    }

    public async Task<Game?> GetGameAsync(int tournamentId, int gameId, bool trackChanges = false)
    {
        return await GetWithCondition(g => g.Id.Equals(gameId) && g.TournamentId.Equals(tournamentId), trackChanges).FirstOrDefaultAsync();
    }

    public void Create(Game tournamentDetails)
    {
        Create(tournamentDetails);
    }

    public void Delete(Game tournamentDetails)
    {
        Delete(tournamentDetails);
    }

    public void Update(Game tournamentDetails)
    {
        //Update(tournamentDetails);
    }
}
