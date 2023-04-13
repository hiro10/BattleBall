using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // x軸の移動力
    [SerializeField, Min(0f)] float startXSpeed = 8f;
    // ｘの最大スピード
    [SerializeField, Min(0f)] float maxXSpeed = 20f;
    // z軸の移動力
    [SerializeField, Min(0f)] float constantYSpeed = 10f;

    [SerializeField, Min(0f)] float extents = 0.5f;

    public float Extents => extents;
    public Vector2 Position => position;
    public Vector2 Velocity => velocity;

    // 位置
    Vector2 position;
    Vector2 velocity;
   
    public void UpdateVisualization()
    {
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
        velocity = new Vector2(startXSpeed, constantYSpeed);
    }

    public void SetPositionAndSpeed(float start,float speedFactor,float deltaTime)
    {
        velocity.x = maxXSpeed * speedFactor;
        position.x = start + velocity.x * deltaTime;
    }

    /// <summary>
    /// x軸の反発力
    /// </summary>
    /// <param name="boundary"></param>
    public void BounceX(float boundary)
    {
        position.x = 2f * boundary - position.x;
        velocity.x = -velocity.x;
    }
    /// <summary>
    /// y軸の反発力
    /// </summary>
    /// <param name="boundary"></param>
    public void BounceY(float boundary)
    {

        position.y = 2f * boundary - position.y;
        velocity.y = -velocity.y;
    }
}
