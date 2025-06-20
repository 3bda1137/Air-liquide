using FluentValidation;
using MediatR;
using MyProject.Infrastructures.DbContexts;
using System.Net;

namespace MyProject.EndPoints
{
    public class EndpointBaseParameters<TRequest>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<TRequest> _validator;

        public IMediator Mediator => _mediator;
        public IValidator<TRequest> Validator => _validator;
        private ApplicationDbContext _context;

        public EndpointBaseParameters(IServiceProvider serviceProvider)
        {
            _context = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            _mediator = (IMediator)serviceProvider.GetService(typeof(IMediator));
            _validator = (IValidator<TRequest>)serviceProvider.GetService(typeof(IValidator<TRequest>));

            SetUserStateData();
        }

        private void SetUserStateData()
        {
        }
    }
}
