using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookService.ControllerExtensions
{
    public static class ControllerBaseExtensions
    {
        /* ==== Conflict() ==== */
        // Http Status Code 409 - Conflict
        public static ConflictResult Conflict(this ControllerBase controller)
            => new ConflictResult();
 
        public static ConflictObjectResult Conflict(this ControllerBase controller, object value)
            => new ConflictObjectResult(value);

        // .Net Core MVC doesn't have any methods for 5xx returns
        // so we implement them...

        /* ==== Conflict() ==== */
        // Http Status Code 500 - Internal Server Error
        public static InternalServerErrorResult InternalServerError(this ControllerBase controller)
            => new InternalServerErrorResult();

        public static InternalServerErrorObjectResult InternalServerError(this ControllerBase controller, object value)
            => new InternalServerErrorObjectResult(value);
    }

    /* ==== ConflictResult ==== */
    /// <summary>
    /// Represents an <see cref="StatusCodeResult"/> that when 
    /// executed will produce a Conflict (409) response.
    /// </summary>
    public class ConflictResult : StatusCodeResult
    {
        /// <summary>
        /// Creates a new <see cref="ConflictResult"/> instance.
        /// </summary>
        public ConflictResult() : base(StatusCodes.Status409Conflict)
        {
        }
    }

    /* ==== ConflictObjectResult ==== */
    /// <summary>
    /// An <see cref="ObjectResult"/> that when executed will produce a Conflict (409) response.
    /// </summary>
    public class ConflictObjectResult : ObjectResult
    {
        /// <summary>
        /// Creates a new <see cref="ConflictObjectResult"/> instance.
        /// </summary>
        /// <param name="value">The value to format in the entity body.</param>
        public ConflictObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }

    /* ==== InternalServerErrorResult ==== */
    /// <summary>
    /// Represents an <see cref="StatusCodeResult"/> that when 
    /// executed will produce a Internal Server Error (500) response.
    /// </summary>
    public class InternalServerErrorResult : StatusCodeResult
    {
        /// <summary>
        /// Creates a new <see cref="ConflictResult"/> instance.
        /// </summary>
        public InternalServerErrorResult() : base(StatusCodes.Status500InternalServerError)
        {
        }
    }

    /* ==== InternalServerErrorObjectResult ==== */
    /// <summary>
    /// An <see cref="ObjectResult"/> that when executed will produce a Internal Server Error (500) response.
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Creates a new <see cref="ConflictObjectResult"/> instance.
        /// </summary>
        /// <param name="value">The value to format in the entity body.</param>
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
