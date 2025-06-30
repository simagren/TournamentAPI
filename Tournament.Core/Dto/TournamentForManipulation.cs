using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto;

public record TournamentForManipulation
{
    [Required(ErrorMessage = "Title is a required field")]
    [MaxLength(40, ErrorMessage = "Maximun length for title is 40 characters")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Start date is a required field")]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
