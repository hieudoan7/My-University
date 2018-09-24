#ifndef TRUCK_H
#define TRUCK_H
#include "Obstacle.h"
#include "Draw.h"
class Truck :public Obstacle{
private:
	vector<Point> m_Pos;
	bool m_on;
public:
	void init(int x, int y){
		m_Pos.resize(3);
		m_Pos[0] = Point(x, y);
		if (x == 83) x = 0;
		m_Pos[1] = Point(x + 1, y);
		if (x + 1 == 83) x = -1;
		m_Pos[2] = Point(x + 2, y);
		m_on = 0;
	}
	void draw(){
		setColor(6+9);
		GotoXY(m_Pos[0]); cout << (char)220;
		GotoXY(m_Pos[1]); cout << (char)219;
		GotoXY(m_Pos[2]); cout << (char)219;
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
				m_Pos[i].turnLeft();
			}
		}
	}
	void tell(){
		cout << "Tao la xe tai! ";
	}
	Point getPoint(int pos){
		return m_Pos[pos];
	}
	int getSize(){
		return m_Pos.size();
	}
	void setLight(int k){
		m_on = k;
	}
};
#endif