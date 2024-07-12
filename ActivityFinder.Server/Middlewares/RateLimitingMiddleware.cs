using System.Collections.Concurrent;

namespace ActivityFinder.Server.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, RateLimitInfo> _clients = new();
        private readonly int _requestLimit = 300;
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/hub"))
            {
                await _next(context);
                return;
            }

            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            if (clientIp == null)
            {
                await _next(context);
                return;
            }

            await _semaphore.WaitAsync();
            try
            {
                var now = DateTime.UtcNow;
                var client = _clients.GetOrAdd(clientIp, _ => new RateLimitInfo(now, 0));

                if (client.ResetTime <= now)
                {
                    client.Requests = 0;
                    client.ResetTime = now.Add(_timeWindow);
                }

                if (client.Requests >= _requestLimit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"] = ((int)(client.ResetTime - now).TotalSeconds).ToString();
                    await context.Response.WriteAsync("Too many requests. Please wait before trying again.");
                    return;
                }

                client.Requests++;
            }
            finally
            {
                _semaphore.Release();
            }

            await _next(context);
        }

        private class RateLimitInfo
        {
            public DateTime ResetTime { get; set; }
            public int Requests { get; set; }

            public RateLimitInfo(DateTime resetTime, int requests)
            {
                ResetTime = resetTime;
                Requests = requests;
            }
        }
    }
}
