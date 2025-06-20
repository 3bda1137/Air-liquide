using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Features.Drivers.DeleteDrivers;
using MyProject.Shared.Models;

namespace MyProject.EndPoints.Drivers.DeleteDrivers
{
    public class DeleteDriverEndPoint : EndpointBase<DeleteDriverRequest, DeleteDriverResponse>
    {
        public DeleteDriverEndPoint(EndpointBaseParameters<DeleteDriverRequest> parameters) : base(parameters)
        {

        }

        [HttpDelete("Driver/Delete")]
        public async Task<EndPointResponse<DeleteDriverResponse>> Handle(DeleteDriverRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request).ConfigureAwait(false);

            if (!validationResult.IsSuccess) { return validationResult; }

            var result = await _mediator.Send(new DeleteDriverOrchestrator(request), cancellationToken).ConfigureAwait(false);

            return result.IsSuccess
                ? EndPointResponse<DeleteDriverResponse>.Success(result.Data, result.Message)
                : EndPointResponse<DeleteDriverResponse>.Failure(result.ErrorCode);
        }
    }
}
