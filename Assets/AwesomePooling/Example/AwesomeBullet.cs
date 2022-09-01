using AwesomePooling;
using UnityEngine;

namespace AwesomePoolingExample
{
    public class AwesomeBullet : MonoBehaviour, IPoolable
    {
        public bool IsInUse { get; set; }

        public void OnPooled()
        {
            Debug.Log(gameObject.name + " is pooled");
        }

        public void OnSelected()
        {
            Debug.Log(gameObject.name + " is selected from pool");
        }
    }
}