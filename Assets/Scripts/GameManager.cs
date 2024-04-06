using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball currentBall;

    [SerializeField]
    private GameObject _prefabBall;

    [SerializeField]
    private Transform _rootTransform;

    [SerializeField]
    private float _nextBallDelay;

    void Start()
    {
        GetNextBall();
    }

    //�V�����{�[�������
    Ball CreateBall()
    {
        return Instantiate(_prefabBall, _rootTransform).GetComponent<Ball>();         
    }

    // ���̃{�[�����擾
    void GetNextBall()
    {
        currentBall = CreateBall();

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
