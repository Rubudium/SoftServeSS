namespace RubiksCube
{
	public class FaceLayout3D
	{

		private FaceLayout3D(sbyte a1, sbyte a2, sbyte a3, sbyte a4, sbyte a5, sbyte a6)
		{
			order = new sbyte[6] { a1, a2, a3, a4, a5, a6 };

			reverseOrder = new sbyte[6];

			for (sbyte i = 0; i < 6; i++)
			{
				for (sbyte j = 0; j < 6; j++)
				{
					if (order[j] == i)
					{
						reverseOrder[i] = j;
					}
				}
			}
		}

		public static FaceLayout3D[] Face3DLayouts;

		sbyte[] order;

		sbyte[] reverseOrder;

		static FaceLayout3D()
		{
			/* A cube has six faces, lets name them 0-5
             *   0 F = front face
             *   1 R = right face
             *   2 U = up face
             *   3 L = left face
             *   4 D = down face
             *   5 B = back face
             *                ___
             *               /2 /|
             *              /__/ | 5
             *             |   |1|   
             *           3 | 0 | / 
             *             |___|/
             *               4  
             * If we open them into 2 dimensions:
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
             *         |   |        
             *         | 5 |
             *         |___|
             * 
             * If we change the front side and right side, we will get these results:
             *                  
             *        ___              ___             ___      
             *       |   |            |   |           |   |
             *       | 0 |            | 0 |           | 0 |
             *    ___|___|___      ___|___|___     ___|___|___
             *   |   |   |   |    |   |   |   |   |   |   |   |
             *   | 4 | 1 | 2 |    | 1 | 2 | 3 |   | 2 | 3 | 4 |
             *   |___|___|___|    |___|___|___|   |___|___|___|
             *       |   |            |   |           |   |
             *       | 5 |            | 5 |           | 5 |
             *       |___|            |___|           |___|
             *       |   |            |   |           |   |       
             *       | 3 |            | 4 |           | 1 |
             *       |___|            |___|           |___|
             * 
             *        ___                  ___
             *       |   |                |   |
             *       | 0 |                | 4 |
             *    ___|___|___          ___|___|___
             *   |   |   |   |        |   |   |   |
             *   | 3 | 4 | 1 |        | 3 | 5 | 1 |
             *   |___|___|___|        |___|___|___|
             *       |   |                |   |
             *       | 5 |                | 2 |
             *       |___|                |___|
             *       |   |                |   |      
             *       | 2 |                | 0 |
             *       |___|                |___|
             * 
             */
			//we'll record the above result in a mattrix:
			Face3DLayouts = new FaceLayout3D[6];

			Face3DLayouts[0] = new FaceLayout3D(0, 1, 2, 3, 4, 5);
			Face3DLayouts[1] = new FaceLayout3D(1, 2, 0, 4, 5, 3);
			Face3DLayouts[2] = new FaceLayout3D(2, 3, 0, 1, 5, 4);
			Face3DLayouts[3] = new FaceLayout3D(3, 4, 0, 2, 5, 1);
			Face3DLayouts[4] = new FaceLayout3D(4, 1, 0, 3, 5, 2);
			Face3DLayouts[5] = new FaceLayout3D(5, 1, 4, 3, 2, 0);
			//Note, the above matrix is not symetric. 
			//If we number the faces in a symetic way, the matrix would look more regular: 0->0, 1->1, 2->2, 3->4, 4->5, 5>3
		}

		public static sbyte MapFrontFrom(sbyte side, sbyte facelet)
		{
			FaceLayout3D refference = Face3DLayouts[side];
			// order[i] - > i
			return refference.reverseOrder[facelet];
		}

		public static sbyte MapFrontTo(sbyte side, sbyte facelet)
		{
			FaceLayout3D refference = Face3DLayouts[side];
			// i - > order[i]
			return refference.order[facelet];
		}	

		public sbyte this[int index]
		{
			get
			{
				return order[index];
			}
		}
	}
}
