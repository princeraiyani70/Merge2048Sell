using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FMGames {
    public class PowerupManager : MonoBehaviour {
        public static PowerupManager _instance;

        [SerializeField] private PowerupBase activePowerup;
        [SerializeField] private PowerupBase nullPowerup; //default powerup which does nothing (removes #if (activePowerup != null)# checks


        private void Awake() {
            _instance = this;
            activePowerup = nullPowerup;
        }


        // Start is called before the first frame update
        void Start() {
        }

        /// <summary>
        /// Clicking the powerup in the ui
        /// </summary>
        /// <param name="pb"></param>
        public void OnClickPowerup(PowerupBase pb) {
            if (pb.OnClickPowerup()) {
                activePowerup.OnClickPowerup();
                activePowerup = pb;
            } else {
                activePowerup = nullPowerup;
            }
        }

        public void OnClickCube(CubeObject c) {
            if (activePowerup.OnClickCube(c)) {
                activePowerup.Disable();
                activePowerup = nullPowerup;

                StartCoroutine(Shooter._instance.EnableShootAfterDelay(0.25f));
            }

        }

        public bool CanShoot() => activePowerup.CanShoot();
    }
}