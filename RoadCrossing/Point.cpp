#include "Point.h"

Point::Point(int x, int y){
	m_X = x;
	m_Y = y;
}
bool Point::operator ==(const Point& B){
	return(m_X == B.m_X && m_Y == B.m_Y);
}
Point& Point::operator= (const Point& B){
	m_X = B.m_X;
	m_Y = B.m_Y;
	return (*this);
}
void Point::turnLeft(){
	m_X--;
	if (m_X <1 ) m_X = 83;
}
void Point::turnRight(){
	m_X++;
	if (m_X>83) m_X = 1;
}
void Point::turnUp(){
	m_Y--;
}
void Point::turnDown(){
	m_Y++;
}
int Point::getX(){
	return m_X;
}
int Point::getY(){
	return m_Y;
}