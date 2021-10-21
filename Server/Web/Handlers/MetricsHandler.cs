using Prometheus;
using Server.Web.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using uhttpsharp;
using uhttpsharp.Headers;
using uhttpsharp.Handlers;

namespace Server.Web.Handlers
{
    public class MetricsHandler : IHttpRequestHandler
    {
        private static readonly Counter TickTock =
        Metrics.CreateCounter("lmp_ticks_total", "Just keeps on ticking");

        public Task Handle(IHttpContext context, Func<Task> next) {
          TickTock.Inc();

          var stream = new MemoryStream();

          Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);

          context.Response = new HttpResponse("text/plain; version=0.0.4", stream, false);

          return Task.Factory.GetCompleted();
        }
    }
}
