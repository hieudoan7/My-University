using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class State
{
    public bool up=true, right=true, down=true, left=true;
   
}
 
public class Map
{
    public static State[][] map = new State[6][];
    public static Vector2Int goal = new Vector2Int();
    public static Vector2Int player = new Vector2Int();
    public static Vector2Int mummy = new Vector2Int();
    public static bool enimyMove = false;
    public static bool gameControl = false;
    public static int level = 0;
    
    public void reset(ref playerControl p,ref mummyControl m)
    {
        if (level == 0)
        {
            player = new Vector2Int(4, 1);
            mummy = new Vector2Int(1, 1);

            p.setPositon(4, 1);
            m.setPositon(1, 1);
            goal = new Vector2Int(0, -1);
        }
        else if(level==1)
        {
            player = new Vector2Int(2, 5);
            mummy = new Vector2Int(4, 4);

            p.setPositon(2, 5);
            m.setPositon(4, 4);
            goal = new Vector2Int(3, 6);
        }
        enimyMove = false;
        gameControl = false;
}
    public void Level1(ref playerControl p,ref mummyControl m)
    {
        map = new State[6][];
        for (int i = 0; i < 6; i++)
        {
            Map.map[i] = new State[6];
            for (int j = 0; j < 6; j++)
                Map.map[i][j] = new State();
        }
        for (int i = 0; i < 6; i++)
        {
            Map.map[0][i].up = false;
            Map.map[5][i].down = false;
            Map.map[i][0].left = false;
            Map.map[i][5].right = false;
        }


        player = new Vector2Int(4, 1);
        mummy = new Vector2Int(1, 1);

        p.setPositon(4, 1);
        m.setPositon(1, 1);
        goal = new Vector2Int(0, -1);
        
        Map.map[0][0].right = false;
        Map.map[0][1].left = false;
        Map.map[1][1].down = false;
        Map.map[1][3].down = false;
        Map.map[2][1].right = false;
        Map.map[2][1].up = false;
        Map.map[2][2].left = false;
        Map.map[2][2].down = false;
        Map.map[2][3].right = false;
        Map.map[2][3].up = false;
        Map.map[2][3].down = false;
        Map.map[2][4].left = false;
        Map.map[3][1].down = false;
        Map.map[3][2].up = false;
        Map.map[3][2].right = false;
        Map.map[3][3].up = false;
        Map.map[3][3].right = false;
        Map.map[3][3].left = false;
        Map.map[3][4].down = false;
        Map.map[3][4].left = false;
        Map.map[4][1].up = false;
        Map.map[4][1].down = false;
        Map.map[4][1].right = false;
        Map.map[4][2].down = false;
        Map.map[4][2].left = false;
        Map.map[4][3].down = false;
        Map.map[4][3].right = false;
        Map.map[4][4].up = false;
        Map.map[4][4].left = false;
        Map.map[5][1].up = false;
        Map.map[5][2].up = false;
        Map.map[5][3].up = false;
        Map.map[0][0].left = true;
    }
    public void Level2(ref playerControl p,ref mummyControl m)
    {
        map = new State[6][];
        for (int i = 0; i < 6; i++)
        {
            Map.map[i] = new State[6];
            for (int j = 0; j < 6; j++)
                Map.map[i][j] = new State();
        }
        for (int i = 0; i < 6; i++)
        {
            Map.map[0][i].up = false;
            Map.map[5][i].down = false;
            Map.map[i][0].left = false;
            Map.map[i][5].right = false;
        }


        player = new Vector2Int(2, 5);
        mummy = new Vector2Int(4, 4);

        p.setPositon(2, 5);
        m.setPositon(4, 4);
        goal = new Vector2Int(3, 6);
        map[0][1].down = false;
        map[0][4].down = false;
        map[1][1].up = false;
        map[1][1].right = false;
        map[1][1].down = false;
        map[1][2].left = false;
        map[1][4].up = false;
        map[1][5].down = false;
        map[2][1].up = false;
        map[2][1].right = false;
        map[2][2].left = false;
        map[2][2].down = false;
        map[2][3].right = false;
        map[2][4].left = false;
        map[2][4].down = false;
        map[2][5].up = false;
        map[2][5].down = false;
        map[3][2].up = false;
        map[3][2].down = false;
        map[3][3].right = false;
        map[3][3].down = false;
        map[3][4].up = false;
        map[3][4].left = false;
        map[3][5].up = false;
        map[4][0].right = false;
        map[4][1].left = false;
        map[4][2].up = false;
        map[4][3].up = false;
        map[4][4].down = false;
        map[5][1].right = false;
        map[5][2].left = false;
        map[5][3].right = false;
        map[5][4].up = false;
        map[5][4].left = false;
        map[3][5].right = true;
    }
    public bool isFail()
    {
        if (player == mummy) return true;
        return false;
    }
    public bool isWin()
    {
        if (player == goal) return true;
        return false;
    }
    public bool playerMove(int t)
    {
        if (t == 0)
            return map[player.x][player.y].up&&!(mummy.x==player.x-1&&mummy.y==player.y);
        if (t == 1)
            return map[player.x][player.y].right&& !(mummy.x == player.x && mummy.y == player.y+1);
        if (t == 2)
            return map[player.x][player.y].down&&!(mummy.x == player.x + 1 && mummy.y == player.y);
        
        return map[player.x][player.y].left&& !(mummy.x == player.x  && mummy.y == player.y-1);
    }
    public bool mummyMove(int t)
    {
        if (t == 0)
            return map[mummy.x][mummy.y].up;
        if (t == 1)
            return map[mummy.x][mummy.y].right;
        if (t == 2)
            return map[mummy.x][mummy.y].down;

        return map[mummy.x][mummy.y].left;
    }
    public void playerUpdate(int x,int y)
    {
        player.x += x;
        player.y += y;
    }
    public int moveUpDown()
    {
        int res = -1;
        if (player.x > mummy.x)
        {
            if (mummyMove(2))
            {
                res = 2;
                mummy.x++;
            }
        }
        else if (player.x < mummy.x)
        {
            if (mummyMove(0))
            {
                res = 0;
                mummy.x--;
            }
        }
        return res;
    }
    public int moveLeftRight()
    {
        int res = -1;
        if (player.y > mummy.y)
        {
            if (mummyMove(1))
            {
                res = 1;
                mummy.y++;
            }
        }
        else if (player.y < mummy.y)
        {
            if (mummyMove(3))
            {
                res = 3;
                mummy.y--;
            }
        }
        return res;
    }
    public Vector2Int mummyUpdate()
    {
        int[] r = new int[2] { -1, -1 };
        for (int i = 0; i < 2; i++)
        {
            if (player.y == mummy.y)
                r[i]=moveUpDown();
            else if (player.x == mummy.x)
                r[i] = moveLeftRight();
            else
            {
                r[i] = moveLeftRight();
                if (r[i] == -1)
                    r[i] = moveUpDown();
            }
            if (r[i] == -1) break;
        }
        Vector2Int res = new Vector2Int(r[0], r[1]);
        return res;
    }
}
public class gameControl : MonoBehaviour
{
    playerControl player;
    mummyControl mummy;
    public GameObject Player;
    public GameObject Mummy;
    public GameObject Characters;
    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject level1;
    public GameObject level2;
    public static bool reset = false;
    public static bool levelUp = false;
    public Map m;
    public static bool Win = false;
    public static bool Lose = false;
    public void Awake()
    {
        //creat map

        m = new Map();
        Win = false;
        Lose = false;
        // ...
        player = Player.GetComponent<playerControl>();
        mummy = Mummy.GetComponent<mummyControl>();
        
        player.Start();
        mummy.Start();
        m.Level1(ref player, ref mummy) ;

    }
    public void FixedUpdate()
    {
        if (!Map.gameControl) return;
        if(levelUp)
        {
            level1.SetActive(false);
            winPanel.SetActive(false);
            Characters.SetActive(true);
            level2.SetActive(true);
            m.Level2(ref player, ref mummy);
            levelUp = false;
            Win = false;
            Lose = false;
            Map.enimyMove = false;
        }
        else if(reset)
        {
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            Characters.SetActive(true);
            if (Map.level == 0)
                level1.SetActive(true);
            else
                level2.SetActive(true);
            m.reset(ref player,ref mummy);
            reset = false;
            Win = false;
            Lose = false;
           
            return;
        }
        else if (m.isFail())
        {
            Debug.Log("LOSE");
            //player.die();
            Characters.SetActive(false);
            if (Map.level == 0)
                level1.SetActive(false);
            else
                level2.SetActive(false);
            losePanel.SetActive(true);
            Lose = true;
        }
        else if(m.isWin())
        {
            Debug.Log("WIN");
            Characters.SetActive(false);
            if (Map.level == 0)
                level1.SetActive(false);
            else
                level2.SetActive(false);
            winPanel.SetActive(true);
            Win = true;
        }
        Map.gameControl = false;
    }
}
