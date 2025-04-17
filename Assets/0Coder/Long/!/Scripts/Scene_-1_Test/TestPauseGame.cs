// Author: Dan_lang_A (DauHang)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace DanLangA
{
    public class TestPauseGame : MonoBehaviour
    {
        public GameObject obj;
        public void PauseGame()
        {
            Time.timeScale = 0;
            obj.transform.DOScale(Vector3.zero, 1f).From().SetUpdate(true);
        }
    }
}