using Server.Log;
using Server.System;

namespace Server.Metrics {
  public static class Contracts {
    public static readonly Prometheus.Gauge Count = Prometheus.Metrics.CreateGauge(
        "lmp_contract_count",
        "The number of contracts in the scenario.",
        new Prometheus.GaugeConfiguration{LabelNames = new[] {
            "guid",
            "type",
            "state",
            "viewed",
            "agent",
            "deadline_type",
            "expiration_type",
            "reference_body"
        }}
    );

    // TODO: metric(s) on contract rewards?

    public static void Init() {
      if (!ScenarioStoreSystem.CurrentScenarios.TryGetValue("ContractSystem", out var scenario)) return;

      var scenariosParentNode = scenario.GetNode("CONTRACTS")?.Value;
      if (scenariosParentNode == null) return;

      var existingContracts = scenariosParentNode.GetNodes("CONTRACT").ToArray();
      foreach(var contract in existingContracts) {
          Metrics.Contracts.Count.WithLabels(
            contract.Value.GetValue("guid").Value,
            contract.Value.GetValue("type").Value,
            contract.Value.GetValue("state").Value,
            contract.Value.GetValue("viewed").Value,
            contract.Value.GetValue("agent").Value,
            contract.Value.GetValue("deadlineType").Value,
            contract.Value.GetValue("expiryType").Value,
            contract.Value.GetValue("targetBody")?.Value ?? "None"
          ).Inc();
      }
    }
  }
}
