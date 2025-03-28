using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UIGame
{
    public class UIPause : BasePopup
    {
        private UnityAction _actionContinue;
        private UnityAction _actionRestart;
        private UnityAction _actionGiveUp;

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
            ShowDisplay(false, null, ()=>  UIController.Instance.UISetting.ShowDisplay(true));
        }

        public void Continue()
        {
            ShowDisplay(false, null,_actionContinue);
            UIController.Instance.UIInGame.ShowDisplay(true);
        }

        public void Restart()
        {
            ShowDisplay(false, null, _actionRestart);
            UIController.Instance.UIInGame.ShowDisplay(true);
        }

        public void GiveUp()
        {
            ShowDisplay(false, null,_actionGiveUp);
            UIController.Instance.UIInGame.ShowDisplay(false);
            UIController.Instance.UISelectLevel.ShowDisplay(true);
        }
    }
}
