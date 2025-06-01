using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UIGame
{
    public class UIPause : BasePopup
    {
        private UnityAction _actionContinue;
        private UnityAction _actionRestart;
        private UnityAction _actionGiveUp;
        public override void ShowDisplay(bool enable, UnityAction onShow = null, UnityAction onClosed = null)
        {
            base.ShowDisplay(enable, onShow, onClosed);
            if (enable)
            {
                AudioManager.StopAudioMusic();
            }
            else
            {
                AudioManager.PlayContinueSound();
            }
        }
        public void SetActionContinue(UnityAction action)
        {
            _actionContinue = action;
        }
        public void SetActionResart(UnityAction action)
        {
            _actionRestart = action;
        }
        public void SetActionGiveUp(UnityAction action)
        {
            _actionGiveUp = action;
        }
        public void Setting()
        {
            AudioManager.PlaySFX(SoundType.FXButtonClick);
            //ShowDisplay(false, null );
            _canvasGroup.gameObject.SetActive(false);
            UIController.Instance.UISetting.ShowDisplay(true);
        }

        public void Continue()
        {
            AudioManager.PlaySFX(SoundType.FXButtonClick);
            Time.timeScale = 1;
            ShowDisplay(false, null,_actionContinue);
            UIController.Instance.UIInGame.ShowDisplay(true);
        }

        public void Restart()
        {
            AudioManager.PlaySFX(SoundType.FXButtonClick);
            Time.timeScale = 1;
            ShowDisplay(false, null, _actionRestart);
            UIController.Instance.UIInGame.ShowDisplay(true);
/*            StartCoroutine(ReloadScene());
*/        }

        public void GiveUp()
        {
            AudioManager.PlaySFX(SoundType.FXButtonClick);
            Time.timeScale = 1;
            ShowDisplay(false, null,_actionGiveUp);
            UIController.Instance.UIInGame.ShowDisplay(false);
            UIController.Instance.UISelectLevel.ShowDisplay(true);
            SceneManager.UnloadSceneAsync("Scene_3_Gameplay");
        } 
      /*  private IEnumerator ReloadScene() 
        { 
           *//* AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("Scene_3_Gameplay");
            while (!unloadOp.isDone) 
            { 
                yield return null; 
            } 
            SceneManager.LoadScene("Scene_3_Gameplay", LoadSceneMode.Additive); *//*
        }*/
    }
}
