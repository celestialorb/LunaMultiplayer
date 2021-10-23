using LunaConfigNode;
using Server.Context;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.System
{
    /// <summary>
    /// Here we keep a copy of all the player vessels in <see cref="Vessel"/> format and we also save them to files at a specified rate
    /// </summary>
    public static class VesselStoreSystem
    {
        public const string VesselFileFormat = ".txt";
        public static string VesselsPath = Path.Combine(ServerContext.UniverseDirectory, "Vessels");

        public static ConcurrentDictionary<Guid, Vessel.Classes.Vessel> CurrentVessels = new ConcurrentDictionary<Guid, Vessel.Classes.Vessel>();

        private static readonly object BackupLock = new object();

        public static bool VesselExists(Guid vesselId) => CurrentVessels.ContainsKey(vesselId);

        /// <summary>
        /// Removes a vessel from the store
        /// </summary>
        public static void RemoveVessel(Guid vesselId)
        {
            CurrentVessels.TryRemove(vesselId, out _);

            Task.Run(() =>
            {
                lock (BackupLock)
                {
                    FileHandler.FileDelete(Path.Combine(VesselsPath, $"{vesselId}{VesselFileFormat}"));
                }
            });

            // Remove the vessel from the metrics.
            Metrics.Vessel.RemoveVessel(vesselId);
        }

        /// <summary>
        /// Returns a vessel in the standard KSP format
        /// </summary>
        public static string GetVesselInConfigNodeFormat(Guid vesselId)
        {
            return CurrentVessels.TryGetValue(vesselId, out var vessel) ?
                vessel.ToString() : null;
        }

        /// <summary>
        /// Load the stored vessels into the dictionary
        /// </summary>
        public static void LoadExistingVessels()
        {
            ChangeExistingVesselFormats();
            lock (BackupLock)
            {
                foreach (var file in Directory.GetFiles(VesselsPath).Where(f => Path.GetExtension(f) == VesselFileFormat))
                {
                    if (Guid.TryParse(Path.GetFileNameWithoutExtension(file), out var vesselId))
                    {
                        Vessel.Classes.Vessel vessel = new Vessel.Classes.Vessel(FileHandler.ReadFileText(file));
                        CurrentVessels.TryAdd(vesselId, vessel);

                        var guid = vesselId.ToString();

                        // Add the vessel to our metrics.
                        Metrics.Vessel.Info.WithLabels(
                            guid,
                            vessel.Fields.GetSingle("name").Value,
                            vessel.Fields.GetSingle("sit").Value,
                            vessel.Fields.GetSingle("type").Value
                        ).IncTo(1);

                        // Add the vessel's epoch.
                        Metrics.Vessel.Epoch.WithLabels(guid).Set(double.Parse(vessel.Orbit.GetSingle("EPH").Value));

                        // Add the vessel's position metrics.
                        Metrics.VesselPosition.Latitude.WithLabels(guid).Set(double.Parse(vessel.Fields.GetSingle("lat").Value));
                        Metrics.VesselPosition.Longitude.WithLabels(guid).Set(double.Parse(vessel.Fields.GetSingle("lon").Value));
                        Metrics.VesselPosition.Altitude.WithLabels(guid).Set(double.Parse(vessel.Fields.GetSingle("alt").Value));
                        Metrics.VesselPosition.Height.WithLabels(guid).Set(double.Parse(vessel.Fields.GetSingle("hgt").Value));

                        // Add the vessel's orbit metrics.
                        Metrics.VesselOrbit.SemimajorAxis.WithLabels(guid).Set(double.Parse(vessel.Orbit.GetSingle("SMA").Value));
                        Metrics.VesselOrbit.Eccentricity.WithLabels(guid).Set(double.Parse(vessel.Orbit.GetSingle("ECC").Value));
                        Metrics.VesselOrbit.Inclination.WithLabels(guid).Set(double.Parse(vessel.Orbit.GetSingle("INC").Value));
                        Metrics.VesselOrbit.LongitudeOfAscendingNode.WithLabels(guid).Set(double.Parse(vessel.Orbit.GetSingle("LAN").Value));
                        Metrics.VesselOrbit.MeanAnomaly.WithLabels(guid).Set(double.Parse(vessel.Orbit.GetSingle("MNA").Value));
                        Metrics.VesselOrbit.ArgumentOfPeriapsis.WithLabels(guid).Set(
                            double.Parse(vessel.Orbit.GetSingle("LPE").Value) - double.Parse(vessel.Orbit.GetSingle("LAN").Value)
                        );

                        // Add the vessel's orientation metrics if we're configured to do so.
                        if(Settings.Structures.MetricsSettings.SettingsStore.EnableVesselOrientationMetrics) {
                            var normal = vessel.Fields.GetSingle("nrm").Value.Split(",");
                            Metrics.VesselOrientation.NormalX.WithLabels(guid).Set(double.Parse(normal[0]));
                            Metrics.VesselOrientation.NormalY.WithLabels(guid).Set(double.Parse(normal[1]));
                            Metrics.VesselOrientation.NormalZ.WithLabels(guid).Set(double.Parse(normal[2]));

                            var surface = vessel.Fields.GetSingle("rot").Value.Split(",");
                            Metrics.VesselOrientation.SurfaceRelativeW.WithLabels(guid).Set(double.Parse(surface[0]));
                            Metrics.VesselOrientation.SurfaceRelativeX.WithLabels(guid).Set(double.Parse(surface[1]));
                            Metrics.VesselOrientation.SurfaceRelativeY.WithLabels(guid).Set(double.Parse(surface[2]));
                            Metrics.VesselOrientation.SurfaceRelativeZ.WithLabels(guid).Set(double.Parse(surface[3]));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Transform OLD Xml vessels into the new format
        /// TODO: Remove this for next version
        /// </summary>
        public static void ChangeExistingVesselFormats()
        {
            lock (BackupLock)
            {
                foreach (var file in Directory.GetFiles(VesselsPath).Where(f => Path.GetExtension(f) == ".xml"))
                {
                    if (Guid.TryParse(Path.GetFileNameWithoutExtension(file), out var vesselId))
                    {
                        var vesselAsCfgNode = XmlConverter.ConvertToConfigNode(FileHandler.ReadFileText(file));
                        FileHandler.WriteToFile(file.Replace(".xml", ".txt"), vesselAsCfgNode);
                    }
                    FileHandler.FileDelete(file);
                }
            }
        }

        /// <summary>
        /// Actually performs the backup of the vessels to file
        /// </summary>
        public static void BackupVessels()
        {
            lock (BackupLock)
            {
                var vesselsInCfgNode = CurrentVessels.ToArray();
                foreach (var vessel in vesselsInCfgNode)
                {
                    FileHandler.WriteToFile(Path.Combine(VesselsPath, $"{vessel.Key}{VesselFileFormat}"), vessel.Value.ToString());
                }
            }
        }
    }
}
