using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Shared.Models;
using FluentValidation;
using MyProject.Filters;
using MyProject.Domain.Enums;
using MyProject.Helpers;

namespace MyProject.EndPoints
{
    [ApiController]
    [LanguageFilter]
    //[ServiceFilter(typeof(AuthorizeFilter))]
    //[Route("V4/")]
    public abstract class EndpointBase<TRequest, TResponse> : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IValidator<TRequest> _validator;

        public EndpointBase(EndpointBaseParameters<TRequest> dependencyCollection)
        {
            _mediator = dependencyCollection.Mediator;
            _validator = dependencyCollection.Validator;

            SetLanguage();
        }

        protected virtual async Task<EndPointResponse<TResponse>> ValidateRequestAsync(TRequest request)
        {
            if (_validator is null)
                return EndPointResponse<TResponse>.Success(default, "Validation successful");

            var validationResults = await _validator.ValidateAsync(request);

            if (validationResults.IsValid)
                return EndPointResponse<TResponse>.Success(default, "Validation successful");


            var validationErrors = string.Join(", ", validationResults.Errors.Select(e => e.ErrorMessage));
            var errMsg = string.Format("Validation failed:\n {0}", validationErrors);
            return EndPointResponse<TResponse>.Failure(ErrorCode.ValidationErrors, errMsg);
        }

        private void SetLanguage()
        {
            string lang = HttpRequestHelper.GetHeaderValue("lang");

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            lang = lang.ToLower();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
        }
    }
}
