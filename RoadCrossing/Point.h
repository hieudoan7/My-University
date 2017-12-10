#ifndef POINT_H
#define POINT_H
#pragma (once)
class Point
{
private:
	int m_X;
	int m_Y;
public:
	Point(){};
	Point(int x, int y);
	bool operator == (const Point& B);
	Point& operator= (const Point& B);
	void turnLeft();
	void turnRight();
	void turnUp();
	void turnDown();
	int getX();
	int getY();
};

#endif