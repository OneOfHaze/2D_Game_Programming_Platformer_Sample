using UnityEngine.SceneManagement;

namespace GameProgramming2D.State
{
    public class MenuState : StateBase
    {
        public MenuState() : base()
        {
            State = StateType.MainMenu;
            AddTransition ( TransitionType.MainMenuToGame, StateType.Game );
        }

        public override void StateActivated ()
        {
            SceneManager.LoadScene ( 0 );
        }
    }
}
