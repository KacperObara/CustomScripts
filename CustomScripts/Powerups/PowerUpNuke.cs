using System.Collections;
using CustomScripts.Managers;
using UnityEngine;

namespace CustomScripts
{
    public class PowerUpNuke : PowerUp
    {
        private Animator animator;
        public MeshRenderer Renderer;

        private void Awake()
        {
            animator = transform.GetComponent<Animator>();
        }

        public override void Spawn(Vector3 pos)
        {
            if (Renderer == null) // for error debugging
            {
                Debug.LogWarning("NukePowerUp spawn failed! renderer == null Tell Kodeman");
                return;
            }
            if (animator == null)
            {
                Debug.LogWarning("NukePowerUp spawn failed! animator == null Tell Kodeman");
                return;
            }

            transform.position = pos;
            Renderer.enabled = true;
            animator.Play("Rotating");
            StartCoroutine(DespawnDelay());
        }

        public override void ApplyModifier()
        {
            StartCoroutine(DelayedKillAll());

            AudioManager.Instance.PowerUpNukeSound.Play();

            Despawn();
        }

        // Partially to make better visuals, partially for better performance (not everything at once)
        private IEnumerator DelayedKillAll()
        {
            // Reversed loop, because I'm deleting elements from this array
            for (int i = ZombieManager.Instance.ExistingZombies.Count - 1; i >= 0; i--)
            {
                ZombieManager.Instance.ExistingZombies[i].OnHit(9999);
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
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
    }
}