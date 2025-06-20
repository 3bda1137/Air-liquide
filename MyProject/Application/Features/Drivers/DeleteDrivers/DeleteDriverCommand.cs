using MediatR;
using MyProject.Domain.Entities;
using MyProject.Infrastructures.Repositories;
using MyProject.Shared.Models;

namespace MyProject.Application.Features.Drivers.DeleteDrivers
{
    public record DeleteDriverCommand(Guid ID) : IRequest<Unit>;

    public class DeleteDriverCommandHandler : RequestHandlerBase<DeleteDriverCommand, Unit>
    {
        private readonly IRepository<Driver> _Repository;

        public DeleteDriverCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> Repository) : base(parameters)
        {
            _Repository = Repository;
        }
        public override async Task<Unit> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = ToDriver(request);
            _Repository.Delete(driver);
            return await Unit.Task;
        }

        private Driver ToDriver(DeleteDriverCommand request)
        {
            return new Driver
            {
                ID = request.ID
            };
        }
    }
}
