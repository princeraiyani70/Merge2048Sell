using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMGames {
    public class PowerupChangeCube : PowerupBase {
        public override bool OnClickCube(CubeObject c) {
            List<int> possibleIds = new List<int>();
            for (int i = 0; i < 5; i++) {
                if (c.CubeData.Id != i)
                    possibleIds.Add(i);
            }

            c.MergeUpgrade(possibleIds[Random.Range(0, possibleIds.Count)] - c.CubeData.Id);
            return true;
        }

        public override bool CanShoot() {
            return false;
        }
    }
}