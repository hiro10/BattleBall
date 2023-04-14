using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Paddle : MonoBehaviour
{
    // �p�h���̑傫��
    [SerializeField, Min(0f)] float minExtents = 4f;
    [SerializeField, Min(0f)] float maxExtents = 4f;

    // ���x
    [SerializeField, Min(0f)] float speed = 10f;
    // ai�p�̗h�炬
    [SerializeField, Min(0f)] float maxTargetBias = 0.75f;

    float targetBias;
    // ai(�G�l�~�[���̔���)
    // ai�̑��x�����̂܂�ܓ�Փx�ɂȂ�
    [SerializeField] bool isAl;

    // �X�R�A�I�u�W�F�N�g
    [SerializeField] TextMeshPro scoreText = default;

    // �X�R�A
    int score;

    // �p�h���̒���
    float extents;

    static readonly int timeOfLasatHitId = Shader.PropertyToID("_TimeOfLastHit");
    static readonly int emissionColorId = Shader.PropertyToID("_EmissionColor");
    static readonly int faceColorId = Shader.PropertyToID("_FaceColor");
    

    // �S�[�������̃����_���[
    [SerializeField] MeshRenderer goalRenderer;
    // 
    [SerializeField, ColorUsage(true, true)] Color goalColor = Color.white;
    Material paddleMaterial;
    Material goalMaterial;
    Material scoreMaterial;
    private void Awake()
    {
        goalMaterial = goalRenderer.material;
        goalMaterial.SetColor(emissionColorId, goalColor);
        paddleMaterial = GetComponent<MeshRenderer>().material;
        scoreMaterial = scoreText.fontMaterial;
        SetScore(0);
    }
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
        target += targetBias * extents;
        if(x<target)
        {
            return Mathf.Min(x + speed * Time.deltaTime, target);
        }
        return Mathf.Max(x - speed * Time.deltaTime, target);
    }
    
    /// <summary>
    /// ai�p�̗h�炬�i�e�𒆐S�őł��Ԃ��Ȃ��悤�Ɂj
    /// </summary>
    void ChangeTargetBias()
    {
        targetBias = Random.Range(-maxTargetBias, maxTargetBias);
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
        ChangeTargetBias();
        hitFactor = (ballX - transform.localPosition.x)/(extents+ballExtents);
        //-1����1�̊Ԃ�Ԃ�
        bool success = -1f <= hitFactor && hitFactor <= 1f;

        if(success)
        {
            paddleMaterial.SetFloat(timeOfLasatHitId, Time.time);
        }
        return success;
        
    }

    /// <summary>
    /// �X�R�A����
    /// </summary>
    void SetScore(int newScore,float pointToWin=1000f)
    {
        score = newScore;
        scoreText.SetText("{0}", newScore);
        scoreMaterial.SetColor(faceColorId, goalColor * (newScore / pointToWin));
        SetExtents(Mathf.Lerp(maxExtents, minExtents, newScore / (pointToWin - 1f)));

    }

    /// <summary>
    /// �Q�[���J�n����
    /// </summary>
    public void StartNewGame()
    {
        SetScore(0);
        ChangeTargetBias();
    }

    /// <summary>
    /// ���_����
    /// </summary>
    /// <param name="posintsToWin"></param>
    /// <returns></returns>
    public bool ScorePoint(int posintsToWin)
    {
        goalMaterial.SetFloat(timeOfLasatHitId, Time.time);
        SetScore(score + 1,posintsToWin);
        return score >= posintsToWin;
    }
    
    void SetExtents(float newExtents)
    {
        extents = newExtents;
        Vector3 s = transform.localScale;
        s.x = 2f * newExtents;
        transform.localScale = s;
    }

}
