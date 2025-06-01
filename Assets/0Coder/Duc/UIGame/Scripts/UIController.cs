using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIGame
{
    public class UIController : Singleton<UIController>
    {
        public UIInGame UIInGame => FindObjectOfType<UIInGame>();
        public UISetting UISetting => FindObjectOfType<UISetting>();
        public UIWin UIWin => FindObjectOfType<UIWin>();
        public UIPause UIPause => FindObjectOfType<UIPause>();
        public UILose UILose => FindObjectOfType<UILose>();
        public UISelectLevel UISelectLevel => FindObjectOfType<UISelectLevel>();
        public UIShowTurn UIShowTurn => FindObjectOfType<UIShowTurn>();
        private void Start()
        {
            AudioManager.PlayBackGroundMusic(SoundType.SelectLevel);
        }
    }
}
