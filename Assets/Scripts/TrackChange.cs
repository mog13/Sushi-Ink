using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TrackChange : MonoBehaviour
    {
        public int track = 0;

        private void Start()
        {
            BGMusicController.Instance.PlayTrack(track);
        }
    }
}