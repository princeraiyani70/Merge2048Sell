using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FMGames {

    public class TrajectorySimulator : MonoBehaviour {
        public int reflections;
        public float maxLength;
        public int maxDotCount;
        public float radius = 50f;

        LineRenderer lineRenderer;
        Ray ray;
        RaycastHit hit;

        // Start is called before the first frame update
        void Start() {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update() {
            ray = new Ray(transform.position, transform.up);

            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, transform.position);

            float remainingLength = maxLength;

            for (int i = 0; i < reflections; i++) {
                if (Physics.SphereCast(ray, radius, out hit, remainingLength)) {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                    remainingLength -= Vector3.Distance(ray.origin, hit.point);

                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                } else {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);

                    remainingLength = 0;
                }
            }

            int dotCount = (int)Mathf.Lerp(0, maxDotCount, 1 - remainingLength / maxLength);
            lineRenderer.material.SetFloat("_Rep", dotCount);
        }

        public void SetRadius(float f) {
            radius = f;
        }
    }
}