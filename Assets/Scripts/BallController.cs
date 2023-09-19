using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] BallSettings settings;
    [SerializeField] LayerMask mask;

    [Space]
    [SerializeField] TrailRenderer trail;
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] MeshRenderer graphic;

    [Space]
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float explosionImpulse = 7f;

    public UnityEngine.Events.UnityEvent onLocalMerge = new();

    public static UnityEngine.Events.UnityEvent<BallController> onSpawn = new();
    public static UnityEngine.Events.UnityEvent<BallController> onDespawn = new();
    public static UnityEngine.Events.UnityEvent<BallController> onMerge = new();

    int _scaleIndex;

    public int GetScale() => _scaleIndex;

    public void SetScale(int scaleIndex)
    {
        graphic.transform.localScale = Vector3.one * settings.balls[scaleIndex].scale;
        graphic.material.SetColor("_BaseColor", settings.balls[scaleIndex].color);

        _scaleIndex = scaleIndex;
    }

    public void Launch(Vector3 dir, float force)
    {
        trail.enabled = true;

        col.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(dir * force, ForceMode.Impulse);
    }

    public void Freeze()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        col.enabled = false;
    }

    public void Merge(BallController other)
    {
        if (_scaleIndex >= settings.balls.Count - 1) return;

        rb.MovePosition(Vector3.Lerp(transform.position, other.transform.position, .5f));

        SetScale(_scaleIndex + 1);

        Destroy(other.gameObject);

        onMerge.Invoke(this);
        onLocalMerge.Invoke();

        var cols = Physics.OverlapSphere(transform.position, explosionRadius, mask);
        foreach (var item in cols)
        {
            //     print("AAAA: " + item.gameObject.name);
            if(item.transform.root.gameObject == gameObject) continue;
            if (item.transform.root.TryGetComponent(out BallController ball))
                ball.rb.AddExplosionForce(explosionImpulse, transform.position, explosionRadius, 5f);
        }

    }

    public void Spawned()
    {
        onSpawn.Invoke(this);
    }

    void OnDestroy()
    {
        onDespawn.Invoke(this);
    }

    void OnCollisionEnter(Collision other)
    {

        trail.enabled = false;
        if (mask.Excludes(other.gameObject.layer)) return;

        if (other.gameObject.TryGetComponent(out BallController otherBall))
            if (GetScale() == otherBall.GetScale())
                Merge(otherBall);

    }
}
