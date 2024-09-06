﻿using ErrorOr;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr<object>
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehaviour(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next();
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors
                .ConvertAll(x => Error.Validation(
                    code: x.PropertyName,
                    description: x.ErrorMessage));

            return (dynamic) errors;
        }
    }
}