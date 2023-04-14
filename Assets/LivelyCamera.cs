using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivelyCamera : MonoBehaviour
{
    [SerializeField, Min(0f)] float jostleStrength = 40f;
    [SerializeField, Min(0f)] float pushStrengh = 1f;
    [SerializeField, Min(0f)] float maxDeltaTime = 1f / 60f;
    [SerializeField, Min(0f)] float springStrength = 100f;
    [SerializeField, Min(0f)] float dampimgStrength = 10f;
    Vector3 velocity;
    Vector3 anchorPosition;

    private void Awake()
    {
        anchorPosition = transform.localPosition;
    }

    public void JostleY()
    {
        velocity.y += jostleStrength;
    }
    
    public void PushXZ(Vector2 impulse)
    {
        velocity.x += pushStrengh * impulse.x;
        velocity.z += pushStrengh * impulse.y;
    }

    private void LateUpdate()
    {
        float dt = Time.deltaTime;
        while(dt>maxDeltaTime)
        {
            TimeStep(maxDeltaTime);
            dt -= maxDeltaTime;
        }
        TimeStep(dt);
    }

    void TimeStep(float dt)
    {
        Vector3 displacement = anchorPosition - transform.localPosition;
        Vector3 ascceleration = springStrength * displacement - dampimgStrength * velocity;
        velocity += ascceleration * dt;
        transform.localPosition += velocity * dt;
    }
}
