using Server.Log;
using Server.System;

namespace Server.Metrics {
  public static class Kerbals {
    public static readonly Prometheus.Gauge Info = Prometheus.Metrics.CreateGauge(
        "lmp_kerbal_info",
        "Information about a Kerbal.",
        new Prometheus.GaugeConfiguration{LabelNames = new[] {
            "name",
            // "gender",
            // "type",
            // "trait",
            // "brave",
            // "dumb",
            // "badS",
            // "veteran",
            // "state"
        }}
    );

    public static void Init() {
        KerbalSystem.InitializeKerbalMetrics();
    }
  }
}
