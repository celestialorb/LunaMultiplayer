using Prometheus;
using Server.System;
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

    private Counter PlayerInfo;

    private Counter SubspaceEpoch;

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

    private Counter ScenarioInfo;
    private Gauge ScenarioScience;
    private Gauge ScenarioFunds;
    private Gauge ScenarioReputation;

    public MetricsHandler(ServerInformation serverInformation) {
      ServerInformation = serverInformation;

      // General metrics.
      PlayerInfo = Metrics.CreateCounter(
        "lmp_player_info",
        "Information about each player currently in the universe.",
        new CounterConfiguration{LabelNames = new[] {"name"}}
      );
      SubspaceEpoch = Metrics.CreateCounter(
        "lmp_subspace_epoch",
        "The current epoch of the subspace.",
        new CounterConfiguration{LabelNames = new[] {"creator"}}
      );

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

      // Space program metrics.
      ScenarioInfo = Metrics.CreateCounter(
        "lmp_scenario_info",
        "Information about the current scenario.",
        new CounterConfiguration{
          LabelNames = new[] {
            "name",
            "type",
          }
        }
      );
      ScenarioScience = Metrics.CreateGauge(
        "lmp_scenario_science_total",
        "The scenario's current total science."
      );
      ScenarioFunds = Metrics.CreateGauge(
        "lmp_scenario_funds_total",
        "The scenario's current total funds."
      );
      ScenarioReputation = Metrics.CreateGauge(
        "lmp_scenario_reputation_total",
        "The scenario's current total reputation."
      );
    }

    public Task Handle(IHttpContext context, Func<Task> next) {
      // Set general metrics.
      foreach(var player in ServerInformation.CurrentState.CurrentPlayers) {
        PlayerInfo.WithLabels(player).IncTo(1);
      }
      foreach(var subpsace in ServerInformation.CurrentState.Subspaces) {
        SubspaceEpoch.WithLabels(subpsace.Creator).IncTo(subpsace.Time);
      }

      // Set vessel metrics.
      foreach(var vessel in ServerInformation.CurrentState.CurrentVessels) {
        var identifier = vessel.Id.ToString();
        if(!BodyMapper.Mapping.TryGetValue(vessel.ReferenceBody, out var body)) {
          body = "Unknown";
        }

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

      // Set space program metrics.
      if(ScenarioStoreSystem.CurrentScenarios.TryGetValue("ResearchAndDevelopment", out var rnd)) {
        if(double.TryParse(rnd.GetValue("sci").Value, out var result)) {
          ScenarioScience.Set(result);
        }
      }
      if(ScenarioStoreSystem.CurrentScenarios.TryGetValue("Funding", out var funding)) {
        if(double.TryParse(funding.GetValue("funds").Value, out var result)) {
          ScenarioFunds.Set(result);
        }
      }
      if(ScenarioStoreSystem.CurrentScenarios.TryGetValue("Reputation", out var reputation)) {
        if(double.TryParse(funding.GetValue("rep").Value, out var result)) {
          ScenarioReputation.Set(result);
        }
      }
      
      // Write out the Prometheus metrics to the response.
      var stream = new MemoryStream();
      Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);
      context.Response = new HttpResponse("text/plain; version=0.0.4", stream, false);
      return Task.Factory.GetCompleted();
    }
  }
}
