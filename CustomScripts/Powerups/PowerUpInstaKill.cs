using System.Collections;
using CustomScripts.Player;
using UnityEngine;

namespace CustomScripts.Powerups
{
    public class PowerUpInstaKill : MonoBehaviour, IPowerUp
    {
        public MeshRenderer Renderer;
        private Animator animator;

        private void Awake()
        {
            animator = transform.GetComponent<Animator>();
        }

        public void Spawn(Vector3 pos)
        {
            if (Renderer == null) // for error debugging
            {
                Debug.LogWarning("InstaKill spawn failed! renderer == null Tell Kodeman");
                return;
            }

            if (animator == null)
            {
                Debug.LogWarning("InstaKill spawn failed! animator == null Tell Kodeman");
                return;
            }

            transform.position = pos;
            Renderer.enabled = true;
            animator.Play("Rotating");
            StartCoroutine(DespawnDelay());
        }

        public void ApplyModifier()
        {
            PlayerData.Instance.InstaKill = true;

            StartCoroutine(DisablePowerUpDelay(30f));

            AudioManager.Instance.PowerUpInstaKillSound.Play();

            Despawn();
        }

        private IEnumerator DisablePowerUpDelay(float time)
        {
            yield return new WaitForSeconds(time);
            AudioManager.Instance.PowerUpDoublePointsEndSound.Play();
            PlayerData.Instance.InstaKill = false;
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