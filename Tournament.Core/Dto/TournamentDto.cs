﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto;

public record TournamentDto
{
    public string Title { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
