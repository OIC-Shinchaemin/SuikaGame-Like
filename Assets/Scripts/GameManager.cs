using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int maxLevel;

    public Ball currentBall;

    [SerializeField]
    private GameObject _prefabBall;
    [SerializeField]
    private Transform _rootTransform;

    [SerializeField]
    private float _nextBallDelay;

    [SerializeField]
    private GameObject _prefabEffect;
    [SerializeField]
    private Transform _effectTransform;

    [SerializeField]
    private Text _scoreText;
    public int score;

    private void Awake()
    {
        // FrameSet
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        GetNextBall();
    }

    private void Update()
    {
        if (_scoreText != null)
        {
            _scoreText.text = score.ToString();
        }
    }

    //新しいボールを作る
    Ball CreateBall()
    {
        return Instantiate(_prefabBall, _rootTransform).GetComponent<Ball>();         
    }

    //新しいボールエフェクトを作る
    public ParticleSystem GetEffect()
    {
        return Instantiate(_prefabEffect, _effectTransform).GetComponent<ParticleSystem>();
    }


    // 次のボールを取得
    void GetNextBall()
    {
        currentBall = CreateBall();
        currentBall.manager = this;
        currentBall.level = UnityEngine.Random.Range(0, maxLevel);
        currentBall.gameObject.SetActive(true);

        StartCoroutine("WaitForNextBall");
    }

    // ボールが落ちるまで待って、次のボール作成を依頼する
    IEnumerator WaitForNextBall()
    {
        while (currentBall !=null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(_nextBallDelay);
        GetNextBall();
    }

    // タッチ関連
    public void OnTouchDown()
    {
        if (currentBall == null) return; 
        currentBall.OnDrag();
    }

    public void OnTouchUp() {
        if (currentBall == null) return;
        currentBall.OnDrop();
        currentBall = null;
    }
}
