using System.Collections;
using Script.Manager;
using Script.Player;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float multiplier = 1.5f;
    public float duration = 5f;
    [SerializeField]private int powerNum;

    public GameObject pickupSpeedEffect;
    public GameObject pickupInvinEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Play("Powerup");
            RandomPower();
            StartCoroutine(Pickup(other));
        }
    }

    private IEnumerator Pickup(Collider player)
    {

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        switch (powerNum)
        {
            case 1:
                SpeedPower(player); //powerUp
                yield return new WaitForSeconds(duration);
                SpeedPowerReset(player); //ResetPowerUp
                break;
            case 2:
                Invincibility(player);
                if (!GameManager.Instance.isInvincibleCheatOn)
                {
                    yield return new WaitForSeconds(duration*2);
                    InvincibilityReset(player);
                }
                break;
        }

        Destroy(gameObject);
    }

    private void RandomPower()
    {
        powerNum = Random.Range(1,3);
    }

    private void SpeedPower(Collider playerCollider)
    {
        Instantiate(pickupSpeedEffect, transform.position, transform.rotation);
        GameManager.Instance.SpeedUpUI(true);
        var movement = playerCollider.GetComponent<PlayerLocomotion>();
        movement.movementSpeed *= multiplier;
        movement.sprintingSpeed *= multiplier;
    }
    private void SpeedPowerReset(Collider playerCollider)
    {
        var movement = playerCollider.GetComponent<PlayerLocomotion>();
        movement.movementSpeed /= multiplier;
        movement.sprintingSpeed /= multiplier;
        GameManager.Instance.SpeedUpUI(false);
    }

    private void Invincibility(Collider playerCollider)
    {
        Instantiate(pickupInvinEffect, transform.position, transform.rotation);
        GameManager.Instance.InvincUI(true);
        var invinc = playerCollider.GetComponent<PlayerManager>();
        invinc.godmode = true;
    }
    private void InvincibilityReset(Collider playerCollider)
    {
        GameManager.Instance.InvincUI(false);
        var invinc = playerCollider.GetComponent<PlayerManager>();
        invinc.godmode = false;
    }
}
