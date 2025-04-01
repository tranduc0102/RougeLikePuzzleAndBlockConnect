using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UIGame
{
    public class UIInGame : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public void ShowDisplay(bool enable, UnityAction onShow = null, UnityAction onClosed = null)
        {
            if (enable)
            {
                _canvasGroup.gameObject.SetActive(true);
                _canvasGroup.DOFade(1, 0.5f).OnComplete(delegate
                {
                    onShow?.Invoke();
                });
            }
            else
            {
                _canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
                {
                    _canvasGroup.gameObject.SetActive(false);
                    onClosed?.Invoke();
                });
            }
        }

        public void PauseGame()
        {
            UIController.Instance.UIPause.ShowDisplay(true,() =>
            {
                Time.timeScale = 0;
            });
        }
    }
}
