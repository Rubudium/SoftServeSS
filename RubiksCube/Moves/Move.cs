namespace RubiksCube
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;


	public class Move
	{
		private Move(string name, MoveFunction action)
		{
			this.Name = name;
			this.Action = action;
		}

		public string Name;

		MoveFunction Action;

		private static Dictionary<string, Move> allMoves;

		delegate void MoveFunction(Cube side);


		static void AddMove(string name, MoveFunction action)
		{
			allMoves.Add(name, new Move(name, action));
		}

		/**
         * http://en.wikipedia.org/wiki/Rubik's_Cube#Move_notation
         * F (Front): the side currently facing you 
         * B (Back): the side opposite the front 
         * U (Up): the side above or on top of the front side 
         * D (Down): the side opposite the top, underneath the Cube 
         * L (Left): the side directly to the left of the front 
         * R (Right): the side directly to the right of the front 
         * f (Front two layers): the side facing you and the corresponding middle layer 
         * b (Back two layers): the side opposite the front and the corresponding middle layer 
         * u (Up two layers) : the top side and the corresponding middle layer 
         * d (Down two layers) : the bottom layer and the corresponding middle layer 
         * l (Left two layers) : the side to the left of the front and the corresponding middle layer 
         * r (Right two layers) : the side to the right of the front and the corresponding middle layer 
         * x (rotate): rotate the Cube up 
         * y (rotate): rotate the Cube to the counter-clockwise 
         * z (rotate): rotate the Cube clockwise 
         * */
		static Move()
		{
			allMoves = new Dictionary<string, Move>();

			AddMove("F", s => RotateFront(s, 1));
			AddMove("F2", s => RotateFront(s, 2));
			AddMove("F'", s => RotateFront(s, 3));

			AddMove("L", s => RotateFace(s, 1, Face.Left));
			AddMove("L2", s => RotateFace(s, 2, Face.Left));
			AddMove("L'", s => RotateFace(s, 3, Face.Left));

			AddMove("U", s => RotateFace(s, 1, Face.Up));
			AddMove("U2", s => RotateFace(s, 2, Face.Up));
			AddMove("U'", s => RotateFace(s, 3, Face.Up));

			AddMove("R", s => RotateFace(s, 1, Face.Right));
			AddMove("R2", s => RotateFace(s, 2, Face.Right));
			AddMove("R'", s => RotateFace(s, 3, Face.Right));

			AddMove("D", s => RotateFace(s, 1, Face.Down));
			AddMove("D2", s => RotateFace(s, 2, Face.Down));
			AddMove("D'", s => RotateFace(s, 3, Face.Down));

			AddMove("B", s => RotateFace(s, 1, Face.Back));
			AddMove("B2", s => RotateFace(s, 2, Face.Back));
			AddMove("B'", s => RotateFace(s, 3, Face.Back));

			AddMove("f", s => { RotateFront(s, 1); RotateFrontMiddleLayer(s, 1); });
			AddMove("f2", s => { RotateFront(s, 2); RotateFrontMiddleLayer(s, 2); });
			AddMove("f'", s => { RotateFront(s, 3); RotateFrontMiddleLayer(s, 3); });

			AddMove("l", s => RotateFaceAndMiddleLayer(s, 1, Face.Left));
			AddMove("l2", s => RotateFaceAndMiddleLayer(s, 2, Face.Left));
			AddMove("l'", s => RotateFaceAndMiddleLayer(s, 3, Face.Left));

			AddMove("u", s => RotateFaceAndMiddleLayer(s, 1, Face.Up));
			AddMove("u2", s => RotateFaceAndMiddleLayer(s, 2, Face.Up));
			AddMove("u'", s => RotateFaceAndMiddleLayer(s, 3, Face.Up));

			AddMove("r", s => RotateFaceAndMiddleLayer(s, 1, Face.Right));
			AddMove("r2", s => RotateFaceAndMiddleLayer(s, 2, Face.Right));
			AddMove("r'", s => RotateFaceAndMiddleLayer(s, 3, Face.Right));

			AddMove("d", s => RotateFaceAndMiddleLayer(s, 1, Face.Down));
			AddMove("d2", s => RotateFaceAndMiddleLayer(s, 2, Face.Down));
			AddMove("d'", s => RotateFaceAndMiddleLayer(s, 3, Face.Down));

			AddMove("b", s => RotateFaceAndMiddleLayer(s, 1, Face.Back));
			AddMove("b2", s => RotateFaceAndMiddleLayer(s, 2, Face.Back));
			AddMove("b'", s => RotateFaceAndMiddleLayer(s, 3, Face.Back));

			AddMove("X", s => { RotateFace(s, 3, Face.Left); RotateFaceAndMiddleLayer(s, 1, Face.Right); });

			AddMove("Y", s => { RotateFace(s, 3, Face.Up); RotateFaceAndMiddleLayer(s, 1, Face.Down); });

			AddMove("Z", s => { RotateFace(s, 1, Face.Up); RotateFaceAndMiddleLayer(s, 3, Face.Down); });
		}

		public static Cube ApplyActions(Cube side, string[] actions)
		{
			side.Print();

			foreach (var action in actions)
			{
				if (!String.IsNullOrEmpty(action))
				{
					allMoves[action].Action(side);

					side.Step++;
					side.Path += action;

					side.Print();
				}
			}
			return side;
		}

		public static Move GetMove(string move)
		{
			return allMoves[move];
		}

		public static bool IsValidMove(string move)
		{
			return allMoves.ContainsKey(move);
		}

		

		   /** The moves are learned from
			* http://peter.stillhq.com/jasmine/rubikscubesolution.html
			* 
			* */

		public Cube ActOn(Cube side)
		{
			Cube returned = side.Clone();

			Action(returned);

			returned.Step++;
			returned.Path += Name;

			return returned;
		}

		private static void RotateFront(Cube s, int n)
		{
			Debug.Assert(n < 4);

			for (int i = 0; i < n; i++)
			{
				s.RotateFrontClockwise90Degree();
			}
		}

		private static void RotateFace(Cube side, int times, sbyte face)
		{
			Debug.Assert(times < 4);
			Debug.Assert(face < 6);

			side.MapFrontFrom(face);

			RotateFront(side, times);

			side.MapFrontTo(face);
		}

		private static void RotateFrontMiddleLayer(Cube side, int times)
		{
			Debug.Assert(times < 4);

			for (int i = 0; i < times; i++)
			{
				side.RotateFrontMiddleLayerClockwise90Degree();
			}
		}

		private static void RotateFaceMiddleLayer(Cube side, int times, sbyte face)
		{
			Debug.Assert(times < 4);
			Debug.Assert(face < 6);

			side.MapFrontFrom(face);

			RotateFrontMiddleLayer(side, times);

			side.MapFrontTo(face);
		}

		private static void RotateFaceAndMiddleLayer(Cube s, int n, sbyte face)
		{
			RotateFace(s, n, face);

			RotateFaceMiddleLayer(s, n, face);
		}
	}
}
