using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int code,row,col;
    public int health = 1;
    [SerializeField] Manager manager;
    private Vector3 mouseStartPos;
    private Vector3 mouseCurrentPos;
    public float dragThreshold = 0.5f;
    public bool isDragged;
    void OnMouseDown()
    {
        if (manager.movingObjects != 0) return;
        isDragged = false;
        mouseStartPos = manager.cam.ScreenToWorldPoint(Input.mousePosition);
        mouseStartPos.z = 0;
    }

    void OnMouseDrag()
    {
        if (manager.movingObjects != 0 || isDragged) return;
        mouseCurrentPos = manager.cam.ScreenToWorldPoint(Input.mousePosition);
        mouseCurrentPos.z = 0; // Keep it 2D

        Vector3 dragDirection = mouseCurrentPos - mouseStartPos;


        if (dragDirection.magnitude > dragThreshold)
        {
            isDragged = true;
            if (Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y))
            {
                if (dragDirection.x > 0)
                {
                    swapRight();
                    Debug.Log("Dragging Right");
                }
                    
                else
                {
                    swapLeft();
                    Debug.Log("Dragging Left");
                }
                  
            }
            else
            {
                if (dragDirection.y > 0)
                {
                    swapUp();
                    Debug.Log("Dragging Up");
                }
                else
                {
                    swapDown();
                    Debug.Log("Dragging Down");
                }
            }
            mouseStartPos = mouseCurrentPos;
        }
    }

    private void swapLeft()
    {
        manager.SwapBoxes(this, manager.getBox(row, col-1));
    }
    private void swapRight()
    {
        manager.SwapBoxes(this,manager.getBox(row, col+1));
    }
    private void swapDown()
    {
        manager.SwapBoxes(this, manager.getBox(row+1, col));
    }
    private void swapUp()
    {
        manager.SwapBoxes(this, manager.getBox(row-1, col));
    }
    public void moveBox(Vector2 targetPos)
    {
        StartCoroutine(move(targetPos));
    }
    IEnumerator move(Vector2 targetPos)
    {
        manager.movingObjects++;
        while((Vector2)transform.position!=targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, manager.gravitySpeed * Time.deltaTime);
            yield return null;
        }
        manager.movingObjects--;
    }
    public void selfDestroy()
    {
        health--;
        if(health<=0)
        {
            manager.removeBoxFromMatrix(this);
            gameObject.SetActive(false);
        }
    }

}
