using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Responses
{
    public record TypesResponse
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }
}
