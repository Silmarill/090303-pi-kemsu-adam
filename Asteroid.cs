using System;
using System.Data;

namespace Asteroid
{
    public class Asteroid
    {
        int CurrentEchos;
        int MaxEchos;
        int SpawnID;
        int CreateID;
    
        public Asteroid()
        {
            CurrentEchos = MaxEchos;
            State = Idle;
        }
    
    }
    
    public enum AsteroidState
    {
        Idle,
        Depleted
    }
}