using System;
using System.Threading.Tasks;
using System.IO;
using uhttpsharp;

using Server.Metrics;

namespace Server.Web.Handlers {
  public class MetricsHandler : IHttpRequestHandler {

    public MetricsHandler() {
      Prometheus.Metrics.SuppressDefaultMetrics();

      // Initialize the various metric subsystems.
      Metrics.Contracts.Init();
      // Metrics.Kerbals.Init();
      Metrics.Player.Init();
      Metrics.Scenario.Init();
      Metrics.Subspace.Init();
      Metrics.Vessel.Init();
    }

    public Task Handle(IHttpContext context, Func<Task> next) {
      // Write out the Prometheus metrics to the response.
      var stream = new MemoryStream();
      Prometheus.Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);
      context.Response = new HttpResponse("text/plain; version=0.0.4", stream, false);
      return Task.Factory.GetCompleted();
    }
  }
}
