using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UIGame
{
    [Serializable]
    public struct Path
    {
        public Transform[] positions;
        public Vector3[] path;

        public void InitializePath()
        {
            path = new Vector3[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                path[i] = positions[i].position;
            }
        }
    }

    public class UIWin : MonoBehaviour
    {
        private UnityAction _actionReplay;
        private UnityAction _actionNextLevel;
        private UnityAction _actionExit;

        [Header("----------------------Setting Show Popup----------------------")]
        [SerializeField] private CanvasGroup _canvasGroup;
        private float timeShow = 0.5f;

        [Header("Star Win")]
        public int AmountStar;
        [SerializeField] private RectTransform[] stars;
        [SerializeField] private List<Path> paths;


        [Header("Button")] 
        [SerializeField] private CanvasGroup btnReplay; 
        [SerializeField] private CanvasGroup btnNextLevel;


        [Header("Banner")] [SerializeField] private Transform _label;
        private float originLabel;

        private void Awake()
        {
            LoadPath();
        }

        private void Start()
        {
            originLabel = _label.localPosition.y;
            // ShowDisplay(true);
        }

        private void LoadPath()
        {
            for (int i = 0; i < paths.Count; i++)
            {
                Path tempPath = paths[i];  
                tempPath.InitializePath();
                paths[i] = tempPath;
            }
        }


        public void ShowStars(int starCount)
        {
            if (starCount <= 0 || starCount > stars.Length) return;

            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < starCount; i++)
            {
                if (i >= paths.Count || paths[i].path == null || paths[i].path.Length < 2) continue;

                int index = i;

                seq.AppendCallback(() =>
                {
                    stars[index].gameObject.SetActive(true);
                    stars[index].position = paths[index].path[0];
                });

                Tween moveTween = stars[index].DOPath(paths[index].path, 0.6f, PathType.CatmullRom)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        stars[index].DOScale(Vector3.one * 1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
                    });

                seq.Append(moveTween);
            }

            seq.Play();
            seq.OnComplete(delegate
            {
                ShowButton(true);
            });
        }
        
        public void ShowDisplay(bool enable, UnityAction onShow = null, UnityAction onClosed = null)
        {
            if (enable)
            {
                onShow?.Invoke();
                _label.position += Vector3.up * 100f;
                _label.DOLocalMoveY(originLabel, 0.8f);
                _canvasGroup.gameObject.SetActive(true);
                _canvasGroup.DOFade(1, timeShow).OnComplete(() => ShowStars(AmountStar));
            }
            else
            {
                _canvasGroup.DOFade(0, timeShow).OnComplete(() =>
                {
                    _canvasGroup.gameObject.SetActive(false);
                    _label.position += Vector3.up * 10f;
                    ShowButton(false);
                    foreach (var star in stars)
                    {
                        star.gameObject.SetActive(false);
                    }
                    onClosed?.Invoke();
                });
            }
        }

        public void ShowButton(bool enable)
        {
            btnReplay.gameObject.SetActive(enable);
            btnReplay.DOFade(enable? 1: 0, 0.2f);
            btnNextLevel.gameObject.SetActive(enable);
            btnNextLevel.DOFade(enable? 1: 0, 0.4f);
        }

        public void SetActionReplay(UnityAction action) => _actionReplay = action;
        public void SetActionNextLevel(UnityAction action) => _actionNextLevel = action;
        public void SetActionExit(UnityAction action) => _actionExit = action;

        public void Replay() => ShowDisplay(false, null, _actionReplay);
        public void NextLevel() => ShowDisplay(false, null, _actionNextLevel);
        public void Exit() => ShowDisplay(false, null, _actionExit);
    }
}
