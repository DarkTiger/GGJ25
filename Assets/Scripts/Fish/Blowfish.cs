using System.Collections;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;

public class Blowfish : Fish
{
    [Header("Blowfish Settings")]

    /// <summary>
    /// The area where the blowfish starts inflating
    /// </summary>
    public SphereCollider DangerZone;

    /// <summary>
    /// The hitbox of the fish. This is the area where the player gets hurt.
    /// </summary>
    public SphereCollider HitBox;

    public float InflationSpeed = 1.0f;
    
    public bool IsInflated = false;

    public float MaxHitBoxRadius => DangerZone.radius;
    public float InitialHitBoxRadius = 0.1f;

    public Animator Animator;


    void Start()
    {
        InitialHitBoxRadius = HitBox.radius;
        
        if (Animator == null)
        {
            Debug.LogError("Animator is not set in the inspector");
        }

        if (HitBox == null)
        {
            Debug.LogError("HitBox is not set in the inspector");
        }

        if (DangerZone == null)
        {
            Debug.LogError("DangerZone is not set in the inspector");
        }
    }

    protected override void Update()
    {
        base.Update();
        ResizeHitBox();
    }

    void ResizeHitBox()
    {
        if (IsInflated)
        {
            HitBox.radius += InflationSpeed * Time.deltaTime;
        }
        else
        {
            HitBox.radius -= InflationSpeed * Time.deltaTime;
        }

        HitBox.radius = Mathf.Clamp(HitBox.radius, InitialHitBoxRadius, MaxHitBoxRadius);
    }

    void Inflate()
    {
        if (IsInflated) return;
        IsInflated = true;
        DangerZone.enabled = true;

        Animator.SetBool("isInflated", true);
    }

    public override Collider GetHitBox()
    {
        return HitBox;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>(true) != null)
        {
            Debug.Log("Player entered the danger zone");
            Inflate();
        }
    }

    void Deflate()
    {
        if (!IsInflated) return;
        IsInflated = false;
        DangerZone.enabled = false;

        Animator.SetBool("isInflated", false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Player>(true) != null)
        {
            Debug.Log("Player left the danger zone");
            Deflate();
        }
    }
}
