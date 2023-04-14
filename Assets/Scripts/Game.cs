using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Game : MonoBehaviour
{
    // パドル
    [SerializeField] Paddle bottomPadle;
    [SerializeField] Paddle topPadle;

    // ボール(複数化する場合は配列化しよう)
    [SerializeField] Ball ball;

    [SerializeField, Min(0f)] Vector2 arenaExtents = new Vector2(10f, 10f);

    [SerializeField, Min(2)] int pointToWin = 5;

    // 開始表示
    [SerializeField] TextMeshPro countDownText;

    [SerializeField, Min(1f)] float newGameDelay = 3f;

    float countdownUntilNewGame;
    // カメラ
    [SerializeField] LivelyCamera livelyCamera;
    private void Awake()
    {
        countdownUntilNewGame = newGameDelay;
    }
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    void StartNewGame()
    {
        ball.StartNewGame();
        bottomPadle.StartNewGame();
        topPadle.StartNewGame();
    }

    private void Update()
    {
        bottomPadle.Move(ball.Position.x, arenaExtents.x);
        topPadle.Move(ball.Position.x, arenaExtents.x);
        // カウントダウンが終わったら
        if(countdownUntilNewGame<=0f)
        {
            // ゲームの開始
            UpdateGame();
        }
        else
        {
            UpdateCountDown();
        }
    }

    /// <summary>
    /// ゲームの更新部分
    /// </summary>
    void UpdateGame()
    {
        ball.Move();
        BounceYIfNeeded();
        BounceXIfNeeded(ball.Position.x);
        ball.UpdateVisualization();
    }

    void UpdateCountDown()
    {
        countdownUntilNewGame -= Time.deltaTime;
        if (countdownUntilNewGame <= 0f)
        {
            // カウントテクストを非表示に
            countDownText.gameObject.SetActive(false);
            StartNewGame();
        }
        else
        {
            float displayValue = Mathf.Ceil(countdownUntilNewGame);
            countDownText.SetText("{0}", displayValue);
        }
    }
    /// <summary>
    /// ボールのy反発処理
    /// </summary>
    void BounceYIfNeeded()
    {
        float yExtents = arenaExtents.y - ball.Extents;
        if(ball.Position.y<-yExtents)
        {
            BounceY(-yExtents,bottomPadle,topPadle);
        }
        else if(ball.Position.y>yExtents)
        {
            BounceY(yExtents,topPadle,bottomPadle);
        }
    }

    void BounceY(float boundary,Paddle defender,Paddle attacker)
    {
        float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
        float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
        BounceXIfNeeded(bounceX);
        bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
        livelyCamera.PushXZ(ball.Velocity);
        ball.BounceY(boundary);
        if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
        {
            ball.SetPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
        // 勝ち点になったら
        else
        {
            livelyCamera.JostleY();
            if (attacker.ScorePoint(pointToWin))
            {
                EndGame();
            }
        }
    }

    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    void EndGame()
    {
        countdownUntilNewGame = newGameDelay;
        countDownText.SetText("GAME OVER");
        countDownText.gameObject.SetActive(true);
        ball.EndGame();
    }

    /// <summary>
    /// ボールのy反発処理
    /// </summary>
    void BounceXIfNeeded(float x)
    {
        float xExtents = arenaExtents.x - ball.Extents;
        if (x < -xExtents)
        {
            livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(-xExtents);
        }
        else if (x > xExtents)
        {
            livelyCamera.PushXZ(ball.Velocity);
            ball.BounceX(xExtents);
        }
    }
}
