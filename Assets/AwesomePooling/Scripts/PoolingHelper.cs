using UnityEngine;

namespace AwesomePooling
{
    public static class PoolingHelper
    {
        public static void SendToPool<T>(this T poolableObject) where T : MonoBehaviour, IPoolable
        {
            PoolingSystem.GetInstance().SendToPool(poolableObject);
        }
    }
}