using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Paddle : MonoBehaviour
{
    // パドルの大きさ
    [SerializeField, Min(0f)] float minExtents = 4f;
    [SerializeField, Min(0f)] float maxExtents = 4f;

    // 速度
    [SerializeField, Min(0f)] float speed = 10f;
    // ai用の揺らぎ
    [SerializeField, Min(0f)] float maxTargetBias = 0.75f;

    float targetBias;
    // ai(エネミーかの判定)
    // aiの速度がそのまんま難易度になる
    [SerializeField] bool isAl;

    // スコアオブジェクト
    [SerializeField] TextMeshPro scoreText = default;

    // スコア
    int score;

    // パドルの長さ
    float extents;

    static readonly int timeOfLasatHitId = Shader.PropertyToID("_TimeOfLastHit");
    static readonly int emissionColorId = Shader.PropertyToID("_EmissionColor");
    static readonly int faceColorId = Shader.PropertyToID("_FaceColor");
    

    // ゴール部分のレンダラー
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
        target += targetBias * extents;
        if(x<target)
        {
            return Mathf.Min(x + speed * Time.deltaTime, target);
        }
        return Mathf.Max(x - speed * Time.deltaTime, target);
    }
    
    /// <summary>
    /// ai用の揺らぎ（弾を中心で打ち返さないように）
    /// </summary>
    void ChangeTargetBias()
    {
        targetBias = Random.Range(-maxTargetBias, maxTargetBias);
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
        ChangeTargetBias();
        hitFactor = (ballX - transform.localPosition.x)/(extents+ballExtents);
        //-1から1の間を返す
        bool success = -1f <= hitFactor && hitFactor <= 1f;

        if(success)
        {
            paddleMaterial.SetFloat(timeOfLasatHitId, Time.time);
        }
        return success;
        
    }

    /// <summary>
    /// スコア処理
    /// </summary>
    void SetScore(int newScore,float pointToWin=1000f)
    {
        score = newScore;
        scoreText.SetText("{0}", newScore);
        scoreMaterial.SetColor(faceColorId, goalColor * (newScore / pointToWin));
        SetExtents(Mathf.Lerp(maxExtents, minExtents, newScore / (pointToWin - 1f)));

    }

    /// <summary>
    /// ゲーム開始処理
    /// </summary>
    public void StartNewGame()
    {
        SetScore(0);
        ChangeTargetBias();
    }

    /// <summary>
    /// 加点処理
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
