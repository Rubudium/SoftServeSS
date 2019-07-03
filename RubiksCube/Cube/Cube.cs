namespace RubiksCube
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	public class Cube
	{
		private Cube()
		{
			Step = 0;
			Faces = new Face[6];
		}

		Face[] Faces;

		public int Step;

		public string Path;

		public static Cube GetInitState()
		{
			Cube cube = new Cube();

			for (sbyte i = 0; i < 6; i++)
			{
				FaceLayout3D row = FaceLayout3D.Face3DLayouts[i];

				cube.Faces[i] = new Face(i, row[1], row[2], row[3], row[4]);
			}
			return cube;
		}

		public static Cube GetCubeFromReader(CubeReader reader)
		{
			Cube ret = GetInitState();

			ret.ResetFromReader(reader);

			return ret;
		}

		public void RotateFrontClockwise90Degree()
		{

			//rotate the front side
			Faces[Face.Front].RotateFrontClockwise90Degree();


			//For the right, up, left, and down side, we need to rotate one row that is close to the front side
			//we start at the up side. The three cubies are the [up, front, {left/front/right}]
			foreach (var side in new sbyte[] { Face.Left, Face.Front, Face.Right })
			{
				sbyte currentFace = Face.Up;

				sbyte currentPosition = side;

				CubieColor saved = Faces[currentFace][currentFace, Face.Front, currentPosition];

				for (int i = 0; i < 4; i++)
				{
					// go counter-clockwise to update each value
					sbyte nextFace = Face.RotateFrontCounterClockwise90Degree(currentFace);

					sbyte nextPosition = Face.RotateFrontCounterClockwise90Degree(currentPosition);

					if (i == 3)
					{
						Faces[currentFace][currentFace, Face.Front, currentPosition] = saved;
					}
					else
					{
						Faces[currentFace][currentFace, Face.Front, currentPosition] = Faces[nextFace][nextFace, Face.Front, nextPosition];
					}
					currentFace = nextFace;
					currentPosition = nextPosition;
				}
			}
		}

		public void RotateFrontMiddleLayerClockwise90Degree()
		{
			//For the right, up, left, and down side, we need to rotate one row that is close to the front side
			//we start at the up side. The three cubies are the [up, front, {left/front/right}]

			foreach (var side in new sbyte[] { Face.Left, Face.Up, Face.Right })
			{
				sbyte currentFace = Face.Up;

				sbyte currentPosition = side;

				CubieColor saved = Faces[currentFace][currentFace, currentFace, currentPosition];

				for (int i = 0; i < 4; i++)
				{
					// go counter-clockwise to update each position
					sbyte nextFace = Face.RotateFrontCounterClockwise90Degree(currentFace);

					sbyte nextPosition = Face.RotateFrontCounterClockwise90Degree(currentPosition);

					if (i == 3)
					{
						Faces[currentFace][currentFace, currentFace, currentPosition] = saved;
					}
					else
					{
						Faces[currentFace][currentFace, currentFace, currentPosition] = Faces[nextFace][nextFace, nextFace, nextPosition];
					}
					currentFace = nextFace;
					currentPosition = nextPosition;
				}
			}

			AdjustFaces();
		}

		public void MapFrontFrom(sbyte side)
		{
			Debug.Assert(side < 6);

			foreach (var face in Faces)
			{
				face.MapFrontFrom(side);
			}

			AdjustFaces();
		}

		public void MapFrontTo(sbyte side)
		{
			Debug.Assert(side < 6);

			foreach (var face in Faces)
			{
				face.MapFrontTo(side);
			}
			AdjustFaces();
		}

		public void Print()
		{
			System.Console.WriteLine($"\n\nStep = {Step}, Path = {Path}");

			//self, right, up, left, down
			Print3Faces(null, FaceLayout2D.Layout[Face.Up], null);

			Print3Faces(FaceLayout2D.Layout[Face.Left],
				FaceLayout2D.Layout[Face.Front],
				FaceLayout2D.Layout[Face.Right]
				);

			Print3Faces(null, FaceLayout2D.Layout[Face.Down], null);
			Print3Faces(null, FaceLayout2D.Layout[Face.Back], null);
		}


		public int CompareTo(Cube other)
		{
			int returned = 0;
			for (int i = 0; i < 6; i++)
			{
				returned = Faces[i].CompareTo(other.Faces[i]);
				if (returned != 0)
				{
					return returned;
				}
			}

			return 0;
		}

		//return two Cubes that are similar
		public bool Similar(Cube other)
		{
			Cube normalizeMe = Clone();
			normalizeMe.Normalize();
			Cube normalizeOther = other.Clone();
			normalizeOther.Normalize();
			return normalizeMe.CompareTo(normalizeOther) == 0;
		}

		public Cube ApplyActions(string[] actions)
		{
			Move.ApplyActions(this, actions);

			return this;
		}

		public Cube Clone()
		{
			Cube returned = new Cube();

			returned.Step = Step;
			returned.Path = Path;

			for (int i = 0; i < Faces.Length; i++)
			{
				returned.Faces[i] = Faces[i].Clone();
			}

			return returned;
		}

		private string[] GetFaceString(FaceLayout2D face)
		{
			if (face == null)
			{
				return new string[3] { "            ", "            ", "            " };
			}

			return Faces[face.Self].GetFaceString(face);
		}

		private void Print3Faces(FaceLayout2D left, FaceLayout2D center, FaceLayout2D right)
		{
			string[] side1 = GetFaceString(left);
			string[] side2 = GetFaceString(center);
			string[] side3 = GetFaceString(right);

			for (int i = 0; i < 3; i++)
			{
				//Console.WriteLine("{0}|{1}|{2}", s1[i], s2[i], s3[i]);
				bool isFirst = true;

				foreach (var side in new string[][] { side1, side2, side3 })
				{
					if (!isFirst)
					{
						Console.Write('|');
					}

					isFirst = false;

					string[] cells = side[i].Split(':');


					if (cells.Length == 1)
					{
						Console.Write($"{side[i]}");
					}
					else
					{
						foreach (var cell in cells)
						{
							if (cell.Trim().Length > 0)
							{
								int color = cell.Trim()[0] - '0';

								//map the color to what is on my cube
								color = (int)Face.GetColorForFace((sbyte)color);

								//set background color with intensity
								using (new ConsoleColor(color | (int)ConsoleColor.BackGroundColor.White))
								{
									Console.Write($"{cell}");
								}
								Console.Write(":");
							}
						}
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine("____________________________________");
		}

		private void ResetFromReader(CubeReader reader)
		{
			for (sbyte side = 0; side < 6; side++)
			{
				Face face = Faces[side];

				face.ResetFromCubeReader(reader, FaceLayout2D.Layout[side], side);
			}

			//adjust the neighbor's color
			for (sbyte side = 0; side < 6; side++)
			{
				Face face = Faces[side];

				foreach (Cubie cube in face.cubies)
				{
					CubiePosition position = cube.Position;

					List<sbyte> list = new List<sbyte>();

					list.Add(position.a1);
					list.Add(position.a2);
					list.Add(position.a3);

					list.Remove(side);

					sbyte a2 = list[0];
					sbyte a3 = list[1];

					cube.Color = new CubieColor(
						Faces[side][position.a1, position.a2, position.a3].Color,
						Faces[a2][position.a1, position.a2, position.a3].Color,
						Faces[a3][position.a1, position.a2, position.a3].Color
						);
				}
			}
		}

		private void Normalize()
		{
			sbyte FaceIndexWithColor0 = GetFaceIndexForColor(Face.Front);
			//Make 0 as the color which is in faces[0]

			MapFrontFrom(FaceIndexWithColor0);
			RotateRightFaceToRight();
			foreach (var face in Faces)
			{
				face.Normalize();
			}
		}

		private void RotateRightFaceToRight()
		{
			sbyte FaceIndexWithColor1 = GetFaceIndexForColor(Face.Right);
			//Make 1 as the color which is in faces[1]
			if (FaceIndexWithColor1 == Face.Right)
			{
				return;
			}

			//Rotate the cube so that the color 'Face.Right' center is facing right
			foreach (var face in Faces)
			{
				for (sbyte side = FaceIndexWithColor1; side != Face.Right; side = Face.RotateFrontClockwise90Degree(side))
				{
					face.RotateFrontClockwise90Degree();
				}
			}

			AdjustFaces();
		}

		private void AdjustFaces()
		{
			//faces[i].centercolor = i
			Face[] backup = (Face[])Faces.Clone();

			foreach (var face in backup)
			{
				Faces[face.Center(true)] = face;
			}
		}

		private sbyte GetFaceIndexForColor(sbyte color)
		{
			sbyte i = 0;

			for (i = 0; i < 6; i++)
			{
				if (Faces[i].Center(false) == color)
				{
					return i;
				}
			}

			throw new ApplicationException(String.Format($"Color {color} is not found"));
		}
	}
}
