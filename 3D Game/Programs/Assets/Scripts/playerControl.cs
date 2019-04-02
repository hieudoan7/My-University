using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    Rigidbody rb;
    Animator ani;
    float walk = 5f;
    bool isRunning = false;
    Vector3[] unit = new Vector3[4] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    Map map;
    
    int direction = 0;
    
    public void setPositon(int px,int py)
    {
        rb.position = new Vector3(-2.5f+py, 0, 2.5f-px);
       
        if (direction != 0)
        {
            Quaternion q = Quaternion.AngleAxis(-90 * direction, Vector3.up);
            rb.MoveRotation(rb.transform.rotation * q);
            direction = 0;
        }
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
        if (Map.gameControl||Map.enimyMove) return;
        bool idle = false;
        if (walk == 5f)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (map.playerMove(0))
                    isRunning = true;
                else idle = true;
                if (direction != 0)
                {
                    Quaternion q = Quaternion.AngleAxis(-90 * direction, Vector3.up);
                    rb.MoveRotation(rb.transform.rotation * q);
                    direction = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (map.playerMove(1))
                    isRunning = true;
                else idle = true;
                if (direction != 1)
                {
                    Quaternion q = Quaternion.AngleAxis(-90 * direction + 90, Vector3.up);
                    rb.MoveRotation(rb.transform.rotation * q);
                    direction = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (map.playerMove(2))
                    isRunning = true;
                else idle = true;
                if (direction != 2)
                {
                    Quaternion q = Quaternion.AngleAxis(-90 * direction + 180, Vector3.up);
                    rb.MoveRotation(rb.transform.rotation * q);
                    direction = 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (map.playerMove(3))
                    isRunning = true;
                else idle = true;
                if (direction != 3)
                {
                    Quaternion q = Quaternion.AngleAxis(90 * (3 - direction), Vector3.up);
                    rb.MoveRotation(rb.transform.rotation * q);
                    direction = 3;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                idle = true;
            }
        }
        if (isRunning)
        {
            ani.SetBool("IsRunning", true);
            ani.Play("Run");
            rb.position = rb.position + 2*unit[direction] * Time.deltaTime;
            walk -= 0.2f;
            if (walk <= 0)
            {
                rb.position = new Vector3(Mathf.Floor(rb.position.x) + 0.5f, 0, Mathf.Floor(rb.position.z) + 0.5f);
                isRunning = false;
                ani.SetBool("IsRunning", false);
                ani.Play("Idle");
                walk = 5f;
                Map.enimyMove = true;
                Map.gameControl = true;
                switch (direction)
                {
                    case 0:
                        map.playerUpdate(-1, 0);
                        break;
                    case 1:
                        map.playerUpdate(0, 1);
                        break;
                    case 2:
                        map.playerUpdate(1, 0);
                        break;
                    case 3:
                        map.playerUpdate(0, -1);
                        break;
                }
            }
        }
        else if(idle)
            Map.enimyMove = true;

        Debug.Log("player:" + Map.player.ToString());

    }

    public void die()
    {
        Quaternion q;
        
        q = Quaternion.AngleAxis(-90, Vector3.left);
        rb.MoveRotation(rb.transform.rotation * q);
        rb.position += unit[direction] * -0.5f;

    }
}
