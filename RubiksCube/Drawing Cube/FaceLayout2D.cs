namespace RubiksCube
{
	using System.Collections.Generic;

	//specify the surrounding neighbors of each Face
	//this is the description of 2D layout of a Cube
	public class FaceLayout2D
	{
		public FaceLayout2D(sbyte self, sbyte right, sbyte up, sbyte left, sbyte down)
		{
			this.Self = self;
			this.Right = right;
			this.Up = up;
			this.Left = left;
			this.Down = down;
		}

		public static Dictionary<sbyte, FaceLayout2D> Layout = new Dictionary<sbyte, FaceLayout2D>();

		public sbyte Self, Right, Up, Left, Down;
	
		static FaceLayout2D()
		{
			Layout.Add(Face.Up, new FaceLayout2D(Face.Up, Face.Right, Face.Back, Face.Left, Face.Front));
			Layout.Add(Face.Left, new FaceLayout2D(Face.Left, Face.Front, Face.Up, Face.Back, Face.Down));
			Layout.Add(Face.Front, new FaceLayout2D(Face.Front, Face.Right, Face.Up, Face.Left, Face.Down));
			Layout.Add(Face.Right, new FaceLayout2D(Face.Right, Face.Back, Face.Up, Face.Front, Face.Down));
			Layout.Add(Face.Down, new FaceLayout2D(Face.Down, Face.Right, Face.Front, Face.Left, Face.Back));
			Layout.Add(Face.Back, new FaceLayout2D(Face.Back, Face.Right, Face.Down, Face.Left, Face.Up));
		}
	}

}
