using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMGames {
    public class ThemeManager : MonoBehaviour {
        [SerializeField] private int themeId;
        public int ThemeId { get => themeId; set => themeId = value; }


        [SerializeField] MeshRenderer[] borders;
        [SerializeField] MeshRenderer[] grounds;

        [SerializeField] SpriteRenderer spriteRenderer;


        // Start is called before the first frame update
        void Start() {
            SetTheme();
        }

        private void SetTheme() {
            ThemeData currentTheme = DataCollection._instance.GetThemeById(themeId);

            foreach (MeshRenderer mr in borders) {
                mr.material = currentTheme.BorderMaterial;
            }
            foreach (MeshRenderer mr in grounds) {
                mr.material = currentTheme.GroundMaterial;
            }

            spriteRenderer.sprite = currentTheme.BackgroundSprite;
        }
    }
}