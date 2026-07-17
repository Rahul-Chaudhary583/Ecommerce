using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Commands
{
    public record DeleteProductByIdCommand(string Id) : IRequest<bool>;
}
