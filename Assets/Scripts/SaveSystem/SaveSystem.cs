using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameProgramming2D
{
	public static class SaveSystem
	{
		private const string SaveFileName = "save.dat";

		public static string SaveFilePath
		{
			get
			{
				return Path.Combine ( Application.persistentDataPath, SaveFileName );
			}
		}

		public static void Save ( object objectToSave )
		{
			// Binary formatter serializes data into binary which can be stored to disk.
			BinaryFormatter bf = new BinaryFormatter ();
			// Binary formatter stores serialization result into stream so let's create a
			// memory stream for that purpose.
			MemoryStream ms = new MemoryStream ();

			// BimaryFormatter.Serilaize method actually serializes the object. Result is
			// stored to ms Stream.
			bf.Serialize ( ms, objectToSave );

			// File.WriteAllBytes writes serialized bytes into a file. Bytes can be
			// acquired from stream by calling its GetBuffer method.
			File.WriteAllBytes ( SaveFilePath, ms.GetBuffer () );
		}

		public static T Load<T> () where T : class
		{
			// We can load file only if it exists.
			if ( File.Exists ( SaveFilePath ) )
			{
				// File.ReadAllBytes reads bytes from a file and returns them as a byte array.
				byte[] data = File.ReadAllBytes ( SaveFilePath );
				// Since we used BinaryFormatter to serialize object, we must use it also to
				// deserialize it.
				BinaryFormatter bf = new BinaryFormatter ();
				// Lets create a MemoryStream which contains our serialized bytes
				MemoryStream ms = new MemoryStream ( data );
				object saveData = bf.Deserialize ( ms );

				return (T)saveData;
			}

			return default ( T );
		}

		public static bool DoesSaveExist ()
		{
			return File.Exists ( SaveFilePath );
		}
	}
}
