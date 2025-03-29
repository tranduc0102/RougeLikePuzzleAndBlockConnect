using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UIGame
{
    public abstract class BasePopup : MonoBehaviour
    {
        [Header("----------------------Setting Show Popup----------------------")]
        [SerializeField] protected CanvasGroup _canvasGroup;
        protected float timeShow = 0.5f;

        public virtual void ShowDisplay(bool enable, UnityAction onShow = null, UnityAction onClosed = null)
        {
            if (enable)
            {
                onShow?.Invoke();
                _canvasGroup.gameObject.SetActive(true);
                _canvasGroup.DOFade(1, timeShow);
            }
            else
            {
                _canvasGroup.DOFade(0, timeShow).OnComplete(delegate
                {
                    _canvasGroup.gameObject.SetActive(false);
                    onClosed?.Invoke();
                });
            }
        }
    }
}
