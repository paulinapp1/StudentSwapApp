﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ListingsService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Condition
    {
        USED,
        NEW,
        LIKE_NEW,
        DAMAGED
    }
}
