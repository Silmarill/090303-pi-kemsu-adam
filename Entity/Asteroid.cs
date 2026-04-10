using AsteroidSimulation.Common;
using System;

namespace AsteroidSimulation.Entity
{
    public class Asteroid : IChroneListener
    {
        private static Random random = new Random();
        private static int _globalCreateCount = 0;

        private int MinRandom = 100;
        private int MaxRandom = 1001;
        private int ResourceDamage = 100;

        public int CurrentEchos;
        public int MaxEchos;
        public int SpawnID;
        public int CreateID;
        public AsteroidState State;

        public Asteroid()
        {
            MaxEchos = random.Next(MinRandom, MaxRandom);
            CreateID = ++_globalCreateCount;
        }

        public void Reset(int newSpawnID)
        {
            CurrentEchos = MaxEchos;
            State = AsteroidState.Idle;
            SpawnID = newSpawnID;
        }

        public void OnChroneTick()
        {
            if (State == AsteroidState.Idle)
            {
                CurrentEchos -= ResourceDamage;
                
                if (CurrentEchos <= 0)
                {
                    CurrentEchos = 0;
                    State = AsteroidState.Depleted;
                }
            }
        }
    }
    
    public enum AsteroidState
    {
        Idle,
        Depleted,
        Mining
    }
}