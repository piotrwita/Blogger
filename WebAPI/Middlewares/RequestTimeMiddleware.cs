using System.Diagnostics;
using WebAPI.Wrappers;

namespace WebAPI.Middlewares
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private const int limitSeconds = 3;
        private const string path = @"C:\BloggerLogs.txt";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            await next.Invoke(context);
            //Thread.Sleep(4000);

            stopWatch.Stop();

            var elapsedSeconds = stopWatch.Elapsed.Seconds;

            if (elapsedSeconds > limitSeconds)
            {
                string[] log = 
                {
                    $"Date: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}",
                    $"Message: Request took more than {limitSeconds} seconds",
                    $"HttpMethod: {context.Request.Method}",
                    $"Path: {context.Request.Path}",
                    String.Empty
                };
                
                await File.AppendAllLinesAsync(path, log);
            }
        }
    }
}
