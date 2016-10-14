using UnityEngine;
using System.Collections;

namespace GameProgramming2D.GUI
{

	public class GameOverGUI : SceneGUI
	{
		public void OnRestartGamePressed()
		{
			GameManager.Instance.StateManager.PerformTransition ( State.TransitionType.GameOverToGame );
		}

		public void OnToMainMenuPressed()
		{
			GameManager.Instance.StateManager.PerformTransition ( State.TransitionType.GameOverToMenu );
		}
	}
}
