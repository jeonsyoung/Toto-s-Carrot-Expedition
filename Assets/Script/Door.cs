using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer sr;
    Collider2D col;

    bool isOpen = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponentInChildren<Collider2D>();
        Close();
    }

    public void Open()
    {
        isOpen = true;

        sr.enabled = false;     // 문 안 보이게
        col.enabled = false;    // 통과 가능

        Debug.Log("문 열림");
    }

    public void Close()
    {
        isOpen = false;

        sr.enabled = true;      // 문 보이게
        col.enabled = true;     // 막기

        Debug.Log("문 닫힘");
    }
}
