using AwesomePooling;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AwesomePoolingExample
{
    public class PoolingCanvas : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private AwesomeBullet _awesomeBulletPrefab;

        [Space]
        [Header("Parent")]
        [SerializeField] private Transform _parent;

        [Space]
        [Header("Text Input")]
        [SerializeField] private InputField _poolCountInput;

        [Space]
        [Header("Buttons")]
        [SerializeField] private Button _poolButton;
        [SerializeField] private Button _getFromPoolButton;
        [SerializeField] private Button _sendToPoolButton;
        [SerializeField] private Button _sendAllObjectsToPoolButton;

        private PoolingSystem _poolingSystem;

        private void Awake()
        {
            _poolingSystem = PoolingSystem.GetInstance();

            _poolButton.onClick.AddListener(PoolObjects);
            _getFromPoolButton.onClick.AddListener(GetFromPool);
            _sendToPoolButton.onClick.AddListener(SendToPool);
            _sendAllObjectsToPoolButton.onClick.AddListener(SendAllObjectsToPool);
        }

        private void OnDestroy()
        {
            _poolButton.onClick.RemoveAllListeners();
            _getFromPoolButton.onClick.RemoveAllListeners();
            _sendToPoolButton.onClick.RemoveAllListeners();
            _sendAllObjectsToPoolButton.onClick.RemoveAllListeners();
        }

        private void PoolObjects()
        {
            _poolingSystem.PoolObjects(_awesomeBulletPrefab, int.Parse(_poolCountInput.text));
        }

        private List<AwesomeBullet> _bulletList = new List<AwesomeBullet>();
        private void GetFromPool()
        {
            AwesomeBullet bullet = _poolingSystem.GetFromPool<AwesomeBullet>();
            _bulletList.Add(bullet);
            bullet.transform.parent = _parent;
            bullet.transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0f, UnityEngine.Random.Range(-10f, 10f));
        }

        private void SendToPool()
        {
            if (_bulletList.Count > 0)
            {
                _bulletList[0].SendToPool();
                _bulletList.RemoveAt(0);
            }
        }

        private void SendAllObjectsToPool()
        {
            _poolingSystem.SendAllObjectsToPool<AwesomeBullet>();
        }
    }
}