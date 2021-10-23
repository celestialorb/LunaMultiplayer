using LmpCommon.Message.Data.Kerbal;
using Server.System;
using System.IO;
using System.Linq;

namespace Server.Metrics {
    public class Kerbal {
        // public static readonly Prometheus.Counter Count = Prometheus.Metrics.CreateCounter(
        //     "lmp_player_online",
        //     "Whether or not the player is currently online.",
        //     new Prometheus.CounterConfiguration{LabelNames = new[] {"name"}}
        // );
        public static readonly Prometheus.Counter Info = Prometheus.Metrics.CreateCounter(
            "lmp_kerbal_info",
            "Information about a Kerbal.",
            new Prometheus.CounterConfiguration{LabelNames = new[] {
                "name",
                "gender",
                "type",
                "trait",
                "badass",
                "veteran"
            }}
        );

        public static readonly Prometheus.Gauge Bravery = Prometheus.Metrics.CreateGauge(
            "lmp_kerbal_bravery",
            "The bravery value of a Kerbal.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"name"}}
        );

        public static readonly Prometheus.Gauge Dumb = Prometheus.Metrics.CreateGauge(
            "lmp_kerbal_dumb",
            "The dumb value of a Kerbal.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"name"}}
        );

        public static readonly Prometheus.Gauge FlightCount = Prometheus.Metrics.CreateGauge(
            "lmp_kerbal_flight_count",
            "The number of flights a Kerbal has been on.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"name"}}
        );

        public static readonly Prometheus.Gauge FlightDeaths = Prometheus.Metrics.CreateGauge(
            "lmp_kerbal_flight_deaths",
            "The number of flights a Kerbal has been on that have resulted in death.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"name"}}
        );

        public static readonly Prometheus.Gauge FlightRecoveries = Prometheus.Metrics.CreateGauge(
            "lmp_kerbal_flight_recoveries",
            "The number of flights a Kerbal has been on that have resulted in recovery.",
            new Prometheus.GaugeConfiguration{LabelNames = new[] {"name"}}
        );

        public static void Update() {
            var kerbalFiles = FileHandler.GetFilesInPath(KerbalSystem.KerbalsPath);
            var kerbalNodes = kerbalFiles.Select(file =>
            {
                return new LunaConfigNode.CfgNode.ConfigNode(File.ReadAllText(file));
            });

            foreach(var node in kerbalNodes) {
                var name = node.GetValue("name").Value;

                Info.WithLabels(
                    name,
                    node.GetValue("gender").Value,
                    node.GetValue("type").Value,
                    node.GetValue("trait").Value,
                    node.GetValue("badS").Value,
                    node.GetValue("veteran").Value
                ).IncTo(1);

                Bravery.WithLabels(name).Set(double.Parse(node.GetValue("brave").Value));
                Dumb.WithLabels(name).Set(double.Parse(node.GetValue("dumb").Value));

                var career = node.GetNode("CAREER_LOG").Value;

                var flights = int.Parse(career.GetValue("flight").Value);
                FlightCount.WithLabels(name).Set(flights);

                var deaths = 0;
                var recoveries = 0;
                foreach(var flight in Enumerable.Range(0, flights)) {
                    var result = career.GetValues(flight.ToString()).Last().Value;

                    if(result == "Die") { deaths += 1; }
                    if(result == "Recover") { recoveries += 1; }
                }

                FlightDeaths.WithLabels(name).Set(deaths);
                FlightRecoveries.WithLabels(name).Set(recoveries);
            }
        }
    }
}
