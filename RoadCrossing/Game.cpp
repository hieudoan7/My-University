#include "Game.h"
#include "Bird.h"
#include "Dinosaur.h"
#include "Truck.h"
#include "Car.h"
#include "Draw.h"
#include <conio.h>
#include <string>
#include <fstream>
using namespace std;
People temp;

Game::Game(int lev){
	level = lev;
	music = 1;
	matrix.resize(8, vector<Obstacle*>(2 * level));
	for (int i = 0; i < 8; i++){
		int x = rand() % 79+1;
		int y = (i + 2) * 3;

		for (int j = 0; j < 2 * level; j++){
			if (i == 0 || i == 4) { matrix[i][j] = new Bird(); }
			if (i == 1 || i == 5) { matrix[i][j] = new Dinosaur(); }
			if (i == 2 || i == 6) { matrix[i][j] = new Truck(); }
			if (i == 3 || i == 7) { matrix[i][j] = new Car(); }
			matrix[i][j]->init(x,y);
			x += 5;
			if (x>83) x = x-83;//quay dau
		}
	}
		m_peo = People();
		m_light.resize(4); 
		m_light[0] = Light(1, 11, 0); //con co the in them 1 den o tren nua
		m_light[1] = Light(83, 14, 0);
		m_light[2] = Light(1, 23, 0);
		m_light[3] = Light(83, 26, 0);
}

void Game::drawGame(){
	//drawBoard();
	temp.erase();
	m_peo.draw();
	//drawline();
	/*for (int i = 0; i < 8; i++){
		for (int j = 0; j < level * 4; j++){
			matrix[i][j]->erase();
			matrix[i][j]->draw();
		}
	}*/
	GotoXY(100, 19); cout << level;
	//ve den giao thong
	for (int i = 0; i < m_light.size(); i++){
		m_light[i].draw();
	}

}
People Game::getPeople(){
	return m_peo;
}
vector<vector<Obstacle*> > Game::getObstacle(){
	return matrix;
}

void Game::resetGame(){
	*this=Game(1);
}

void Game::settingGame(){
	drawMenuSetting();
	GotoXY(65, 15);
	int tmp;
	cin >> tmp;
	this->music = tmp;
	//system("pause");
	//this->level = 2;

	//getchar();
}
void Game::exitGame(){
	system("cls");
	exit(0);
}
void Game::startGame(){
	int temp;
	int y = 13; //tung do hien tai cua pointer
	while (1){
		temp=toupper(getch());
		if (temp == 'S'){
			GotoXY(40, y); cout << " ";
			y++;
			if (y > 16) y = 13;
			GotoXY(40, y); cout << (char)254;
		}
		if (temp == 'W') {
			GotoXY(40, y); cout << " ";
			y--;
			if (y < 13) y = 16;
			GotoXY(40, y); cout << (char)254;
		}
		if (temp == 13) { //enter la 13
			switch (y){
			case 13:
				this->drawGame();
				break;
			case 14:
				this->loadGame();
				break;
			case 15:
				this->settingGame();
				break;
			case 16:
				this->exitGame();
			}
			break; //cho nay ms break while(1)
		}
	}
	
}
void Game::pauseGame(HANDLE t){
	SuspendThread(t);
}
void Game::resumeGame(HANDLE t){
	ResumeThread(t);
}
void Game::updatePosPeople(char c){
	temp = m_peo;
	switch (c){
	case 'A': 
		m_peo.left();
		break;
	case 'D':
		m_peo.right();
		break;
	case 'W':
		m_peo.up();
		break;
	case 'S':
		m_peo.down();
		break;
	}
}
void Game::updatePosObstacle(){
	for (int i = 0; i < m_light.size(); i++){
		int k = m_light[i].getLight();
		for (int j = 0; j < level * 2; j++){
			int iMax; //dòng iMax của Matrix chứa Obstacle loại i
			switch (i){
				case 0: iMax = 2; break;
				case 1: iMax = 3; break;
				case 2: iMax = 6; break;
				case 3: iMax = 7; break;
			}
			matrix[iMax][j]->setLight(k);
		}
	}
	for (int i = 0; i < 8; i++){
		for (int j = 0; j < level * 2; j++){
			matrix[i][j]->erase();
			matrix[i][j]->move();
			matrix[i][j]->draw();
		}
	}
}
void Game::cleanGame(){
	//m_peo.erase();
	for (int i = 0; i < matrix.size(); i++){
		for (int j = 0; j < matrix[0].size(); j++){
			matrix[i][j]->erase();
		}
	}
	for (int i = 0; i < m_light.size(); i++){
		m_light[i].erase();
	}
}

