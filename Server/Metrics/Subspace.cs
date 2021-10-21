using Server.Context;

namespace Server.Metrics {
  public class Subspace {
    public static Prometheus.Gauge Epoch = Prometheus.Metrics.CreateGauge(
      "lmp_subspace_epoch",
      "The epoch of the subspace.",
      new Prometheus.GaugeConfiguration{LabelNames = new[] {"creator"}}
    );

    public static void Init() {}
  }
}
