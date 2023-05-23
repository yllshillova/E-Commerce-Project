using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware
{
    // kjo eshte klasa e cila eshte zevendsuar me default developers error page e cila shfaqet kur ndodh nddonje gabim
    // kta e kem bo ne menyr qe me ju shfaq klientav ni error sa ma user friendly jo me detaje komplekse ashtu siq shfaqet by default
    public class ExceptionMiddleware
    {
        // RequestDelegate is going to allow us to execute that next method and pass that on the request to the next piece of middleware,
        // ILogger its gonna be used to logg any exception we get and the logger takes the type as type of the class that we are using
        // IHostEnvironment its gonna be used so that we can check if we're running on development mode or production mode
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // pjesa e cila ekzekutohet kur thirret kjo metod InvokeAsync
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                // kjo ContentType tregon tipin e pergjigjjes qe do te kthehet nga serveri
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                // we create a new ProblemDetails so it retains the same format as the rest of our errors in our application(the ones that are in buggyController).
                var response = new ProblemDetails
                {
                    Status = 500,
                    Detail = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null,
                    Title = ex.Message
                };
                // we are creating some options for json serialize because when we return this json file outside of an API controller it loses some defaults
                var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                                                //response eshte objekti qe kthehet ne json, kurse options ndihmon ne konvertimin e json gjat formatimit 
                var json = JsonSerializer.Serialize(response, options);
                                        // kjo osht qka ko me kthy kjo metod.
                await context.Response.WriteAsync(json);
            }
        }


    }
}