void Game::youWin(){
	cleanGame();
	this->getPeople().setDead();
	GotoXY(35, 15);
	cout << "CONGRATULATION!";
	GotoXY(25, 16);
	cout << "Press Y to replay game or anykey to exit!";
}


 
int Game::getLevel(){
	return level;
}

void Game::xulyDead(){
	this->getPeople().setDead();
	this->getPeople().dead();
	Sleep(1000);
	cleanGame(); //xóa vật cản
	this->getPeople().erase(); //xóa người
	GotoXY(35, 15); cout << "YOU LOSE!";
	GotoXY(22, 16);
	cout << "Press Y to replay or anykey to exit!";
}

void Game::xulyToiDich(){
	People tmp = this->getPeople();
	while (tmp.getPoint(2).getX() < (84 - 4 * level)){
		tmp.erase();
		tmp.right();
		tmp.draw();
		Sleep(10); //thao tac di chuyen cham lai
 		drawline(level);
	}
	tmp.draw();
	Sleep(500); //cho nó chậm lại
}

void Game::updateLight(){
	for (int i = 0; i < m_light.size(); i++){
		int k = rand() % 2;
		m_light[i].setLight(k);
	}
}

void Game::saveGame(){
	drawMenuSave();
	string name;
	getline(cin, name);
	name += ".bin";
	ofstream fout(name, ios::out | ios::binary);
	if (fout.is_open()){
		fout.seekp(0, ios::beg);
		fout.write((char*)&this->level, sizeof(int));
		int pX = this->getPeople().getPoint(0).getX();
		fout.write((char*)&pX, sizeof(int));
		int pY = this->getPeople().getPoint(0).getY();
		fout.write((char*)&pY, sizeof(int));

		int pState = this->getPeople().getState();
		fout.write((char*)&pState, sizeof(int));
		int x, y;
		for (int i = 0; i < 8; i++){
			x = matrix[i][0]->getPoint(0).getX();
			y = matrix[i][0]->getPoint(0).getY();
			fout.write((char*)&x, sizeof(int));
			fout.write((char*)&y, sizeof(int));
		}
	}
	else{ cout << "Can't save!"; }
	fout.close();
}
void Game::loadGame(){
	drawMenuLoad();
	string name;
	getline(cin, name);
	name += ".bin";
	ifstream fin(name, ios::in | ios::binary);
	m_peo.erase();
	this->cleanGame();
	if (fin.is_open()){
		//int f_lev; //bien doc data form file
		fin.seekg(0, ios::beg);
		fin.read((char*)&level, sizeof(int));
		int x, y;
		int sta; //đọc trực tiếp kiểu bool bị lỗi liền
		fin.read((char*)&x, sizeof(int));
		fin.read((char*)&y, sizeof(int));
		fin.read((char*)&sta, sizeof(int));

		m_peo = People(x, y, sta);
		matrix.resize(8, vector<Obstacle*>(2 * level));
		int fX, fY;

		for (int i = 0; i < 8; i++){
			fin.read((char*)&fX, sizeof(int));
			fin.read((char*)&fY, sizeof(int));

			//cout << fX << " " << fY << endl;
			for (int j = 0; j < 2 * level; j++){
				if (i == 0 || i == 4) { matrix[i][j] = new Bird(); }
				if (i == 1 || i == 5) { matrix[i][j] = new Dinosaur(); }
				if (i == 2 || i == 6) { matrix[i][j] = new Truck(); }
				if (i == 3 || i == 7) { matrix[i][j] = new Car(); }
				matrix[i][j]->init(fX, fY);
				fX += 5;
				if (fX>83) fX -= 83;
			}
		}
	}
	else { cout << "Can't load this game"; }
	fin.close();
}
