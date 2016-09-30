using UnityEngine;
using GameProgramming2D.State;
using System.Collections;

namespace GameProgramming2D.GUI
{
    public class MainMenuGUI : MonoBehaviour
    {
        public void OnStartGamePressed()
        {
            GameManager.Instance.StateManager
                .PerformTransition ( TransitionType.MainMenuToGame );
        }

        public void OnExitGamePressed()
        {
            Application.Quit ();
        }
    }
}
