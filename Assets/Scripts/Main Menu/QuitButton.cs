using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class for a button
/// </summary>
public class QuitButton : MonoBehaviour
{
    /// <summary>
    /// Quits application
    /// </summary>
	public void QuitGame()
    {
        GameController.Instance.QuitGame();
    }
}
