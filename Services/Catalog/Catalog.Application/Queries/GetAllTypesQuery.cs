using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public record GetAllTypesQuery : IRequest<IList<TypesResponse>>
    {
    }
}
