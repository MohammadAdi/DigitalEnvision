using AutoWrapper.Wrappers;
using DigitalEnvision.Assigment.Infrastructures;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Features.Location.Queries
{
    public class GetLocations
    {
        public class Query : IRequest<ApiResponse>
        {
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
            }
        }

        public class Handler : IRequestHandler<Query, ApiResponse>
        {
            private readonly IApplicationDbContext _context;
            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var locations = await _context.Locations.ToListAsync();
                    return new ApiResponse("User is saved", locations);
                }
                catch (Exception ex)
                {
                    throw new ApiException(ex.Message);
                }
            }
        }

    }
}
