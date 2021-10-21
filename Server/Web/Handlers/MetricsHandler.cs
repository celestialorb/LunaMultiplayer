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
    private Gauge VesselAltitude;
    private Gauge VesselLatitude;
    private Gauge VesselLongitude;
    private Gauge VesselSemimajorAxis;
    private Gauge VesselEccentricity;
    private Gauge VesselInclination;

    public MetricsHandler(ServerInformation serverInformation) {
      ServerInformation = serverInformation;

      GaugeConfiguration VesselConfiguration = new GaugeConfiguration{
        LabelNames = new[] {"id", "name", "type"}
      };

      // General metrics.
      PlayerCount = Metrics.CreateGauge("lmp_players_online_total", "The total number of players currently online.");
      SubspaceCount = Metrics.CreateGauge("lmp_subspace_total", "The total number of individual subspaces.");

      // Traditional coordinate metrics.
      VesselAltitude = Metrics.CreateGauge("lmp_vessel_altitude_meters", "The altitude of the vessel.", VesselConfiguration);
      VesselLatitude = Metrics.CreateGauge("lmp_vessel_latitude_degrees", "The latitude of the vessel.", VesselConfiguration);
      VesselLongitude = Metrics.CreateGauge("lmp_vessel_longitude_degrees", "The longitude of the vessel.", VesselConfiguration);

      // Orbital metrics.
      VesselSemimajorAxis = Metrics.CreateGauge("lmp_vessel_semimajor_axis_meters", "The semimajor axis of the vessel's orbit.", VesselConfiguration);
      VesselEccentricity = Metrics.CreateGauge("lmp_vessel_eccentricity", "The eccentricity of the vessel's orbit.", VesselConfiguration);
      VesselInclination = Metrics.CreateGauge("lmp_vessel_inclination_degrees", "The inclination of the vessel's orbit.", VesselConfiguration);
    }

    public Task Handle(IHttpContext context, Func<Task> next) {
      PlayerCount.Set(ServerInformation.CurrentState.CurrentPlayers.Count);
      SubspaceCount.Set(ServerInformation.CurrentState.Subspaces.Count);

      foreach(var vessel in ServerInformation.CurrentState.CurrentVessels) {
        var identifier = vessel.Id.ToString();

        VesselAltitude.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Alt);
        VesselLatitude.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Lat);
        VesselLongitude.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Lon);

        VesselSemimajorAxis.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.SemiMajorAxis);
        VesselEccentricity.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Eccentricity);
        VesselInclination.WithLabels(identifier, vessel.Name, vessel.Type).Set(vessel.Inclination);
      }

      var stream = new MemoryStream();
      Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);
      context.Response = new HttpResponse("text/plain; version=0.0.4", stream, false);
      return Task.Factory.GetCompleted();
    }
  }
}
