namespace Server.Metrics {
    public class Utilities {
        public static string GetCelestialBodyName(int id) {
            // Surely a mapping of these *must* already exist somewhere, right?
            // Or is that maybe only client-side?
            switch(id) {
                case -1: return "None";
                case 0: return "Kerbol";
                case 1: return "Kerbin";
                case 2: return "Mun";
                case 3: return "Minmus";
                case 4: return "Moho";
                case 5: return "Eve";
                case 6: return "Duna";
                case 7: return "Ike";
                case 8: return "Jool";
                case 9: return "Laythe";
                case 10: return "Vall";
                case 11: return "Bop";
                case 12: return "Tylo";
                case 13: return "Gilly";
                case 14: return "Pol";
                case 15: return "Dres";
                case 16: return "Eeloo";

                // TODO: add planets for well-known planet mods
            }
            return "Unknown";
        }
    }
}