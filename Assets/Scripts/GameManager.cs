using UnityEngine;
using UnityEngine.SceneManagement;
using GameProgramming2D.State;
using System.Collections;
using System;
using System.Collections.Generic;

namespace GameProgramming2D
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager _instance;

		public static GameManager Instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = FindObjectOfType<GameManager> ();
				}
				return _instance;
			}
		}

		public delegate void SceneLoadedDelegate ( int sceneIndex );
		public event SceneLoadedDelegate SceneLoaded;

		private Pauser _pauser;
		private InputManager _inputManager;
		private PlayerControl _playerControl;
		private List<Enemy> _enemies = new List<Enemy> ();

		[SerializeField]
		private Enemy _enemyWithShip;
		[SerializeField]
		private Enemy _enemyWithoutShip;

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
				if ( _playerControl == null )
				{
					_playerControl = FindObjectOfType<PlayerControl> ();
				}
				return _playerControl;
			}
		}

		public GameStateManager StateManager { get; private set; }

		private void Awake ()
		{
			if ( _instance == null )
			{
				// No instances of this singleton has been instantiated so far.
				// This will be our only instance.

				// Let's call DontDestroyOnLoad for this GameObject so it won't
				// get destroyed when new scene is loaded.
				DontDestroyOnLoad ( gameObject );
				_instance = this;
				Init ();
			}
			else if ( _instance != this )
			{
				// An instance has already been created. Let's destroy this one.
				Destroy ( this );
			}
		}

		protected void OnLevelWasLoaded(int levelIndex)
		{
			if(SceneLoaded != null)
			{
				SceneLoaded ( levelIndex );
			}
		}

		private void Init ()
		{
			// Let's get required references to other components.
			_pauser = gameObject.GetOrAddComponent<Pauser> ();
			_inputManager = gameObject.GetOrAddComponent<InputManager> ();
			_playerControl = FindObjectOfType<PlayerControl> ();
			InitGameStateManager ();
		}

		private void InitGameStateManager ()
		{
			// Creates GameStateManager and sets MenuState as game's initial state.
			StateManager = new GameStateManager ( new MenuState () );
			// Let's add all other states too.
			StateManager.AddState ( new GameState () );
			StateManager.AddState ( new GameOverState () );
		}

		public void Reload ()
		{
			StateManager.PerformTransition ( TransitionType.GameToGameOver );
			// Application.LoadLevel(Application.loadedLevel);
			//SceneManager.LoadScene ( SceneManager.GetActiveScene ().name );
		}

		public void Save()
		{
			// We collect all data we want to save to GameData object.
			GameData data = new GameData ();

			// Player's data we want to save.
			data.PlayerData = new PlayerData ();
			data.PlayerData.Health = Player.Health.health;
			data.PlayerData.FacingRight = Player.FacingRight;
			data.PlayerData.Position = Player.transform.position;
			data.PlayerData.Velocity = Player.Rigidbody.velocity;

			// This list contains all EnemyData objects.
			data.EnemyDatas = new List<EnemyData> ();

			foreach(Enemy enemy in _enemies)
			{
				// There is one EnemyData object for each enemy in our scene.
				EnemyData enemyData = new EnemyData ();
				enemyData.Health = enemy.HP;
				enemyData.Type = enemy.Type;
				enemyData.XScale = enemy.transform.localScale.x;
				enemyData.Position = enemy.transform.position;
				enemyData.Velocity = enemy.Rigidbody.velocity;
				data.EnemyDatas.Add ( enemyData );
			}

			Score score = GameObject.FindObjectOfType<Score> ();
			data.Score = score.CurrentScore;

			// Save the collected data to disk.
			SaveSystem.Save ( data );
		}

		public void AddEnemy ( Enemy enemy )
		{
			// Only add enemy to list if it isn't added already.
			if ( !_enemies.Contains ( enemy ) )
			{
				_enemies.Add ( enemy );
			}
		}

		public bool RemoveEnemy(Enemy enemy)
		{
			// Returns the result of removing (did it succeed or not)
			return _enemies.Remove ( enemy );
		}

		public void LoadGame()
		{
			StateManager.StateLoaded += HandleStateLoaded;
			StateManager.PerformTransition ( TransitionType.MainMenuToGame );
		}

		private void HandleStateLoaded ( StateType type )
		{
			StateManager.StateLoaded -= HandleStateLoaded;

			if(type == StateType.Game)
			{
				GameData data = SaveSystem.Load<GameData> ();

				var score = GameObject.FindObjectOfType<Score> ();
				score.CurrentScore = data.Score;

				Player.transform.position = (Vector3)data.PlayerData.Position;
				Player.FacingRight = data.PlayerData.FacingRight;
				var playerScale = Player.transform.localScale;
				playerScale.x *= Player.FacingRight ? 1 : -1; // ehto ? true : false;
				Player.transform.localScale = playerScale;

				Player.Health.health = data.PlayerData.Health;
				Player.Rigidbody.velocity = (Vector2) data.PlayerData.Velocity;

				foreach (var enemyData in data.EnemyDatas)
				{
					Enemy enemyPrefab = GetEnemyPrefab ( enemyData.Type );
					Enemy enemy = Instantiate ( enemyPrefab );
					enemy.transform.position = (Vector3)enemyData.Position;
					enemy.Rigidbody.velocity = (Vector2)enemyData.Velocity;
					enemy.HP = enemyData.Health;
					Vector3 enemyScale = enemy.transform.localScale;
					enemyScale.x = enemyData.XScale;
					enemy.transform.localScale = enemyScale;
				}
			}
		}

		private Enemy GetEnemyPrefab(Enemy.EnemyType type)
		{
			if(type == Enemy.EnemyType.WithoutShip)
			{
				return _enemyWithoutShip;
			}
			else
			{
				return _enemyWithShip;
			}
		}
	}
}
