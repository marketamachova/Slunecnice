using System;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class FinalUI : MonoBehaviour
    {
        private GameController _gameController;
        private void Awake()
        {
            _gameController = GetComponent<GameController>();
        }

        public void OnQuit()
        {
            Application.Quit();
        }

        public void OnPlayAgain()
        {
            SceneManager.LoadScene("Scenes/MainScene");
        }
    }
}