namespace Server.Metrics {
    public class Technology {
        public static readonly Prometheus.Gauge Cost = Prometheus.Metrics.CreateGauge(
            "lmp_technology_researched_cost",
            "The science cost of researched technology for the space center.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"id"}}
        );

        public static readonly Prometheus.Gauge PartCount = Prometheus.Metrics.CreateGauge(
            "lmp_technology_part_count",
            "The number of parts a technology unlocks for the space center.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"id"}}
        );

        public static void Update() {
            if(!System.ScenarioStoreSystem.CurrentScenarios.TryGetValue("ResearchAndDevelopment", out var scenario)) { return; }

            foreach(var technology in scenario.GetNodes("Tech")) {
                var name = technology.Value.GetValue("id").Value;

                Cost.WithLabels(name).Set(double.Parse(technology.Value.GetValue("cost").Value));
                PartCount.WithLabels(name).Set(technology.Value.GetValues("part").Count);
            }
        }
    }
}
