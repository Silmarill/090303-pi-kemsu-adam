using System;
using System.Collections.Generic;

namespace Asteroid
{
    public class AsteroidEmitter
    {
        private Queue<Asteroid> _avaible = new Queue<Asteroid>();
        private int _spawnCounter = 0;

        public AsteroidEmitter(int initialSize)
        {
            for (int iteration = 0; iteration < initialSize; ++iteration)
            {
                Asteroid asteroid = new Asteroid();
                _avaible.Enqueue(asteroid);
            }
        }

        public Asteroid Spawn()
        {
            _spawnCounter++;
            Asteroid asteroid;
            
            if (_avaible.Count == 0)
            {
                Console.WriteLine("Пул пуст, создаются новые стероиды");
                asteroid = new Asteroid();
            }
            else
            {
                asteroid = _avaible.Dequeue();
            }

            asteroid.Reset(_spawnCounter);
            return asteroid;
        }

        public void Recycle(Asteroid asteroid)
        {
            asteroid.Reset(0);
            _avaible.Enqueue(asteroid);
        }
    }
}