using System.Collections;
using UnityEngine;

public class IngameCoding : MonoBehaviour
{
    public static IngameCoding Instance;

    Vector3 startPos;
    RabbitDir startDir;

    public enum RabbitDir
    {
        Front,
        Right,
        Back,
        Left
    }

    Sprite rabbit_Front;
    Sprite rabbit_Right;
    Sprite rabbit_Left;
    Sprite rabbit_Back;

    public SpriteRenderer cur_Rabbit;
    public RabbitDir curDir;

    public float moveDistance = 1.5f;

    BoxCollider2D col;
    
    bool reachedPortal = false;

    bool isTouchingCarrot = false;
    bool hasCarrot = false;
    GameObject carrot;

    public ButtonTile currentButton;

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
        ApplySkin();
    }

    public void ApplySkin()
    {
        int index = GameManager.Instance.currentSkinIndex;

        if (index < 0 || index >= GameManager.Instance.allSkins.Length)
            index = 0;

        PlayerSkin skin = GameManager.Instance.allSkins[index];

        rabbit_Front = skin.front;
        rabbit_Right = skin.right;
        rabbit_Left = skin.left;
        rabbit_Back = skin.back;

        UpdateSprite();
    }



    public IEnumerator PushButton()
    {
        if (currentButton != null)
            currentButton.Press();

        yield return new WaitForSeconds(1f);
    }

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
    public LayerMask pathLayer;
    public LayerMask obstacleLayer; // 문, 벽 레이어

    public IEnumerator MoveForward()
    {
        Vector3 dir = GetDirectionVector();
        Vector3 prevPos = transform.position;

        // 일단 이동
        transform.position += dir * moveDistance;

        // 물리 판정 기다림
        yield return new WaitForFixedUpdate();

        // 길이 아니거나 / 문에 막히면
        if (!IsOnPath() || IsBlocked())
        {
            transform.position = prevPos;
        }

        yield return new WaitForSeconds(1f);
    }

    bool IsBlocked()
    {
        Collider2D hit = Physics2D.OverlapBox(
            col.bounds.center,
            col.bounds.size * 0.9f,
            0f,
            obstacleLayer
        );

        if (hit != null)
            Debug.Log("문/장애물에 막힘: " + hit.name);

        return hit != null;
    }
    bool IsBlockedAt(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapBox(
            pos,
            col.bounds.size * 0.9f,
            0f,
            obstacleLayer
        );

        return hit != null;
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

        Vector3 midPos = prevPos + dir * moveDistance;
        Vector3 endPos = prevPos + dir * moveDistance * 2;

        // 중간 칸 → 문(Obstacle)만 체크
        if (IsBlockedAt(midPos))
        {
            Debug.Log("점프 실패: 중간에 문 있음");
            yield break;
        }

        transform.position = endPos;
        yield return new WaitForFixedUpdate();

        if (!IsOnPath() || IsBlocked())
        {
            transform.position = prevPos;
            yield break;
        }

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
