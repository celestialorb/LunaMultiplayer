namespace Server.Metrics {
    public class Program {
        public static readonly Prometheus.Gauge Funds = Prometheus.Metrics.CreateGauge(
            "lmp_program_funds",
            "The current funds of the space program."
        );

        public static readonly Prometheus.Gauge Reputation = Prometheus.Metrics.CreateGauge(
            "lmp_program_reputation",
            "The current reputation of the space program."
        );

        public static readonly Prometheus.Gauge Science = Prometheus.Metrics.CreateGauge(
            "lmp_program_science",
            "The current science of the space program."
        );
    }
}
