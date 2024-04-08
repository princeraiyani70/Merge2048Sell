using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FMGames {
    public abstract class PowerupBase : MonoBehaviour {
        [SerializeField] private string id;

        [SerializeField] private TMP_Text amountText;
        [SerializeField] private GameObject activatedObject;

        private bool isActivated;

        private void Start() {
            isActivated = false;
            SetUi();
        }

        public abstract bool OnClickCube(CubeObject c);

        public abstract bool CanShoot();

        public void Disable() {
            isActivated = false;
            SetUi();
        }

        /// <summary>
        /// Triggered if the player clicks on the powerup's icon in the bottom bar of the UI
        /// Returns true if the powerup became active
        /// </summary>
        public virtual bool OnClickPowerup() {
            if (GameManager._instance.isPaused) return false;

            if (isActivated) {
                //Cancels the activation
                isActivated = false;

                AddAmount(+1);
                SetUi();
            } else if (AddAmount(-1)) {

                isActivated = true;
                SetUi();

            } else {
                CurrencyManager._instance.OpenCoinShop();
            }

            return isActivated;
        }

        private void SetUi() {
            if (amountText != null)
                amountText.text = "x" + GetAmount();

            if (activatedObject != null)
                activatedObject.SetActive(isActivated);
        }


        public string GetPlayerPrefsId() => DataCollection.POWERUP_ID + id;

        /// <summary>
        /// Returns the amount of usages of the powerup
        /// </summary>
        /// <returns></returns>
        private int GetAmount() => PlayerPrefs.GetInt(GetPlayerPrefsId(), 5);

        /// <summary>
        /// Adds usage amount to the powerup
        /// Returns false if the new amount would be negative
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool AddAmount(int n) {
            int currentAmount = GetAmount();
            if (currentAmount + n < 0) return false;

            PlayerPrefs.SetInt(GetPlayerPrefsId(), n + currentAmount);
            SetUi();
            return true;
        }
    }
}