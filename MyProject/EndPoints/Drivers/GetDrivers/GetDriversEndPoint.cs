using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Features.Drivers.GetDrivers;
using MyProject.Shared.Models;

namespace MyProject.EndPoints.Drivers.GetDrivers
{
    public class GetDriversEndPoint : EndpointBase<GetDriversRequest, GetDriversResponse>
    {
        public GetDriversEndPoint(EndpointBaseParameters<GetDriversRequest> parameters) : base(parameters)
        {

        }

        [HttpGet("Driver/Get")]
        public async Task<EndPointResponse<PagingDto<GetDriversResponse>>> Handle([FromQuery]GetDriversRequest request, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetDriversQuery(request), cancellationToken).ConfigureAwait(false);

            return result;

        }
    }
}
