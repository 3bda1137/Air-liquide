using MediatR;
using MyProject.Domain.Enums;
using MyProject.EndPoints.Drivers.EditDrivers;
using MyProject.Shared.Models;

namespace MyProject.Application.Features.Drivers.EditDrivers
{
    public record EditDriverOrchestrator(EditDriverRequest request) : IRequest<RequestResult<EditDriverResponse>>;

    public class EditDriverOrchestratorHandler : RequestHandlerBase<EditDriverOrchestrator, RequestResult<EditDriverResponse>>
    {
        public EditDriverOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<RequestResult<EditDriverResponse>> Handle(EditDriverOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = Validation(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }
            await _mediator.Send(new EditDriverCommand
            (
                ID: Guid.Parse(request.request.ID),
                Name: request.request.Name,
                PhoneNumber: request.request.PhoneNumber,
                PlantNumber: request.request.PlantNumber,
                CarModel: request.request.CarModel
            ), cancellationToken);

            return RequestResult<EditDriverResponse>.Success(null, "Driver Edited successfully.");
        }

        private RequestResult<EditDriverResponse> Validation(EditDriverOrchestrator request)
        {
            if (string.IsNullOrWhiteSpace(request.request.ID))
            {
                return RequestResult<EditDriverResponse>.Failure(ErrorCode.IdRequired);
            }

            if (!Guid.TryParse(request.request.ID, out _))
            {
                return RequestResult<EditDriverResponse>.Failure(ErrorCode.InvalidIdFormat);
            }

            return RequestResult<EditDriverResponse>.Success();
        }
    }
}
