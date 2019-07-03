namespace RubiksCube
{
	public class Cubie
	{
		public Cubie(CubiePosition position, CubieColor color)
		{
			this.Position = position;
			this.Color = color;
		}

		public CubiePosition Position;

		public CubieColor Color;
	}
}
