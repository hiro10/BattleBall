using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // x���̈ړ���
    [SerializeField, Min(0f)] float maxStartXSpeed = 2f;
    // ���̍ő�X�s�[�h
    [SerializeField, Min(0f)] float maxXSpeed = 20f;
    // z���̈ړ���
    [SerializeField, Min(0f)] float constantYSpeed = 10f;

    [SerializeField, Min(0f)] float extents = 0.5f;

    [SerializeField] ParticleSystem bounceParticle;
    [SerializeField] ParticleSystem startParticle;
    [SerializeField] ParticleSystem trailParticle;
    [SerializeField] int bounceParticleEmission = 20;
    [SerializeField] int startParticleEmission = 100;

    public float Extents => extents;
    public Vector2 Position => position;
    public Vector2 Velocity => velocity;

    // �ʒu
    Vector2 position;
    Vector2 velocity;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void UpdateVisualization()
    {
        trailParticle.transform.localPosition= new Vector3(position.x, 0f, position.y); 
        transform.localPosition = new Vector3(position.x, 0f, position.y);
    }

    public void Move()
    {
        position += velocity * Time.deltaTime;
    }

    public void StartNewGame()
    {
        position = Vector2.zero;
        UpdateVisualization();
        velocity.x = Random.Range(-maxStartXSpeed, maxStartXSpeed);
        velocity.y = -constantYSpeed;
        gameObject.SetActive(true);
        startParticle.Emit(startParticleEmission);
        SetTrailEmission(true);
        trailParticle.Play();
    }
    /// <summary>
    /// �Q�[���I�����̏���
    /// </summary>
    public void EndGame()
    {
        position.x = 0f;
        gameObject.SetActive(false);
        SetTrailEmission(false);
    }

    public void SetPositionAndSpeed(float start,float speedFactor,float deltaTime)
    {
        velocity.x = maxXSpeed * speedFactor;
        position.x = start + velocity.x * deltaTime;
    }

    /// <summary>
    /// x���̔�����
    /// </summary>
    /// <param name="boundary"></param>
    public void BounceX(float boundary)
    {
        float durationAfterBounce = (position.x - boundary) / velocity.x;
        position.x = 2f * boundary - position.x;
        velocity.x = -velocity.x;
        EmitBounceParticles(boundary, 
            position.y - velocity.y * durationAfterBounce,
            boundary < 0f ? 90f : 270f);
    }
    /// <summary>
    /// y���̔�����
    /// </summary>
    /// <param name="boundary"></param>
    public void BounceY(float boundary)
    {
        float durationAfterBounce = (position.y - boundary) / velocity.y;
        position.y = 2f * boundary - position.y;
        velocity.y = -velocity.y;
        EmitBounceParticles(position.x - velocity.x * durationAfterBounce,
            boundary,
            boundary < 0f ? 0f : 180f);
    }

    void EmitBounceParticles(float x, float z, float rotation)
    {
        ParticleSystem.ShapeModule shape = bounceParticle.shape;
        shape.position = new Vector3(x, 0f, z);
        shape.rotation = new Vector3(0f, rotation, 0f);
        bounceParticle.Emit(bounceParticleEmission);
    }

    void SetTrailEmission(bool enabled)
    {
        ParticleSystem.EmissionModule emission = trailParticle.emission;
        emission.enabled = enabled;
    }
}
