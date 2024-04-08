using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

namespace FMGames {
    public class GameUi : MonoBehaviour {
        public static GameUi _instance;

        [SerializeField] GameObject gamePanel;

        [SerializeField] GameObject victoryPanel, loosePanel;

        [SerializeField] TMP_Text levelText;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] Image fillImage;

        private void Awake() {
            _instance = this;
        }

        public void ShowVictory() {
            Invoke("ShowVictoryPanel", 2f);
        }

        public void ShowGameOver() {
            Invoke("ShowLoosePanel", 2f);
        }

        void ShowVictoryPanel() {
            victoryPanel.SetActive(true);
        }

        void ShowLoosePanel() {
            loosePanel.SetActive(true);
        }

        public void RestartScene(int n) {
            SceneManager.LoadScene(n);
        }

        public void SetScore(int n) {
            scoreText.text = CurrencyManager.GetSuffix(n);
        }

        public void InitLevel(int lvl) {
            SetScore(0);
            levelText.text = "Level " + (lvl + 1);

            SetProgressionSlider(0, 1);
        }

        public void SetProgressionSlider(int score, int maxScore) {
            float val = score / (float)maxScore;

            fillImage.fillAmount = val;
        }

        public IEnumerator FadeOutObject(GameObject g, float time) {
            TMP_Text[] texts = g.GetComponentsInChildren<TMP_Text>();
            Image[] images = g.GetComponentsInChildren<Image>();

            float t = 0;
            while (t < time) {
                t += Time.deltaTime;

                foreach (Image img in images)
                    img.color = Color.Lerp(img.color, new Color(0, 0, 0, 0), t / time);

                foreach (TMP_Text txt in texts)
                    txt.color = Color.Lerp(txt.color, new Color(0, 0, 0, 0), t / time);

                yield return null;
            }

            g.SetActive(false);

            foreach (Image img in images)
                img.color = Color.white;

            foreach (TMP_Text txt in texts)
                txt.color = Color.white;
        }
    }
}