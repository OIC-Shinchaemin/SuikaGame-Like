using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{    
    private const float TOP = 7.5f;
    private const float WIDTH = 4.0f;
    private const int MAX_BALL_LEVEL = 7;

    private float _leftBorder, _rightBorder;    
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private Animator _animator;

    public bool isDrag;
    public bool isMerge;
    public GameManager manager;

    [Range(0, 1)]
    [SerializeField]
    private float _speed = 0;
    
    public int level;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.SetInteger("Level", level);    
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
            // �}�E�X�|�W�V����
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // �ǃ`�F�b�N
            if (mousePosition.x < _leftBorder) {
                mousePosition.x = _leftBorder;
            }else if (mousePosition.x > _rightBorder) {
                mousePosition.x = _rightBorder;
            }
            // Y�AZ�͌Œ�
            mousePosition.y = TOP;
            mousePosition.z = 0;

            transform.position = Vector3.Lerp(transform.position, mousePosition, _speed);
        }
    }

    public void OnDrag()
    {
        isDrag = true;
    }
    public void OnDrop()
    {
        isDrag= false;
        // ������悤�ɂ���
        GetComponent<Rigidbody2D>().simulated = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            var other = collision.gameObject.GetComponent<Ball>();
            
            if (level == other.level && !isMerge && !other.isMerge && level < MAX_BALL_LEVEL) { 
                float mx = transform.position.x;
                float my = transform.position.y;
                float ox = other.transform.position.x;
                float oy = other.transform.position.y;

                if (my < oy || (my == oy && mx > ox))
                {
                    other.Hide(transform.position);
                    LevelUp();
                }
            }
        }
    }

    public void Hide(Vector3 tpos)
    {
        isMerge = true;
        
        _rigidbody2D.simulated = false;
        _circleCollider2D.enabled = false;

        StartCoroutine(HideRoutine(tpos));
    }

    IEnumerator HideRoutine(Vector3 tpos)
    {
        // �A�j���[�V�����݂����Ɍ����邽�߂̃J�E���g
        int animcnt = 5;
        while (animcnt > 0) {
            animcnt--;
            transform.position = Vector3.Lerp(transform.position, tpos,1.0f);
            yield return null;
        }

        isMerge = false;
        //�����Ȃ��悤�ɂ���B
        gameObject.SetActive(false); 
        //�폜����B
        GameObject.Destroy(gameObject);
        yield return null;
    }

    void LevelUp()
    {
        isMerge = true;
        // ���x���Ȃ���
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0;

        StartCoroutine(LevelupRoutine());
    }

    IEnumerator LevelupRoutine()
    {
        //yield return new WaitForSeconds(0.05f);
        yield return null;
        // LevelUp�A�j���[�V��������
        _animator.SetInteger("Level", level + 1);
        
        // �G�t�F�N�g�\��
        var effect = manager.GetEffect();
        effect.transform.position = transform.position;
        effect.Play();

        // �A�j���[�V�����^�C�~���O�ɍ��킹��
        yield return new WaitForSeconds(0.3f);
        level++;

        // ��������Ball��MaxLevel��ς���
        manager.maxLevel = Mathf.Max(level,manager.maxLevel);

        isMerge = false;
    }
}
