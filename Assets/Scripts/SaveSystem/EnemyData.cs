using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProgramming2D
{
	[Serializable]
	public class EnemyData : UnitData
	{
		public int Health;
		public Enemy.EnemyType Type;
		public float XScale;
	}
}
