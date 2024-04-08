using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FMGames {
    public class Shooter : MonoBehaviour {
        public static Shooter _instance;

        private void Awake() {
            _instance = this;
        }

        [SerializeField] private CubeObject cubePrefab;
        [SerializeField] private Transform fireTransform;

        private bool canShoot;

        [SerializeField] private TrajectorySimulator trajectory;
        [SerializeField] private float cubeShootSpeed = 5f;

        private CubeObject currentCube;

        private void Start() {
            canShoot = true;
            SpawnCube();
        }

        // Update is called once per frame
        void Update() {
            if (IsPointerOverUi()) return;

            if (PowerupManager._instance.CanShoot() && canShoot && !GameManager._instance.isPaused) {
                if (Input.GetMouseButtonUp(0))
                    Shoot();
            }

            Rotate();
        }

        private void Shoot() {
            canShoot = false;
            currentCube.Shoot(cubeShootSpeed);

            StartCoroutine(EnableShootAfterDelay(1.2f));
        }

        void SpawnCube() {
            CubeData data = DataCollection._instance.CubeDatas[GetCubeDataIdx()];
            trajectory.SetRadius(data.Scale / 2 * 1.12f);

            currentCube = Instantiate(cubePrefab, fireTransform);
            currentCube.Init(data);

            currentCube.transform.position = fireTransform.position;
            currentCube.transform.eulerAngles = transform.eulerAngles;
        }

        //Rotating the shooter object
        void Rotate() {
            if (Input.GetMouseButton(0) && canShoot) {
                trajectory.gameObject.SetActive(true);

                float angle = Mathf.Lerp(-65f, 65f, Input.mousePosition.x / Screen.width) + 180;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

            } else {
                trajectory.gameObject.SetActive(false);
            }
        }

        public IEnumerator EnableShootAfterDelay(float delay) {
            canShoot = false;
            yield return new WaitForSeconds(delay);

            canShoot = true;
            SpawnCube();
        }

        /// <summary>
        /// Randomly chooses the next cube's data
        /// </summary>
        /// <returns></returns>
        private int GetCubeDataIdx() {
            int n = Random.Range(0, 100);

            if (n < 40) return 0;
            if (n < 65) return 1;
            if (n < 85) return 2;
            return 3;
        }

        bool IsPointerOverUi() {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                // Check if finger is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                    return true;
                }
            }

            return EventSystem.current.IsPointerOverGameObject();
        }

    }
}