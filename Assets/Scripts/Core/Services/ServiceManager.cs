using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace Core
{
    public class ServiceManager : MonoBehaviour
    {
        private readonly Dictionary<Type, IService> _servicesByType = new();

        [SerializeField]
        private InterfaceField<IService>[] _services;

        public static ServiceManager Instance { get; private set; }

        private void Start()
        {
            Assert.IsNull(Instance, $"{typeof(ServiceManager)} instance is already set");
            Instance = this;

            StartCoroutine(Initialize());
        }

        private void OnDestroy()
        {
            Deinicilize();
        }

        private IEnumerator Initialize()
        {
            Debug.Log("=== <b>Game Systems Initialization</b> ===");
            foreach (var service in _services)
            {
                if (!_servicesByType.TryAdd(service.Value.GetType(), service.Value))
                {
                    Debug.LogWarning($"Duplicated {service.GetType()} system");
                }
                else
                {
                    Debug.Log($"<b>Initialization...</b> {service.Value.GetType()}");
                    yield return StartCoroutine(service.Value.Init());
                }
            }
        }

        private void Deinicilize()
        {
            Debug.Log("=== <b>Game System Desincilization</b> ===");
            for (int i = _services.Length - 1; i >= 0; i--)
            {
                var service = _services[i].Value;
                Debug.Log($"<b>Desincilization...</b> {service.GetType()}");
                service.Deinit();
            }
            _servicesByType.Clear();
            Instance = null;
        }

        public TSystem Get<TSystem>() where TSystem : IService
        {
            if (_servicesByType.TryGetValue(typeof(TSystem), out var service))
            {
                return (TSystem)service;
            }

            Debug.LogError($"Service {typeof(TSystem)} is not register in {name}", this);
            return default;
        }
    }
}