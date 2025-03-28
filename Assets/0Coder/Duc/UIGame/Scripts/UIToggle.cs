using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIGame
{
    public class UIToggle : MonoBehaviour
    {
        public Toggle toggle;
        public Image switchHandle;
        public Image switchBackground;

        public Sprite handelOn;
        public Sprite handelOff;
        
        public Sprite switchOn;
        public Sprite switchOff;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        public virtual void SwitchChange()
        {
            if (toggle.isOn)
            {
                //Audio ?
                switchHandle.transform.DOLocalMoveX(50, 0.3f).SetEase(Ease.OutSine).OnPlay(() =>
                {
                    switchBackground.sprite = switchOn;
                    switchHandle.sprite = handelOn;
                });
            }
            else
            {
                //Audio ?

                switchHandle.transform.DOLocalMoveX(-50, 0.3f).SetEase(Ease.OutSine).OnPlay(() =>
                {
                    switchBackground.sprite = switchOff;
                    switchHandle.sprite = handelOff;
                });
            }
        }
    }
}