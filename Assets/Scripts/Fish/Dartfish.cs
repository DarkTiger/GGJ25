using UnityEngine;

/// <summary>
/// This fish roams around and when the player gets close, it will dart towards the player.
/// </summary>
public class Dartfish : Fish
{
    [Header("Dartfish")]
    public float AimDuration = 1.0f;
    public float ChargeDuration = 4.0f;
    public float ChargeSpeed = 20.0f;
    public Collider DetectionZone;
    public Collider HitBox;

    private Vector3 targetPosition;
    private Vector3 targetDirection;
    private float AimTime;
    private float ChargeTime;
    private bool isCharging = false;
    private bool isAiming = false;
    private GameObject aimedPlayer;

    protected override void Update()
    {

        if (isAiming)
        {
            AimTime -= Time.deltaTime;

            // Rotate the fish to face the player
            // if the player is to the right of the fish, rotate it to face right
            // also rotate on the z axis to additionally face towards the player


            float angleTowardPlayer = Mathf.Atan2(aimedPlayer.transform.position.y - transform.position.y, aimedPlayer.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

            if (aimedPlayer.transform.position.x < transform.position.x)
            {
                // Player is on the left side
                transform.eulerAngles = new Vector3(0f, 180f, -angleTowardPlayer);
                Debug.Log("Aiming left: " + transform.eulerAngles);
            }
            else
            {
                // Player is on the right side
                transform.eulerAngles = new Vector3(0f, 0f, angleTowardPlayer);
                Debug.Log("Aiming right: " + transform.eulerAngles);
            }


            if (AimTime <= 0)
            {
                isAiming = false;
                isCharging = true;
                ChargeTime = ChargeDuration;

                targetPosition = aimedPlayer.transform.position;
                targetDirection = (targetPosition - transform.position).normalized;
            }
        }

        if (isCharging)
        {
            if (ChargeTime <= 0)
            {
                isCharging = false;
            }

            // move towards the player, but don't stop charging until a timer runs out
            transform.position += targetDirection * ChargeSpeed * Time.deltaTime;

            ChargeTime -= Time.deltaTime;
        }

        if (!isCharging && !isAiming)
        {
            base.Update();
        }
    }

    private void StartAiming(GameObject player)
    {
        if (isCharging || isAiming)
        {
            return;
        }
        aimedPlayer = player;
        isAiming = true;
        AimTime = AimDuration;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>() != null)
        {
            StartAiming(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Player>() != null)
        {
            StartAiming(other.gameObject);
        }
    }



}