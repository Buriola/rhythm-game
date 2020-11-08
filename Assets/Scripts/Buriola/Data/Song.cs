using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.Data
{
	[CreateAssetMenu(menuName = "Create Song", fileName = "New Song Asset")]
	public class Song : ScriptableObject
	{
		[FormerlySerializedAs("songName")] 
		public string SongName;
		[FormerlySerializedAs("artistName")] 
		public string ArtistName;
		[FormerlySerializedAs("musicFile")] 
		public AudioClip MusicFile;
		[FormerlySerializedAs("chartFile")] 
		public TextAsset ChartFile;

	}
}
