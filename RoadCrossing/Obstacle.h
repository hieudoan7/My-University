#ifndef OBSTACLE_H
#define OBSTACLE_H
#include "Point.h"
#include <vector>
using namespace std;
class Obstacle
{
public:
	virtual void init(int,int)=0;
	virtual void move()=0;
	virtual void tell()=0;
	virtual void draw()=0;
	virtual void erase()=0;
	virtual Point getPoint(int pos) = 0;
	virtual int getSize() = 0;
	virtual void setLight(int k) = 0;
};
#endif
