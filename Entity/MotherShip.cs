using AsteroidSimulation.Common;
using AsteroidSimulation.Entity;
using AsteroidSimulation.Observer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace AsteroidSimulation.Entity
{
    public static class MotherShip
    {
        private static List<HarvesterShip> _harvesterList = new List<HarvesterShip>();
        
        public static Dictionary<string, List<Report>> Worklog = new Dictionary<string, List<Report>>();

        public static int ResourcesMined = 0;
        
        public static void AddReport(string shipName, Report report)
        {
            if (!Worklog.ContainsKey(shipName))
            {
                
            }
        }

    }
}