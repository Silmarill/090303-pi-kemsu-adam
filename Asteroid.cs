using System;

public class Asteroid : IChronListener
{
    public const int MinEchos = 100;
    public const int MaxEchosLimit = 1000;
    public const int DegradationValue = 100;

    private static int _globalIdCounter = 0;
    private static Random _rng = new Random();

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Asteroid()
    {
        _globalIdCounter++;
        CreateID = _globalIdCounter;

        MaxEchos = _rng.Next(MinEchos, MaxEchosLimit + 1);
        CurrentEchos = MaxEchos;
        State = AsteroidState.Idle;
    }

    public void Reset()
    {
        CurrentEchos = MaxEchos;
        State = AsteroidState.Idle;
    }

    public void OnChroneTick()
    {
        if (State == AsteroidState.Idle)
        {
            CurrentEchos -= DegradationValue; 

            if (CurrentEchos <= 0)
            {
                CurrentEchos = 0;
                State = AsteroidState.Depleted;
            }
        }
    }

    public override string ToString()
    {
        return $"[ID:{CreateID} Spawn:{SpawnID}] Эхо: {CurrentEchos}/{MaxEchos} | Статус: {State}";
    }
}