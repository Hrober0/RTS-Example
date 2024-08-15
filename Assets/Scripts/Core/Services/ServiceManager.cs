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

        private readonly List<IService> _inicializedServices = new();

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

        public TSystem Get<TSystem>() where TSystem : IService
        {
            if (_servicesByType.TryGetValue(typeof(TSystem), out var service))
            {
                if (service == null)
                {
                    Debug.LogError($"Service {typeof(TSystem)} is not register in {name}", this);
                    return default;
                }

                return (TSystem)service;
            }

            foreach (var newService in _inicializedServices)
            {
                if (newService is TSystem typedService)
                {
                    _servicesByType.Add(typeof(TSystem), typedService);
                    return typedService;
                }
            }

            Debug.LogError($"Service {typeof(TSystem)} is not register in {name}", this);
            _servicesByType.Add(typeof(TSystem), null);
            return default;
        }

        private IEnumerator Initialize()
        {
            Debug.Log("=== <b>Game Systems Initialization</b> ===");
            foreach (var serviceField in _services)
            {
                var service = serviceField.Value;
                if (!_servicesByType.TryAdd(service.GetType(), service))
                {
                    Debug.LogWarning($"Duplicated {service.GetType()} system");
                }
                else
                {
                    Debug.Log($"<b>Initialization...</b> {service.GetType()}");
                    yield return StartCoroutine(service.Init());
                    _inicializedServices.Add(service);
                }
            }
        }

        private void Deinicilize()
        {
            Debug.Log("=== <b>Game System Desincilization</b> ===");
            for (int i = _inicializedServices.Count - 1; i >= 0; i--)
            {
                var service = _inicializedServices[i];
                Debug.Log($"<b>Desincilization...</b> {service.GetType()}");
                service.Deinit();
                _inicializedServices.RemoveAt(i);
            }
            Instance = null;
        }
    }
}