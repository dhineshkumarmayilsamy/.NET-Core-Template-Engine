﻿using Microsoft.AspNetCore.Builder;

namespace RepositoryPatternTemplate.Middleware
{
    public static class LoggerMiddleware
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerProvider>();
        }
    }
}
