using System.Collections;
using UnityEngine;

public class IngameCoding : MonoBehaviour
{
    public enum RabbitDir
    {
        Front,
        Right,
        Left,
        Back
    }

    public Sprite rabbit_Front;
    public Sprite rabbit_Right;
    public Sprite rabbit_Left;
    public Sprite rabbit_Back;
    
    public SpriteRenderer cur_Rabbit;

    public RabbitDir curDir; // 현재 방향 상태

    private void Start()
    {
        // 시작 방향 (원하는 값으로)
        curDir = RabbitDir.Right;
        UpdateSprite();
    }

    // ================= 이동 =================
    public IEnumerator MoveForward()
    {
        Debug.Log("MoveForward 실행됨");
        Vector3 dir = Vector3.zero;

        switch (curDir)
        {
            case RabbitDir.Front:
                dir = Vector3.up;
                break;
            case RabbitDir.Right:
                dir = Vector3.right;
                break;
            case RabbitDir.Left:
                dir = Vector3.left;
                break;
            case RabbitDir.Back:
                dir = Vector3.down;
                break;
        }

        transform.position += dir * 1.5f;
        yield return new WaitForSeconds(1f);
    }

    // ================= 회전 =================
    public IEnumerator TurnRight()
    {
        switch (curDir)
        {
            case RabbitDir.Front:
                curDir = RabbitDir.Right;
                break;
            case RabbitDir.Right:
                curDir = RabbitDir.Back;
                break;
            case RabbitDir.Back:
                curDir = RabbitDir.Left;
                break;
            case RabbitDir.Left:
                curDir = RabbitDir.Front;
                break;
        }

        UpdateSprite();
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator TurnLeft()
    {
        switch (curDir)
        {
            case RabbitDir.Front:
                curDir = RabbitDir.Left;
                break;
            case RabbitDir.Left:
                curDir = RabbitDir.Back;
                break;
            case RabbitDir.Back:
                curDir = RabbitDir.Right;
                break;
            case RabbitDir.Right:
                curDir = RabbitDir.Front;
                break;
        }

        UpdateSprite();
        yield return new WaitForSeconds(1f);
    }

    // ================= 기타 =================
    public IEnumerator Jump() // 2칸 전진
    {
        Vector3 dir = GetDirectionVector();
        transform.position += dir * 3f; // 1.5f * 2
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator PushButton()
    {
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator CarrotGet()
    {
        yield return new WaitForSeconds(1f);
    }

    // ================= 공통 =================
    Vector3 GetDirectionVector()
    {
        switch (curDir)
        {
            case RabbitDir.Front: return Vector3.right;
            case RabbitDir.Right: return Vector3.up;
            case RabbitDir.Left: return Vector3.down;
            case RabbitDir.Back: return Vector3.left;
        }
        return Vector3.zero;
    }

    void UpdateSprite()
    {
        switch (curDir)
        {
            case RabbitDir.Front:
                cur_Rabbit.sprite = rabbit_Front;
                break;
            case RabbitDir.Right:
                cur_Rabbit.sprite = rabbit_Right;
                break;
            case RabbitDir.Left:
                cur_Rabbit.sprite = rabbit_Left;
                break;
            case RabbitDir.Back:
                cur_Rabbit.sprite = rabbit_Back;
                break;
        }
    }
}
