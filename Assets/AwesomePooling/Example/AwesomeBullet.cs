using AwesomePooling;
using UnityEngine;

namespace AwesomePoolingExample
{
    public class AwesomeBullet : MonoBehaviour, IPoolable
    {
        public bool IsInUse { get; set; }
    }
}