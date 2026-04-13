public class Report {
  public int JobNumber;
  public int AsteroidSpawnID;
  public int AmountMined;

  // Конструктор для создания отчета
  public Report(int jobNumber, int asteroidSpawnID, int amountMined) {
    JobNumber = jobNumber;
    AsteroidSpawnID = asteroidSpawnID;
    AmountMined = amountMined;
  }

  // Перегрузка метода ToString для удобного отображения информации о отчете
  public override string ToString() {
    return $"Job #{JobNumber} | Asteroid SpawnID: {AsteroidSpawnID} | Mined: {AmountMined}";
  }
}