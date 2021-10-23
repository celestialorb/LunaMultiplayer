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

        public static readonly Prometheus.Gauge Epoch = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_epoch_seconds",
            "The vessel's epoch.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
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

            Epoch.RemoveLabelled(id.ToString());
            DistanceTraveled.RemoveLabelled(id.ToString());
            VesselPosition.RemoveVessel(id);
            VesselOrbit.RemoveVessel(id);
            VesselOrientation.RemoveVessel(id);
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

        public static readonly Prometheus.Gauge LongitudeOfAscendingNode = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orbit_longitude_of_ascending_node_degrees",
            "The longitude of the ascending node of the vessel's orbit.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge ArgumentOfPeriapsis = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orbit_argument_of_periapsis_degrees",
            "The argument of periapsis of the vessel's orbit.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge MeanAnomaly = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orbit_mean_anomaly_radians",
            "The mean anomaly of the vessel's orbit.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static void RemoveVessel(Guid id) {
            SemimajorAxis.RemoveLabelled(id.ToString());
            Inclination.RemoveLabelled(id.ToString());
            Eccentricity.RemoveLabelled(id.ToString());
            LongitudeOfAscendingNode.RemoveLabelled(id.ToString());
            ArgumentOfPeriapsis.RemoveLabelled(id.ToString());
            MeanAnomaly.RemoveLabelled(id.ToString());
        }
    }

    public class VesselOrientation {
        public static readonly Prometheus.Gauge NormalX = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orientation_normal_x",
            "The x-component of the vessel's normal vector.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge NormalY = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orientation_normal_y",
            "The y-component of the vessel's normal vector.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge NormalZ = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orientation_normal_z",
            "The z-component of the vessel's normal vector.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge SurfaceRelativeW = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orientation_surface_w",
            "The w-component of the vessel's surface-relative quaternion orientation.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge SurfaceRelativeX = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orientation_surface_x",
            "The x-component of the vessel's surface-relative quaternion orientation.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge SurfaceRelativeY = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orientation_surface_y",
            "The y-component of the vessel's surface-relative quaternion orientation.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static readonly Prometheus.Gauge SurfaceRelativeZ = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_orientation_surface_z",
            "The z-component of the vessel's surface-relative quaternion orientation.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static void RemoveVessel(Guid id) {
            NormalX.RemoveLabelled(id.ToString());
            NormalY.RemoveLabelled(id.ToString());
            NormalZ.RemoveLabelled(id.ToString());

            SurfaceRelativeW.RemoveLabelled(id.ToString());
            SurfaceRelativeX.RemoveLabelled(id.ToString());
            SurfaceRelativeY.RemoveLabelled(id.ToString());
            SurfaceRelativeZ.RemoveLabelled(id.ToString());
        }
    }
}
