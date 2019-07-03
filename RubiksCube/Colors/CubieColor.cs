namespace RubiksCube
{
	using System;
	using System.Diagnostics;


	public class CubieColor
	{
		/*
         * For position:
         *  1. when a1=a2=a3, it is a center cubie
         *  2. when a1==a2 < a3, it is a edge cubie
         *  3. when a1<a3<a3, it is is a corner cubie
         *  */
		public CubieColor(sbyte b1, sbyte b2, sbyte b3)
		{
			Color = b1;

			NeighborColor1 = Math.Min(b2, b3);
			NeighborColor2 = Math.Max(b2, b3);

			if (IsEdge())
			{
				//if this is edge facelet, keep neighborColor1 smaller of the two colors
				//this hels compare two CubieColor
				if (NeighborColor2 == Color)
				{
					Debug.Assert(NeighborColor2 != NeighborColor1 && NeighborColor1 != Color);
					NeighborColor2 = NeighborColor1;
				}

				NeighborColor1 = Math.Min(Color, NeighborColor2);
			}
		}

		public sbyte Color, NeighborColor1, NeighborColor2;

		public CubieColor Clone()
		{
			return new CubieColor(Color, NeighborColor1, NeighborColor2);
		}

		public int CompareTo(CubieColor positionColor)
		{
			if (positionColor.Color != Color)
			{
				return Color - positionColor.Color;
			}

			if (positionColor.NeighborColor1 != NeighborColor1)
			{
				return NeighborColor1 - positionColor.NeighborColor1;
			}

			return NeighborColor2 - positionColor.NeighborColor2;
		}

		public bool IsCenter()
		{
			return Color == NeighborColor1 && NeighborColor1 == NeighborColor2;
		}

		public bool IsCorner()
		{
			return Color != NeighborColor1 &&
				Color != NeighborColor2 &&
				NeighborColor1 < NeighborColor2;
		}

		public bool IsEdge()
		{
			return !IsCorner() && !IsCenter();
		}

		public override string ToString()
		{
			if (IsCenter())
			{
				return String.Format($" {Color} :");
			}
			if (IsEdge())
			{
				return String.Format($"{Color}{(Color == NeighborColor1 ? NeighborColor2 : NeighborColor1)} :");
			}

			Debug.Assert(IsCorner());

			return String.Format($"{Color}{NeighborColor1}{NeighborColor2}:");
		}
	}
}
