using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : ComponentBehavior
{
    private int m_Level;
    [HideInInspector] private Button m_Button;

    public int Level
    {
        get => m_Level;
        set
        {
            if (m_Level != value)
            {
                m_Level = value;
                //dosomething Here
            }
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (m_Button == null) m_Button = transform.GetComponentInChildren<Button>();
    }

    public void SetLevelUnlock()
    {
        m_Button.interactable = true;
        //Update UI late
    }

    public void SetLevelLock()
    {
        m_Button.interactable = false;
        // Update UI Late
    }
}
