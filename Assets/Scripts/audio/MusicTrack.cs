using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    [CreateAssetMenu(fileName = "MusicTrack", menuName = "MusicTrackContainer")]
    public class MusicTrack : ScriptableObject
    {
        public AudioClip[] musicStems;

    }

}