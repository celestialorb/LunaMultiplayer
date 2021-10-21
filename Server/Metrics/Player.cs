using Server.Log;

namespace Server.Metrics {
  public static class Player {
    public static readonly Prometheus.Gauge Online = Prometheus.Metrics.CreateGauge(
      "lmp_player_online",
      "The online status of the player in the scenario.",
      new Prometheus.GaugeConfiguration{LabelNames = new[] {"name"}}
    );

    public static void Init() {}
  }
}
