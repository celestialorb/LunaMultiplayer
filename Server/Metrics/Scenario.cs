using Server.Log;

namespace Server.Metrics {
  public static class Scenario {
    public static readonly Prometheus.Gauge Funds = Prometheus.Metrics.CreateGauge(
      "lmp_scenario_funds",
      "The amount of funds for the scenario."
    );

    public static readonly Prometheus.Gauge Reputation = Prometheus.Metrics.CreateGauge(
      "lmp_scenario_reputation",
      "The amount of reputation for the scenario."
    );

    public static readonly Prometheus.Gauge Science = Prometheus.Metrics.CreateGauge(
      "lmp_scenario_science",
      "The amount of science for the scenario."
    );

    public static void Init() {}
  }
}
