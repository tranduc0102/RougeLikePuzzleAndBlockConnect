using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private Queue<Tween> m_Tweens = new Queue<Tween>();
    private void OnDisable()
    {
        while (m_Tweens.Count > 0)
        {
            m_Tweens.Dequeue()?.Kill();
        }
    }
    
    public void ButtonSetting()
    {
        
    }

    public void ButtonPlay(RectTransform rect)
    {
        m_Tweens.Enqueue(rect.DOScale(rect.localScale * 1.25f, 0.25f)
            .OnComplete(() =>
            {
                m_Tweens.Enqueue(rect.DOScale(rect.lossyScale / 1.25f, 0.25f)
                    .OnComplete(() =>
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }));
            }));
    }

    public void ButtonRank()
    {
        
    }
    
    public void ButtonInfo()
    {
        
    }
    
    public void ButtonExit()
    {
        Application.Quit();
    }
    
}
