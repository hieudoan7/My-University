#ifndef DINOSAUR_H
#define DINOSAUR_H
#include "Obstacle.h"
#include "Draw.h"
class Dinosaur :public Obstacle{
private:
	vector<Point> m_Pos;
public:
	void init(int x, int y){
		m_Pos.resize(4);
		m_Pos[0] = Point(x, y);
		if (x == 83) x = 0;
		m_Pos[1] = Point(x + 1, y);
		if (x + 1 == 83) x = -1;
		m_Pos[2] = Point(x + 2, y);
		m_Pos[3] = Point(x + 2, y - 1);
	}
	void draw(){
		setColor(5+9-1);
		GotoXY(m_Pos[0]); cout << (char)220;
		GotoXY(m_Pos[1]); cout << (char)219;
		GotoXY(m_Pos[2]); cout << (char)221;
		GotoXY(m_Pos[3]); cout << (char)220;

	}
	void erase(){
		for (int i = 0; i < m_Pos.size(); i++){
			GotoXY(m_Pos[i]);
			cout << " ";
		}
	}
	void move(){
		for (int i = 0; i < m_Pos.size(); i++){
			m_Pos[i].turnLeft();
		}
	}
	void tell(){
		cout << "Tao la khung long! ";
	}
	Point getPoint(int pos){
		return m_Pos[pos];
	}
	int getSize(){
		return m_Pos.size();
	}
	void setLight(int k){};
};
#endif