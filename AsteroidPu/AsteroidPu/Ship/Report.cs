using System;

namespace AsteroidPu.Ship {
  public class Report {

    public int jobNumber = 0, 
      asteroidSpawnID,
      amountMined;

    public Report(int jobNum, int spawnID, int mined) {
      jobNumber = jobNum;
      asteroidSpawnID = spawnID;
      amountMined = mined;
    }

    public void Print() {
      Console.WriteLine($"Job num: {jobNumber}. \nSpawn ID: {asteroidSpawnID}" +
                        $"\n Amount mined: {amountMined}");
    }
  }
}
