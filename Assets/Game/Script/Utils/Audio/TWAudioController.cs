/* TWAudio version 3.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using DG.Tweening;
using Fungus;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TomWill
{
    public class TWAudioController : MonoBehaviour
    {
        public enum PlayType
        {
            TRANSITION,
            DEFAULT
        }

        private static TWAudioController instance;
        private TWAudioLibrary audioLibrary;
        private TWAudioSourceLibrary audioSources;
        private GameObject audioParent;
        private GameObject prefab;
        private float timeFade;
        private float globalSFXVolume = 1;
        private float globalBGMVolume = 1;

        private AudioLoop audioLoopFunction;

        #region Setting AudioController
        public static TWAudioController Instance
        {
            get
            {
                if (instance == null)
                {
                    // Create a variable AudioController and call CreateInstance method directly from that class
                    Debug.Log("Please call CreateInstance method before using this Controller");
                }

                return instance;
            }
        }

        public void CreateInstance()
        {
            createInstance();
        }

        public static void SetupAudioLibrary([NotNull] TWAudioLibrary library, TWAudioSourceLibrary audioSources, GameObject audioParent)
        {
            // Only support 2 channel for fading
            Instance?.setupAudioController(library, audioSources, audioParent);
            Instance?.SetTimeFade();
        }

        public void SetTimeFade(float time = 2)
        {
            Instance?.setTimeFade(time);
        }
        #endregion

        #region Public Function
        public static void PlayBGM(string audioSource, string musicName, PlayType playType = PlayType.TRANSITION, bool loop = true)
        {
            Instance?.playBGM(audioSource, musicName, playType, loop);
        }

        public static void PlayBGMAdvanced(string audioSource, string musicName, AudioLoop.LoopType loopType, float start, float end ,PlayType playType = PlayType.TRANSITION)
        {
            Instance?.playBGMAdvance(audioSource, musicName, loopType, start, end, playType);
        }

        public static void StopBGMPlayed(string audioSource, bool immediatly = true)
        {
            Instance?.stopBGMPlayed(audioSource, immediatly);
        }

        public static void PlaySFX(string audioSource, string musicName)
        {
            Instance.playSFX(audioSource, musicName);
        }

        public static float AudioLength(string name, string type)
        {
            return Instance ? Instance.getAudioLength(name, type) : -1;
        }
        #endregion

        #region Internal Function
        private float getAudioLength(string name, string type)
        {
            if (type.Equals("SFX")) return audioLibrary.GetSFXClip(name).clip.length;
            else if (type.Equals("BGM")) return audioLibrary.GetBGMClip(name).clip.length;
            else return -1;
        }

        private void createInstance()
        {
            if (instance == null)
            {
                instance = this;
                prefab = Resources.Load("audiosource") as GameObject;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void playBGM(string audioSource, string musicName, PlayType playType, bool loop)
        {
            switch (playType)
            {
                case PlayType.TRANSITION:
                    playBGMTransition(audioSource, musicName, loop);
                    break;
                case PlayType.DEFAULT:
                    playBGMDefault(audioSource, musicName, loop);
                    break;
            }
        }

        private void playBGMAdvance(string audioSource, string musicName, AudioLoop.LoopType loopType, float start, float end, PlayType playType)
        {
            switch (playType)
            {
                case PlayType.TRANSITION:
                    playBGMTransitionAdvance(audioSource, musicName, loopType, start, end);
                    break;
                case PlayType.DEFAULT:
                    playBGMDefaultAdvance(audioSource, musicName, loopType, start, end);
                    break;
            }
        }

        private void playSFX(string audioSource, string musicName)
        {
            AudioSource audio = audioSources.GetAudio(audioSource).audio;

            if (!audio)
            {
                var source = setNewAudioSource(audioSource, false);
                audio = source.audio;
            }

            ClipDetail clipDetail = audioLibrary.GetSFXClip(musicName);
            audio.PlayOneShot(clipDetail.clip, globalSFXVolume * clipDetail.localVolume);
        }

        private void playBGMTransition(string audioSource, string musicName, bool loop)
        {
            var audioSourceData = audioSources.GetAudio(audioSource);
            AudioSource audio = audioSourceData.audio;
            AudioLoopCallbacks callback = audioSourceData.callback;

            if (!audio)
            {
                var source = setNewAudioSource(audioSource, false);
                audio = source.audio;
                callback = source.callback;
            }

            if (audio.isPlaying)
            {
                ClipDetail clipDetail = audioLibrary.GetBGMClip(musicName);
                DOVirtual.Float(1, 0, timeFade, (x) =>
                {
                    audio.volume = globalBGMVolume * clipDetail.localVolume * x;
                }).OnComplete(() => playBGMInTransition(audio, musicName, loop));
            } else
            {
                playBGMInTransition(audio, musicName, loop);
            }
        }

        private void playBGMInTransition(AudioSource audio, string musicName, bool loop)
        {
            ClipDetail clipDetail = audioLibrary.GetBGMClip(musicName);
            audio.loop = loop;
            audio.clip = clipDetail.clip;
            audio.volume = 0;
            audio.Play();
            DOVirtual.Float(0, 1, timeFade, (x) =>
            {
                audio.volume = globalBGMVolume * clipDetail.localVolume * x;
            });
        }

        private void playBGMDefault(string audioSource, string musicName, bool loop)
        {
            var audioSourceData = audioSources.GetAudio(audioSource);
            AudioSource audio = audioSourceData.audio;
            AudioLoopCallbacks callback = audioSourceData.callback;

            ClipDetail clipDetail = audioLibrary.GetBGMClip(musicName);

            if (!audio)
            {
                var source = setNewAudioSource(audioSource, false);
                audio = source.audio;
                callback = source.callback;
            }

            if (audio.isPlaying)
            {
                stopBGMPlayed(audio, clipDetail, callback, true);
            }

            audio.loop = loop;
            audio.clip = clipDetail.clip;
            audio.volume = globalBGMVolume * clipDetail.localVolume;
            audio.Play();
        }

        private void playBGMTransitionAdvance(string audioSource, string musicName, AudioLoop.LoopType loopType, float start, float end)
        {
            var audioSourceData = audioSources.GetAudio(audioSource);
            AudioSource audio = audioSourceData.audio;
            AudioLoopCallbacks callback = audioSourceData.callback;

            if (!audio)
            {
                var source = setNewAudioSource(audioSource, true);
                audio = source.audio;
                callback = source.callback;
            }

            if (audio.isPlaying)
            {
                ClipDetail clipDetail = audioLibrary.GetBGMClip(audio.clip);
                DOVirtual.Float(1, 0, timeFade, (x) =>
                {
                    audio.volume = globalBGMVolume  * clipDetail.localVolume * x;
                }).OnComplete(() => playBGMInTransitionAdvance(audio, musicName, loopType, start, end));
            }
            else
            {
                playBGMInTransitionAdvance(audio, musicName, loopType, start, end);
            }
        }

        private void playBGMInTransitionAdvance(AudioSource audio, string musicName, AudioLoop.LoopType loopType, float start, float end)
        {
            ClipDetail clipDetail = audioLibrary.GetBGMClip(musicName);

            audio.loop = true;
            audio.clip = clipDetail.clip;
            audio.volume = 0;
            audio.Play();
            DOVirtual.Float(0, 1, timeFade, (x) =>
            {
                audio.volume = globalBGMVolume * clipDetail.localVolume * x;
            });
        }

        private void playBGMDefaultAdvance(string audioSource, string musicName, AudioLoop.LoopType loopType, float start, float end)
        {
            var audioSourceData = audioSources.GetAudio(audioSource);
            AudioSource audio = audioSourceData.audio;
            AudioLoopCallbacks callback = audioSourceData.callback;

            ClipDetail clipDetail = audioLibrary.GetBGMClip(musicName);

            if (!audio)
            {
                var source = setNewAudioSource(audioSource, true);
                audio = source.audio;
                callback = source.callback;
            }

            if (audio.isPlaying)
            {
                stopBGMPlayed(audio, clipDetail, callback, true);
            }

            audio.loop = true;
            audio.clip = clipDetail.clip;
            audio.volume = globalBGMVolume * clipDetail.localVolume;
            audioLoopFunction.SetAudioSource(ref audio, ref callback);
            audioLoopFunction.PlayLoops(loopType, start, end); 
        }

        private void stopBGMPlayed(AudioSource audio, ClipDetail clipDetail, AudioLoopCallbacks callback, bool immediatly)
        {
            if (!audio) return;

            if (immediatly)
            {
                audio.volume = 0;
                audio.Stop();
                audio.clip = null;
                callback?.Unsubscribe();
            }
            else
            {
                DOVirtual.Float(1, 0, timeFade, (x) =>
                {
                    audio.volume = globalBGMVolume * clipDetail.localVolume * x;
                }).OnComplete(() =>
                {
                    audio.volume = 0;
                    audio.Stop();
                    audio.clip = null;
                    callback?.Unsubscribe();
                });
            }
        }

        private void stopBGMPlayed(string audioSource, bool immediatly)
        {
            var audioSourceData = audioSources.GetAudio(audioSource);
            AudioSource audio = audioSourceData.audio;
            AudioLoopCallbacks callback = audioSourceData.callback;

            Debug.Log(audio + " " + audioSourceData);

            if (!audio || !audio.isPlaying) return;
                
            if (immediatly)
            {
                audio.volume = 0;
                audio.Stop();
                audio.clip = null;
                callback?.Unsubscribe();
            }
            else
            {
                ClipDetail clipDetail = audioLibrary.GetBGMClip(audio.clip);

                DOVirtual.Float(1f, 0f, timeFade, (x) =>
                {
                    audio.volume = globalBGMVolume * clipDetail.localVolume * x;
                }).OnComplete(() =>
                {
                    audio.volume = 0;
                    audio.Stop();
                    audio.clip = null;
                    callback?.Unsubscribe();
                });
            }
        }

        private void setupAudioController(TWAudioLibrary library, TWAudioSourceLibrary audioSources, GameObject audioParent)
        {
            instance.audioSources = audioSources;
            instance.audioSources.Init();
            instance.audioLibrary = library;
            instance.audioParent = audioParent;
        }

        private void setTimeFade(float timeFade)
        {
            this.timeFade = timeFade;
        }

        private (AudioSource audio, AudioLoopCallbacks callback) setNewAudioSource(string audioSource, bool usingCallbackFunction)
        {
            GameObject audioObject = Instantiate(prefab, audioParent.transform);
            AudioSource audio = audioObject.GetComponent<AudioSource>();
            AudioLoopCallbacks callback = audioObject.GetComponent<AudioLoopCallbacks>();

            callback.enabled = usingCallbackFunction;

            audioSources.SetAudio(audioSource, audio, callback);

            return (audio, callback);
        }
        #endregion
    }
}