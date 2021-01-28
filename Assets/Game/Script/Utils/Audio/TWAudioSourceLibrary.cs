/* TWAudio version 3.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TomWill
{
    public class TWAudioSourceLibrary : MonoBehaviour
    {
        List<TWAudioSource> audioSources;

        public void Init()
        {
            audioSources = new List<TWAudioSource>();
        }

        public (AudioSource audio, AudioLoopCallbacks callback) GetAudio(string audioSourceName)
        {
            if (audioSources == null) Init();

            if (audioSources.Any(e => e.audioSourceName == audioSourceName))
            {
                TWAudioSource dataSource = audioSources.First(e => e.audioSourceName == audioSourceName);
                return (dataSource.audio, dataSource.callback);
            }
            else
            {
                return (null, null);
            }
        }

        public void SetAudio(string audioSourceName, AudioSource source, AudioLoopCallbacks callback = null)
        {
            if (audioSources == null) Init();

            if (!audioSources.Any(e => e.audioSourceName == audioSourceName) && source != null)
            {
                audioSources.Add(new TWAudioSource(audioSourceName, source, callback));
            }
        }
    }

    public class TWAudioSource
    {
        public string audioSourceName;
        public AudioSource audio;
        public AudioLoopCallbacks callback;

        public TWAudioSource(string audioSourceName, AudioSource audio, AudioLoopCallbacks callback)
        {
            this.audioSourceName = audioSourceName;
            this.audio = audio;
            this.callback = callback;
        }
    }
}