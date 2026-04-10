# Laboratory Work №5: Observer and Object Pool Patterns

### Author
**Me**

**Variant:** Asteroid Field Simulation & Resource Management

---

### Objective
Exploration of behavioral and creational design patterns in C#. Implementation of the Observer pattern for synchronized time-step (chron) management and the Object Pool pattern for efficient memory allocation and reuse of game objects.

---

### Project Structure
* **Program.cs:** Main entry point handling the simulation loop, user input, and real-time console rendering.
* **Asteroid.cs:** Core entity class representing an asteroid with resource degradation logic and state tracking.
* **AsteroidEmitter.cs:** Service implementing the Object Pool pattern for spawning and recycling asteroid instances.
* **ChroneManager.cs:** Static service implementing the Observer pattern to notify listeners about time-step events.
* **IChroneListener.cs:** Interface defining the contract for objects that react to time-step ticks.
* **AsteroidState.cs:** Enumeration defining the lifecycle stages of an asteroid (Idle, Depleted).
* **AsteroidsLab.csproj:** Modern SDK-style project file configured for .NET environment.

---

### Coding Standards Applied
* **Indentation:** Strictly 2 spaces (no tabs).
* **Naming Convention:** lowerCamelCase for variables, local properties, and descriptive loop indices (e.g., ++asteroidIndex).
* **Logic Separation:** Explicit separation between variable declaration and initialization/calculation.
* **State Management:** Use of the AsteroidState enum to manage object lifecycles without violating encapsulation.
* **Memory Optimization:** Use of the Object Pool (Queue-based) to minimize GC pressure by reusing objects.
* **Formatting:** All increments use prefix notation (++index). Comments are placed strictly above the code lines.

---

### Key Features
* **Object Pooling:** Efficient management of asteroids to prevent constant memory allocation and deallocation.
* **Observer Mechanism:** Centralized time management system that notifies all active objects simultaneously.
* **Resource Degradation:** Automated reduction of "Echos" resources based on synchronized time-steps.
* **Dynamic Spawning:** Adaptive system that populates the simulation with new asteroids at specific intervals.
* **Console UI:** Real-time dashboard showing the status, IDs, and resource levels of all active entities.

---

### How to Run
1.  **Environment:** Ensure you have .NET SDK installed.
2.  **Navigate:** Open your terminal in the project folder: cd AsteroidsLab.
3.  **Clean (Optional):** `dotnet clean`.
4.  **Run:** Execute the application using:
```powershell
dotnet run
```