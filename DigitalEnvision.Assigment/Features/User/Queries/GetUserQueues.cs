using AutoWrapper.Wrappers;
using DigitalEnvision.Assigment.Helpers.Enums;
using DigitalEnvision.Assigment.Infrastructures;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Features.User.Queries
{
    public class GetUserQueues
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
                    var users = await _context.Users.Include(x => x.Alerts)
                                            .Where(x => !x.Alerts.Any(x => x.LastExecution.Year <= DateTime.UtcNow.Year))
                                            .Select(x => new
                                            {
                                                x.Id,
                                                x.FirstName,
                                                x.LastName
                                            }).ToArrayAsync();

                    return new ApiResponse("Success", users);
                }
                catch (Exception ex)
                {
                    throw new ApiException(ex.Message);
                }
            }
        }

    }
}
