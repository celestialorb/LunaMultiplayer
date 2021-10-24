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
                "type",
                "target_body"
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

        public static readonly Prometheus.Gauge CurrentStage = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_current_stage",
            "The current stage of the vessel.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"guid"}}
        );

        public static void RemoveVessel(Guid id) {
            foreach(var labels in Info.GetAllLabelValues()) {
                if(labels[0] != id.ToString()) { continue; }

                Info.RemoveLabelled(labels);
                break;
            }

            Epoch.RemoveLabelled(id.ToString());
            DistanceTraveled.RemoveLabelled(id.ToString());
            CurrentStage.RemoveLabelled(id.ToString());
            VesselPosition.RemoveVessel(id);
            VesselOrbit.RemoveVessel(id);
            VesselOrientation.RemoveVessel(id);
            VesselPartResource.RemoveVessel(id);
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

    public class VesselPartResource {
        public static readonly Prometheus.Gauge Amount = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_part_resource_amount",
            "The current amount of a resource in the vessel part.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {
                "vessel",
                "part",
                "resource"
            }}
        );

        public static readonly Prometheus.Gauge Capacity = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_part_resource_capacity",
            "The resource capacity for a vessel part.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {
                "vessel",
                "part",
                "resource"
            }}
        );

        public static readonly Prometheus.Gauge FlowState = Prometheus.Metrics.CreateGauge(
            "lmp_vessel_part_resource_flow_state",
            "The current state of the resource flow for a vessel part.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {
                "vessel",
                "part",
                "resource"
            }}
        );

        public static void Update() {
            foreach(var vessel in System.VesselStoreSystem.CurrentVessels) {
                UpdateVessel(vessel.Key);
            }
        }

        public static void UpdateVessel(Guid id) {
            RemoveVessel(id);

            System.VesselStoreSystem.CurrentVessels.TryGetValue(id, out var vessel);

            foreach(var part in vessel.Parts.GetAll()) {
                foreach(var resource in part.Value.Resources.GetAll()) {
                    var labels = new[] {
                        id.ToString(),
                        part.Key.ToString(),
                        resource.Key
                    };

                    Amount.WithLabels(labels).Set(double.Parse(resource.Value.GetValue("amount").Value));
                    Capacity.WithLabels(labels).Set(double.Parse(resource.Value.GetValue("maxAmount").Value));
                    FlowState.WithLabels(labels).Set(bool.Parse(resource.Value.GetValue("flowState").Value) ? 1 : 0);
                }
            }
        }

        public static void RemoveVessel(Guid id) {
            foreach(var labels in FlowState.GetAllLabelValues()) {
                if(labels[0] != id.ToString()) { continue; }

                Amount.RemoveLabelled(labels);
                Capacity.RemoveLabelled(labels);
                FlowState.RemoveLabelled(labels);
            }
        }
    }
}
