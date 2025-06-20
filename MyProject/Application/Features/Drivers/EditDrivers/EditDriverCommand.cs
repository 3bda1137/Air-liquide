using MediatR;
using MyProject.Domain.Entities;
using MyProject.Infrastructures.Repositories;
using MyProject.Shared.Models;

namespace MyProject.Application.Features.Drivers.EditDrivers
{
    public record EditDriverCommand
    (
        Guid ID,
        string Name,
        string PhoneNumber,
        string PlantNumber,
        string CarModel
    ) : IRequest<Unit>;

    public class EditDriverCommandHandler : RequestHandlerBase<EditDriverCommand, Unit>
    {
        private readonly IRepository<Driver> _Repository;

        public EditDriverCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> Repository) : base(parameters)
        {
            _Repository = Repository;
        }
        public override async Task<Unit> Handle(EditDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = ToDriver(request);
            _Repository.SaveIncluded(driver,
                nameof(Driver.Name),
                nameof(Driver.PhoneNumber),
                nameof(Driver.PlantNumber),
                nameof(Driver.CarModel)
                );
            return await Unit.Task;
        }

        private Driver ToDriver(EditDriverCommand request)
        {
            return new Driver
            {
                ID = request.ID,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                PlantNumber = request.PlantNumber,
                CarModel = request.CarModel
            };
        }
    }
}
