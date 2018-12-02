using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rlc
{

    [CreateAssetMenu(fileName = "MusicTrack", menuName = "MusicTrackContainer")]
    public class MusicTrack : ScriptableObject
    {
        public int max_parallel_tracks = 2;
        public int min_parallel_tracks = 2;

        public AudioClip[] musicStems;

    }

}