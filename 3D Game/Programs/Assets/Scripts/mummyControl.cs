using UnityEngine;
using System.Collections;

public class mummyControl : MonoBehaviour 
{
    Rigidbody rb;
    Animator ani;
    float walk = 5f;
    bool isRunning = false;
    Vector3[] unit = new Vector3[4] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    int direction = 0;
    Map map;
    int move = 0;
    int[] vectorMove = new int[2] { -2, -1 };
    public void setPositon(int px, int py)
    {
        rb.position = new Vector3(-2.5f + py, 0, 2.5f - px);
        if (direction != 0)
        {
            Quaternion q = Quaternion.AngleAxis(-90 * direction, Vector3.up);
            rb.MoveRotation(rb.transform.rotation * q);
            direction = 0;
        }
        vectorMove[0] = -2;
        move = 0;
    }
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        map = new Map();
    }
    public void FixedUpdate()
    {
        if (gameControl.Win || gameControl.Lose) return;
        if (Map.enimyMove == false||Map.gameControl) return;
        if (move == 2) { Map.enimyMove = false; move = 0;vectorMove[0] = -2; return; }
        if (vectorMove[0] == -2)
        {
            Vector2Int t=map.mummyUpdate();
            vectorMove[0] = t.x;
            vectorMove[1] = t.y;
        }
        if (vectorMove[move]==0)
        {
                isRunning = true;
            if (direction != 0)
            {
                Quaternion q = Quaternion.AngleAxis(-90 * direction, Vector3.up);
                rb.MoveRotation(rb.transform.rotation * q);
                direction = 0;
            }
            vectorMove[move] = -1;
        }
        else if (vectorMove[move]==1)
        {
                isRunning = true;
            if (direction != 1)
            {
                Quaternion q = Quaternion.AngleAxis(-90 * direction + 90, Vector3.up);
                rb.MoveRotation(rb.transform.rotation * q);
                direction = 1;
            }
            vectorMove[move] = -1;
        }
        else if (vectorMove[move]==2)
        {
                isRunning = true;
            if (direction != 2)
            {
                Quaternion q = Quaternion.AngleAxis(-90 * direction + 180, Vector3.up);
                rb.MoveRotation(rb.transform.rotation * q);
                direction = 2;
            }
            vectorMove[move] = -1;
        }
        else if (vectorMove[move]==3)
        {
                isRunning = true;
            if (direction != 3)
            {
                Quaternion q = Quaternion.AngleAxis(90 * (3 - direction), Vector3.up);
                rb.MoveRotation(rb.transform.rotation * q);
                direction = 3;
            }
            vectorMove[move] = -1;
        }

        if (isRunning)
        {
            ani.SetBool("isRun", true);
            ani.Play("Run");
            rb.position = rb.position + 2 * unit[direction] * Time.deltaTime;
            walk -= 0.2f;

        }
        else if(vectorMove[move]==-1)
        {
            move++;
        }

        if (walk <= 0)
        {
            rb.position = new Vector3(Mathf.Floor(rb.position.x) + 0.5f, 0, Mathf.Floor(rb.position.z) + 0.5f);
            isRunning = false;
            ani.SetBool("isRun", false);
            ani.Play("Idle");
            walk = 5f;
            move++;
            Map.gameControl = true;
        }
        Debug.Log("mummy:" + Map.mummy.ToString());
    }
}