using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] BallSettings settings;
    [SerializeField] LayerMask mask;

    [Space]
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] MeshRenderer graphic;

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

        Destroy(other.gameObject);

        onMerge.Invoke(this);
        onLocalMerge.Invoke();
        
        SetScale(_scaleIndex + 1);
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
        if (mask.Excludes(other.gameObject.layer)) return;

        if (other.gameObject.TryGetComponent(out BallController otherBall))
            if (GetScale() == otherBall.GetScale())
                Merge(otherBall);

    }
}
