using Prometheus;

namespace Server.Metrics {
  public class Vessel {
    private static readonly string[] LabelNames = new[] {"id", "name", "type"};

    public static readonly Prometheus.Counter StagingEvent = Prometheus.Metrics.CreateCounter(
      "lmp_vessel_staging_event",
      "The real-world timestamp of the vessel's staging event.",
      new Prometheus.CounterConfiguration{LabelNames = new[] {"id", "name", "stage", "type"}}
    );

    public static readonly Prometheus.Gauge Epoch = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_epoch",
      "The epoch of the vessel.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge DistanceTraveled = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_distance_traveled_meters",
      "The total distance traveled by the vessel.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge Altitude = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_altitude_meters",
      "The altitude of the vessel over its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge Latitude = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_latitude_degrees",
      "The latitude of the vessel over its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge Longitude = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_longitude_degrees",
      "The longitude of the vessel over its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge SemimajorAxis = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_semimajor_axis_meters",
      "The semimajor axis of the vessel's orbit around its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge Eccentricity = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_eccentricity",
      "The eccentricity of the vessel's orbit around its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge Inclination = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_inclination_degrees",
      "The inclination of the vessel's orbit over its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge ArgumentOfPeriapsis = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_argument_of_periapsis_degrees",
      "The argument of periapsis of the vessel's orbit over its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge LongitudeOfAscendingNode = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_longitude_of_ascending_node_degrees",
      "The longitude of ascending node of the vessel's orbit over its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static readonly Prometheus.Gauge MeanAnomaly = Prometheus.Metrics.CreateGauge(
      "lmp_vessel_mean_anomaly_radians",
      "The mean anomaly of the vessel's orbit over its reference body.",
      new Prometheus.GaugeConfiguration{LabelNames = LabelNames}
    );

    public static void Init() {}
  }
}
