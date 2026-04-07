using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation {
  public interface IChroneListener {
    void OnChronTick();
  }
}