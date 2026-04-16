using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAdam {
  public class MotherShip {
    public List<HarvesterShip> Fleet = new List<HarvesterShip>();
    public bool isZoneStabilized = true;
    public Dictionary<string, List<Report>> Worklog = new Dictionary<string, List<Report>>();

  }
}
