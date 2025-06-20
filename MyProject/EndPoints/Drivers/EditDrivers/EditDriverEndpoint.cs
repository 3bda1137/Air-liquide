using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Features.Drivers.EditDrivers;
using MyProject.Shared.Models;

namespace MyProject.EndPoints.Drivers.EditDrivers
{
    public class EditDriverEndPoint : EndpointBase<EditDriverRequest, EditDriverResponse>
    {
        public EditDriverEndPoint(EndpointBaseParameters<EditDriverRequest> parameters) : base(parameters)
        {

        }

        [HttpPut("Driver/Edit")]
        public async Task<EndPointResponse<EditDriverResponse>> Handle(EditDriverRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request).ConfigureAwait(false);

            if (!validationResult.IsSuccess) { return validationResult; }

            var result = await _mediator.Send(new EditDriverOrchestrator(request), cancellationToken).ConfigureAwait(false);

            return result.IsSuccess
                ? EndPointResponse<EditDriverResponse>.Success(result.Data, result.Message)
                : EndPointResponse<EditDriverResponse>.Failure(result.ErrorCode);
        }
    }
}
