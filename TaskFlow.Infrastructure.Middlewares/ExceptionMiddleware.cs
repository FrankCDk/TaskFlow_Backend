using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TaskFlow.Application.Interfaces;
using TaskFlow.Shared.Dtos;

namespace TaskFlow.Infrastructure.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {

        //Agregar los logs
        private readonly ILogsRepository _logsRepository;
        private readonly IConfiguration _configuration;
        public ExceptionMiddleware(ILogsRepository logsRepository, IConfiguration configuration)
        {
            _logsRepository = logsRepository;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await ErrorHandler(context, ex);
            }
        }

        public async Task ErrorHandler(HttpContext context, Exception ex)
        {

            ApiErrorResponse apiErrorResponse = new ApiErrorResponse();

            switch (ex)
            {
                case ValidationException validation:
                    apiErrorResponse.Status = System.Net.HttpStatusCode.BadRequest;
                    apiErrorResponse.Title = "Validation error occurred.";
                    apiErrorResponse.Errors = validation.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());
                    break;

                case ArgumentException:
                    apiErrorResponse.Status = System.Net.HttpStatusCode.BadRequest;
                    apiErrorResponse.Title = "Invalid argument provided.";
                    apiErrorResponse.Detail = ex.Message;
                    break;
                default:
                    apiErrorResponse.Status = System.Net.HttpStatusCode.InternalServerError;
                    apiErrorResponse.Title = "Error interno del servidor.";
                    apiErrorResponse.Detail = ex.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true //Indica que el json debe ser formateado de manera legible
            };

            var jsonResponse = JsonSerializer.Serialize(apiErrorResponse, jsonOptions);
            await context.Response.WriteAsync(jsonResponse);

        }

    }
}
