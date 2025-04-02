using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIGame
{
    public class UISetting : BasePopup
    {
        [Header("Slider Sound FX")]
        [SerializeField] private Slider _sliderSoundFX;
        [SerializeField] private Image iconSound;
        public Sprite iconOnSoundFX;
        public Sprite iconOffSoundFX;
        [Header("Slider Music")]
        [SerializeField] private Slider _sliderMusic;
        [SerializeField] private Image iconMusic;
        public Sprite iconOnMusic;
        public Sprite iconOffMusic;
        private UnityAction _actionClosed;

        private void Start()
        {
            
            if (_sliderSoundFX.value == 0)
            {
                iconSound.sprite = iconOffSoundFX;
            }
            else
            {
                iconSound.sprite = iconOnSoundFX;
            }
            
            
            if (_sliderMusic.value == 0)
            {
                iconMusic.sprite = iconOffMusic;
            }
            else
            {
                iconMusic.sprite = iconOnMusic;
            }
            _sliderSoundFX.onValueChanged.AddListener(delegate
            {
                if (_sliderSoundFX.value == 0)
                {
                    iconSound.sprite = iconOffSoundFX;
                }

                if (iconSound.sprite == iconOffSoundFX && _sliderSoundFX.value != 0)
                {
                    iconSound.sprite = iconOnSoundFX;
                }
            });
            
            _sliderMusic.onValueChanged.AddListener(delegate
            {
                if (_sliderMusic.value == 0)
                {
                    iconMusic.sprite = iconOffMusic;
                }

                if (iconMusic.sprite == iconOffMusic && _sliderMusic.value != 0)
                {
                    iconMusic.sprite = iconOnMusic;
                }
            });
            
        }

        public void SetActionClosed(UnityAction action)
        {
            _actionClosed = action;
        }
        public void Close()
        {
            
            ShowDisplay(false, null, _actionClosed);
            
        }

        public override void ShowDisplay(bool enable, UnityAction onShow = null, UnityAction onClosed = null)
        {
            if (enable)
            {
                Time.timeScale = 1;
                _canvasGroup.gameObject.SetActive(true);
                _canvasGroup.alpha = 1;
                Time.timeScale = 0;
                
                onShow?.Invoke();
            }
            else
            {
              
                _canvasGroup.alpha = 0;
                _canvasGroup.gameObject.SetActive(false);
                Time.timeScale = 1;
                onClosed?.Invoke();
            }
            
        }
    }
}
