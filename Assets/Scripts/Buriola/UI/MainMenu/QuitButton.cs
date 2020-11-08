using UnityEngine;

namespace Buriola.UI.MainMenu
{
    public class QuitButton : MonoBehaviour
    {
        public void QuitGame()
        {
            GameController.Instance.QuitGame();
        }
    }
}
