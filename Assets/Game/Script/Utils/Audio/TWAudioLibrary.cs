/* TWAudio version 3.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using System.Linq;
using UnityEngine;

namespace TomWill
{
    public class TWAudioLibrary : MonoBehaviour
    {
        [SerializeField] private ClipDetail[] bgmClips;
        [SerializeField] private ClipDetail[] sfxClips;

        public ClipDetail GetBGMClip(string name)
        {
            if (bgmClips.Any(e => e.name == name))
            {
                return bgmClips.First(e => e.name == name);
            }

            return null;
        }

        public ClipDetail GetBGMClip(AudioClip clip)
        {
            if (bgmClips.Any(e => e.clip == clip))
            {
                return bgmClips.First(e => e.clip == clip);
            }

            return null;
        }

        public ClipDetail GetSFXClip(string name)
        {
            if (sfxClips.Any(e => e.name == name))
            {
                return sfxClips.First(e => e.name == name);
            }

            return null;
        }
    }

    [System.Serializable]
    public class ClipDetail
    {
        public string name;
        [Range(0f, 1f)] public float localVolume = 1;
        public AudioClip clip;
    }
}
