using System.Collections;
using CustomScripts.Gamemode.GMDebug;
using CustomScripts.Player;
using FistVR;
using UnityEngine;
using ItemSpawner = WurstMod.MappingComponents.Generic.ItemSpawner;

namespace CustomScripts.Powerups
{
    public class PowerUpDeathMachine : MonoBehaviour, IPowerUp
    {
        public MeshRenderer Renderer;
        private Animator animator;

        public CustomItemSpawner MinigunSpawner;
        public CustomItemSpawner MagazineSpawner;

        private FVRPhysicalObject MinigunObject;
        private FVRPhysicalObject MagazineObject;

        //private bool objectsSpawned = false;
        //private Vector3 spawnPos;

        private void Awake()
        {
            animator = transform.GetComponent<Animator>();
        }

        public void Spawn(Vector3 pos)
        {
            if (Renderer == null) // for error debugging
            {
                Debug.LogWarning("DeathMachine spawn failed! renderer == null Tell Kodeman");
                return;
            }

            if (animator == null)
            {
                Debug.LogWarning("DeathMachine spawn failed! animator == null Tell Kodeman");
                return;
            }

            transform.position = pos;
            Renderer.enabled = true;
            animator.Play("Rotating");
            StartCoroutine(DespawnDelay());
        }

        public void ApplyModifier()
        {
            MinigunSpawner.Spawn();
            MagazineSpawner.Spawn();

            //StartCoroutine(DelayedTest());

            StartCoroutine(DisablePowerUpDelay(30f));

            AudioManager.Instance.PowerUpDeathMachineSound.Play();

            Despawn();
        }

        // private IEnumerator DelayedTest()
        // {
        //     while (MinigunObject == null)
        //     {
        //         yield return new WaitForSeconds(.5f);
        //         Debug.Log("Waiting");
        //     }
        //
        //     Debug.Log("Almost FVRPhysicalObject");
        //     MinigunObject = MinigunSpawner.SpawnedObject.GetComponent<FVRPhysicalObject>();
        //     MinigunObject.SpawnLockable = false;
        //     MinigunObject.UsesGravity = false;
        //
        //     MagazineObject = MagazineSpawner.SpawnedObject.GetComponent<FVRPhysicalObject>();
        //     MagazineObject.SpawnLockable = false;
        //     MagazineObject.UsesGravity = false;
        // }

        private IEnumerator DisablePowerUpDelay(float time)
        {
            yield return new WaitForSeconds(time);
            AudioManager.Instance.PowerUpDoublePointsEndSound.Play();

            MinigunObject = MinigunSpawner.SpawnedObject.GetComponent<FVRPhysicalObject>();
            MinigunObject.SpawnLockable = false;
            MinigunObject.UsesGravity = false;

            MagazineObject = MagazineSpawner.SpawnedObject.GetComponent<FVRPhysicalObject>();
            MagazineObject.SpawnLockable = false;
            MagazineObject.UsesGravity = false;

            MinigunObject.ForceBreakInteraction();
            MinigunObject.IsPickUpLocked = true;
            Destroy(MinigunObject.gameObject);

            MagazineObject.ForceBreakInteraction();
            MagazineObject.IsPickUpLocked = true;
            Destroy(MagazineObject.gameObject);
        }

        private void Despawn()
        {
            transform.position += new Vector3(0, -1000f, 0);
        }

        private IEnumerator DespawnDelay()
        {
            yield return new WaitForSeconds(15f);

            for (int i = 0; i < 5; i++)
            {
                Renderer.enabled = false;
                yield return new WaitForSeconds(.3f);
                Renderer.enabled = true;
                yield return new WaitForSeconds(.7f);
            }

            Despawn();
        }

        // private IEnumerator CheckIfExists()
        // {
        //     while (!objectsSpawned)
        //     {
        //         yield return new WaitForSeconds(0.3f);
        //
        //         int maxColliders = 30;
        //         Collider[] hitColliders = new Collider[maxColliders];
        //         int numColliders = Physics.OverlapSphereNonAlloc(spawnPos, 20, hitColliders);
        //         for (int i = 0; i < numColliders; i++)
        //         {
        //             hitColliders[i].SendMessage("AddDamage");
        //
        //             FVRPhysicalObject obj = hitColliders[i].GetComponent<FVRPhysicalObject>();
        //
        //             if (obj && obj.ObjectWrapper != null)
        //             {
        //                 if (obj.IDSpawnedFrom.ItemID == "M134Minigun")
        //                 {
        //                     MinigunObject = obj;
        //                 }
        //                 else if (obj.IDSpawnedFrom.ItemID == "MagazineM134")
        //                 {
        //                     MagazineObject = obj;
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}