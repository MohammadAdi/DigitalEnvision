using AutoWrapper.Wrappers;
using DigitalEnvision.Assigment.Infrastructures;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Features.User.Commands
{
    public class Create
    {
        public class Command : IRequest<ApiResponse>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirtdayDate { get; set; }
            public long LocationId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName cannot be null");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName cannot be null");
                RuleFor(x => x.BirtdayDate).NotEmpty().WithMessage("BirthDay Date cannot be null");
                RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location cannot be null");
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
                    var location = await _context.Locations.FirstOrDefaultAsync(x => x.Id == request.LocationId);
                    if (location is null)
                        return new ApiResponse("Location is not found", statusCode: 404);

                    var user = new Models.User()
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        BirtdayDate = request.BirtdayDate,
                        LocationId = request.LocationId
                    };
                    _context.Users.Add(user);
                    await _context.SaveChanges();
                    return new ApiResponse("User is saved", user);
                }
                catch (Exception ex)
                {
                    throw new ApiException(ex.Message);
                }
            }
        }


    }
}
