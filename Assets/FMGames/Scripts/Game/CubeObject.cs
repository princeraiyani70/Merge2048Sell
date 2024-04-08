using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FMGames
{

    public class CubeObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        public List<Rigidbody> rb_Man;
        public Material rendererR;
        public Texture rendererColor;
        [SerializeField] private Collider collider;
        public List<Collider> colliders;

        private CubeData cubeData;
        public CubeData CubeData { get => cubeData; private set => cubeData = value; }

        [SerializeField] private TMP_Text tmpText;

        [Space(12)]
        [Header("Audio")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] hitSound, mergeSound;

        private bool isBouncing = true;

        private float mergeTimer;


        private void Update()
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
        }

        private void FixedUpdate()
        {
            mergeTimer += Time.fixedDeltaTime;
        }

        public void Init(CubeData data)
        {
            rb.useGravity = false;
            rb.isKinematic = true;

            for (int i = 0; i < rb_Man.Count; i++)
            {
                rb_Man[i].useGravity = false;
                rb_Man[i].isKinematic = true;
            }

            collider.enabled = false;
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].enabled = false;
            }

            CubeData = data;
            SetDesign();

            transform.localScale = new Vector3(CubeData.Scale, CubeData.Scale, 0.7f);
        }

        private void OnCollisionEnter(Collision col)
        {
            PlayPopEffect();

            CubeObject otherCube = col.gameObject.GetComponent<CubeObject>();
            if (otherCube != null)
            {
                DisableBouncing();
                TryMerge(otherCube);
            }

            //if (col.gameObject.tag== "Character")
            //{
            //    DisableBouncing();
            //    //TryMerge(col.gameObject.GetComponentInParent<CubeObject>());
            //}

            if (col.gameObject.CompareTag("BottomBorder"))
            {
                DisableBouncing();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Death"))
            {
                if (!isBouncing)
                {
                    GameManager._instance.GameOver(false);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Death"))
            {
                if (!isBouncing)
                {
                    GameManager._instance.GameOver(false);
                }
            }
        }

        private void OnCollisionStay(Collision col)
        {
            CubeObject otherCube = col.gameObject.GetComponent<CubeObject>();
            if (otherCube != null)
            {
                TryMerge(otherCube);
            }
        }

        [ContextMenu("Shoot")]
        private void ShootFromInspector()
        {
            Shoot(7);
        }

        public void Shoot(float speed)
        {
            transform.SetParent(null);

            collider.enabled = true;
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].enabled = true;
            }
            rb.isKinematic = false;

            rb.velocity = transform.up * speed;

            for (int i = 0; i < rb_Man.Count; i++)
            {
                rb_Man[i].isKinematic = true;
                rb_Man[i].velocity = transform.up * speed;
            }

        }

        /// <summary>
        /// Checks if merging is possible with the collided other cube
        /// </summary>
        /// <param name="otherCube"></param>
        private void TryMerge(CubeObject otherCube)
        {
            if (otherCube.CubeData.Id != CubeData.Id) return;
            if (mergeTimer < 0.5f || otherCube.mergeTimer < 0.5f) return;

            CubeObject toDestroy, toUpgrade;
            if (otherCube.transform.position.y > transform.position.y)
            {
                toDestroy = this;
                toUpgrade = otherCube;
            }
            else
            {
                toDestroy = otherCube;
                toUpgrade = this;
            }

            GameManager._instance.GetLevel().MergeCubes(toUpgrade, toDestroy, DataCollection._instance.CubeDatas[CubeData.Id + 1].Value);

            Destroy(toDestroy.gameObject);

            toUpgrade.MergeUpgrade(1);
        }

        /// <summary>
        /// Upgrades the cube
        /// </summary>
        /// <param name="levelsToAdd"></param>
        public void MergeUpgrade(int levelsToAdd)
        {
            mergeTimer = 0;

            CubeData = DataCollection._instance.CubeDatas[CubeData.Id + levelsToAdd];
            SetDesign();

            StopAllCoroutines();
            StartCoroutine(MergeCoroutine());

            PlayMergeEffect();
        }

        private void DisableBouncing()
        {
            collider.material.bounciness = 0;

            collider.material.dynamicFriction = 0.5f;
            collider.material.staticFriction = 0.5f;
            collider.material.frictionCombine = PhysicMaterialCombine.Maximum;

            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].material.bounciness = 0;
                colliders[i].material.dynamicFriction = 0.5f;
                colliders[i].material.staticFriction = 0.5f;
                colliders[i].material.frictionCombine = PhysicMaterialCombine.Maximum;
            }

            rb.useGravity = true;

            for (int i = 0; i < rb_Man.Count; i++)
            {
                rb_Man[i].useGravity = true;
            }

            isBouncing = false;
        }

        /// <summary>
        /// Changes the scale of the cube with lerping
        /// </summary>
        /// <returns></returns>
        private IEnumerator MergeCoroutine()
        {
            Vector3 baseScale = transform.localScale;
            Vector3 endScale = new Vector3(baseScale.x * 0.85f, baseScale.x * 0.85f, baseScale.z);
            Vector3 endScale2 = new Vector3(CubeData.Scale, CubeData.Scale, 0.7f);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 3;
                transform.localScale = Vector3.Lerp(baseScale, endScale, t);

                yield return null;
            }

            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 3;
                transform.localScale = Vector3.Lerp(endScale, endScale2, t);

                yield return null;
            }

            transform.localScale = endScale2;
        }

        private void SetDesign()
        {
            rendererR = CubeData.BaseMaterial;
            //rendererColor.c = CubeData.BaseMaterial;
            tmpText.text = CubeData.Value + "";
        }

        private void PlayPopEffect()
        {
            if (audioSource.isPlaying) return;
            if (rb.velocity.magnitude < 2) return;

            audioSource.clip = hitSound[Random.Range(0, hitSound.Length)];
            audioSource.volume = 0.6f;

            audioSource.Play();
        }

        private void PlayMergeEffect()
        {
            audioSource.clip = mergeSound[Random.Range(0, mergeSound.Length)];
            audioSource.volume = 1.0f;
            audioSource.Play();
        }

        private void OnMouseUpAsButton()
        {
            PowerupManager._instance.OnClickCube(this);
        }



        [ContextMenu("Set Cube Data 7")]
        private void InitWithCube7()
        {
            Init(DataCollection._instance.CubeDatas[7]);
        }

        [ContextMenu("Set Cube Data 8")]
        private void InitWithCube8()
        {
            Init(DataCollection._instance.CubeDatas[8]);
        }

        [ContextMenu("Set Cube Data 9")]
        private void InitWithCube9()
        {
            Init(DataCollection._instance.CubeDatas[9]);
        }

        [ContextMenu("Set Cube Data 10")]
        private void InitWithCube10()
        {
            Init(DataCollection._instance.CubeDatas[10]);
        }
    }
}