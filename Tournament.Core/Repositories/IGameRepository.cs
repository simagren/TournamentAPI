using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetGamesAsync(int tournamentId, bool trackChanges = false);
    Task<Game?> GetGameAsync(int tournamentId, int gameId, bool trackChanges = false);
    Task<bool> AnyAsync(int id);
    void Add(Game tournamentDetails);
    void Update(Game tournamentDetails);
    void Remove(Game tournamentDetails);
}
