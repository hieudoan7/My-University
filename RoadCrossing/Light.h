#ifndef LIGHT_H
#define LIGHT_H
#include "Point.h"
#include "Draw.h"
class Light
{
private:
	Point m_pos;
	bool m_sta; //trang thai bat/ tat ~ do/xanh ~ 1/0
public:
	Light(){};
	Light(int x, int y, bool sta){
		m_pos = Point(x, y);
		m_sta = sta;
	}
	void draw(){
		if (m_sta) setColor(12);
		else setColor(10);
		GotoXY(m_pos);
		cout << (char)254;
	}
	void erase(){
		GotoXY(m_pos);
		cout << " ";
	}
	bool On(){
		return m_sta; 
	}
	bool getLight(){
		return m_sta;
	}
	void setLight(int k){
		m_sta = k;
	}

};
#endif