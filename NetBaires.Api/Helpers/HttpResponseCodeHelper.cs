using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetBaires.Api.Helpers
{
    public static class HttpResponseCodeHelper
    {
        public static ObjectResult NotFound(string message = "") =>
                 new ObjectResult(message) { StatusCode = 404 };

        public static ObjectResult Error(string message = "") =>
                 new ObjectResult(message) { StatusCode = 400 };

        public static ObjectResult Conflict(string message = "") =>
                 new ObjectResult(message) { StatusCode = 409 };

        public static StatusCodeResult NotContent() =>
                 new StatusCodeResult(204);

        public static ObjectResult Ok<TReturn>(TReturn message) =>
             message != null ?
            new ObjectResult(message) { StatusCode = 200 }
            : new ObjectResult(string.Empty) { StatusCode = 204 };
    }
}
