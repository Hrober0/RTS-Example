using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Core;
using System;
using DG.Tweening;

namespace UI
{
    public class UINotification : MonoBehaviour
    {
        public event Action<UINotification> OnNotificationHide;

        [SerializeField] private float _displayTime = 2f;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Show(INotification notification)
        {
            gameObject.SetActive(true);
            _label.text = notification.GetMessage();
             DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InQuad))
                .AppendInterval(_displayTime)
                .Append(_canvasGroup.DOFade(0, 1f).SetEase(Ease.OutQuad))
                .OnComplete(Hide);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            OnNotificationHide?.Invoke(this);
        }
    }
}