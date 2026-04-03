using System;

namespace Asteroid
{
    public class AsteroidEmitter
    {
        private Queue<Asteroid> _avaible = new Queue<Asteroid>();

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
            if (_avaible.Count == 0)
            {
                return new Asteroid();
            }
            return _avaible.Dequeue();
        }

        public void Recycle(Asteroid asteroid)
        {
            asteroid.Reset();
            _avaible.Enqueue(asteroid);
        }
    }
}