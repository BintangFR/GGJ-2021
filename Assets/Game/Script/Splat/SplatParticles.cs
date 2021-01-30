using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatParticles : MonoBehaviour
{
    public SplatData data;
    public List<Transform> coba = new List<Transform>();
    public ParticleSystem splatParticles;
    public GameObject splatPrefabs;
    public Transform splatHolder;
    private List<ParticleCollisionEvent> collisionEvent = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(splatParticles, other, collisionEvent);

        int count = collisionEvent.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject splat = Instantiate(splatPrefabs, collisionEvent[i].intersection, Quaternion.identity) as GameObject;
            splat.transform.SetParent(splatHolder, true);
            Splat splatScript = splat.GetComponent<Splat>();
            splatScript.Initialize();
        }
    }
}
