#ifndef CAR_H
#define CAR_H
#include "Obstacle.h"
#include "Draw.h"
class Car :public Obstacle{
private:
	vector<Point> m_Pos;
	bool m_on; //tin hieu den 1: đỏ
public:
	void init(int x, int y){
		m_Pos.resize(4);
		m_Pos[0] = Point(x, y);
		if (x == 83) x = 0;
		m_Pos[1] = Point(x + 1, y);
		if (x + 1 == 83) x = -1;
		m_Pos[2] = Point(x + 2, y);
		if (x + 2 == 83) x = -2;
		m_Pos[3] = Point(x + 3, y);
		m_on = 0;
	}
	void draw(){
		setColor(1+9);
		GotoXY(m_Pos[0]); cout << (char)220;
		GotoXY(m_Pos[1]); cout << (char)219;
		GotoXY(m_Pos[2]); cout << (char)219;
		GotoXY(m_Pos[3]); cout << (char)220;
	}
	void erase(){
		for (int i = 0; i < m_Pos.size(); i++){
				GotoXY(m_Pos[i]);
				cout << " ";
		}
	}
	void move(){
		if (!m_on){
			for (int i = 0; i < m_Pos.size(); i++){
				m_Pos[i].turnRight();
			}
		}
	}
	void tell(){
		cout << "Tao la o to! ";
	}
	Point getPoint(int pos){
		return m_Pos[pos];
	}
	int getSize(){
		return m_Pos.size();
	}
	/*void changeLight(){
		m_light = 1 - m_light;
	}*/
	void setLight(int k){
		m_on = k;
	}
};
#endif