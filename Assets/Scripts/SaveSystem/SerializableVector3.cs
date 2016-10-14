using System;
using UnityEngine;

namespace GameProgramming2D
{
	/// <summary>
	/// Since we cannot serialize UnityEngine.Vector3 we need to create a serializable
	/// class for that.
	/// </summary>
	[Serializable]
	public class SerializableVector3
	{
		public float X, Y, Z;

		// Constructor which creates SerializableVector3 from Vector3
		public SerializableVector3(Vector3 vector)
		{
			X = vector.x;
			Y = vector.y;
			Z = vector.z;
		}

		// Constructor which creates SerializableVector3 from Vector2
		public SerializableVector3 ( Vector2 vector )
		{
			X = vector.x;
			Y = vector.y;
			Z = 0;
		}

		public static implicit operator SerializableVector3(Vector3 vector)
		{
			return new SerializableVector3 ( vector );
		}

		public static explicit operator Vector3(SerializableVector3 vector)
		{
			return new Vector3 ( vector.X, vector.Y, vector.Z );
		}

		public static implicit operator SerializableVector3 ( Vector2 vector )
		{
			return new SerializableVector3 ( vector );
		}

		public static explicit operator Vector2 ( SerializableVector3 vector )
		{
			return new Vector2 ( vector.X, vector.Y );
		}
	}
}
