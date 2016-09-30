using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	private int score = 0;					// The player's score.


	private PlayerControl playerControl;	// Reference to the player control script.
	private GUIText _guiText;

	public int CurrentScore
	{
		get { return score; }
		set
		{
			score = value;
			_guiText.text = "Score: " + score;
			playerControl.StartCoroutine ( playerControl.Taunt () );
		}
	}


	void Awake ()
	{
		// Setting up the reference.
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
		_guiText = GetComponent<GUIText> ();
	}

}
