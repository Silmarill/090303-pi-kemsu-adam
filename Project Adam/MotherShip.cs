using System.Collections.Generic;

namespace Project_Adam {
  public class MotherShip {
    public int FleetSize = 5;

    public List<HarvesterShip> Fleet;
    public Dictionary<string, List<Report>> Worklog;

    public MotherShip() {
      Fleet = new List<HarvesterShip>();
      Worklog = new Dictionary<string, List<Report>>();

      string[] shipNames = { "Horizon", "Voyager", "Pathfinder", "Odyssey", "Starlight" };

      for (int shipIndex = 0; shipIndex < FleetSize; ++shipIndex) {
        HarvesterShip newShip = new HarvesterShip(shipNames[shipIndex]);
        Fleet.Add(newShip);
        Worklog[newShip.Name] = new List<Report>();
      }
    }
  }
}