public class Report {
  public int JobNumber { get; set; }
  public int AsteroidSpawnID { get; set; }
  public int AmountMined { get; set; }

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