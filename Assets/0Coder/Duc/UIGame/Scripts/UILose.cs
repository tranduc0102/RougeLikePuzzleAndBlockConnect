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
            ShowDisplay(false, null, actionExit);
            UIController.Instance.UIInGame.ShowDisplay(false);
            UIController.Instance.UISelectLevel.ShowDisplay(true);
            SceneManager.UnloadSceneAsync("Scene_3_Gameplay");
        }

        public void Replay()
        {
            ShowDisplay(false, null, actionReplay);
            StartCoroutine(ReloadScene());

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