using UnityEngine.SceneManagement;

namespace GameProgramming2D.State
{
    public class GameState : StateBase
    {
        public GameState() : base()
        {
            State = StateType.Game;
            AddTransition ( TransitionType.GameToGameOver, StateType.GameOver );
        }

        public override void StateActivated ()
        {
            SceneManager.LoadScene ( 1 );
        }
    }
}
