namespace RubiksCube
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;


	public class Face
	{
		public Face(sbyte front, sbyte right, sbyte up, sbyte left, sbyte down)
		{
			//Each face has 3*3=9 cubies
			AddCubie(front, left, up); //left up corner cubie
			AddCubie(front, left, front); //up edge cubie
			AddCubie(front, left, down);
			AddCubie(front, front, up);
			AddCubie(front, front, front); //center cubie
			AddCubie(front, front, down);
			AddCubie(front, right, up);
			AddCubie(front, right, front);
			AddCubie(front, right, down);
		}

		private Face()
		{

		}

		public CubieColor this[sbyte a1, sbyte a2, sbyte a3]
		{
			get
			{
				CubiePosition position = new CubiePosition(a1, a2, a3);
				foreach (Cubie cubie in cubies)
				{
					if (cubie.Position.CompareTo(position) == 0)
					{
						return cubie.Color;
					}
				}
				throw new ApplicationException(String.Format($"Position {position} not found"));
			}

			set
			{
				CubiePosition position = new CubiePosition(a1, a2, a3);

				for (int i = 0; i < cubies.Count; i++)
				{
					Cubie cubie = cubies[i];
					if (cubie.Position.CompareTo(position) == 0)
					{
						cubie.Color = value;
						return;
					}
				}
				throw new ApplicationException(String.Format($"position {position} not found in set"));
			}
		}


		/*
         * A cube has six faces
         *   0 F = front face
         *   1 R = right face
         *   2 U = up face
         *   3 L = left face
         *   4 D = down face
         *   5 B = back face
         * */
		public const sbyte Front = 0;
		public const sbyte Right = 1;
		public const sbyte Up = 2;
		public const sbyte Left = 3;
		public const sbyte Down = 4;
		public const sbyte Back = 5;

		public static readonly Dictionary<int, string> FaceNames = new Dictionary<int, string>();		//{ "Front", "Right", "Up", "Left", "Down", "Back" };

		public static readonly Dictionary<sbyte, ConsoleColor.BackGroundColor> FaceColors = new Dictionary<sbyte, ConsoleColor.BackGroundColor>();

		public static readonly Dictionary<string, sbyte> ColorToFace = new Dictionary<string, sbyte>();

		internal List<Cubie> cubies = new List<Cubie>();

		//Colors on my cube

		static Face()
		{
			AddFaceColorMap(Front, "Front", ConsoleColor.BackGroundColor.Grey, "W");
			AddFaceColorMap(Right, "Right", ConsoleColor.BackGroundColor.Yellow, "Y");
			AddFaceColorMap(Up, "Up", ConsoleColor.BackGroundColor.Magenta, "P");
			AddFaceColorMap(Left, "Left", ConsoleColor.BackGroundColor.Green, "G");
			AddFaceColorMap(Down, "Down", ConsoleColor.BackGroundColor.Red, "R");
			AddFaceColorMap(Back, "Back", ConsoleColor.BackGroundColor.Blue, "B");
		}

		public static ConsoleColor.BackGroundColor GetColorForFace(sbyte face)
		{
			return FaceColors[face];
		}


		/*
         * If we rotate the front face clockwise, we'll change
         *          ___
         *         |   |
         *         | 2 |
         *      ___|___|___
         *     |   |   |   |
         *     | 3 | 0 | 1 |
         *     |___|___|___|
         *         |   |
         *         | 4 |
         *         |___|
         * into         
         *          ___
         *         |   |
         *         | 3 |
         *      ___|___|___
         *     |   |   |   |
         *     | 4 | 0 | 2 |
         *     |___|___|___|
         *         |   |
         *         | 1 |
         *         |___|
         * */
		public static sbyte RotateFrontClockwise90Degree(sbyte rotate)
		{
			switch (rotate)
			{
				case Face.Front:
					return (sbyte)Face.Front;
				case Face.Right:
					return (sbyte)Face.Down;
				case Face.Down:
					return (sbyte)Face.Left;
				case Face.Left:
					return (sbyte)Face.Up;
				case Face.Up:
					return (sbyte)Face.Right;
				case Face.Back:
					return Face.Back;
				default:
					throw new ApplicationException("Invalid input");
			}
		}

		//rotat 270degree clockwise is same as counter clockwise 90 degree
		public static sbyte RotateFrontCounterClockwise90Degree(sbyte rotate)
		{
			rotate = RotateFrontClockwise90Degree(rotate);
			rotate = RotateFrontClockwise90Degree(rotate);
			rotate = RotateFrontClockwise90Degree(rotate);

			return rotate;
		}

		public void ResetFromCubeReader(CubeReader rotate, FaceLayout2D postion, int times)
		{
			this[postion.Self, postion.Left, postion.Up] = FakeColor(rotate[times, 0, 0]);
			this[postion.Self, postion.Self, postion.Up] = FakeColor(rotate[times, 0, 1]);
			this[postion.Self, postion.Right, postion.Up] = FakeColor(rotate[times, 0, 2]);
			this[postion.Self, postion.Left, postion.Self] = FakeColor(rotate[times, 1, 0]);
			this[postion.Self, postion.Self, postion.Self] = FakeColor(rotate[times, 1, 1]);
			this[postion.Self, postion.Right, postion.Self] = FakeColor(rotate[times, 1, 2]);
			this[postion.Self, postion.Left, postion.Down] = FakeColor(rotate[times, 2, 0]);
			this[postion.Self, postion.Self, postion.Down] = FakeColor(rotate[times, 2, 1]);
			this[postion.Self, postion.Right, postion.Down] = FakeColor(rotate[times, 2, 2]);
		}

		public void RotateFrontClockwise90Degree()
		{
			foreach (var cubie in cubies)
			{
				cubie.Position.RotateFrontClockwise90Degree();
			}
		}

		public void MapFrontFrom(sbyte side)
		{
			foreach (var cubie in cubies)
			{
				cubie.Position.MapFrontFrom(side);
			}
		}

		public void MapFrontTo(sbyte side)
		{
			foreach (var cubie in cubies)
			{
				cubie.Position.MapFrontTo(side);
			}
		}

		public void Normalize()
		{
			cubies.Sort((x, y) => x.Position.CompareTo(y.Position));
		}

		public string[] GetFaceString(FaceLayout2D position)
		{
			string[] ret = new string[3];

			ret[0] = String.Format("{0}{1}{2}",
				this[position.Self, position.Left, position.Up],
				this[position.Self, position.Self, position.Up],
				this[position.Self, position.Right, position.Up]
				);

			ret[1] = String.Format("{0}{1}{2}",
				this[position.Self, position.Left, position.Self],
				this[position.Self, position.Self, position.Self],
				this[position.Self, position.Right, position.Self]
				);

			ret[2] = String.Format("{0}{1}{2}",
				this[position.Self, position.Left, position.Down],
				this[position.Self, position.Self, position.Down],
				this[position.Self, position.Right, position.Down]
				);

			return ret;
		}

		public sbyte Center(bool needPositon)
		{
			foreach (var cubie in cubies)
			{
				CubiePosition cube = cubie.Position;

				if (cube.IsCenter())
				{
					Debug.Assert(cubie.Color.IsCenter());
					if (needPositon)
					{
						return cubie.Position.a1;
					}
					else
					{
						return cubie.Color.Color;
					}
				}
			}

			throw new ApplicationException("No center found");
		}

		public int CompareTo(Face other)
		{
			for (int i = 0; i < cubies.Count; i++)
			{
				CubiePosition position = cubies[i].Position;

				int ret = cubies[i].Color.CompareTo(other[position.a1, position.a2, position.a3]);

				if (ret != 0)
				{
					return ret;
				}
			}

			return 0;
		}

		public Face Clone()
		{
			Face returned = new Face();

			foreach (var cubie in cubies)
			{
				returned.cubies.Add(new Cubie(cubie.Position.Clone(), cubie.Color.Clone()));
			}

			return returned;
		}

		private static void AddFaceColorMap(sbyte face, string faceName, ConsoleColor.BackGroundColor color, string colorName)
		{
			FaceNames.Add((int)face, faceName);

			FaceColors.Add(face, color);

			ColorToFace.Add(colorName, face);
		}

		private void AddCubie(sbyte side1, sbyte side2, sbyte side3)
		{
			cubies.Add(new Cubie(new CubiePosition(side1, side2, side3), new CubieColor(side1, side2, side3)));
		}

		private CubieColor FakeColor(sbyte fakeColor)
		{
			return new CubieColor(fakeColor, fakeColor, fakeColor);
		}
	}
}
