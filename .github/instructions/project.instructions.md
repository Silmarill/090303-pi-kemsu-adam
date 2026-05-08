---
applyTo: "**/*.cs"
---


# Copilot review instructions for C# asteroid lab

Apply these instructions when reviewing C# files (`*.cs`) for the asteroid simulation lab.

The goal of the review is to check whether the solution implements the assignment architecture and expected behavior correctly. Do **not** nitpick cosmetic details, wording of console messages, emojis, exact variable names, or minor formatting if the code remains readable and idiomatic.

## Review priorities

Focus comments on issues that can break the assignment requirements, make the architecture inconsistent, or hide an incorrect implementation of the required patterns.

High-priority findings:

- Missing or incorrect implementation of the required patterns:
  - Observer: `IChronListener` and `ChroneManager` / chron manager behavior.
  - Object Pool: `AsteroidEmitter` reusing `Asteroid` objects through a pool.
- Incorrect asteroid lifecycle: spawn → active degradation → depleted → recycle → reset → reuse.
- Creating new asteroids unnecessarily instead of reusing pooled instances.
- Incorrect chron/tick logic, especially initial spawn, tick counting, spawning every 5 chrons, and recycling depleted asteroids.
- Collection misuse that changes the required architecture, for example using a `List<Asteroid>` as the free-object pool instead of `Queue<Asteroid>`.
- State/data bugs that make `CreateID`, `SpawnID`, `MaxEchos`, `CurrentEchos`, or `AsteroidState` misleading or incorrect.
- Code organization problems that make the solution hard to understand, for example placing all classes in `Program.cs` when separate files/classes are expected.

Low-priority or non-issues:

- Exact text of console messages.
- Presence or absence of emojis in output.
- Exact colors used for optional color output unless the student implemented that complication and it is functionally wrong.
- Small naming differences if the intent is clear and the public behavior is correct.
- Minor formatting differences already covered by `.editorconfig` or existing style checks.

## Assignment-specific requirements to verify

### Asteroid state and data model

Check that the solution has an `AsteroidState` enum or equivalent clear state model. It should at least distinguish active/idle asteroids from depleted asteroids.

For `Asteroid`, verify that:

- The constructor initializes:
  - `MaxEchos` to a random value in the expected range, normally 100–1000 unless the student intentionally implemented an extension with different emitter ranges.
  - `CurrentEchos = MaxEchos`.
  - `State = Idle` or the equivalent active state.
  - A stable unique creation identifier (`CreateID`) for each actual object instance.
- `Reset()` restores the asteroid for reuse:
  - `CurrentEchos = MaxEchos`.
  - `State = Idle` or equivalent.
  - It must **not** change `CreateID`.
  - It should update or prepare a spawn/reuse identifier (`SpawnID`) if the implementation uses one to track reuse.
- `OnChronTick()` or equivalent degradation logic:
  - Reduces `CurrentEchos` by 100 per chron for active/idle asteroids.
  - Does not allow `CurrentEchos` to become negative unless the code immediately normalizes it before state/output is used.
  - Changes the state to `Depleted` when the resource reaches 0.
  - Does not degrade asteroids that are already depleted or in the pool.

Flag a bug if `Reset()` creates a new random `MaxEchos` every time unless the code clearly documents that this is an intentional spawn-level redesign. The base task expects object identity to be reused and resource reset to the asteroid’s max value.

### Object Pool / AsteroidEmitter

For `AsteroidEmitter`, verify that:

- It owns a `Queue<Asteroid>` for free/reusable asteroids.
- The constructor accepts an initial pool size and pre-populates the pool with that many `Asteroid` objects.
- `Spawn()` retrieves an asteroid from the queue when possible.
- `Spawn()` creates a new `Asteroid` only when the queue is empty.
- `Spawn()` returns an asteroid ready to be active in the simulation.
- `Recycle(Asteroid asteroid)` calls `Reset()` and returns the object to the queue.
- Recycled objects can later be spawned again as the same object instance, preserving `CreateID`.

Flag these as important issues:

- `Spawn()` always uses `new Asteroid()` even when the pool has available objects.
- `Recycle()` forgets to enqueue the asteroid back into the pool.
- `Recycle()` enqueues the asteroid without resetting it.
- The same asteroid instance can appear simultaneously in active asteroids and in the free pool.
- The pool stores active asteroids and free asteroids in the same collection without a clear separation.

### Observer / chron architecture

The assignment expects an Observer-style mechanism around chron ticks.

Verify that:

