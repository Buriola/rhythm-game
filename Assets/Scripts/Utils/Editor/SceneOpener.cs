using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneOpener
{
    private static string scenePath = Application.dataPath + "/Scenes/";

    [MenuItem("Rhythm Game/Scenes/0: Main Menu")]
	private static void OpenMainMenu()
    {
        EditorSceneManager.OpenScene(scenePath + "MainMenu.unity", OpenSceneMode.Single);
    }

    [MenuItem("Rhythm Game/Scenes/1: Game")]
    private static void OpenGameScene()
    {
        EditorSceneManager.OpenScene(scenePath + "Game.unity", OpenSceneMode.Single);
    }
}
