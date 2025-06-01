using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UIGame
{
    public class UISelectLevel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<LevelButton> lvBtns;
        public List<LevelButton> LevelButtons { get { return lvBtns; } }
        public void ShowDisplay(bool enable, UnityAction onShow = null, UnityAction onClosed = null)
        {
            if (enable)
            {
                onShow?.Invoke();
                _canvasGroup.gameObject.SetActive(true);
                _canvasGroup.DOFade(1, 0.5f);
                AudioManager.PlayBackGroundMusic(SoundType.SelectLevel);
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
    }

}