#ifndef DRAW_H
#define DRAW_H
#include <iostream>
#include <Windows.h>

using namespace std;
void FixConsoleWindow();
void NoCursorType();
void GotoXY(Point A);
void GotoXY(int, int);
void drawBoard();
void drawMenu();
void drawline(int level);
void setColor(int x);
void cleanNoti();
void drawMenuSave();
void cleanMenuSave(); //Load cung nhu z
void drawMenuLoad();
void drawMenuSetting();
void drawBar();
void cleanBar();
void cleanLight();
#endif