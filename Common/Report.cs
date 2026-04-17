namespace AsteroidSimulation.Common {
    public class Report {
        public int JobNumber;
        public int AmountMined;
        public int AsteroidSpawnID;

        public Report(int job, int amount, int asteroidID) {
            JobNumber = job;
            AmountMined = amount;
            AsteroidSpawnID = asteroidID;
        }
    }
}