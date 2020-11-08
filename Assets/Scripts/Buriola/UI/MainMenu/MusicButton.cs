using Buriola.Data;
using UnityEngine;

namespace Buriola.UI.MainMenu
{
    public class MusicButton : MonoBehaviour
    {
        public Song SongReference;
        
        public void SelectSong()
        {
            GameController.SetSelectedSong(SongReference);
            GameController.Instance.LoadLevel("Game");
        }
    }
}
