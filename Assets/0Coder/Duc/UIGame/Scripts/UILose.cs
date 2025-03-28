using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            
        }

        public void Replay()
        {
            ShowDisplay(false, null, actionReplay);
        }
    }

}