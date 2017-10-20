using System;
using System.Reflection;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Models
{
    /// <summary>
    /// An object to encapsulate a web api return that did not succeed.
    /// </summary>
    public class WebAPIError
    {
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
