using System;

namespace BookService.Models
{
    public static class WebAPIErrorConvenience
    {
        /// <summary>
        /// Returns a WebAPIError object
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WebAPIError WebError(string message, string code = null)
        {
            return new WebAPIError(message, code);
        }
    }

    /// <summary>
    /// An object to encapsulate a web api return that did not succeed.
    /// </summary>
    public class WebAPIError
    {
        /// <summary>
        /// Create a new WebAPIError object.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public WebAPIError(string message, string code = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                // throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(message));
                throw new ArgumentException("ArgumentCannotBeNullOrEmpty", nameof(message));
            }
            ErrorMessage = message;
            ErrorCode = code;
        }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }
    }
}
