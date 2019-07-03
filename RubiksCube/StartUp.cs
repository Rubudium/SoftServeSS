namespace RubiksCube
{
	using System;

	public class StartUp
	{
		public static void Main()
		{
			Console.Title = "Hristo's Rubik";
			Console.CursorVisible = false;

			MoveHistory history = RubikPattern.GetPattern(0);

			while (true)
			{
				Console.WriteLine($"Type a command: help, [LRFBUD][2'], [xyz], Exit, reset, undo, history, list, apply, read, or [0-{RubikPattern.Patterns.Count - 1}]");

				string input = Console.ReadLine()
					.ToUpper()
					.Trim();

				if (String.IsNullOrEmpty(input))
				{
					history.PrintLastState();
				}

				else if (input == "EXIT")
				{
					return;
				}

				else if (input == "HELP")
				{
					string helpText = String.Format($"Help: this command.{Environment.NewLine}" +
						$"Exit: exit this program.{Environment.NewLine}" +
						$"Reset: reset the cube to start position.{Environment.NewLine}" +
						$"Undo: undo the last move.{Environment.NewLine}" +
						$"History: moves you have made so far.{Environment.NewLine}" +
						$"List: list the {0} cube patterns for start position.{Environment.NewLine}" +
						$"<n>: set the start position to the <n>th (n=0..{0}) pattern in the list.{Environment.NewLine}" +
						$"Read: read the start position from console.{Environment.NewLine}" +
						$"Apply [LRFBUD][2']] [ [LRFBUD][2']]*: apply a sequence of moves, separated with space.{Environment.NewLine}" +
						$"   Example: Apply U D{Environment.NewLine}" +
						$"x: (rotate): rotate the Cube up.{Environment.NewLine}" +
						$"y: (rotate): rotate the Cube to the counter-clockwise.{Environment.NewLine}" +
						$"z: (rotate): rotate the Cube clockwise.{Environment.NewLine}" +
						$"[LRFBUD][2']: a move for the cube:{Environment.NewLine}", RubikPattern.Patterns.Count - 1);

					foreach (var direction in new string[] { "Left", "Right", "Front", "Back", "Up", "Down" })
					{
						helpText = helpText + String.Format($"\t{0},{0}2,{0}': move {1} face for 90, 180, 270 degree clockwise. {Environment.NewLine}", direction[0], direction);
					}
					Console.WriteLine(helpText);
				}

				else if (input == "RESET")
				{
					history = new MoveHistory(history.GetStartState());

					history.PrintLastState();
				}

				else if (input == "HISTORY")
				{
					history.PrintHistory();
				}

				else if (input == "UNDO")
				{
					history.Undo();
					history.PrintLastState();
				}

				else if (input == "LIST")
				{
					for (int i = 0; i < RubikPattern.Patterns.Count; i++)
					{
						Console.WriteLine($"{i}:\t{RubikPattern.Patterns[i].Description}");
					}
				}

				else if (Char.IsDigit(input, 0))
				{
					try
					{
						int index = Convert.ToInt32(input);

						history = RubikPattern.GetPattern(index);
					}
					catch (Exception exception)
					{
						Console.WriteLine(exception);
					}
				}

				else if (input.StartsWith("READ"))
				{
					try
					{
						CubeReader reader = new CubeReader();

						Cube side = reader.ReadCube();

						if (side != null)
						{
							history = new MoveHistory(side);
						}
					}
					catch (Exception exception)
					{
						Console.WriteLine(exception);
					}
					history.PrintLastState();
				}

				else if (input.StartsWith("APPLY"))
				{
					string[] moves = input.Substring("APPLY".Length).Split(' ');

					int moveCount = 0;

					bool isFine = true;

					foreach (var move in moves)
					{
						if (!String.IsNullOrEmpty(move) && !Move.IsValidMove(move))
						{
							Console.WriteLine($"Please remove the invalid move [{move}] from input {input}");
							isFine = false;
						}
						else if (Move.IsValidMove(move))
						{
							moveCount++;
						}
					}

					if (moveCount == 0)
					{
						Console.WriteLine("Please specify a sequence of moves");
					}
					else if (isFine)
					{
						Cube s = Move.ApplyActions(history.GetCurrent().Clone(), moves);
						history.AddToHistoryIfNotDuplicate(s);
					}
				}

				else if (!Move.IsValidMove(input))
				{
					using (new ConsoleColor((int)ConsoleColor.ForeGroundColor.Red | (int)ConsoleColor.ForeGroundColor.White))
					{
						Console.WriteLine("Invalid input!");
					}
				}

				else
				{
					Move move = Move.GetMove(input);

					Cube state = move.ActOn(history.GetCurrent());

					history.AddToHistoryIfNotDuplicate(state);
				}
			}
		}
	}
}
