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
      VesselDistanceTravelled = Metrics.CreateCounter(
        "lmp_vessel_distance_travelled_meters",
        "The total distance travelled by the vessel.",
        new CounterConfiguration{LabelNames = new[] {"id", "name", "type"}}
      );
      VesselEpoch = Metrics.CreateCounter(
        "lmp_vessel_epoch_seconds",
        "The epoch of the vessel.",
        new CounterConfiguration{LabelNames = new[] {"id", "name", "type"}}
      );

      // Traditional coordinate metrics.
      VesselAltitude = Metrics.CreateGauge(
        "lmp_vessel_altitude_meters",
        "The altitude of the vessel.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );
      VesselLatitude = Metrics.CreateGauge(
        "lmp_vessel_latitude_degrees",
        "The latitude of the vessel.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );
      VesselLongitude = Metrics.CreateGauge(
        "lmp_vessel_longitude_degrees",
        "The longitude of the vessel.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );

      // Orbital metrics.
      VesselSemimajorAxis = Metrics.CreateGauge(
        "lmp_vessel_semimajor_axis_meters",
        "The semimajor axis of the vessel's orbit.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );
      VesselEccentricity = Metrics.CreateGauge(
        "lmp_vessel_eccentricity",
        "The eccentricity of the vessel's orbit.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );
      VesselInclination = Metrics.CreateGauge(
        "lmp_vessel_inclination_degrees",
        "The inclination of the vessel's orbit.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );

      // Orbital positional metrics.
      VesselArgumentOfPeriapsis = Metrics.CreateGauge(
        "lmp_vessel_argument_of_periapsis_degrees",
        "The vessel's argument of periapsis in its current orbit.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );
      VesselLongitudeOfAscendingNode = Metrics.CreateGauge(
        "lmp_vessel_longitude_of_ascending_node_degrees",
        "The vessel's longitude of ascending node in its current orbit.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );
      VesselMeanAnomaly = Metrics.CreateGauge(
        "lmp_vessel_mean_anomaly_radians",
        "The vessel's mean anomaly.",
        new GaugeConfiguration{LabelNames = new[] {"body", "id", "name", "type"}}
      );
    }

    public Task Handle(IHttpContext context, Func<Task> next) {
      // TODO: time to get scrape duration.

      // Set general metrics.
      PlayerCount.Set(ServerInformation.CurrentState.CurrentPlayers.Count);
      SubspaceCount.Set(ServerInformation.CurrentState.Subspaces.Count);

      // Set vessel metrics.
      foreach(var vessel in ServerInformation.CurrentState.CurrentVessels) {
        var identifier = vessel.Id.ToString();
        var body = BodyMapper.Mapping[vessel.ReferenceBody];

        // Set vessel general metrics.
        VesselEpoch.WithLabels(identifier, vessel.Name, vessel.Type).IncTo(vessel.Epoch);
        VesselDistanceTravelled.WithLabels(identifier, vessel.Name, vessel.Type).IncTo(vessel.DistanceTravelled);

        // Set vessel traditional coordinate metrics.
        VesselAltitude.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.Alt);
        VesselLatitude.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.Lat);
        VesselLongitude.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.Lon);

        // Set vessel orbital metrics.
        VesselSemimajorAxis.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.SemiMajorAxis);
        VesselEccentricity.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.Eccentricity);
        VesselInclination.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.Inclination);

        // Set vessel orbital positional metrics.
        VesselArgumentOfPeriapsis.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.ArgumentOfPeriapsis);
        VesselLongitudeOfAscendingNode.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.LongitudeOfAscendingNode);
        VesselMeanAnomaly.WithLabels(body, identifier, vessel.Name, vessel.Type).Set(vessel.MeanAnomaly);
      }

      var stream = new MemoryStream();
      Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);
      context.Response = new HttpResponse("text/plain; version=0.0.4", stream, false);
      return Task.Factory.GetCompleted();
    }
  }
}
