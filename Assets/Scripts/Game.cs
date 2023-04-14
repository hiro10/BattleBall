using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Game : MonoBehaviour
{
    // �p�h��
    [SerializeField] Paddle bottomPadle;
    [SerializeField] Paddle topPadle;

    // �{�[��(����������ꍇ�͔z�񉻂��悤)
    [SerializeField] Ball ball;

    [SerializeField, Min(0f)] Vector2 arenaExtents = new Vector2(10f, 10f);

    [SerializeField, Min(2)] int pointToWin = 5;

    // �J�n�\��
    [SerializeField] TextMeshPro countDownText;

    [SerializeField, Min(1f)] float newGameDelay = 3f;

    float countdownUntilNewGame;
    // �J����
    [SerializeField] LivelyCamera livelyCamera;
    private void Awake()
    {
        countdownUntilNewGame = newGameDelay;
    }
    
    /// <summary>
    /// ����������
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
        // �J�E���g�_�E�����I�������
        if(countdownUntilNewGame<=0f)
        {
            // �Q�[���̊J�n
            UpdateGame();
        }
        else
        {
            UpdateCountDown();
        }
    }

    /// <summary>
    /// �Q�[���̍X�V����
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
            // �J�E���g�e�N�X�g���\����
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
    /// �{�[����y��������
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
        // �����_�ɂȂ�����
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
    /// �Q�[���I�����̏���
    /// </summary>
    void EndGame()
    {
        countdownUntilNewGame = newGameDelay;
        countDownText.SetText("GAME OVER");
        countDownText.gameObject.SetActive(true);
        ball.EndGame();
    }

    /// <summary>
    /// �{�[����y��������
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
