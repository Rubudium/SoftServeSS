namespace RubiksCube
{
	using System;
	using System.Collections.Generic;

	public class MoveHistory
	{
		public MoveHistory(Cube initState)
		{
			Add(initState);
		}

		int step = 0;

		List<Cube> listOfCubes = new List<Cube>();

		public void Add(Cube state)
		{
			step++;
			listOfCubes.Add(state);
		}


		public void AddToHistoryIfNotDuplicate(Cube state)
		{
			Cube oldOne = FindExactMatch(state);

			if (oldOne != null)
			{
				state.Print();

				using (new ConsoleColor((int)ConsoleColor.ForeGroundColor.Yellow | 0x8))
				{
					Console.WriteLine($"The above is duplicate state to step = {oldOne.Step}, path = {oldOne.Path} ignored");
				}
			}
			else
			{
				oldOne = FindSimilarMatch(state);

				if (oldOne != null)
				{
					using (new ConsoleColor((int)ConsoleColor.ForeGroundColor.Yellow))
					{
						Console.WriteLine($"Warning: the above is simliar state to step = {oldOne.Step}, path = {oldOne.Path}");
					}
				}
				Add(state);

				PrintLastState();
			}
		}

		public void Undo()
		{
			if (step > 1)
			{
				Cube s = listOfCubes[step - 1];

				listOfCubes.RemoveAt(step - 1);
				step--;
			}
			else
			{
				Console.WriteLine("Cannot undo the initial state");
			}
		}

		public void PrintHistory()
		{
			foreach (Cube side in listOfCubes)
			{
				side.Print();
			}
		}
		public void PrintLastState()
		{
			GetCurrent().Print();
		}

		public Cube FindExactMatch(Cube state)
		{
			return listOfCubes.Find((obj) => obj.CompareTo(state) == 0);
		}

		//If we rotate the whole cube to another cube, then they are similar
		public Cube FindSimilarMatch(Cube state)
		{
			return listOfCubes.Find((obj) => obj.Similar(state));
		}

		public Cube GetStartState()
		{
			return listOfCubes[0];
		}

		public Cube GetCurrent()
		{
			if (step > 0)
			{
				return listOfCubes[step - 1];
			}
			else
			{
				throw new ApplicationException("No current value");
			}
		}
	}
}
