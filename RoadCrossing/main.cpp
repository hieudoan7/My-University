#include <iostream>
#include <conio.h>
#include "Game.h"
#include <thread>
#include <time.h>
#include "Draw.h"
#include <Windows.h>
#include <MMSystem.h>
#define HEIGH_CONSOLE 32
#define WIDTH_CONSOLE 85
using namespace std;
//int Game::level = 1;
char MOVING;
bool ISRUNNING = 1;
Game g;
bool isJoin = 0; //để có thể dừng thread và join vào main khi exit
//bool on_off[4] = { 1 };
int cnt = 0; //số bước đi để dừng đèn đỏ
int chayNhac1lan;

void musicThread(){
	while (!isJoin){
		if (ISRUNNING){
		PlaySound(TEXT("mario.wav"), NULL, SND_SYNC);
		}
	}
}
void SubThreat(){
	
	while (!isJoin){
		if (ISRUNNING){
			if (!g.getPeople().isDead())
			{
				g.updatePosPeople(MOVING);
			}
			MOVING = ' ';
			if (cnt > 12) {
				g.updateLight();
				cnt = 0;
			}

			g.updatePosObstacle();
			g.drawGame();
			if (g.getPeople().isImpact(g.getObstacle())){
				ISRUNNING = 0;
				PlaySound(0, 0, 0); chayNhac1lan = 1;

				g.xulyDead();
			}

			if (g.getPeople().isFinish()){
				//cout << " toi dich roi";
				g.xulyToiDich();

				g.cleanGame();
				int lev = g.getLevel() + 1;
				if (lev > 3) {
					g.youWin();
					ISRUNNING = 0;
				}
				else g = Game(lev);
			}
			Sleep(100);//toc do game: nguoi + obstacle
		}
	}
}

/*void lightThread(){
	while (!isJoin){
		if (ISRUNNING){
			for (int i = 0; i < 4; i++){
				int k = rand() % 2;
				g.getLight(i).setLight(k);
				Sleep(3000);
			}
		}
	}
}*/
void main()
{
	srand(time(NULL));
	int temp;
	g = Game(1);
	drawMenu();

	g.startGame();
	drawBoard();
	drawBar();

	cleanBar();
	thread t1(SubThreat);
	//thread t2(lightThread);
	//thread t2(musicThread);
	//t2.detach();
	if(g.isMusic()) PlaySound(TEXT("mario.wav"), NULL, SND_ASYNC);

	while (1){
		 temp = toupper(getch()); //getch() in conio.h library
		if (/*!g.getPeople().isDead()*/ ISRUNNING)
		{
			if(temp=='W' || temp=='S') cnt+=3;
			if (temp == 27){
				//ISRUNNING = 0; ko kết thúc lấy gì join :v
				isJoin = 1;
				t1.join();
				//t2.join();
				g.exitGame();
				return ;
			}
			else if (temp == 'P'){
				g.pauseGame(t1.native_handle());
				PlaySound(0, 0, 0); chayNhac1lan = 1;

				//g.pauseGame(t2.native_handle());
			}
			else if (temp == 'L'){
				//luu game
				g.pauseGame(t1.native_handle());
				PlaySound(0, 0, 0); chayNhac1lan = 1;

				Sleep(200);
				g.saveGame();
				Sleep(500);
				cleanMenuSave();
			}
			else if (temp == 'T'){
				//tai lai
				g.pauseGame(t1.native_handle());
				PlaySound(0, 0, 0); chayNhac1lan = 1;

				g.loadGame();
				cleanMenuSave();
				Sleep(500);
				drawBar();
				cleanBar();
				g.drawGame();
				g.resumeGame((HANDLE)t1.native_handle());
				//g.pauseGame(t1.native_handle());
			}
			else{
				g.resumeGame((HANDLE)t1.native_handle());
				if (chayNhac1lan && g.isMusic()) { 
					PlaySound(TEXT("mario.wav"), NULL, SND_ASYNC); 
					chayNhac1lan = 0;
				}

				//g.resumeGame((HANDLE)t2.native_handle());
				MOVING = temp;
				cnt++;
			}
		}
		else{
			//cout << "Tao da chet";
			if (temp == 'Y'){
				g = Game(1);
				cleanNoti(); drawline(0);
				GotoXY(70, 2); cout << "              ";
				ISRUNNING = 1;
			}
			else {
				g.exitGame();
				//return 0;
			}
		}
	}
}