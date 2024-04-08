using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMGames {

    public class DataCollection : MonoBehaviour {
        public static DataCollection _instance;

        public static string LEVEL_NUM = "LEVEL_NUM";
        public static string POWERUP_ID = "POWERUP";

        [SerializeField] private CubeData[] cubeDatas;
        public CubeData[] CubeDatas { get => cubeDatas; }

        [SerializeField] private ThemeData[] themeDatas;


        private void Awake() {
            _instance = this;
        }

        public ThemeData GetThemeById(int id) {
            foreach (ThemeData t in themeDatas) {
                if (t.Id == id) return t;
            }

            return null;
        }

        [ContextMenu("Set level to 10")]
        public void AddLevel() {
            PlayerPrefs.SetInt(LEVEL_NUM, 10);
        }

        [ContextMenu("Set level to 20")]
        public void AddLevel2() {
            PlayerPrefs.SetInt(LEVEL_NUM, 20);
        }

        [ContextMenu("Set level to 30")]
        public void AddLevel3() {
            PlayerPrefs.SetInt(LEVEL_NUM, 30);
        }
    }

    [System.Serializable]
    public class CubeData {
        [SerializeField] private int id;
        [SerializeField] private int value;
        [SerializeField] private Material baseMaterial;
        [SerializeField] private float scale;

        public int Id { get => id; }
        public int Value { get => value; }
        public Material BaseMaterial { get => baseMaterial; }
        public float Scale { get => scale; }
    }

    [System.Serializable]
    public class ThemeData {
        [SerializeField] private int id;
        [SerializeField] private Material groundMaterial;
        [SerializeField] private Material borderMaterial;
        [SerializeField] private Sprite backgroundSprite;

        public int Id { get => id; }
        public Material GroundMaterial { get => groundMaterial; set => groundMaterial = value; }
        public Material BorderMaterial { get => borderMaterial; set => borderMaterial = value; }
        public Sprite BackgroundSprite { get => backgroundSprite; set => backgroundSprite = value; }
    }
}