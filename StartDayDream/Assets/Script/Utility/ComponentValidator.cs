using UnityEngine;

public static class ComponentValidator
{
    public static bool GetRequiredComponent<T>(this MonoBehaviour mono, out T component) where T : Component
    {
        if (mono.TryGetComponent(out component))
            return true;

        Debug.LogError($"[{mono.GetType().Name}] Required component '{typeof(T).Name}' not found on '{mono.gameObject.name}'", mono);
        return false;
    }
}