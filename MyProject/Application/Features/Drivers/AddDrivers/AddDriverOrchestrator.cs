using MediatR;
using MyProject.EndPoints.Drivers.AddDrivers;
using MyProject.Shared.Models;

namespace MyProject.Application.Features.Drivers.AddDrivers
{
    public record AddDriverOrchestrator(AddDriverRequest request) : IRequest<RequestResult<AddDriverResponse>>;

    public class AddDriverOrchestratorHandler : RequestHandlerBase<AddDriverOrchestrator, RequestResult<AddDriverResponse>>
    {
        public AddDriverOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<RequestResult<AddDriverResponse>> Handle(AddDriverOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddDriverCommand
            (
                Name: request.request.Name,
                PhoneNumber: request.request.PhoneNumber,
                PlantNumber: request.request.PlantNumber,
                CarModel: request.request.CarModel
            ), cancellationToken);

            return RequestResult<AddDriverResponse>.Success(null, "Driver added successfully.");
        }
    }
}
