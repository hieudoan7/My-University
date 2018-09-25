#include "People.h"
#include "Draw.h"
People::People(){
	m_Pos.resize(4); //4 cell to save People 's coordinates
	int rootX = 40, rootY = 30;
	m_Pos[0] = Point(rootX, rootY);
	m_Pos[1] = Point(rootX + 1, rootY);
	m_Pos[2] = Point(rootX + 2, rootY); //phải nhất
	m_Pos[3] = Point(rootX + 1, rootY - 1);
}
People::People(int x, int y, bool sta){
	m_Pos.resize(4); //4 cell to save People 's coordinates
	int rootX = x, rootY = y;
	m_Pos[0] = Point(rootX, rootY);
	m_Pos[1] = Point(rootX + 1, rootY);
	m_Pos[2] = Point(rootX + 2, rootY); //phải nhất
	m_Pos[3] = Point(rootX + 1, rootY - 1);
	m_state = sta;
}
void People::draw(){
	setColor(8);
	GotoXY(m_Pos[0]); cout << (char)220;
	GotoXY(m_Pos[1]); cout << (char)219;
	GotoXY(m_Pos[2]); cout << (char)220;
	GotoXY(m_Pos[3]); cout << (char)218;
}

void People::erase(){
	for (int i = 0; i < m_Pos.size(); i++){
		GotoXY(m_Pos[i]);
		cout << " ";
	}
}

void People::up(){
	for (int i = 0; i < 3; i++){
		for(int i=0;i<m_Pos.size();i++) m_Pos[i].turnUp();
	}
}
void People::down(){
	if (m_Pos[0].getY() == 30){
		return;
	}
	else{
		for (int i = 0; i < 3; i++){
			for (int i = 0; i < m_Pos.size(); i++) m_Pos[i].turnDown();
		}
	}
}
void People::left(){
	for (int i = 0; i < m_Pos.size(); i++){
		m_Pos[i].turnLeft();
	}
}
void People::right(){
	for (int i = 0; i < m_Pos.size(); i++){
		m_Pos[i].turnRight();
	}
}
Point People::getPoint(int i){
	return m_Pos[i];
}
bool People::PeoVsObs(Obstacle* a){
	int k = 0;
	for (int i = 0; i < a->getSize(); i++){
		for (k; k < m_Pos.size(); k++){
			if (a->getPoint(i) == m_Pos[k]){
				return true;
			}
		}
	}
	return false;
}
bool People::isImpact(vector<vector<Obstacle*> > matrix){
	for (int i = 0; i < matrix.size(); i++){
		for (int j = 0; j < matrix[0].size(); j++){
			if (this->PeoVsObs(matrix[i][j])) {
				m_state = 0;
				return true;
			}
		}
	}
	return false;
}
bool People::isDead(){
	return (m_state == 0);
}
void People::dead(){
	for (int i = 10; i <= 12; i++){
		Beep(523 + 300, 300); // 523 hertz (C5) for 500 milliseconds
		Beep(587 + 300, 300);
		setColor(i);
		GotoXY(m_Pos[0]); cout << (char)220;
		GotoXY(m_Pos[1]); cout << (char)219;
		GotoXY(m_Pos[2]); cout << (char)220;
		GotoXY(m_Pos[3]); cout << (char)218;
		Sleep(300);
	}
	
	Beep(659+300, 300);
	Beep(698+300, 300);
	Beep(784, 300);
	Sleep(1000);
	
}
bool People::isFinish(){
	return (m_Pos[0].getY() == 3);
}
