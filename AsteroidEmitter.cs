using System;
using System.Collections.Generic;

public class AsteroidEmitter
{
    public Queue<Asteroid> Available = new Queue<Asteroid>();
    private int _spawnCounter = 0;

    public AsteroidEmitter(int initialSize)
    {
        for (int i = 0; i < initialSize; i++)
        {
            Available.Enqueue(new Asteroid());
        }
    }

    public Asteroid Spawn()
    {
        Asteroid a;
        if (Available.Count > 0)
        {
            a = Available.Dequeue();
        }
        else
        {
            Console.WriteLine("(!) Пул пуст, создаю новый объект");
            a = new Asteroid();
        }

        _spawnCounter++;
        a.SpawnID = _spawnCounter;

        ChroneManager.AddListener(a);
        return a;
    }

    public void Recycle(Asteroid a)
    {
        ChroneManager.RemoveListener(a);
        a.Reset();
        Available.Enqueue(a);
    }
}