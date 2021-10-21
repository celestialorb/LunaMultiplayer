using System.Collections.Generic;

namespace Server.Web.Structures {
  public class BodyMapper {
    // TODO: determine if there is a way to obtain this information at startup.
    // TODO: determine reference body identifiers for popular planet mods
    public static readonly Dictionary<int, string> Mapping = new Dictionary<int, string>{
      {0, "Kerbol"},
      {1, "Kerbin"},
      {2, "Mun"},
      {3, "Minmus"},
      {4, "Moho"},
      {5, "Eve"},
      {6, "Duna"},
      {7, "Ike"},
      {8, "Jool"},
      {9, "Laythe"},
      {10, "Vall"},
      {11, "Bop"},
      {12, "Tylo"},
      {13, "Gilly"},
      {14, "Pol"},
      {15, "Dres"},
      {16, "Eeloo"},
    };
  }
}
