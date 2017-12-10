#ifndef PEOPLE_H
#define PEOPLE_H
#pragma(once)
#include "Point.h"
#include "Obstacle.h"
#include <vector>
using namespace std;
class People
{
private:
	vector<Point> m_Pos;
	bool m_state;
public:
	People();
	People(int x, int y, bool sta);
	void draw();
	void erase();
	void up(); //di chuyen 1 buoc 3
	void down(); //buoc 3
	void left(); //buoc 1
	void right(); //buoc 1
	Point getPoint(int i);
	bool PeoVsObs(Obstacle* a);
	bool isImpact(vector<vector<Obstacle*> > matrix);
	bool isDead();
	bool isFinish();
	void dead(); //xu ly khi chet
	void setDead(){
		m_state = 0;
	}
	bool getState(){
		return m_state;
	}
}; 
#endif