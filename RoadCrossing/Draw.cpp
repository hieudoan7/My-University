#ifndef DRAW_H
#define DRAW_H
#include <Windows.h>
#include "Point.h"
#include <iostream>
using namespace std;
HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
void FixConsoleWindow()
{
	HWND consoleWindow = GetConsoleWindow();
	LONG style = GetWindowLong(consoleWindow, GWL_STYLE);
	style = style&~(WS_MAXIMIZEBOX)&~(WS_THICKFRAME);
	SetWindowLong(consoleWindow, GWL_STYLE, style);
	system("mode 120,32"); //chinh 85,32
}
void NoCursorType()
{
	CONSOLE_CURSOR_INFO Info;
	Info.bVisible = FALSE;
	Info.dwSize = 20;
	SetConsoleCursorInfo(GetStdHandle(STD_OUTPUT_HANDLE), &Info);
}
void GotoXY(Point A)
{
	COORD coord;
	coord.X = A.getX();
	coord.Y = A.getY();
	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
}
void GotoXY(int x, int y)
{
	COORD coord;
	coord.X = x;
	coord.Y = y;
	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
}
void setColor(int x);
void drawBoard(){
	setColor(15);
	FixConsoleWindow(); //xoa luon cai menuboard roi
	NoCursorType();
	GotoXY(0, 0);
	cout << (char)219;
	for (int i = 1; i < 85; i++) cout << (char)219;
	GotoXY(0, 31); cout << (char)219;
	for (int i = 1; i < 85; i++) cout << (char)219;
	for (int i = 1; i < 32; i++){
		GotoXY(0, i); cout << (char)219;
		GotoXY(84, i); cout << (char)219;
	}
	for (int x = 1; x < 84; x++){
		GotoXY(x, 3);
		cout << "_";
	}
	//vẽ cái menu bên phải nè
	GotoXY(93, 4); cout << "Road Crossing";
	GotoXY(93, 5); cout << "T   (Upload)";
	GotoXY(93, 6); cout << "L   (Save)";
	GotoXY(93, 7); cout << "P   (Pause)";
	GotoXY(93, 8); cout << "ESC (Exit)";
	GotoXY(98, 18); cout << "LEVEL";
	GotoXY(0, 0);
}
void drawMenu(){
	NoCursorType();
	GotoXY(41, 12);
	cout << " Road Crossing";
	GotoXY(40, 13);
	cout <<(char)254<< " New Game\n";
	GotoXY(40, 14);
	cout << "  Load Game\n";
	GotoXY(40, 15);
	cout << "  Settings";
	GotoXY(40, 16);
	cout << "  Exit";
	GotoXY(0, 0);
}
void cleanMenu(){

}
void drawline(int level){
	setColor(15);
	for (int x = 1; x < 84-4*level; x++){
		GotoXY(x, 3);
		cout << "_";
	}
	GotoXY(0, 0);
}

void setColor(int x){
	SetConsoleTextAttribute(consoleHandle, x);
}

void cleanNoti(){
	GotoXY(35, 15); cout << "               ";
	GotoXY(22, 16);
	cout << "                                            ";
}

void drawMenuSave(){
	setColor(12);
	GotoXY(30, 13);
	for (int i = 0; i < 20; i++) cout << (char)219;
	GotoXY(30, 14);
	cout << (char)219 << "    Save Game     "<<(char)219;
	GotoXY(30, 15);
	cout << (char)219 << "Name:             "<<(char)219;
	GotoXY(30, 16);
	for (int i = 0; i < 20; i++) cout << (char)219;
	GotoXY(37, 15); //dua con tro den day de nhap ten 
	//string name;
}
void cleanMenuSave(){
	GotoXY(30, 13);
	for (int i = 0; i < 20; i++) cout << " ";
	GotoXY(30, 14);
	cout << " " << "                  " << " ";
	GotoXY(30, 15);
	cout << " " << "                  " << " ";
	GotoXY(30, 16);
	for (int i = 0; i < 20; i++) cout << " ";
}
void drawMenuLoad(){
	setColor(12);
	for(int i=0;i<5;i++) GotoXY(30, 13);
	
	for (int i = 0; i < 20; i++) cout << (char)219;
	GotoXY(30, 14);
	cout << (char)219 << "    Load Game     " << (char)219;
	GotoXY(30, 15);
	cout << (char)219 << "Name:             " << (char)219;
	GotoXY(30, 16);
	for (int i = 0; i < 20; i++) cout << (char)219;
	GotoXY(37, 15); //dua con tro den day de nhap ten 
	//string name;
}
void drawMenuSetting(){
	GotoXY(51, 15);
	cout << "MUSIC (1/0): ";
}
void drawBar(){
	setColor(2);
	GotoXY(38, 18);
	cout << "LOADING";
	GotoXY(15, 20);
	int i = 15;
	for (i; i < 65; i++){
		GotoXY(i, 20);
		cout << (char)219;
		Sleep(100);
	}
	Sleep(1000);
}
void cleanBar(){
	GotoXY(38, 18);
	cout << "       ";
	GotoXY(15, 20);  
	int i = 15;
	for (i; i < 65; i++){
		GotoXY(i, 20);
		cout << " ";
	}
}
void cleanLight(){
	GotoXY(1, 11);
	cout << " ";
	GotoXY(83, 14);
	cout << " ";
	GotoXY(1, 23);
	cout << " ";
	GotoXY(83, 26);
	cout << " ";
}
#endif