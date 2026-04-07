using System;
using System.Collections.Generic;

namespace ProjectAdam {
  class Program {
    static void Main(string[] args) {
      AsteroidEmitter emitter = new AsteroidEmitter(5);
      List<Asteroid> activeAsteroids = new List<Asteroid>();

      int chroneCount = 0;

      for (int i = 0; i < 3; i++) {
        Asteroid asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChroneManager.AddListener(asteroid);
      }
    }
  }
}