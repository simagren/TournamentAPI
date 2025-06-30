﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto;

public record TournamentCreateDto : TournamentForManipulation
{
    public IEnumerable<GameDto> Games { get; set; }
}
