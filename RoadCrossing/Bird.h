#ifndef BIRD_H
#define BIRD_H
#include "Obstacle.h"
#include "Draw.h"

class Bird :public Obstacle{
private:
	vector<Point> m_Pos;
public:
	void init(int x,int y){
		m_Pos.resize(3);
		m_Pos[0] = Point(x, y);
		if (x == 83) x = 0;
		m_Pos[1] = Point(x + 1, y);
		if (x+1 == 83) x = -1;
		m_Pos[2] = Point(x + 2, y);
	}
	void draw(){
		setColor(2+9);
		GotoXY(m_Pos[0]); cout << (char)223;
		GotoXY(m_Pos[1]); cout << (char)254;
		GotoXY(m_Pos[2]); cout << (char)223;
	}
	void erase(){
		for (int i = 0; i < m_Pos.size(); i++){
			GotoXY(m_Pos[i]);
			cout << " ";
		}
	}
	void move(){
		for (int i = 0; i < m_Pos.size(); i++){
			m_Pos[i].turnRight();
		}
	}
	void tell(){
		cout << "Tao la chim! ";
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