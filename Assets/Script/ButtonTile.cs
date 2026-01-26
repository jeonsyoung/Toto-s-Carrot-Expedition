using UnityEngine;

public class ButtonTile : MonoBehaviour
{
    public Door targetDoor;

    bool isPlayerOn = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerOn = true;
        other.GetComponent<IngameCoding>().currentButton = this;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerOn = false;
        other.GetComponent<IngameCoding>().currentButton = null;
    }

    public void Press()
    {
        if (!isPlayerOn)
        {
            Debug.Log("버튼 누르기 실패 (위에 아님)");
            return;
        }

        Debug.Log("버튼 눌림!");
        targetDoor.Open();
    }
}
