using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicTrack", menuName = "MusicTrackContainer")]
public class MusicTrack : ScriptableObject {
	public float BPM;
	public AudioClip[] musicStems;
	
}
