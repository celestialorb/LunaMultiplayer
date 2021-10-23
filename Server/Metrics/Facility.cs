namespace Server.Metrics {
    public class Facility {
        public static readonly Prometheus.Gauge Level = Prometheus.Metrics.CreateGauge(
            "lmp_facility_level",
            "The level of the space center facility.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"name"}}
        );

        public static void Update() {
            if(!System.ScenarioStoreSystem.CurrentScenarios.TryGetValue("ScenarioUpgradeableFacilities", out var scenario)) { return; }

            foreach(var facility in scenario.GetAllNodes()) {
                Level.WithLabels(facility.Name).Set(double.Parse(facility.GetValue("lvl").Value));
            }
        }
    }
}
