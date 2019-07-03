namespace RubiksCube
{
	using System;
	using System.Collections.Generic;

	public class RubikPattern
	{
		public RubikPattern(string description, string[] step)
		{
			this.Description = description;
			this.Steps = step;
		}

		public static List<RubikPattern> Patterns = new List<RubikPattern>();

		public string Description;

		string[] Steps;


		static void AddPattern(string description, string moves)
		{
			Patterns.Add(new RubikPattern(description, moves.Split(' ')));
		}

		static RubikPattern()
		{
			AddPattern("Initial state", "");
			//patterns listed in http://kociemba.org/cube.htm

			AddPattern("Pretty Pattern:Superflip", "R2 F B R B2 R U2 L B2 R U' D' R2 F D2 B2 U2 R' L U");

			AddPattern("Pretty Pattern:Colored Anaconda 1", "F' U' L' B' F' R' U' D B R2 U B R");

			AddPattern("Pretty Pattern:Colored Anaconda 2", "F R D F2 R' U' D B L' R' F' D' R'");

			AddPattern("Pretty Pattern:Colored Phyton 1", "D' L' R' B F D' U' L' R' B' F' U'");

			AddPattern("Pretty Pattern:Colored Phyton 2", "D' L' R' B' F' D U L R B' F' U'");

			AddPattern("Pretty Pattern:Two Colored Rings", "L U' B F' L D' U' R B F' U' R");

			AddPattern("Pretty Pattern:Six Square Blocks", "F2 D F2 D2 L2 U L2 U' L2 B D2 R2");

			AddPattern("Pretty Pattern:Pons Asinorum", "U2 D2 F2 B2 R2 L2");

			AddPattern("Pretty Pattern:Pons Asinorum composed with Superflip", "B' D' L' F' D' F' B U F' B R2 L U D' F L U R D");

			AddPattern("Schoenflies-Symbol Th", "U2 L2 F2 D2 U2 F2 R2 U2");

			AddPattern("Schoenflies-Symbol T", "B F L R B' F' D' U' L R D U");

			AddPattern("Schoenflies-Symbol D3d", "U L D U L' D' U' R B2 U2 B2 L' R' U'");

			AddPattern("Schoenflies-Symbol C3v", "U L' R' B2 U' R2 B L2 D' F2 L' R' U'");

			AddPattern("Schoenflies-Symbol D3", "D B D U2 B2 F2 L2 R2 U' F U");

			AddPattern("Schoenflies-Symbol S6", "B' D' U L' R B' F U");

			AddPattern("Schoenflies-Symbol C3", "L' R U2 R2 D2 F2 L R D2");

			AddPattern("Schoenflies-Symbol D4h", "U2 D2");

			AddPattern("Schoenflies-Symbol D4h", "U D");

			AddPattern("Schoenflies-Symbol S4", "U R2 L2 U2 R2 L2 D");

			AddPattern("Schoenflies-Symbol D2d (edge)", "U F2 B2 D2 F2 B2 U");

			AddPattern("Schoenflies-Symbol D2d (face)", "U R L F2 B2 R' L' U");

			AddPattern("Schoenflies-Symbol D2h (edge)", "U R2 L2 D2 F2 B2 U");

			AddPattern("Schoenflies-Symbol D2h(face)", "B2 D2 U2 F2");

			AddPattern("Schoenflies-Symbol D2 (edge)", "U F2 U2 D2 F2 D");

			AddPattern("Schoenflies-Symbol D2 (face)", "R2 L2 F B");

			AddPattern("Schoenflies-Symbol C2v (a1)", "U R2 L2 U2 F2 B2 U'");

			AddPattern("Schoenflies-Symbol C2v (a2)", "R2 L2 U2");

			AddPattern("Schoenflies-Symbol C2v (b)", "B2 R2 B2 R2 B2 R2");

			AddPattern("Schoenflies-Symbol C2h (a)", "U' D F2 B2");

			AddPattern("Schoenflies-Symbol C2h (b)", "U R2 U D R2 D");

			AddPattern("Schoenflies-Symbol C2 (a)", "L R U2");

			AddPattern("Schoenflies-Symbol C2 (b)", "U R2 D' U' R2 U'");

			AddPattern("Schoenflies-Symbol Cs (b)", "U B2 U D B2 D'");

			AddPattern("Schoenflies-Symbol Ci", "U D' R L'");


			//moves listed in http://peter.stillhq.com/jasmine/rubikscubesolution.html
			AddPattern("Beginner Practice:Middle layer1:Move (4,0,0) to (3,0,0), and keep (0,0,4) in face 0", "D L D' L' D' F' D F");

			AddPattern("Beginner Practice:Middle layer2:Move (4,3,3) to (0,0,3), and keep (3,4) in face 3", "D' F' D F D L D' L'");

			AddPattern("Beginner Practice:Last layer:Orienting LL Edges: state2(L)", "F U R U' R' F'");

			AddPattern("Beginner Practice:Last layer:Orienting LL Edges: state3(-)", "F R U R' U' F'");

			AddPattern("Beginner Practice:Last layer:Swapping adjacent corners", "L U' R' U L' U' R U2");

			AddPattern("Beginner Practice:State 1. Twisting three corners anti-clockwise", "R' U' R U' R' U2 R U2");

			AddPattern("Beginner Practice:State 2. Twisting three corners clockwise", "R U R' U R U2 R' U2");

			AddPattern("Beginner Practice:Permuting the LL Edges step1", "R2 U F B' R2 F' B U R2");

			AddPattern("Beginner Practice:Permuting the LL Edges step2", "R2 U' F B' R2 F' B U' R2");
		}

		public static MoveHistory GetPattern(int index)
		{
			if (index >= Patterns.Count)
			{
				index = 0;
			}
			return Patterns[index].GetPattern();
		}
		
		public MoveHistory GetPattern()
		{
			MoveHistory ret = new MoveHistory(Move.ApplyActions(Cube.GetInitState(), Steps));

			Console.WriteLine("Set cube to pattern: " + Description);
			return ret;
		}
	}
}
