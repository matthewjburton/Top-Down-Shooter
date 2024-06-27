using UnityEngine;

public static partial class MonoBehaviourExtensions
{
    public static bool TryGetComponentWithWarning<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponent<T>();
        if (component == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have a {typeof(T).Name} component!");
            return false;
        }
        return true;
    }
}