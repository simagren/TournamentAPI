﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto;

public record TournamentUpdateDto : TournamentForManipulation
{
    public int Id { get; set; }
}
