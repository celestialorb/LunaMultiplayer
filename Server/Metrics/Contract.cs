using System.Linq;

namespace Server.Metrics {
    public class Contract {
        public static readonly Prometheus.Counter Info = Prometheus.Metrics.CreateCounter(
            "lmp_contract_info",
            "Information about a contract.",
            new Prometheus.CounterConfiguration{LabelNames = new[] {
                "guid",
                "type",
                "state",
                "viewed",
                "agent",
                "deadline_type",
                "expiration_type",
                "target_body"
            }}
        );

        public static void Update() {
            System.ScenarioStoreSystem.CurrentScenarios.TryGetValue("ContractSystem", out var scenario);

            var scenariosParentNode = scenario.GetNode("CONTRACTS")?.Value;
            if (scenariosParentNode == null) return;

            var existingContracts = scenariosParentNode.GetNodes("CONTRACT").Select(c => c.Value).ToArray();
            if(!existingContracts.Any()) { return; }

            foreach(var contract in existingContracts) {
                // First clear any contract metric we already have for this contract.
                foreach(var labels in Info.GetAllLabelValues()) {
                    if(labels[0] == contract.GetValue("guid").Value) {
                        Info.RemoveLabelled(labels);
                    }
                }

                var target_body_name = "None";
                if(int.TryParse(contract?.GetValue("targetBody")?.Value ?? "-1", out var target_body)) {
                    target_body_name = Utilities.GetCelestialBodyName(target_body);
                }

                // Readd the metric.
                Info.WithLabels(
                    contract.GetValue("guid").Value,
                    contract.GetValue("type").Value,
                    contract.GetValue("state").Value,
                    contract.GetValue("viewed").Value,
                    contract.GetValue("agent").Value,
                    contract.GetValue("deadlineType").Value,
                    contract.GetValue("expiryType").Value,
                    target_body_name
                ).IncTo(1);
            }
        }
    }
}
