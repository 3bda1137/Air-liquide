using System.Linq.Expressions;
using LinqKit;
using MediatR;
using MyProject.Domain.Entities;
using MyProject.EndPoints.Drivers.GetDrivers;
using MyProject.Extensions;
using MyProject.Infrastructures.Repositories;
using MyProject.Shared.Models;

namespace MyProject.Application.Features.Drivers.GetDrivers
{
    public record GetDriversQuery(GetDriversRequest request) : IRequest<RequestResult<PagingDto<GetDriversResponse>>>;

    public class GetDriversQueryHandler : RequestHandlerBase<GetDriversQuery, RequestResult<PagingDto<GetDriversResponse>>>
    {
        private readonly IRepository<Driver> _Repository;

        public GetDriversQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> Repository) : base(parameters)
        {
            _Repository = Repository;
        }
        public override async Task<RequestResult<PagingDto<GetDriversResponse>>> Handle(GetDriversQuery query, CancellationToken cancellationToken)
        {
            var predicate = CreatePredicate(query);

            var drivers = await _Repository.Get(predicate)
                .Select(MapToResponse())
                .OrderByPropertyName(query.request.OrderBy ?? nameof(GetDriversResponse.CreatedAt), query.request.IsAscending.GetValueOrDefault())
                .ToPagingDto(query.request.PageIndex.GetValueOrDefault(), query.request.PageSize.GetValueOrDefault(), cancellationToken);
            return RequestResult<PagingDto<GetDriversResponse>>.Success(drivers, "Drivers retrieved successfully.");
        }
        private Expression<Func<Driver, GetDriversResponse>> MapToResponse()
        {
            return driver => new GetDriversResponse
            {
                ID = driver.ID,
                Name = driver.Name,
                PhoneNumber = driver.PhoneNumber,
                PlantNumber = driver.PlantNumber,
                CarModel = driver.CarModel,
                CreatedAt =driver.CreatedAt
            };
        }

        private ExpressionStarter<Driver> CreatePredicate(GetDriversQuery query)
        {
            var predicate = PredicateBuilder.New<Driver>(true);

            if (!string.IsNullOrEmpty(query.request.Name))
            {
                predicate = predicate.And(d => d.Name.Contains(query.request.Name));
            }
            if (!string.IsNullOrEmpty(query.request.PhoneNumber))
            {
                predicate = predicate.And(d => d.PhoneNumber.Contains(query.request.PhoneNumber));
            }
            if (!string.IsNullOrEmpty(query.request.PlantNumber))
            {
                predicate = predicate.And(d => d.PlantNumber.Contains(query.request.PlantNumber));
            }
            if (!string.IsNullOrEmpty(query.request.CarModel))
            {
                predicate = predicate.And(d => d.CarModel.Contains(query.request.CarModel));
            }
            if (query.request.FromDate.HasValue)
            {
                predicate = predicate.And(d => DateOnly.FromDateTime(d.CreatedAt) >= query.request.FromDate.Value);
            }
            if (query.request.ToDate.HasValue)
            {
                predicate = predicate.And(d => DateOnly.FromDateTime(d.CreatedAt) <= query.request.ToDate.Value);
            }

            return predicate;
        }
    }
}