- There is an `IChronListener` interface or equivalent abstraction for objects that react to chron ticks.
- There is a static `ChroneManager` / `ChronManager` or equivalent central manager that stores listeners and notifies them on each chron.
- The manager supports subscription and notification. Unsubscription is recommended if dynamic listeners are used.
- Tick notification does not require every future listener type to be hard-coded directly in `Program.Main`.

Do not reject a solution solely because `Program.Main` still coordinates high-level flow. However, flag it if the observer pattern is nominal only and all chron behavior is implemented through direct calls with no meaningful listener registration/notification mechanism.

### Program flow and chron loop

Check that `Program.Main` or the application entry point implements the required simulation flow:

- Creates an `AsteroidEmitter` with an initial pool size, usually 5.
- Creates and maintains `List<Asteroid>` for active asteroids.
- Spawns exactly 3 active asteroids at chron 0 / application start.
- Advances time by keyboard input, normally Enter for next chron and Esc or another clear command for exit.
- On each chron:
  - Increments the chron counter.
  - Applies chron tick/degradation to active asteroids, directly or through `ChroneManager`.
  - Every 5 chrons (`chron % 5 == 0`), spawns 1–3 new asteroids and adds them to the active list.
  - Finds asteroids whose state is `Depleted`.
  - Recycles depleted asteroids through the emitter.
  - Removes recycled asteroids from the active list.
  - Prints current active asteroid state.

Flag ordering bugs only when they affect observable behavior. For example, recycling depleted asteroids immediately after they become depleted is acceptable if the assignment’s expected result is still met, but the review may mention if the wording says they disappear on the next chron.

### Safe collection handling

Pay attention to mutation during iteration.

Flag code that removes asteroids from `activeAsteroids` inside a `foreach` over the same list, because this can throw or skip elements. Prefer safe patterns such as:

- collecting depleted asteroids into a temporary list, then recycling/removing them;
- iterating backwards by index;
- using `RemoveAll` only if recycling side effects are handled clearly and safely.

### Randomness

Check that random generation is reasonable:

- Avoid creating many `new Random()` instances in rapid succession in methods that can be called often.
- Prefer a shared `Random` instance or `Random.Shared` where available.
- Spawn count every 5 chrons should be 1–3 inclusive.
- `MaxEchos` should be 100–1000 inclusive or match the implemented extension’s documented range.

### IDs and reuse tracking

When IDs are implemented, check their semantics:

- `CreateID` should identify the physical object instance and stay the same across reuse.
- `SpawnID` should identify a spawn/reuse event and should change when the asteroid is spawned or reset for a new active lifecycle.
- IDs should be generated centrally or safely enough to avoid accidental duplicates in normal single-threaded console execution.

Do not demand a specific ID format. Numeric counters, GUIDs, or clear string IDs are all acceptable if semantics are correct.

### Optional complications

Only review optional complications if the student actually attempted them. Do not require complications for the base task.

If attempted, check them according to the assignment intent:

- Multiple emitters: each emitter has independent spawn intervals and clear asteroid ownership/labeling.
- Black hole / resource intervention: implemented as chron listeners, not as unrelated random code disconnected from ticks.
- Colored output: resets `Console.ForegroundColor` after writing.
- XML save/load: preserves enough state to continue the simulation consistently.
- Delayed recycling: depleted asteroids are not immediately available until the required delay passes.
- Logger singleton: globally accessible logger with controllable debug output.
- Asteroid types/anomalies/statistics: integrated into lifecycle and chron mechanics without breaking pooling.

## Review tone

Use review comments that explain the assignment impact, not just the code smell.

Prefer:

> `Spawn()` always creates a new `Asteroid`, so the object pool is not actually used. The assignment expects reuse from `Queue<Asteroid>` first and `new Asteroid()` only when the pool is empty.

Avoid:

> This is bad style.

Prefer:

> This removes items from `activeAsteroids` while iterating it with `foreach`, which can throw at runtime. Collect depleted asteroids first, then recycle/remove them after the loop.

Avoid comments about personal formatting preferences unless they directly affect maintainability or correctness.

## What not to block on

Do not block approval only because:

- Output wording differs from the examples.
- The student uses `Chron` instead of `Chrone` in names, or fixes the spelling consistently.
- The project uses slightly different folder names but still keeps one class per file and remains understandable.
- The console UI is simple.
- Optional tasks are not implemented.

Block or strongly request changes when the required patterns or simulation behavior are missing, incorrect, or only superficially implemented.
