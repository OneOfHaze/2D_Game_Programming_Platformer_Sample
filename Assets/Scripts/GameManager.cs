using UnityEngine;
using UnityEngine.SceneManagement;
using GameProgramming2D.State;
using System.Collections;
using System;

namespace GameProgramming2D
{
	public class GameManager : MonoBehaviour
	{
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<GameManager> ();
                }
                return _instance;
            }
        }

        private Pauser _pauser;
        private InputManager _inputManager;
        private PlayerControl _playerControl;

        public Pauser Pauser
        {
            get
            {
                return _pauser;
            }
        }

        public PlayerControl Player
        {
            get
            {
                if(_playerControl == null)
                {
                    _playerControl = FindObjectOfType<PlayerControl> ();
                }
                return _playerControl;
            }
        }

        public GameStateManager StateManager { get; private set; }

        private void Awake()
        {
            if(_instance == null)
            {
                DontDestroyOnLoad ( gameObject );
                _instance = this;
                Init ();
            }
            else if(_instance != this )
            {
                Destroy ( this );
            }
        }

        private void Init()
        {
            _pauser = gameObject.GetOrAddComponent<Pauser> ();
            _inputManager = gameObject.GetOrAddComponent<InputManager> ();
            _playerControl = FindObjectOfType<PlayerControl> ();
            InitGameStateManager ();
        }

        private void InitGameStateManager ()
        {
            StateManager = new GameStateManager ( new MenuState () );
            StateManager.AddState ( new GameState () );
        }

        public void Reload ()
        {
            // Application.LoadLevel(Application.loadedLevel);
            SceneManager.LoadScene ( SceneManager.GetActiveScene ().name );
        }
    }
}
