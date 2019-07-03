namespace RubiksCube
{
	using System;

	public class CubiePosition
	{
		/*
         * For position:
         *  1. when a1=a2=a3, it is a center cubie
         *  2. when a1==a2 < a3, it is a edge cubie
         *  3. when a1<a3<a3, it is is a corner cubie
         *  */
		public CubiePosition(sbyte b1, sbyte b2, sbyte b3)
		{
			a1 = b1;
			a2 = b2;
			a3 = b3;

			AdjustOrder();
		}

		public sbyte a1, a2, a3;

		public void RotateFrontClockwise90Degree()
		{
			a1 = Face.RotateFrontClockwise90Degree(a1);
			a2 = Face.RotateFrontClockwise90Degree(a2);
			a3 = Face.RotateFrontClockwise90Degree(a3);

			AdjustOrder();
		}

		public void MapFrontFrom(sbyte side)
		{
			a1 = FaceLayout3D.MapFrontFrom(side, a1);
			a2 = FaceLayout3D.MapFrontFrom(side, a2);
			a3 = FaceLayout3D.MapFrontFrom(side, a3);

			AdjustOrder();
		}

		public void MapFrontTo(sbyte side)
		{
			a1 = FaceLayout3D.MapFrontTo(side, a1);
			a2 = FaceLayout3D.MapFrontTo(side, a2);
			a3 = FaceLayout3D.MapFrontTo(side, a3);

			AdjustOrder();
		}

		public CubiePosition Clone()
		{
			return new CubiePosition(a1, a2, a3);
		}

		public int CompareTo(CubiePosition p)
		{
			if (p.a1 != a1)
			{
				return a1 - p.a1;
			}

			if (p.a2 != a2)
			{
				return a2 - p.a2;
			}

			return a3 - p.a3;
		}

		public bool IsCenter()
		{
			return a1 == a2 && a2 == a3;
		}

		public bool IsEdge()
		{
			return (a1 == a2 || a2 == a3)
				&& (!IsCenter());
		}

		public bool IsCorner()
		{
			return a1 < a2 && a2 < a3;
		}

		public override string ToString()
		{
			return String.Format($"({a1},{a2},{a3})");
		}

		private void AdjustOrder()
		{
			//a1<=a3<=a3
			sbyte b1 = a1;
			sbyte b2 = a2;
			sbyte b3 = a3;

			a1 = Math.Min(b1, Math.Min(b2, b3));
			a3 = Math.Max(b1, Math.Max(b2, b3));

			a2 = (sbyte)(b1 + b2 + b3 - a1 - a3); //middle

			if (a2 == a3)
			{
				//only keep the smaller
				a2 = a1;
			}
		}
	}
}
