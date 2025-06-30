﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface ITournamentRepository
{
    Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames = false, bool trackChanges = false);
    Task<TournamentDetails?> GetAsync(int id, bool trackChanges = false);
    Task<bool> AnyAsync(int id);
    void Add(TournamentDetails tournamentDetails);
    void Update(TournamentDetails tournamentDetails);
    void Remove(TournamentDetails tournamentDetails);
}
