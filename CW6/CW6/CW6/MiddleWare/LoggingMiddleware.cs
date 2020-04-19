using CW6.Models;
using CW6.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW6.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IOService service)
        {
            context.Request.EnableBuffering();
            if (context.Request != null)
            {
                Log log = new Log();
                log.Path = context.Request.Path;// api/studenst
                log.Method = context.Request.Method;//get, post etc.
                log.QueryString = context.Request.QueryString.ToString();
                //log.Body= "";

                using(var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    log.Body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; //wracamy na początek strumienia bo pozostałe middleware tez chca czytac od początku
                }
                // zapis do pliku
                string logs = JsonConvert.SerializeObject(log);
                service.Write(logs);



            }

           if(_next!=null) await _next(context);
        }
    }
}
