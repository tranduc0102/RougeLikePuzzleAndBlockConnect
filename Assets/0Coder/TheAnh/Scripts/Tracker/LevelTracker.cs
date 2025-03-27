using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelTracker : ComponentBehavior
{
    [SerializeField] private List<LevelButton> m_LevelButtons;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        m_LevelButtons = new List<LevelButton>(GetComponentsInChildren<LevelButton>());
    }

    private void Start()
    {
        for (int i = 0; i < m_LevelButtons.Count; ++i)
        {
            LevelButton levelButton = m_LevelButtons[i];

            levelButton.Level = i;
            bool isLevelUnlock = (PlayerPrefs.GetInt("IsLevelCompleted", 0) == 1);
            if(isLevelUnlock) levelButton.SetLevelUnlock();
            else levelButton.SetLevelLock();
        }
       
    }
}
