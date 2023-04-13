using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // �ړ��͈�
    [SerializeField, Min(0f)] float extents = 4f;
    // ���x
    [SerializeField, Min(0f)] float speed = 10f;
    // ai(�G�l�~�[���̔���)
    // ai�̑��x�����̂܂�ܓ�Փx�ɂȂ�
    [SerializeField] bool isAl;

    /// <summary>
    /// �ړ�����
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
    /// Ai��
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
    /// Player�̏ꍇ
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
    /// �{�[�����͂˂鏈��
    /// </summary>
    public bool HitBall(float ballX,float ballExtents,out float hitFactor)
    {
        hitFactor = (ballX - transform.localPosition.x)/(extents+ballExtents);
        //-1����1�̊Ԃ�Ԃ�
        return -1f <= hitFactor && hitFactor <= 1f;
    }

}
