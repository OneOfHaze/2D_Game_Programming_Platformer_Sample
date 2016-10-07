using System;
using System.Collections.Generic;

namespace GameProgramming2D
{
	[Serializable]
	public class GameData
	{
		public int Score;
		public PlayerData PlayerData;
		public List<EnemyData> EnemyDatas;
	}
}
