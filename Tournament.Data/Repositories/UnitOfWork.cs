using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class UnitOfWork(TournamentApiContext context) : IUnitOfWork
{
    public ITournamentRepository TournamentRepository { get; private set; } = new TournamentRepository(context);

    public IGameRepository GameRepository { get; private set; } = new GameRepository(context);

    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}
