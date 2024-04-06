using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float TOP = 8.0f;
    private const float WIDTH = 4.0f;

    private float _leftBorder, _rightBorder;    
    private Rigidbody2D _rigidbody2D;
    
    public bool isDrag;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _leftBorder = -WIDTH + transform.localScale.x / 2f;
        _rightBorder = WIDTH - transform.localScale.x / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrag)
        {
            // マウスポジション
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 壁チェック
            if (mousePosition.x < _leftBorder) {
                mousePosition.x = _leftBorder;
            }else if (mousePosition.x > _rightBorder) {
                mousePosition.x = _rightBorder;
            }
            // Y、Zは固定
            mousePosition.y = TOP;
            mousePosition.z = 0;

            transform.position = mousePosition;
        }
    }

    public void Drag()
    {
        isDrag = true;
    }
    public void Drop()
    {
        isDrag= false;
        // 落ちるようにする
        GetComponent<Rigidbody2D>().simulated = true;
    }
}
