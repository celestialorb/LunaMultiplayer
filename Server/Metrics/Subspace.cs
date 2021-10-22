using Server.Context;

namespace Server.Metrics {
  public class Subspace {
    public static Prometheus.Gauge Delta = Prometheus.Metrics.CreateGauge(
      "lmp_subspace_delta_seconds",
      "The time difference of the subspace from the server time.",
      new Prometheus.GaugeConfiguration{LabelNames = new[] {"key", "creator"}}
    );

    public static void Init() {
      foreach(var subspace in WarpContext.Subspaces) {
        Delta.WithLabels(subspace.Key.ToString(), subspace.Value.Creator).Set(subspace.Value.Time);
      }
    }
  }
}
