using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Splat Data", fileName = "New Splat Data")]
public class SplatData : ScriptableObject
{
    public List<GameObject> splatObject = new List<GameObject>();
    public List<Transform> splatTransform = new List<Transform>();

    public void StoreData(GameObject obj, Transform transform)
    {
        splatObject.Add(obj);
        splatTransform.Add(transform);
    }
}
