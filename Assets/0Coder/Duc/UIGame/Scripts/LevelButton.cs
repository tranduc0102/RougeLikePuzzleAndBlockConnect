using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIGame
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private int id; // ID của Level
        [SerializeField] private Button button;
        [SerializeField] private Image lockIcon;
        [SerializeField] private TypePath _typePath; // Loại đường
        [SerializeField] private List<int> nextLevels; // Danh sách ID level tiếp theo


        [Header("Setting")] 
        [SerializeField] private bool isMainStage;

        [SerializeField] private GameObject forcus;
        [SerializeField] private List<GameObject> _stars;
        private void Start()
        {
            UpdateState();
        }

        private void OnEnable()
        {
            if (isMainStage)
            {
                // Nếu nó là đường chính thì Update Stat, dùng Obsever
              //UpdateStar
            }
        }

        private void UpdateStar(int amountStar)
        {
            PlayerPrefs.SetInt($"{id} + Star", amountStar);
            for (int i = 0; i < PlayerPrefs.GetInt($"{id} + Star", amountStar); i++)
            {
                _stars[i].SetActive(true);
            }
        }

        private void UpdateState()
        {
            int status = LevelManager.Instance.GetLevelStatus(id);
            if (status == 0) // Bị khóa
            {
                button.interactable = false;
                lockIcon.gameObject.SetActive(true);
                if (isMainStage)
                {
                    forcus.SetActive(false);
                }
            }
            else if (status == 1) // Đã mở khóa
            {
                button.interactable = true;
                lockIcon.gameObject.SetActive(false);
                if (isMainStage)
                {
                    forcus.SetActive(true);
                }
            }
            else if (status == 2) // Đã hoàn thành
            {
                button.interactable = true;
                lockIcon.gameObject.SetActive(false);
                if (isMainStage)
                {
                    forcus.SetActive(false);
                }
            }
        }

        public void OnLevelSelected()
        {
            if (LevelManager.Instance.GetLevelStatus(id) > 0)
            {
                Debug.Log($"Chơi Level {id}");
                UIController.Instance.UIInGame.ShowDisplay(true);
                UIController.Instance.UISelectLevel.ShowDisplay(false);
            }
        }

        public void CompleteLevel()
        {
            LevelManager.Instance.CompleteLevel(id, _typePath, nextLevels);
            if (isMainStage)
            {
                forcus.SetActive(false);
            }
        }
    }

    public enum TypePath
    {
        OnePath,
        TwoPath,
        ThreePath,
        FinishPath
    }
}