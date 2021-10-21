using Prometheus;
using Server.Web.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using uhttpsharp;
using uhttpsharp.Headers;
using uhttpsharp.Handlers;

namespace Server.Web.Handlers {
  public class MetricsHandler : IHttpRequestHandler {
    private readonly ServerInformation ServerInformation = null;

    private Gauge PlayerCount;
    private Gauge SubspaceCount;

    private Counter VesselEpoch;
    private Counter VesselDistanceTravelled;

    private Gauge VesselAltitude;
    private Gauge VesselLatitude;
    private Gauge VesselLongitude;
    private Gauge VesselSemimajorAxis;
    private Gauge VesselEccentricity;
    private Gauge VesselInclination;

    private Gauge VesselArgumentOfPeriapsis;
    private Gauge VesselLongitudeOfAscendingNode;
    private Gauge VesselMeanAnomaly;

    public MetricsHandler(ServerInformation serverInformation) {
      ServerInformation = serverInformation;

      CounterConfiguration VesselCounterConfiguration = new CounterConfiguration{
        LabelNames = new[] {"id", "name", "type"}
      };
      GaugeConfiguration VesselGaugeConfiguration = new GaugeConfiguration{
        LabelNames = new[] {"id", "name", "type"}
      };

      // General metrics.
      PlayerCount = Metrics.CreateGauge("lmp_players_online_total", "The total number of players currently online.");
      SubspaceCount = Metrics.CreateGauge("lmp_subspace_total", "The total number of individual subspaces.");

      // Vessel general metrics.
      VesselDistanceTravelled = Metrics.CreateCounter("lmp_vessel_distance_travelled_meters", "The total distance travelled by the vessel.", VesselCounterConfiguration);
      VesselEpoch = Metrics.CreateCounter("lmp_vessel_epoch_seconds", "The epoch of the vessel.", VesselCounterConfiguration);

      // Traditional coordinate metrics.
      VesselAltitude = Metrics.CreateGauge("lmp_vessel_altitude_meters", "The altitude of the vessel.", VesselGaugeConfiguration);
      VesselLatitude = Metrics.CreateGauge("lmp_vessel_latitude_degrees", "The latitude of the vessel.", VesselGaugeConfiguration);
      VesselLongitude = Metrics.CreateGauge("lmp_vessel_longitude_degrees", "The longitude of the vessel.", VesselGaugeConfiguration);

      // Orbital metrics.
      VesselSemimajorAxis = Metrics.CreateGauge("lmp_vessel_semimajor_axis_meters", "The semimajor axis of the vessel's orbit.", VesselGaugeConfiguration);
      VesselEccentricity = Metrics.CreateGauge("lmp_vessel_eccentricity", "The eccentricity of the vessel's orbit.", VesselGaugeConfiguration);
      VesselInclination = Metrics.CreateGauge("lmp_vessel_inclination_degrees", "The inclination of the vessel's orbit.", VesselGaugeConfiguration);

      // Orbital positional metrics.
      VesselArgumentOfPeriapsis = Metrics.CreateGauge("lmp_vessel_argument_of_periapsis_degrees", "The vessel's argument of periapsis in its current orbit.", VesselGaugeConfiguration);
      VesselLongitudeOfAscendingNode = Metrics.CreateGauge("lmp_vessel_longitude_of_ascending_node_degrees", "The vessel's longitude of ascending node in its current orbit.", VesselGaugeConfiguration);
      VesselMeanAnomaly = Metrics.CreateGauge("lmp_vessel_mean_anomaly_radians", "The vessel's mean anomaly.", VesselGaugeConfiguration);
    }

    public Task Handle(IHttpContext context, Func<Task> next) {
      // Set general metrics.
      PlayerCount.Set(ServerInformation.CurrentState.CurrentPlayers.Count);
      SubspaceCount.Set(ServerInformation.CurrentState.Subspaces.Count);

      // Set vessel metrics.
      foreach(var vessel in ServerInformation.CurrentState.CurrentVessels) {
        var identifier = vessel.Id.ToString();

        // Set vessel general metrics.
        VesselEpoch.WithLabels(identifier, vessel.Name, vessel.Type).IncTo(vessel.Epoch);
        VesselDistanceTravelled.WithLabels(identifier, vessel.Name, vessel.Type).IncTo(vessel.DistanceTravelled);

        // Set vessel traditional coordinate metrics.
        VesselAltitude.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Alt);
        VesselLatitude.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Lat);
        VesselLongitude.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Lon);

        // Set vessel orbital metrics.
        VesselSemimajorAxis.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.SemiMajorAxis);
        VesselEccentricity.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Eccentricity);
        VesselInclination.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Inclination);

        // Set vessel orbital positional metrics.
        VesselArgumentOfPeriapsis.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.ArgumentOfPeriapsis);
        VesselLongitudeOfAscendingNode.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.LongitudeOfAscendingNode);
        VesselMeanAnomaly.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.MeanAnomaly);
      }

      var stream = new MemoryStream();
      Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);
      context.Response = new HttpResponse("text/plain; version=0.0.4", stream, false);
      return Task.Factory.GetCompleted();
    }
  }
}
