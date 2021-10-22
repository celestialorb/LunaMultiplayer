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

    public static void Init() {
      if(System.ScenarioStoreSystem.CurrentScenarios.TryGetValue("Funding", out var funding)) {
        double.TryParse(funding.GetValue("funds").Value, out double result);
        Metrics.Scenario.Funds.Set(result);
      }

      if(System.ScenarioStoreSystem.CurrentScenarios.TryGetValue("Reputation", out var reputation)) {
        double.TryParse(reputation.GetValue("rep").Value, out double result);
        Metrics.Scenario.Reputation.Set(result);
      }

      if(System.ScenarioStoreSystem.CurrentScenarios.TryGetValue("ResearchAndDevelopment", out var rnd)) {
        double.TryParse(rnd.GetValue("sci").Value, out double result);
        Metrics.Scenario.Science.Set(result);
      }
    }
  }
}
