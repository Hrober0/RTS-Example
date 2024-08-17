using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UI
{
    public class UINotificationMenu : MonoBehaviour
    {
        [SerializeField] private Transform _notificationParent;
        [SerializeField] private UINotification _notificationPrefab;

        private IAgentService _agentService;
        private ObjectPool<UINotification> _notificationPool;

        private void Start()
        {
            _notificationPool = new(
                createFunc: () =>
                {
                    var notification = Instantiate(_notificationPrefab);
                    notification.transform.SetParent(_notificationParent, false);
                    return notification;
                });
        }

        private void OnEnable()
        {
            _agentService = ServiceManager.Instance.Get<IAgentService>();
            _agentService.OnNotification += DisaplyNewNotification;
        }

        private void OnDisable()
        {
            _agentService.OnNotification -= DisaplyNewNotification;
        }

        private void DisaplyNewNotification(INotification notification)
        {
            var notificationPanel = _notificationPool.Get();
            notificationPanel.transform.SetAsFirstSibling();
            notificationPanel.Show(notification);
            notificationPanel.OnNotificationHide += ReturnToPool;
        }

        private void ReturnToPool(UINotification notification)
        {
            notification.OnNotificationHide -= ReturnToPool;
            _notificationPool.Release(notification);
        }
    }
}