using UnityEngine;

public static class ExtendTransform
{
    //Extend function from Transform class to destroy automatically all children object of transform
    public static void DestroyChildrens(this Transform transform, bool destroyImmediately = false)
    {
        foreach (Transform child in transform)
        {
            if (destroyImmediately) MonoBehaviour.DestroyImmediate(child.gameObject);
            else MonoBehaviour.Destroy(child.gameObject);
        }
    }
}