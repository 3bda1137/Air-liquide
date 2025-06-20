using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Features.Drivers.AddDrivers;
using MyProject.Shared.Models;

namespace MyProject.EndPoints.Drivers.AddDrivers
{
    public class AddDriverEndPoint : EndpointBase<AddDriverRequest, AddDriverResponse>
    {
        public AddDriverEndPoint(EndpointBaseParameters<AddDriverRequest> parameters) : base(parameters)
        {

        }

        [HttpPost("Driver/Add")]
        public async Task<EndPointResponse<AddDriverResponse>> Handle(AddDriverRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request).ConfigureAwait(false);

            if (!validationResult.IsSuccess) { return validationResult; }

            var result = await _mediator.Send(new AddDriverOrchestrator(request), cancellationToken).ConfigureAwait(false);

            return result.IsSuccess
                ? EndPointResponse<AddDriverResponse>.Success(result.Data, result.Message)
                : EndPointResponse<AddDriverResponse>.Failure(result.ErrorCode);
        }
    }
}
