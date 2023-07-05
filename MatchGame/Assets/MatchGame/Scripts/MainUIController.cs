using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MatchGame
{
    public class MainUIController : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextButton;

        private MatchGame _matchGame;

        [Inject]
        void Construct(MatchGame matchGame)
        {
            _matchGame = matchGame;
        }

        private void Start()
        {
            _restartButton.onClick.AddListener(OnClickRestart);
            _nextButton.onClick.AddListener(OnClickNext);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnClickRestart);
            _nextButton.onClick.RemoveListener(OnClickNext);
        }

        private void OnClickNext()
        {
            _matchGame.NextLevel();
        }

        private void OnClickRestart()
        {
            _matchGame.RestartLevel();
        }
    }
}