using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
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

    [SerializeField]
    private GameObject _prefabEffect;
    [SerializeField]
    private Transform _effectTransform;


    private void Awake()
    {
        // FrameSet
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        GetNextBall();
    }

    //�V�����{�[�������
    Ball CreateBall()
    {
        return Instantiate(_prefabBall, _rootTransform).GetComponent<Ball>();         
    }

    //�V�����{�[���G�t�F�N�g�����
    public ParticleSystem GetEffect()
    {
        return Instantiate(_prefabEffect, _effectTransform).GetComponent<ParticleSystem>();
    }


    // ���̃{�[�����擾
    void GetNextBall()
    {
        currentBall = CreateBall();
        currentBall.manager = this;
        currentBall.level = UnityEngine.Random.Range(0, maxLevel);
        currentBall.gameObject.SetActive(true);

        StartCoroutine("WaitForNextBall");
    }

    // �{�[����������܂ő҂��āA���̃{�[���쐬���˗�����
    IEnumerator WaitForNextBall()
    {
        while (currentBall !=null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(_nextBallDelay);
        GetNextBall();
    }

    // �^�b�`�֘A
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
