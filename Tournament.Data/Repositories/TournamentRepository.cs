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

public class TournamentRepository(TournamentApiContext context) : RepositoryBase<TournamentDetails>(context), ITournamentRepository
{
    public async Task<bool> AnyAsync(int id)
    {
        return await GetWithCondition(t => t.Id.Equals(id)).AnyAsync();
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames = false, bool trackChanges = false)
    {
        return includeGames ? await GetAll(trackChanges).Include(t => t.Games).ToListAsync()
                            : await GetAll(trackChanges).ToListAsync();
    }

    public async Task<TournamentDetails?> GetAsync(int id, bool trackChanges = false)
    {
        return await GetWithCondition(t => t.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
    }

    //public void Add(TournamentDetails tournamentDetails)
    //{
    //    Create(tournamentDetails);
    //}

    //public void Remove(TournamentDetails tournamentDetails)
    //{
    //    Delete(tournamentDetails);
    //}

    //public void Update(TournamentDetails tournamentDetails)
    //{
    //    //Update(tournamentDetails);
    //}
}
