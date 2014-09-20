using UnityEngine;

namespace Assets.Scripts
{
    static public class MonoBehaviourTransformExtension
    {
        /// <summary>
        /// Gets or add a component. Usage example:
        /// transform.GetOrAddComponent();
        /// </summary>
        static public T GetOrAddComponent<T>(this Component child) where T : Component
        {
            var result = child.GetComponent<T>() ?? child.gameObject.AddComponent<T>();
            return result;
        }
    }
}
