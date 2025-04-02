using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Slider m_loading;
    [SerializeField] private float m_timeLoading;
    private float m_curTime;

    private void Awake()
    {
        m_loading = GetComponent<Slider>();
        m_loading.value = 0f;
        m_curTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (m_loading.value < 1)
        {
            m_loading.value += (Time.time - m_curTime) / m_timeLoading;
            m_curTime = Time.time;
        }
        else
        {
            m_loading.value = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
