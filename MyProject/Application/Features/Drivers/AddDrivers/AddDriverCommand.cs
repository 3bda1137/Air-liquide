using MediatR;
using MyProject.Domain.Entities;
using MyProject.Infrastructures.Repositories;
using MyProject.Shared.Models;

namespace MyProject.Application.Features.Drivers.AddDrivers
{
    public record AddDriverCommand
    (
        string Name,
        string PhoneNumber,
        string PlantNumber,
        string CarModel
    ) : IRequest<Unit>;

    public class AddDriverCommandHandler : RequestHandlerBase<AddDriverCommand, Unit>
    {
        private readonly IRepository<Driver> _Repository;

        public AddDriverCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> Repository) : base(parameters)
        {
            _Repository = Repository;
        }
        public override async Task<Unit> Handle(AddDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = ToDriver(request);
            _Repository.Add(driver);
            return await Unit.Task;
        }

        private Driver ToDriver(AddDriverCommand request)
        {
            return new Driver
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                PlantNumber = request.PlantNumber,
                CarModel = request.CarModel
            };
        }
    }
}
