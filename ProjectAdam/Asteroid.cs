using System;
using System.Threading;

namespace ProjectAdam
{
	public class Asteroid : IChronListener
	{
		private static int s_nextCreateId;
		private const int MinEchos = 100;
		private const int MaxEchosExclusive = 1001;

		public int CurrentEchos { get; private set; }
		public int MaxEchos { get; }
		public AsteroidState State { get; private set; }
		public int SpawnID { get; internal set; }
		public int CreateID { get; }
		public bool IsBeingMined { get; private set; }

		public Asteroid()
		{
			CreateID = Interlocked.Increment(ref s_nextCreateId);
			MaxEchos = new Random().Next(MinEchos, MaxEchosExclusive);
			CurrentEchos = MaxEchos;
			State = AsteroidState.Idle;
			IsBeingMined = false;
		}

		public void Reset()
		{
			CurrentEchos = MaxEchos;
			State = AsteroidState.Idle;
			IsBeingMined = false;
		}

		public void OnChronTick()
		{
		}

		public void StartMining()
		{
			if (State == AsteroidState.Idle && !IsBeingMined)
			{
				IsBeingMined = true;
			}
		}

		public void StopMining()
		{
			IsBeingMined = false;
		}

		public int Mine(int biteSize)
		{
			if (State == AsteroidState.Depleted) return 0;

			int mined;
			mined = Math.Min(biteSize, CurrentEchos);
			CurrentEchos -= mined;

			if (CurrentEchos == 0)
			{
				State = AsteroidState.Depleted;
				IsBeingMined = false;
			}

			return mined;
		}

		public void PrintInfo()
		{
			Console.WriteLine(
				$"CreateID={CreateID}, SpawnID={SpawnID}, MaxEchos={MaxEchos}, " +
				$"CurrentEchos={CurrentEchos}, State={State}, BeingMined={IsBeingMined}");
		}
	}
}