using GameProgramming2D.State;
using UnityEngine;

namespace GameProgramming2D.GUI
{
	public class GUIManager : MonoBehaviour
	{
		// The dialog prefab from which we instantiate all our dialog objects.
		// Unitys prefab system implements the prototype pattern. See http://gameprogrammingpatterns.com/prototype.html
		[SerializeField] private Dialog _dialogPrefab;

		// The reference to a SceneGUI object. There should be one in every scene and that should be the parent
		// object of our gui.
		public SceneGUI SceneGUI { get; private set; }

		/// <summary>
		/// Initializes the GUIManager object.
		/// </summary>
		public void Init()
		{
			// Register to listen to StateManager.StateLoaded event. When fired,
			// call HandleStateLoaded method.
			GameManager.Instance.StateManager.StateLoaded += HandleStateLoaded;
			SceneGUI = FindObjectOfType< SceneGUI >(); // Find SceneGUI component also when game starts.
		}

		/// <summary>
		/// Called when GUIManager becomes inactive.
		/// </summary>
		void OnDisable()
		{
			// All event listeners should be unregistered at some point. Otherwise there is a risk of memory leaks
			// (garbace collection cannot destroy an object because there is a reference to it, it is referred from
			// the event).
			GameManager.Instance.StateManager.StateLoaded -= HandleStateLoaded;
		}

		/// <summary>
		/// Event handler for GameStateManager.StateLoaded. Executed when GameStateManager.StateLoaded is fired.
		/// </summary>
		/// <param name="type"></param>
		private void HandleStateLoaded( StateType type )
		{
			// Find the SceneGUI object from scene. If not found there is probably some error in scene.
			SceneGUI = FindObjectOfType< SceneGUI >();
			if ( SceneGUI == null )
			{
				Debug.LogWarning( "Could not find a SceneGUI component from loaded scene. Is this intentional?" );
			}
		}

		/// <summary>
		/// Creates a dialog, parents it properly and returns it to caller of this method.
		/// </summary>
		/// <returns></returns>
		public Dialog CreateDialog()
		{
			// Object.Instantiate is used to instantiate a prefab. Return the instantiated object as same type as the
			// prefab is.
			var dialog = Instantiate( _dialogPrefab );
			dialog.transform.SetParent( SceneGUI.transform );
			dialog.transform.localPosition = Vector3.zero;
			// Unity draws gui in order of local transform list. First child is drawn first and childen after that are drawn
			// on the top of it. We want to draw dialog on top of everything so we should set it as the last sibling in its
			// sibling hierarchy.
			dialog.transform.SetAsLastSibling();
			return dialog;
		}
	}
}
