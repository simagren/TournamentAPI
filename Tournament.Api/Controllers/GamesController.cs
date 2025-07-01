using AutoMapper;
using Bogus.DataSets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Data.Data;
using Tournament.Data.Repositories;

namespace Tournament.Api.Controllers;

[Route("api/tournamentDetails/{tournamentId}/games")]
[ApiController]
public class GamesController(TournamentApiContext _context, IMapper _mapper) : ControllerBase
{
    private readonly IMapper mapper = _mapper;
    private readonly UnitOfWork unitOfWork = new(_context);


    // GET: api/Games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int tournamentId)
    {
        if (!await TournamentExists(tournamentId))
            return NotFound();

        IEnumerable<Game> games = await unitOfWork.GameRepository.GetGamesAsync(tournamentId);
        IEnumerable<GameDto> gamesDto = mapper.Map<IEnumerable<GameDto>>(games);
        return Ok(gamesDto);
    }



    // GET: api/Games/5
    [HttpGet("{gameId}")]
    public async Task<ActionResult<GameDto>> GetGame(int tournamentId, int gameId)
    {
        if (! await TournamentExists(tournamentId))
            return NotFound();

        Game? game = await unitOfWork.GameRepository.GetGameAsync(tournamentId, gameId);
        if (game == null)
            return NotFound();

        var gameDto = mapper.Map<GameDto>(game);
        return Ok(gameDto);
    }


    [HttpGet("search")]
    public async Task<ActionResult<GameDto>> GetGame(string title)
    {
        var games = await unitOfWork.GameRepository.GetGamesByTitleAsync(title);
        if(!games.Any())
            return NotFound("No games containing that title");

        var gameDto = mapper.Map<IEnumerable<GameDto>>(games);
        return Ok(gameDto);
    }



    // PUT: api/Games/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int tournamentId, int gameId, GameDto gameDto)
    {
        if (gameId != gameDto.Id)
            return BadRequest();

        if (! await TournamentExists(tournamentId))
            return NotFound();

        var existingGame = await unitOfWork.GameRepository.GetGameAsync(tournamentId, gameId);
        if (existingGame == null)
            return NotFound("Game does not exist");

        mapper.Map(gameDto, existingGame);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }


    // POST: api/Games
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Game>> PostGame(int tournamentId, GameUpdateDto gameDto)
    {
        if (!await TournamentExists(tournamentId))
            return NotFound();

        var game = mapper.Map<Game>(gameDto);
        game.TournamentId = tournamentId;

        unitOfWork.GameRepository.Create(game);
        await unitOfWork.CompleteAsync();

        var createdGame = mapper.Map<GameDto>(game);
        return CreatedAtAction(nameof(GetGame), new { tournamentId = tournamentId, id = game.Id }, createdGame);
    }


    // DELETE: api/Games/5
    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteGame(int tournamentId, int gameId)
    {
        if(! await TournamentExists(tournamentId))
            return NotFound();

        var game = await unitOfWork.GameRepository.GetGameAsync(tournamentId, gameId);
        if (game == null)
            return NotFound();

        unitOfWork.GameRepository.Delete(game);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }


    [HttpPatch("{id:int}")]
    public async Task<ActionResult<GameDto>> PatchGame(int tournamentId, int id,
    JsonPatchDocument<GameUpdateDto> patchDocument)
    {
        if (patchDocument is null) 
            return BadRequest("No patchdocument");

       
        if (! await TournamentExists(tournamentId))
            return NotFound("Tournament does not exist");

        var gameToPatch = await unitOfWork.GameRepository.GetGameAsync(tournamentId, id, true);
        if (gameToPatch == null)
            return NotFound("Game does not exist");

        var dto = mapper.Map<GameUpdateDto>(gameToPatch);
        patchDocument.ApplyTo(dto, ModelState);
        TryValidateModel(dto);

        if(!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        mapper.Map(dto, gameToPatch);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }


    private bool GameExists(int id)
    {
        return _context.Game.Any(e => e.Id == id);
    }


    private async Task<bool> TournamentExists(int tournamentId)
    {
        return await unitOfWork.TournamentRepository.AnyAsync(tournamentId);
    }
}
