using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMGames {
    public class PowerupRemoveCube : PowerupBase {
        public override bool OnClickCube(CubeObject c) {
            Destroy(c.gameObject);
            return true;
        }

        public override bool CanShoot() {
            return false;
        }
    }
}