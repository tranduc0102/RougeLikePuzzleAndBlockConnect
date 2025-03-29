using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnClickTween(RectTransform rect)
    {
        rect.DOScale(rect.localScale * 1.25f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    
    public void ButtonSetting()
    {
        
    }

    public void ButtonPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
