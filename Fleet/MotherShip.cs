using AsteroidsLab.Asteroids;

namespace AsteroidsLab.Fleet;

public class MotherShip
{
  public List<HarvesterShip> Fleet;
  public Dictionary<string, List<Report>> WorkLog;

  private List<Asteroid> _activeAsteroids;
  private AsteroidEmitter? _asteroidEmitter;

  public MotherShip(int harvesterCount, int cargoCapacity, int biteSize)
  {
    int shipIndex;
    HarvesterShip ship;

    Fleet = new List<HarvesterShip>();
    WorkLog = new Dictionary<string, List<Report>>();
    _activeAsteroids = new List<Asteroid>();

    for (shipIndex = 1; shipIndex <= harvesterCount; ++shipIndex)
    {
      ship = new HarvesterShip(shipIndex, "Harvester-" + shipIndex, cargoCapacity, biteSize, this);
      Fleet.Add(ship);
    }
  }

  public void SetEmitter(AsteroidEmitter emitter)
  {
    _asteroidEmitter = emitter;
  }

  public void AddAsteroid(Asteroid asteroid)
  {
    _activeAsteroids.Add(asteroid);
  }

  public void RemoveAsteroid(Asteroid asteroid)
  {
    if (_asteroidEmitter != null)
    {
      _asteroidEmitter.Recycle(asteroid);
    }

    _activeAsteroids.Remove(asteroid);
  }

  public Asteroid? GetIdleAsteroid()
  {
    int activeAsteroidIndex;

    for (activeAsteroidIndex = 0; activeAsteroidIndex < _activeAsteroids.Count; ++activeAsteroidIndex)
    {
      if (_activeAsteroids[activeAsteroidIndex].State == AsteroidState.Idle)
      {
        return _activeAsteroids[activeAsteroidIndex];
      }
    }

    return null;
  }

  public void AssignIdleHarvesters()
  {
    int fleetSlotIndex;
    HarvesterShip ship;
    Asteroid? rock;

    for (fleetSlotIndex = 0; fleetSlotIndex < Fleet.Count; ++fleetSlotIndex)
    {
      ship = Fleet[fleetSlotIndex];
      if (ship.State != HarvesterState.Idle)
      {
        continue;
      }

      rock = GetIdleAsteroid();
      if (rock != null)
      {
        ship.StartMining(rock);
      }
    }
  }

  public void FinishHarvest(HarvesterShip harvester)
  {
    Report report;
    Asteroid? rock;
    bool wasDepleted;

    report = harvester.CreateReport();
    AddReportEntry(harvester.Name, report);
    harvester.CargoCurrent = 0;

    rock = harvester.CurrentAsteroid;
    if (rock == null)
    {
      return;
    }

    wasDepleted = rock.State == AsteroidState.Depleted;
    if (wasDepleted)
    {
      ++harvester.AsteroidsMined;
      RemoveAsteroid(rock);
    }
    else
    {
      rock.State = AsteroidState.Idle;
    }

    harvester.CurrentAsteroid = null;
    harvester.State = HarvesterState.Idle;
  }

  private void AddReportEntry(string harvesterName, Report report)
  {
    List<Report> listForShip;

    if (!WorkLog.ContainsKey(harvesterName))
    {
      listForShip = new List<Report>();
      WorkLog[harvesterName] = listForShip;
    }

    WorkLog[harvesterName].Add(report);
  }

  public int GetActiveAsteroidsCount()
  {
    return _activeAsteroids.Count;
  }

  public int GetTotalMinedForHarvester(string harvesterName)
  {
    int total;
    List<Report>? listForShip;
    int reportIndex;

    total = 0;
    if (!WorkLog.TryGetValue(harvesterName, out listForShip))
    {
      return 0;
    }

    for (reportIndex = 0; reportIndex < listForShip.Count; ++reportIndex)
    {
      total += listForShip[reportIndex].AmountMined;
    }

    return total;
  }

  public void PrintAsteroidsInfo()
  {
    int activeAsteroidIndex;

    for (activeAsteroidIndex = 0; activeAsteroidIndex < _activeAsteroids.Count; ++activeAsteroidIndex)
    {
      _activeAsteroids[activeAsteroidIndex].PrintInfo();
    }
  }

  public void PrintHarvestersInfo()
  {
    int fleetSlotIndex;

    for (fleetSlotIndex = 0; fleetSlotIndex < Fleet.Count; ++fleetSlotIndex)
    {
      Fleet[fleetSlotIndex].PrintInfo();
    }
  }

  public void PrintTotalMined()
  {
    int fleetSlotIndex;

    Console.WriteLine("--- Totals mined (worklog) ---");
    for (fleetSlotIndex = 0; fleetSlotIndex < Fleet.Count; ++fleetSlotIndex)
    {
      Console.WriteLine(
        Fleet[fleetSlotIndex].Name + ": " + GetTotalMinedForHarvester(Fleet[fleetSlotIndex].Name));
    }

    Console.WriteLine("-----------------------------------");
  }

  public void PrintFullWorklog()
  {
    string shipName;
    List<Report>? reports;
    string worklogBannerStart;
    string worklogBannerEnd;
    int fleetIndex;
    int reportIndex;

    worklogBannerStart = "========== FULL WORKLOG (chron % 15) ==========";
    worklogBannerEnd = "================================================";

    Console.WriteLine(worklogBannerStart);

    for (fleetIndex = 0; fleetIndex < Fleet.Count; ++fleetIndex)
    {
      shipName = Fleet[fleetIndex].Name;
      Console.WriteLine(">> " + shipName);

      if (!WorkLog.TryGetValue(shipName, out reports) || reports.Count == 0)
      {
        Console.WriteLine("   (no reports yet)");
        continue;
      }

      for (reportIndex = 0; reportIndex < reports.Count; ++reportIndex)
      {
        reports[reportIndex].Print();
      }
    }

    Console.WriteLine(worklogBannerEnd);
  }
}
