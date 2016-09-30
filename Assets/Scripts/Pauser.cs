using UnityEngine;
using System.Collections;
using System;

public class Pauser : MonoBehaviour {
	private bool paused = false;

    public void TogglePause ()
    {
        paused = !paused;
        if ( paused )
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
