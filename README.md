# Group Stage 6 — Matriarch

## Story

After mastering the stabilization of the first anomalous zones, the Galactic Academy deployed space stations of the **“Matriarch” class**. These massive platforms not only stabilize space but also serve as bases for a fleet of collectors — small harvester ships that extract a mysterious resource called **Echos**.

The “Matriarch” station keeps detailed statistics about its cycles: how many asteroids were processed, how much Echos was extracted, and how many cycles were spent. This information is stored in a **Worklog**, which is later analyzed by the academy to improve fleet efficiency.

While the station is active, asteroids do not decay over time — the Matriarch’s stabilization field protects them. All extraction work is handled by the harvester ships.

Your task is to implement the station, a fleet of five harvesters, a mining system with cargo limits, and a reporting system.

---

## Objectives

### 1. MotherShip (Matriarch station)

The station must:

* Contain a fleet of 5 harvesters (`List<HarvesterShip>`)
* Stabilize the zone (asteroids do not lose Echos over time)
* Maintain a Worklog (`Dictionary<string, List<Report>>`)
* Provide output:

  * total mined resources per harvester (on key press R)
  * full Worklog on the 15th cycle

---

### 2. Asteroid

* Add a `Mining` state (asteroid is currently being mined)

---

### 3. HarvesterShip

Fields:

* `ID`
* `Name`
* `AsteroidsMined`
* `CargoCapacity` (e.g. 500)
* `CargoCurrent`
* `BiteSize` (e.g. 50)

States:

* `Idle`
* `Mining`

Mining logic (`Mine(Asteroid asteroid)`):

* reduces `CurrentEchos` by `BiteSize`
* increases `CargoCurrent`
* if `CurrentEchos == 0` → asteroid becomes `Depleted`
* if `CargoCurrent >= CargoCapacity` → return to station and unload

---

### 4. Report

Fields:

* `JobNumber`
* `AsteroidSpawnID`
* `AmountMined`

---

## Game Logic

* At start:

  * create station
  * create 5 harvesters
  * create 3 asteroids

* Each cycle:

  * idle harvesters select available `Idle` asteroids
  * set asteroid state to `Mining`
  * start mining

* During mining:

  * each cycle harvester calls `Mine`
  * asteroid resource decreases
  * cargo fills up

* On completion:

  * if asteroid is depleted → `Depleted`
  * if cargo is full → return to station
  * unload cargo (reset `CargoCurrent`)
  * create a `Report`
  * add it to Worklog
  * set harvester back to `Idle`

* Depleted asteroids are returned to the pool via `Recycle`

---

## Output

Each cycle:

* asteroid states
* harvester states
* total mined resources

Every 15 cycles:

* full Worklog output

---

## Key Classes

* `Asteroid` — resource container and state holder
* `HarvesterShip` — mining unit
* `MotherShip` — fleet manager and logging system
* `Report` — mining record
* `ChroneManager` — tick system

---

## Checklist

* 5 harvesters are created at start
* 3 asteroids are created at start
* asteroid locking via `Mining` works correctly
* resource is only reduced by harvesters
* asteroids do not decay over time
* Worklog and Summary work correctly
* full Worklog prints every 15 cycles
* asteroid pooling system works
* program exits on `Esc`

---

## Expected Result

The system simulates Echos extraction:

* harvesters mine asteroids
* asteroids deplete and are recycled
* the station records all activity
* every 15th cycle outputs a full report
