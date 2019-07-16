namespace RubiksCube
{
	using System;


	//Read your own cube from Console
	public class CubeReader
	{
		sbyte[,,] map = new sbyte[6, 3, 3];

		public sbyte this[int i, int j, int k]
		{
			get
			{
				return map[i, j, k];
			}
		}

		public Cube ReadCube()
		{
			string example =
				"g g w;r r r;b b r\n" + //0
				"r g g;w g g;y g g\n" + //1
				"b b b;b b b;r r y\n" + //2
				"y y w;y y y;y y r\n" + //3
				"g r b;w w w;w w w\n" + //4
				"p p p;p p p;p p p\n";  //5

			Console.WriteLine("Please input the colors of the cube, for example:\n"
				+ example);

			for (int i = 0; i < 6; i++)
			{
				bool isFine = false;

				while (!isFine)
				{
					isFine = true;

					Console.Write($"Face {Face.FaceNames[i]}:");

					string[] rows = Console.ReadLine().ToLower().Split(';');

					if (rows.Length != 3)
					{
						if (rows[0] == "exit" || rows[0] == "quit")
						{
							return null;
						}
						Console.WriteLine($"Expect 3 rows in {Face.FaceNames[i]}, actual rows = {rows.Length}");

						isFine = false;
						continue;
					}

					for (int j = 0; j < 3; j++)
					{
						string[] splitStr = rows[j].Trim().ToUpper().Split(' ');

						if (splitStr.Length != 3)
						{
							Console.WriteLine($"Expect 3 cubies in row {j}, actual cubie# = {splitStr.Length}, cubies = {rows[j]}");

							isFine = false;
							continue;
						}
						else
						{
							Console.WriteLine($"{splitStr[0]},{splitStr[1]},{splitStr[2]}");

							for (int k = 0; k < 3; k++)
							{
								string s = splitStr[k].Trim();

								if (!Face.ColorToFace.ContainsKey(s))
								{
									string[] validColors = new string[Face.ColorToFace.Keys.Count];

									Face.ColorToFace.Keys.CopyTo(validColors, 0);

									Console.WriteLine($"Column {k} [{s}] is not valid color. Valid colors are: {String.Join(",", validColors)}");

									isFine = false;
								}
								else
								{
									map[i, j, k] = Face.ColorToFace[s];
								}
							}
						}
					}
				}
			}

			return Cube.GetCubeFromReader(this);
		}
	}
}
