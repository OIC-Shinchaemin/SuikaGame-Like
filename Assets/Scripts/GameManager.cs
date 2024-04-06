using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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


    private void Awake()
    {
        // FrameSet
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        GetNextBall();
    }

    //新しいボールを作る
    Ball CreateBall()
    {
        return Instantiate(_prefabBall, _rootTransform).GetComponent<Ball>();         
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
