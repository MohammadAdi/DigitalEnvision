using AutoWrapper.Wrappers;
using DigitalEnvision.Assigment.Infrastructures;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Features.User.Commands
{
    public class Delete
    {
        public class Command : IRequest<ApiResponse>
        {
            public long Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be null");
            }
        }

        public class Handler : IRequestHandler<Command, ApiResponse>
        {
            private readonly IApplicationDbContext _context;
            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

                    if (existingUser is null)
                        return new ApiResponse("User Not Found", statusCode: 404);

                    _context.Users.Remove(existingUser);
                    await _context.SaveChanges();
                    return new ApiResponse("User is deleted", request.Id);
                }
                catch (Exception ex)
                {
                    throw new ApiException(ex.Message);
                }
            }
        }


    }
}
