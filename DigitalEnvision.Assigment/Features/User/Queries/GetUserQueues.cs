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
                    var birthDayUser = await _context.Users.Include(x => x.Alerts)
                                            .Where(x => !x.Alerts.Any(x => x.LastExecution !=null|| x.LastExecution.Value.Year <= DateTime.UtcNow.Year))
                                            .Select(x => x.Id).ToArrayAsync();

                    var userIds = await _context.Users.Include(x => x.Alerts)
                            .Where(x => !x.Alerts.Any(y => y.LastExecution !=null || y.LastExecution.Value.Year <= DateTime.UtcNow.Year) && !x.Alerts.Any(x=>birthDayUser.Contains(x.Id)))
                            .Select(x => new { x.Id, x.FirstName, x.LastName
                            }).ToArrayAsync();

                    return new ApiResponse("Success", userIds);
                }
                catch (Exception ex)
                {
                    throw new ApiException(ex.Message);
                }
            }
        }

    }
}
