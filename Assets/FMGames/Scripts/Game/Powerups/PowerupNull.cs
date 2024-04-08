using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMGames {
    public class PowerupNull : PowerupBase {
        public override bool CanShoot() {
            return true;
        }

        public override bool OnClickCube(CubeObject c) {
            return false;
        }

        public override bool OnClickPowerup() {
            return false;
        }
    }
}