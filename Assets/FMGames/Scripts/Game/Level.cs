using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMGames {
    public class Level : MonoBehaviour {
        private int score;
        public int Score {
            get => score; set {
                score = value;

                GameUi._instance.SetScore(score);
                GameUi._instance.SetProgressionSlider(score, scoreToComplete);

                if (score > scoreToComplete) {
                    GameManager._instance.GameOver(true);
                }
            }
        }

        private int scoreToComplete;

        private int mergeCountCombo; //How many merges happened in the past few seconds
        private float timeToResetCombo = 1.5f;
        private float comboTimer;

        public void Init(int levelNum) {
            scoreToComplete = 130 + levelNum * 50;
        }

        // Update is called once per frame
        void Update() {
            comboTimer += Time.deltaTime;
            if (comboTimer > timeToResetCombo) {
                comboTimer = 0;
                mergeCountCombo = 0;
            }
        }

        public void MergeCubes(CubeObject cube1, CubeObject cube2, int score) {
            mergeCountCombo++;

            if (CheckMergeCombo(cube1)) {
                score *= 2;
                ParticleManager._instance.ComboEffect(cube1.transform.position + Vector3.back + Vector3.up / 2);
            }

            Score += score;

            ParticleManager._instance.PoofEffect(cube1.transform.position + Vector3.back);
            ParticleManager._instance.TMPTextPopup("+" + score, cube1.transform.position + Vector3.back);
        }

        private bool CheckMergeCombo(CubeObject cube) {
            if (mergeCountCombo > 1) {
                comboTimer = 0;
                return true;
            }

            return false;
        }
    }
}