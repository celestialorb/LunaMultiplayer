using System;

namespace Server.Metrics {
    public class Vessel {
        public static readonly Prometheus.Counter Info = Prometheus.Metrics.CreateCounter(
            "lmp_vessel_info",
            "Information about a vessel.",
            new Prometheus.CounterConfiguration{LabelNames = new[] {
                "guid",
                "name",
                "situation",
                "type"
            }}
        );

        public static readonly Prometheus.Gauge DistanceTraveled = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_distance_traveled_meters",
            "The total distance traveled by the vessel.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Counter StagingEvent = Prometheus.Metrics.CreateCounter(
            "lmp_vessel_staging_event_timestamp",
            "The timestamp in seconds from the UNIX epoch of a staging event for a vessel.",
            new Prometheus.CounterConfiguration{LabelNames = new[] {"guid", "stage"}}
        );

        public static void RemoveVessel(Guid id) {
            foreach(var labels in Info.GetAllLabelValues()) {
                if(labels[0] != id.ToString()) { continue; }

                Info.RemoveLabelled(labels);
                break;
            }

            DistanceTraveled.RemoveLabelled(id.ToString());
            VesselPosition.RemoveVessel(id);
            VesselOrbit.RemoveVessel(id);
        }
    }

    public class VesselPosition {
        public static readonly Prometheus.Gauge Latitude = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_position_latitude_degrees",
            "The current latitude of the vessel over its reference body.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge Longitude = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_position_longitude_degrees",
            "The current longitude of the vessel over its reference body.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge Altitude = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_position_altitude_meters",
            "The current mean sea-level altitude of the vessel over its reference body.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge Height = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_position_height_meters",
            "The current absolute altitude of the vessel over the reference body terrain.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static void RemoveVessel(Guid id) {
            Latitude.RemoveLabelled(id.ToString());
            Longitude.RemoveLabelled(id.ToString());
            Altitude.RemoveLabelled(id.ToString());
            Height.RemoveLabelled(id.ToString());
        }
    }

    public class VesselOrbit {
        public static readonly Prometheus.Gauge SemimajorAxis = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orbit_semimajor_axis_meters",
            "The length of the semimajor axis of the vessel's orbit.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge Inclination = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orbit_inclination_degrees",
            "The inclination of the vessel's orbit.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge Eccentricity = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orbit_eccentricity",
            "The eccentricity of the vessel's orbit.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static void RemoveVessel(Guid id) {
            SemimajorAxis.RemoveLabelled(id.ToString());
            Inclination.RemoveLabelled(id.ToString());
            Eccentricity.RemoveLabelled(id.ToString());
        }
    }
}
