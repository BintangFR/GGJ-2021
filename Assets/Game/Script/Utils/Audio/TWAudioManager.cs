/* TWAudio version 2.0
 * Update Date : 21/08/2020
 * Created by TomWill
 */
using UnityEngine;

namespace TomWill
{
    [RequireComponent(typeof(TWAudioController), typeof(TWAudioLibrary))]
    public class TWAudioManager : MonoBehaviour
    {
        [SerializeField] private TWAudioController audioController;
        [SerializeField] private TWAudioLibrary audioLibrary;
        [SerializeField] private TWAudioSourceLibrary audioSources;
        [SerializeField] private GameObject audioParent;

        void Awake()
        {
            if (TWAudioController.Instance == null)
            {
                audioController = GetComponent<TWAudioController>();
                audioLibrary = GetComponent<TWAudioLibrary>();

                audioController.CreateInstance();
                TWAudioController.SetupAudioLibrary(audioLibrary, audioSources, audioParent);
            }

            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}