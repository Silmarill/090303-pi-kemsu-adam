using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation.Models {
  public enum HarvesterState {
    Idle,    // На станции, ждёт задание
    Mining   // Добывает астероид
  }
}
