using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UIGame
{
    public class UILose : BasePopup
    {
        private UnityAction actionReplay;
        private UnityAction actionExit;
        public override void ShowDisplay(bool enable, UnityAction onShow = null, UnityAction onClosed = null)
        {
            base.ShowDisplay(enable, onShow, onClosed);
            if (enable)
            {
                AudioManager.PlaySFX(SoundType.FXLose);
                AudioManager.StopAudioMusic();
            }
            else
            {
                AudioManager.PlayContinueSound();
            }
        }
        public void SetActionReplay(UnityAction action)
        {
            actionReplay = action;
        }
        public void SetActionExit(UnityAction action)
        {
            actionExit = action;
        }

        public void Exit()
        {
            AudioManager.PlaySFX(SoundType.FXButtonClick);
            ShowDisplay(false, null, actionExit);
            UIController.Instance.UIInGame.ShowDisplay(false);
            UIController.Instance.UISelectLevel.ShowDisplay(true);
            SceneManager.UnloadSceneAsync("Scene_3_Gameplay");
        }

        public void Replay()
        {
            AudioManager.PlaySFX(SoundType.FXButtonClick);
            ShowDisplay(false, null, actionReplay);
/*            StartCoroutine(ReloadScene());
*/
        }   
        private IEnumerator ReloadScene()
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("Scene_3_Gameplay");

            while (!unloadOp.isDone)
            {
                yield return null;
            }
            SceneManager.LoadScene("Scene_3_Gameplay", LoadSceneMode.Additive);
        }

    }

}