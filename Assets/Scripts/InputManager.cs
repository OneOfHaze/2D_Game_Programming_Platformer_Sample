using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameProgramming2D
{
	public class InputManager : MonoBehaviour
	{
		private void Update ()
		{
			if ( Input.GetKeyUp ( KeyCode.P ) )
			{
				// Pause game
				GameManager.Instance.Pauser.SetPauseOn ();
			}

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				GameManager.Instance.QuitGame ();
			}

			HandlePlayerInputs ();
		}

		private static void HandlePlayerInputs ()
		{
			if ( GameManager.Instance.Player == null )
			{
				return;
			}

			if ( Input.GetKeyDown ( KeyCode.S ) )
			{
				GameManager.Instance.Save ();
			}

			if ( Input.GetButtonDown ( "Jump" ) )
			{
				GameManager.Instance.Player.Jump = true;
			}

			if ( Input.GetButtonDown ( "Fire1" ) )
			{
				GameManager.Instance.Player.Gun.Fire ();
			}

			if ( Input.GetButtonDown ( "Fire2" ) )
			{
				GameManager.Instance.Player.LayBomb ();
			}

			var horizontal = Input.GetAxis ( "Horizontal" );
			GameManager.Instance.Player.SetHorizontal ( horizontal );
		}
	}
}
