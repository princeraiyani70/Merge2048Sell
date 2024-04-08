using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FMGames {
    public class ParticleManager : MonoBehaviour {
        [SerializeField] private GameObject poofEffect;
        [SerializeField] private GameObject comboEffect;

        [SerializeField] private TMP_Text tmpText;


        public static ParticleManager _instance;

        private void Awake() {
            _instance = this;
        }

        public void TMPTextPopup(string text, Vector3 worldPos) {
            var tmp = InstantiateEffect(tmpText.gameObject, worldPos + Vector3.right * Random.Range(-1f, 1f), Vector3.zero);
            tmp.GetComponent<TMP_Text>().text = text;

            StartCoroutine(TextPopupCoroutine(tmp.GetComponent<TMP_Text>()));
        }

        public void PoofEffect(Vector3 worldPos) {
            InstantiateEffect(poofEffect, worldPos, Vector3.zero);
        }

        public void ComboEffect(Vector3 worldPos) {
            InstantiateEffect(comboEffect, worldPos, Vector3.zero);
        }

        private GameObject InstantiateEffect(GameObject prefab, Vector3 worldPos, Vector3 eulerAngles) {
            var particle = Instantiate(prefab);

            particle.transform.position = worldPos;
            particle.transform.eulerAngles = eulerAngles;

            return particle;
        }

        private IEnumerator TextPopupCoroutine(TMP_Text tmp) {
            float t = 0;
            Color baseColor = tmp.color;

            while (t < 1) {
                t += Time.deltaTime;

                tmp.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t * 3);
                tmp.transform.position += Vector3.up * Time.deltaTime;

                tmp.color = Color.Lerp(baseColor, Color.clear, (t - 0.75f) * 4f);

                yield return null;
            }

            Destroy(tmp.gameObject);
        }
    }
}