using UnityEngine.SceneManagement;

namespace GameProgramming2D.State
{
	public class GameState : StateBase
	{
		public GameState () : base ()
		{
			State = StateType.Game;
			AddTransition ( TransitionType.GameToGameOver, StateType.GameOver );
		}

		public override void StateActivated ()
		{
			GameManager.Instance.SceneLoaded += HandleSceneLoaded;
			SceneManager.LoadScene ( 1 );
		}
	}
}
