using AutoMapper;
using Azure;
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

[Route("api/[controller]")]
[ApiController]
public class TournamentDetailsController(TournamentApiContext _context, IMapper _mapper) : ControllerBase
{
    private readonly IMapper mapper = _mapper;
    private readonly UnitOfWork unitOfWork = new(_context);



    // GET: api/TournamentDetails
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails(bool includeGames)
    {
        var tournaments = await unitOfWork.TournamentRepository.GetAllAsync(includeGames);
        var tournamentDto = mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        return Ok(tournamentDto);
    }



    // GET: api/TournamentDetails/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
    {
        TournamentDetails? tournamentDetails = await unitOfWork.TournamentRepository.GetAsync(id);

        if (tournamentDetails == null)
            return NotFound();

        return mapper.Map<TournamentDto>(tournamentDetails);
    }



    // PUT: api/TournamentDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, TournamentUpdateDto tournamentDto)
    {
        if (id != tournamentDto.Id)
            return BadRequest();


        //_context.Entry(tournamentDetails).State = EntityState.Modified;
        var existingTournament = await unitOfWork.TournamentRepository.GetAsync(id, true);
        if (existingTournament == null)
            return NotFound("Tournament does not exist");

        mapper.Map(tournamentDto, existingTournament);

        await unitOfWork.CompleteAsync();
        return NoContent();
        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!TournamentDetailsExists(id))
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        throw;
        //    }
        //}

        //return NoContent();
    }

    // POST: api/TournamentDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentDetails>> PostTournamentDetails(TournamentCreateDto tournamentDto)
    {
        var tournament = mapper.Map<TournamentDetails>(tournamentDto);
        unitOfWork.TournamentRepository.Create(tournament);
        await unitOfWork.CompleteAsync();
        var createdTournament = mapper.Map<TournamentDto>(tournament);
        return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournament.Id }, createdTournament);

        //_context.TournamentDetails.Add(tournamentDetails);
        //await _context.SaveChangesAsync();

        //return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDetails);
    }

    // DELETE: api/TournamentDetails/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentDetails(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return NotFound("Tournament not found");

        unitOfWork.TournamentRepository.Delete(tournament);
        await unitOfWork.CompleteAsync();
        return NoContent();


        //var tournamentDetails = await _context.TournamentDetails.FindAsync(id);
        //if (tournamentDetails == null)
        //{
        //    return NotFound();
        //}

        //_context.TournamentDetails.Remove(tournamentDetails);
        //await _context.SaveChangesAsync();

        //return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<TournamentDto>>PatchTournament(int id, 
        JsonPatchDocument<TournamentUpdateDto> patchDocument)
    {
        if (patchDocument is null)
            return BadRequest("No patchdocument");

        var tournamentToPatch = await unitOfWork.TournamentRepository.GetAsync(id, true);
        if (tournamentToPatch == null)
            return NotFound("Tournament does not exist");

        var dto = mapper.Map<TournamentUpdateDto>(tournamentToPatch);
        patchDocument.ApplyTo(dto, ModelState);
        TryValidateModel(dto);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        mapper.Map(dto, tournamentToPatch);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }


    private bool TournamentDetailsExists(int id)
    {
        return _context.TournamentDetails.Any(e => e.Id == id);
    }
}
