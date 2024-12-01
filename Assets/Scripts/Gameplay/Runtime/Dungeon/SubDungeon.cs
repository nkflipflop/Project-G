using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Runtime.Dungeon
{
	[System.Serializable]
	public class SubDungeon
	{
		public SubDungeon left, right;
		public Rect rect;
		public Rect room = new(-1, -1, 0, 0); // null
		public Rect removedPiece = new(-1, -1, 0, 0); // null
		public List<Rect> corridors = new();
		public List<Rect> bridges = new();
		public int debugId;
		public bool hasTurret;

		private static int debugCounter = 0;

		public SubDungeon(Rect rect)
		{
			this.rect = rect;
			debugId = debugCounter;
			debugCounter++;
			hasTurret = false;
		}

		public void CreateRoom()
		{
			left?.CreateRoom();
			right?.CreateRoom();
			if (left != null && right != null)
			{
				CreateCorridorBetween(left, right);
			}

			if (IAmLeaf())
			{
				int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
				int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
				int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
				int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

				// room position will be absolute in the board, not relative to the sub-deungeon
				room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);

				int shouldEditRoom = Random.Range(0, 4); // 25% chance for editing the shape of the room
				if (shouldEditRoom == 0)
				{
					removedPiece = GetPieceToRemove();
				}
			}
		}

		private Rect GetPieceToRemove()
		{
			int x = 0, y = 0, xMax = 0, yMax = 0;
			int randWidth = Random.Range(1, (int)(room.width / 2));
			int randHeight = Random.Range(1, (int)(room.height / 2));
			int randCorner = Random.Range(0, 4);
			switch (randCorner)
			{
				case 0: // bottom left
					x = (int)room.x;
					y = (int)room.y;
					break;
				case 1: // bottom right
					x = (int)room.xMax - randWidth;
					y = (int)room.y;
					break;
				case 2: // top left
					x = (int)room.x;
					y = (int)room.yMax - randHeight;
					break;
				case 3: // top right
					x = (int)room.xMax - randWidth;
					y = (int)room.yMax - randHeight;
					break;
			}

			xMax = x + randWidth;
			if (xMax > room.xMax)
			{
				xMax = (int)room.xMax;
			}
			
			yMax = y + randHeight;
			if (yMax > room.yMax)
			{
				yMax = (int)room.yMax;
			}

			return new Rect(x, y, xMax - x, yMax - y);
		}

		private void CreateCorridorBetween(SubDungeon left, SubDungeon right)
		{
			Rect leftRoom = left.GetRoom();
			Rect rightRoom = right.GetRoom();

			//Debug.Log("Creating corridor(s) between " + left.debugId + "(" + lroom + ") and " + right.debugId + " (" + rroom + ")");

			// attach the corridor to a random point in each room
			Vector2 leftPoint, rightPoint;

			do
			{
				leftPoint = new Vector2((int)Random.Range(leftRoom.x + 1, leftRoom.xMax - 1),
					(int)Random.Range(leftRoom.y + 1, leftRoom.yMax - 1));
			} while (leftPoint.x >= left.removedPiece.x && leftPoint.x <= left.removedPiece.xMax &&
					 leftPoint.y >= left.removedPiece.y && leftPoint.y <= left.removedPiece.yMax);

			do
			{
				rightPoint = new Vector2((int)Random.Range(rightRoom.x + 1, rightRoom.xMax - 1),
					(int)Random.Range(rightRoom.y + 1, rightRoom.yMax - 1));
			} while (rightPoint.x >= right.removedPiece.x && rightPoint.x <= right.removedPiece.xMax &&
					 rightPoint.y >= right.removedPiece.y && rightPoint.y <= right.removedPiece.yMax);

			// always be sure that left point is on the left to simplyfy code
			if (leftPoint.x > rightPoint.x)
			{
				(leftPoint, rightPoint) = (rightPoint, leftPoint);
			}

			int w = (int)(leftPoint.x - rightPoint.x);
			int h = (int)(leftPoint.y - rightPoint.y);

			int thickness = Random.Range(1, 4); // getting a random thickness
			List<Rect> connections = (thickness > 1) ? corridors : bridges; // if the tickness > 1, it is a corridor; otherwise, it's a bridge

			//Debug.Log("lpoint: " + lpoint + ", rpoint: " + rpoint + ", w: " + w + ", h: " + h);

			// if the points are not aligned horizontally
			if (w != 0)
			{
				if (Random.Range(0, 2) > 0)
				{
					// add a corridor to the right
					connections.Add(new Rect(leftPoint.x, leftPoint.y, Mathf.Abs(w) + 1, thickness));

					// if left point is below right point go up otherwise go down
					connections.Add(h < 0
						? new Rect(rightPoint.x, leftPoint.y, thickness, Mathf.Abs(h))
						: new Rect(rightPoint.x, rightPoint.y, thickness, Mathf.Abs(h)));
				}
				else
				{
					// go up or down
					connections.Add(h < 0
						? new Rect(leftPoint.x, leftPoint.y, thickness, Mathf.Abs(h))
						: new Rect(leftPoint.x, rightPoint.y, thickness, Mathf.Abs(h)));

					// then go right
					connections.Add(new Rect(leftPoint.x, rightPoint.y, Mathf.Abs(w) + 1, thickness));
				}
			}
			else
			{
				// if the points are aligned horizontally go up or down depending on the positions
				connections.Add(h < 0
					? new Rect((int)leftPoint.x, (int)leftPoint.y, thickness, Mathf.Abs(h))
					: new Rect((int)rightPoint.x, (int)rightPoint.y, thickness, Mathf.Abs(h)));
			}
		}

		private Rect GetRoom()
		{
			if (IAmLeaf())
			{
				return room;
			}

			if (left != null)
			{
				Rect leftRoom = left.GetRoom();
				if (leftRoom.x != -1)
				{
					return leftRoom;
				}
			}

			if (right != null)
			{
				Rect rightRoom = right.GetRoom();
				if (rightRoom.x != -1)
				{
					return rightRoom;
				}
			}

			// workaround non nullable structs
			return new Rect(-1, -1, 0, 0);
		}

		public bool IAmLeaf() => left == null && right == null;

		/* choose a vertical or horizontal split depending on the proportion
		i.e. if too wide split vertically, or too long horizontally,
		or if nearly square choose vertical or horizontal at random */
		public bool Split(int minRoomSize, int maxRoomSize)
		{
			if (!IAmLeaf())
			{
				return false;
			}

			bool splitH;
			if (rect.width / rect.height >= 1.25)
			{
				splitH = false;
			}
			else if (rect.height / rect.width >= 1.25)
			{
				splitH = true;
			}
			else
			{
				splitH = Random.Range(0.0f, 1.0f) > 0.5;
			}

			if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
			{
				return false;
			}

			if (splitH)
			{
				// split so that the resulting sub-dungeons widths are not too small
				// (since we are splitting horizontally)
				int split = Random.Range(minRoomSize, (int)(rect.width - minRoomSize));

				left = new SubDungeon(new Rect(rect.x, rect.y, rect.width, split));
				right = new SubDungeon(new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
			}
			else
			{
				int split = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));

				left = new SubDungeon(new Rect(rect.x, rect.y, split, rect.height));
				right = new SubDungeon(new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
			}

			return true;
		}
	}
}