#ifndef GAME_H
#define GAME_H
#include "People.h"
#include "Obstacle.h"
#include <vector>
#include <Windows.h>
#include <thread>
#include "Light.h"
using namespace std;
class Game
{
private:
	 int level;
	vector<vector<Obstacle*> > matrix;
	People m_peo;
	vector<Light> m_light;
	bool music;
public:
	Game(){};
	Game(int lev);
	void drawGame();
	People getPeople();
	vector<vector<Obstacle*> > getObstacle();
	void resetGame();
	void exitGame();
	void startGame();
	void settingGame();
	//void saveGame();
	void pauseGame(HANDLE t);
	void resumeGame(HANDLE t);
	void updatePosPeople(char c);
	void updatePosObstacle();
	//void nextLevel();
	void youWin();
	void cleanGame();
	int getLevel();
	void xulyDead();
	void xulyToiDich();
	Light getLight(int i){
		return m_light[i];
	};
	void updateLight();
	void saveGame();
	void loadGame();
	bool isMusic(){
		return music;
	}
};

#endif