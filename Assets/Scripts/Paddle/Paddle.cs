using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // 移動範囲
    [SerializeField, Min(0f)] float extents = 4f;
    // 速度
    [SerializeField, Min(0f)] float speed = 10f;
    // ai(エネミーかの判定)
    // aiの速度がそのまんま難易度になる
    [SerializeField] bool isAl;

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move(float target,float arenaExtents)
    {
        Vector3 position = transform.localPosition;
        position.x = isAl ? AdjustByAi(position.x, target) : AdjustByPlayer(position.x);
        float limit = arenaExtents - extents;
        position.x = Mathf.Clamp(position.x, -limit, limit);
        transform.localPosition = position;
    }

    /// <summary>
    /// Ai版
    /// </summary>
    /// <param name="x"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    float AdjustByAi(float x,float target)
    {
        if(x<target)
        {
            return Mathf.Min(x + speed * Time.deltaTime, target);
        }
        return Mathf.Max(x - speed * Time.deltaTime, target);
    }

    /// <summary>
    /// Playerの場合
    /// </summary>
    /// <returns></returns>
    float AdjustByPlayer(float x)
    {
        bool goRight = Input.GetKey(KeyCode.RightArrow);
        bool goLeft = Input.GetKey(KeyCode.LeftArrow);

        if(goRight&&!goLeft)
        {
            return x + speed * Time.deltaTime;
        }
        else if(goLeft&&!goRight)
        {
            return x - speed * Time.deltaTime;

        }
        return x;
    }

    /// <summary>
    /// ボールをはねる処理
    /// </summary>
    public bool HitBall(float ballX,float ballExtents,out float hitFactor)
    {
        hitFactor = (ballX - transform.localPosition.x)/(extents+ballExtents);
        //-1から1の間を返す
        return -1f <= hitFactor && hitFactor <= 1f;
    }

}
