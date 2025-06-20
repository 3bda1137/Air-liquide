using MediatR;
using MyProject.Domain.Enums;
using MyProject.EndPoints.Drivers.DeleteDrivers;
using MyProject.Shared.Models;

namespace MyProject.Application.Features.Drivers.DeleteDrivers
{
    public record DeleteDriverOrchestrator(DeleteDriverRequest request) : IRequest<RequestResult<DeleteDriverResponse>>;

    public class DeleteDriverOrchestratorHandler : RequestHandlerBase<DeleteDriverOrchestrator, RequestResult<DeleteDriverResponse>>
    {
        public DeleteDriverOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<RequestResult<DeleteDriverResponse>> Handle(DeleteDriverOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = Validation(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }
            await _mediator.Send(new DeleteDriverCommand
            (
               ID:Guid.Parse(request.request.ID)
            ), cancellationToken);

            return RequestResult<DeleteDriverResponse>.Success(null, "Driver Deleteed successfully.");
        }

        private RequestResult<DeleteDriverResponse> Validation(DeleteDriverOrchestrator request)
        {
            if (string.IsNullOrWhiteSpace(request.request.ID))
            {
                return RequestResult<DeleteDriverResponse>.Failure(ErrorCode.IdRequired);
            }

            if (!Guid.TryParse(request.request.ID, out _))
            {
                return RequestResult<DeleteDriverResponse>.Failure(ErrorCode.InvalidIdFormat);
            }

            return RequestResult<DeleteDriverResponse>.Success();
        }
    }
}
