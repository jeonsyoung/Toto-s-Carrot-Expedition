using System.Collections;
using UnityEngine;

public class IngameCoding : MonoBehaviour
{
    Vector3 startPos;
    RabbitDir startDir;

    public enum RabbitDir
    {
        Front,
        Right,
        Back,
        Left
    }

    public Sprite rabbit_Front;
    public Sprite rabbit_Right;
    public Sprite rabbit_Left;
    public Sprite rabbit_Back;

    public SpriteRenderer cur_Rabbit;
    public RabbitDir curDir;

    public float moveDistance = 1.5f;
    public LayerMask pathLayer;   // Path 레이어

    BoxCollider2D col;
    
    bool reachedPortal = false;

    bool isTouchingCarrot = false;
    bool hasCarrot = false;
    GameObject carrot;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        startDir = curDir;

        carrot = GameObject.FindGameObjectWithTag("Carrot");
    }


    void Start()
    {
        curDir = RabbitDir.Right;
        startDir = curDir;
        startPos = transform.position;
        UpdateSprite();
    }


    public IEnumerator PushButton() { yield return new WaitForSeconds(1f); }
    public IEnumerator CarrotGet()
    {
        if (isTouchingCarrot && !hasCarrot)
        {
            hasCarrot = true;

            if (carrot != null)
                carrot.SetActive(false);

            LevelManager.Instance.ShowCarrotIcon();
        }

        yield return new WaitForSeconds(1f);
    }



    public void ResetRabbit()
    {
        StopAllCoroutines();
        transform.position = startPos;
        curDir = startDir;
        reachedPortal = false;

        hasCarrot = false;
        isTouchingCarrot = false;

        //  당근 다시 등장
        if (carrot != null)
            carrot.SetActive(true);

        LevelManager.Instance.HideCarrotIcon();

        UpdateSprite();
    }



    // ================= 이동 =================
    public IEnumerator MoveForward()
    {
        Vector3 dir = GetDirectionVector();
        Vector3 prevPos = transform.position;

        //일단 이동
        transform.position += dir * moveDistance;

        //물리 판정 한 프레임 대기
        yield return new WaitForFixedUpdate();

        //길인지 체크
        if (!IsOnPath())
        {
            // 길 아니면 되돌리기
            transform.position = prevPos;
        }

        yield return new WaitForSeconds(1f);
    }

    bool IsOnPath()
    {
        // 콜라이더 중심 기준으로 겹침 검사
        Collider2D hit = Physics2D.OverlapBox(
            col.bounds.center,
            col.bounds.size * 0.8f,
            0f,
            pathLayer
        );

        return hit != null;
    }

    // ================= 회전 =================
    public IEnumerator TurnRight()
    {
        curDir = (RabbitDir)(((int)curDir + 1) % 4);
        UpdateSprite();
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator TurnLeft()
    {
        curDir = (RabbitDir)(((int)curDir + 3) % 4);
        UpdateSprite();
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator Jump()
    {
        Vector3 prevPos = transform.position;
        Vector3 dir = GetDirectionVector();

        transform.position += dir * moveDistance * 2;
        yield return new WaitForFixedUpdate();

        if (!IsOnPath())
            transform.position = prevPos;

        yield return new WaitForSeconds(1f);
    }

    // ================= 공통 =================
    Vector3 GetDirectionVector()
    {
        switch (curDir)
        {
            case RabbitDir.Front: return Vector3.down;
            case RabbitDir.Right: return Vector3.right;
            case RabbitDir.Back: return Vector3.up;
            case RabbitDir.Left: return Vector3.left;
        }
        return Vector3.zero;
    }

    void UpdateSprite()
    {
        switch (curDir)
        {
            case RabbitDir.Front: cur_Rabbit.sprite = rabbit_Front; break;
            case RabbitDir.Right: cur_Rabbit.sprite = rabbit_Right; break;
            case RabbitDir.Left: cur_Rabbit.sprite = rabbit_Left; break;
            case RabbitDir.Back: cur_Rabbit.sprite = rabbit_Back; break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Carrot"))
        {
            isTouchingCarrot = true;
        }
        else if (other.CompareTag("Portal"))
        {
            reachedPortal = true;
            LevelManager.Instance.OnReachPortal();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Carrot"))
        {
            isTouchingCarrot = false;
        }
    }



    public bool HasReachedPortal()
    {
        return reachedPortal;
    }

}
