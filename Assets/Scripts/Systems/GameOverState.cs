using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace GameProgramming2D.State
{
	public class GameOverState : StateBase
	{
		public GameOverState()
		{
			State = StateType.GameOver;
			AddTransition ( TransitionType.GameOverToGame, StateType.Game );
			AddTransition ( TransitionType.GameOverToMenu, StateType.MainMenu );
		}

		public override void StateActivated ()
		{
			GameManager.Instance.SceneLoaded += HandleSceneLoaded;
			SceneManager.LoadScene ( 2 );
		}
	}
}
