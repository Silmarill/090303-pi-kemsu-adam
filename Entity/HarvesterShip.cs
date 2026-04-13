using AsteroidSimulation.Common;
using System;

namespace AsteroidSimulation.Entity
{
    public class HarvesterShip : IChroneListener
    {
        private static int _globalIDCounter = 0;

        public int CargoCapacity = 300;
        public int CargoCurrent = 0;
        public int BiteSize = 40;
        public int AsteroidMined = 0;

        public int ID;
        public string Name;
        
        private Asteroid _targetAsteroid;
        public AsteroidState HarvesterState = AsteroidState.Idle;
        
        public HarvesterShip(string name)
        {
            ID = ++_globalIDCounter;
            Name = name;
        }

        public void Target(Asteroid asteroid)
        {
            if (asteroid != null)
            {
                _targetAsteroid = asteroid;
                _targetAsteroid.State = AsteroidState.Mining;
                HarvesterState = AsteroidState.Mining;
            }
        }

        public void Mine()
        {
            int space = 0;
            int amountTake = 0;

            space = CargoCapacity - CargoCurrent;
            amountTake = Math.Min(BiteSize, Math.Min(space, _targetAsteroid.CurrentEchos));
            
            _targetAsteroid.CurrentEchos -= amountTake;
            CargoCurrent += amountTake;

            // Проверка на ресурс астероида и вместимость корабля
            if (_targetAsteroid.CurrentEchos <= 0 || CargoCurrent >= CargoCapacity)
            {
                if (_targetAsteroid.CurrentEchos <= 0)
                {
                    _targetAsteroid.State = AsteroidState.Depleted;
                }
                else
                {
                    _targetAsteroid.State = AsteroidState.Idle;
                }

                FinishMining();
            }
        }

        public void OnChroneTick()
        {
            if (_targetAsteroid != null)
            {
                Mine();
            }
        }

        private void FinishMining()
        {
            ++AsteroidMined;

            Report report = new Report(Observer.ChroneManager.CurrentChrone, CargoCurrent, _targetAsteroid.SpawnID);
            MotherShip.AddReport(Name, report);

            CargoCurrent = 0;
            _targetAsteroid = null;
            HarvesterState = AsteroidState.Idle;
        }
    }
}