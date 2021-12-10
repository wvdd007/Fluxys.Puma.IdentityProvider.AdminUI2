using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Mail;

namespace Fluxys.Puma.IdentityProvider.STS.Identity.Custom
{
    // from https://inthetechpit.com/2020/01/09/global-exception-handling-and-logging-in-aspnet-core-webapi/
    public static class FluxysExceptionFactory
        {
            public static void ConfigureExceptionHandler(this IApplicationBuilder app, int StatusCode = 0, string message = "")
            {
                app.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        //if (contextFeature != null)
                        {
                            Console.WriteLine($"*****Something went wrong: {contextFeature.Error}");

                            await context.Response.WriteAsync($"StatusCode: {context.Response.StatusCode}");
                        }
                    });
                });
            }
        }
}
