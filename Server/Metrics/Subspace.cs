namespace Server.Metrics {
    public class Subspace {
        public static readonly Prometheus.Gauge TimeDelta = Prometheus.Metrics.CreateGauge(
            "lmp_subspace_time_delta_seconds",
            "The time difference in seconds of the subspace from the server.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"key", "creator"}}
        );

        public static void RemoveSubspace(int key) {
            foreach(var labels in TimeDelta.GetAllLabelValues()) {
                // If this subspace isn't the one, skip it and continue on.
                if(labels[0] != key.ToString()) { continue; }

                // Otherwise, remove it. This should be the only one so we can immediately return.
                TimeDelta.RemoveLabelled(labels);
                return;
            }
        }
    }
}
