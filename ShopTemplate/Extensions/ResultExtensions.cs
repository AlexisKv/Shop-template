using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Extensions;

public static class ResultExtensions
{
    public static ActionResult<TOut> ToActionResult<T, TOut>(this Result<T> result, Func<T, TOut> map)
    {
        if (result.IsFailed)
            return BuildErrorResponse(result.Errors.First());

        return new OkObjectResult(map(result.Value));
    }


    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsFailed)
        {
            return BuildErrorResponse(result.Errors.First());
        }
        
        return new OkResult();
    }

    private static ActionResult BuildErrorResponse(IError error)
    {
        var failureType = error.Metadata.TryGetValue("Type", out var typeObj) && typeObj is FailureTypes type
            ? type
            : default;

        var errorResponse = new
        {
            message = error.Message,
            code = failureType.ToString()
        };

        return failureType switch
        {
            FailureTypes.NotFound          => new NotFoundObjectResult(errorResponse),
            FailureTypes.InvalidPassword   => new UnauthorizedObjectResult(errorResponse),
            FailureTypes.InvalidOrderStatus => new BadRequestObjectResult(errorResponse),
            FailureTypes.PaymentFailed     => new BadRequestObjectResult(errorResponse),
            FailureTypes.AlreadyExists     => new ConflictObjectResult(errorResponse),
            _                              => new ObjectResult(errorResponse) { StatusCode = 501 }
        };
    }
}