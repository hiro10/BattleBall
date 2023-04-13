using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // パドル
    [SerializeField] Paddle bottomPadle;
    [SerializeField] Paddle topPadle;

    // ボール(複数化する場合は配列化しよう)
    [SerializeField] Ball ball;

    [SerializeField, Min(0f)] Vector2 arenaExtents = new Vector2(10f, 10f);

    private void Awake()
    {
        ball.StartNewGame();
    }
    private void Update()
    {
        bottomPadle.Move(ball.Position.x, arenaExtents.x);
        topPadle.Move(ball.Position.x, arenaExtents.x);
        ball.Move();
        BounceYIfNeeded();
        BounceXIfNeeded(ball.Position.x);
        ball.UpdateVisualization();
    }
    /// <summary>
    /// ボールのy反発処理
    /// </summary>
    void BounceYIfNeeded()
    {
        float yExtents = arenaExtents.y - ball.Extents;
        if(ball.Position.y<-yExtents)
        {
            BounceY(-yExtents,bottomPadle);
        }
        else if(ball.Position.y>yExtents)
        {
            BounceY(yExtents,topPadle);
        }
    }

    void BounceY(float boundary,Paddle defender)
    {
        float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
        float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
        BounceXIfNeeded(bounceX);
        bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
        ball.BounceY(boundary);
        if(defender.HitBall(bounceX,ball.Extents,out float hitFactor))
        {
            ball.SetPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
    }

    /// <summary>
    /// ボールのy反発処理
    /// </summary>
    void BounceXIfNeeded(float x)
    {
        float xExtents = arenaExtents.x - ball.Extents;
        if (x < -xExtents)
        {
            ball.BounceX(-xExtents);
        }
        else if (x > xExtents)
        {
            ball.BounceX(xExtents);
        }
    }
}
