/* TWLoading version 1.0
 * Update Date : 29/08/2020
 * Created by TomWill
 */
using System.Linq;
using UnityEngine;

namespace TomWill
{
    public class TWLoadingImageLib : MonoBehaviour
    {
        [System.Serializable]
        struct ImageLibrary
        {
            public string name;
            public Sprite image;
        }

        [SerializeField] private ImageLibrary[] backgroundLibrary;

        public Sprite GetImage(string name)
        {
            if (backgroundLibrary.Any(e => e.name == name))
            {
                return backgroundLibrary.First(e => e.name == name).image;
            }

            return null;
        }
    }
}