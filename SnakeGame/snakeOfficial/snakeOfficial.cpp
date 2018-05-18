#include <Windows.h>
#include <time.h>
#include <iostream>
#include <conio.h>
#include <thread>
using namespace std;
//#define MAX_SIZE_SNAKE 11
#define MAX_SIZE_FOOD 4
#define MAX_SPEED 4
POINT snake[19];
POINT food[4];
int CHAR_LOCK;
int MOVING;
int SPEED;
int HEIGH_CONSOLE, WIDTH_CONSOLE;
int FOOD_INDEX;
int SIZE_SNAKE;
int STATE;
char nameSnake[] = { '1', '6', '1', '2', '1', '9', '8' };
POINT gate[6] = { 0 };
int temp;
void FixConsoleWindow()
{
	HWND consoleWindow = GetConsoleWindow();
	LONG style = GetWindowLong(consoleWindow, GWL_STYLE);
	style = style&~(WS_MAXIMIZEBOX)&~(WS_THICKFRAME);
	SetWindowLong(consoleWindow, GWL_STYLE, style);
	system("mode 120,33");
}
void GotoXY(int x, int y)
{
	COORD coord;
	coord.X = x;
	coord.Y = y;
	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
}
bool isValid(int x, int y)
{
	for (int i = 0; i < SIZE_SNAKE; i++)
	{
		if (snake[i].x == x && snake[i].y == y) return false;
	}
	return true;
}
void GenerateFood()
{
	int x, y;
	srand(time(NULL));
	for (int i = 0; i < MAX_SIZE_FOOD; i++)
	{
		do{
			x = rand() % (WIDTH_CONSOLE - 4) + 2;
			y = rand() % (HEIGH_CONSOLE - 4) + 2;
		} while (isValid(x, y) == false);
		food[i] = { x, y };
	}

}
void ResetData()
{
	CHAR_LOCK = 'A', MOVING = 'D', SPEED = 1;
	FOOD_INDEX = 0, WIDTH_CONSOLE = 85, HEIGH_CONSOLE = 32, SIZE_SNAKE = 7;
	snake[0] = { 10, 5 }; snake[1] = { 11, 5 };
	snake[2] = { 12, 5 }; snake[3] = { 13, 5 };
	snake[4] = { 14, 5 }; snake[5] = { 15, 5 };
	snake[6] = { 16, 5 };
	//gate[0].x = 0; gate[0].y = 0;
	GenerateFood();
}
void DrawBoard(int x, int y, int width, int height, int curPosX = 0, int curPosY = 0)
{
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(consoleHandle, 10);
	GotoXY(x, y);
	cout << 'X';
	for (int i = 1; i < width; i++) cout << 'X';
	cout << 'X';
	GotoXY(x, height + y); cout << 'X';
	for (int i = 1; i < width; i++) cout << 'X';
	cout << 'X';
	for (int i = y + 1; i < height + y; i++){
		GotoXY(x, i); cout << 'X';
		GotoXY(x + width, i); cout << 'X';
	}
	GotoXY(curPosX, curPosY);
}
void drawInfoFrame()
{
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(consoleHandle, 12);
	GotoXY(WIDTH_CONSOLE + 1, 0);
	for (int i = 1; i < 119 - WIDTH_CONSOLE; i++) cout << 'X';
	GotoXY(WIDTH_CONSOLE + 1, HEIGH_CONSOLE);
	for (int i = 1; i < 120 - WIDTH_CONSOLE; i++) cout << 'X';
	for (int i = 0; i < HEIGH_CONSOLE; i++){
		GotoXY(WIDTH_CONSOLE + 1, i); cout << 'X';
		GotoXY(119, i); cout << 'X';
	}
	GotoXY(WIDTH_CONSOLE + 13, 3);
	cout << "My Snake";
	GotoXY(WIDTH_CONSOLE + 10, 5); cout << ">> T   (upload)";
	GotoXY(WIDTH_CONSOLE + 10, 6); cout << ">> L   (save)";
	GotoXY(WIDTH_CONSOLE + 10, 7); cout << ">> P   (pause)";
	GotoXY(WIDTH_CONSOLE + 10, 8); cout << ">> Esc (exit)";
	GotoXY(0, 0);
}
void StartGame()
{
	system("cls");
	ResetData();
	DrawBoard(0, 0, WIDTH_CONSOLE, HEIGH_CONSOLE);
	drawInfoFrame();
	STATE = 1;
}
void ExitGame()
{
	system("cls"); 
	exit(0);
}
void PauseGame(HANDLE t)
{
	SuspendThread(t);
}

void generateGate()
{
	int x, y;
	srand(time(NULL));
	do{
		x = rand() % (WIDTH_CONSOLE - 5) + 3;
		y = rand() % (HEIGH_CONSOLE - 30) + 10;
	} while (isValid(x, y) == false);
	gate[0] = { x - 1, y + 1 };
	gate[1] = { x - 1, y };
	gate[2] = { x, y };
	gate[3] = { x + 1, y };
	gate[4] = { x + 1, y + 1 };
	gate[5] = { x, y + 1 };
}
void drawGate()
{
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(consoleHandle, 12);
	for (int i = 0; i < 5; i++){
		GotoXY(gate[i].x, gate[i].y);
		cout << "o";
	}
}
void cleanGate()
{
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(consoleHandle, 12);
	for (int i = 0; i < 5; i++){
		GotoXY(gate[i].x, gate[i].y);
		cout << " ";
		gate[i].x = 0; gate[i].y = 0;
	}
}
void drawFood(char x);
void Eat()
{
	snake[SIZE_SNAKE] = food[FOOD_INDEX];
	if (FOOD_INDEX == MAX_SIZE_FOOD - 1)
	{
		//FOOD_INDEX = 0;
		temp = SIZE_SNAKE;
		generateGate();
		drawGate();
	}
	else{
		FOOD_INDEX++;
		SIZE_SNAKE++;
	}
}
void ProcessDead()
{
	STATE = 0;
	gate[0].x = 0; gate[0].y = 0;
	int k = 0;
	while (k < 30){
		HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
		SetConsoleTextAttribute(consoleHandle, rand() % 7 + 1);
		int j = 0;
		for (int i = 0; i < SIZE_SNAKE; i++){
			GotoXY(snake[i].x, snake[i].y);

			printf("%c", nameSnake[j++]);
			if (j>6) j = 0;
			Sleep(50);
			k++;
		}
	}
	GotoXY(20, HEIGH_CONSOLE / 2);
	cout << "Dead, type y to continue or anykey to exit";
}
void drawFood(char x)
{
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(consoleHandle, 7);
	GotoXY(food[FOOD_INDEX].x, food[FOOD_INDEX].y);
	printf("%c", x);
}
void DrawSnake(char *str)
{
	HANDLE consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(consoleHandle, 14);
	/*GotoXY(food[FOOD_INDEX].x, food[FOOD_INDEX].y);
	printf("*");*/
	int j = 0;
	for (int i = 0; i < SIZE_SNAKE; i++){
		GotoXY(snake[i].x, snake[i].y);

		printf("%c", str[j++]);
		if (j>6) j = 0;
	}
}
void MoveRight()
{
	if (FOOD_INDEX == MAX_SIZE_FOOD - 1){
		for (int i = 0; i < 5; i++){
			if (gate[i].x == snake[SIZE_SNAKE - 1].x&&gate[i].y == snake[SIZE_SNAKE - 1].y) {
				ProcessDead(); break;
			}
		}
	}

	for (int i = 0; i < SIZE_SNAKE - 2; i++){
		if (snake[i].x == snake[SIZE_SNAKE - 1].x + 1 && snake[i].y == snake[SIZE_SNAKE - 1].y) {
			ProcessDead();
			break;
		}
	}
	if (snake[SIZE_SNAKE - 1].x + 1 == WIDTH_CONSOLE){
		ProcessDead();
	}
	else{
		if (snake[SIZE_SNAKE - 1].x + 1 == food[FOOD_INDEX].x && snake[SIZE_SNAKE - 1].y == food[FOOD_INDEX].y){
			Eat();
		}
		for (int i = 0; i < SIZE_SNAKE - 1; i++){
			snake[i].x = snake[i + 1].x;
			snake[i].y = snake[i + 1].y;
		}
		snake[SIZE_SNAKE - 1].x++;
	}
}
void MoveLeft()
{
	if (FOOD_INDEX == MAX_SIZE_FOOD - 1){
		for (int i = 0; i < 5; i++){
			if (gate[i].x == snake[SIZE_SNAKE - 1].x&&gate[i].y == snake[SIZE_SNAKE - 1].y) {
				ProcessDead(); break;
			}
		}
	}
	for (int i = 0; i < SIZE_SNAKE - 2; i++){
		if (snake[i].x == snake[SIZE_SNAKE - 1].x - 1 && snake[i].y == snake[SIZE_SNAKE - 1].y) {
			ProcessDead();
			break;
		}
	}
	if (snake[SIZE_SNAKE - 1].x - 1 == 0) {
		ProcessDead();
	}
	else {
		if (snake[SIZE_SNAKE - 1].x - 1 == food[FOOD_INDEX].x && snake[SIZE_SNAKE - 1].y == food[FOOD_INDEX].y) {
			Eat();
		}
		for (int i = 0; i < SIZE_SNAKE - 1; i++) {
			snake[i].x = snake[i + 1].x;
			snake[i].y = snake[i + 1].y;
		}
		snake[SIZE_SNAKE - 1].x--;
	}
}
void MoveDown()
{
	if (FOOD_INDEX == MAX_SIZE_FOOD - 1){
		for (int i = 0; i < 5; i++){
			if (gate[i].x == snake[SIZE_SNAKE - 1].x&&gate[i].y == snake[SIZE_SNAKE - 1].y) {
				ProcessDead(); break;
			}
		}
	}
	for (int i = 0; i < SIZE_SNAKE - 2; i++){
		if (snake[i].x == snake[SIZE_SNAKE - 1].x && snake[i].y == snake[SIZE_SNAKE - 1].y + 1) {
			ProcessDead();
			break;
		}
	}
	if (snake[SIZE_SNAKE - 1].y + 1 == HEIGH_CONSOLE) {
		ProcessDead();
	}
	else {
		if (snake[SIZE_SNAKE - 1].x == food[FOOD_INDEX].x && snake[SIZE_SNAKE - 1].y + 1 == food[FOOD_INDEX].y) {
			Eat();
		}
		for (int i = 0; i < SIZE_SNAKE - 1; i++) {
			snake[i].x = snake[i + 1].x;
			snake[i].y = snake[i + 1].y;
		}
		snake[SIZE_SNAKE - 1].y++;
	}
}

void MoveUp()
{
	if (FOOD_INDEX == MAX_SIZE_FOOD - 1){
		for (int i = 0; i < 5; i++){
			if (gate[i].x == snake[SIZE_SNAKE - 1].x&&gate[i].y == snake[SIZE_SNAKE - 1].y)
			{
				ProcessDead();
				break;
			}
		}
		if (snake[SIZE_SNAKE - 1].x == gate[5].x&&snake[SIZE_SNAKE - 1].y == gate[5].y) SIZE_SNAKE--;
		if (SIZE_SNAKE == 0)	{
			SPEED++;
			if (SPEED == MAX_SPEED)
			{
				cleanGate();
				ResetData();
			}
			else{
				FOOD_INDEX = 0;
				SIZE_SNAKE = temp++;
				cleanGate();
				//DrawSnake(nameSnake);
			}
		}
	}
	for (int i = 0; i < SIZE_SNAKE - 2; i++){
		if (snake[i].x == snake[SIZE_SNAKE - 1].x && snake[i].y == snake[SIZE_SNAKE - 1].y - 1) {
			ProcessDead();
			break;
		}
	}
	if (snake[SIZE_SNAKE - 1].y - 1 == 0) {
		ProcessDead();
	}
	else {
		if (snake[SIZE_SNAKE - 1].x == food[FOOD_INDEX].x && snake[SIZE_SNAKE - 1].y - 1 == food[FOOD_INDEX].y) {
			Eat();
		}
		for (int i = 0; i < SIZE_SNAKE - 1; i++) {
			snake[i].x = snake[i + 1].x;
			snake[i].y = snake[i + 1].y;
		}
		snake[SIZE_SNAKE - 1].y--;
	}
}

void ThreadFunc()
{
	while (1)
	{
		if (STATE == 1) {
			char space[19];
			for (int i = 0; i < 19; i++)
				space[i] = 32;
			DrawSnake(space);
			if (gate[0].x == 0 && gate[0].y == 0) drawFood('*');
			switch (MOVING){
			case 'A':
				MoveLeft();
				break;
			case 'D':
				MoveRight();
				break;
			case 'W':
				MoveUp();
				break;
			case 'S':
				MoveDown();
				break;
			}
			DrawSnake(nameSnake);
			Sleep(100 / SPEED);
		}
	}
}
void saveGame()
{
	char nameGame[40];
	GotoXY(27, HEIGH_CONSOLE / 2 - 2);
	cout << "|-------------------------|";
	GotoXY(27, HEIGH_CONSOLE / 2 - 1);
	cout << "|     Save Game!          |";
	GotoXY(27, HEIGH_CONSOLE / 2);
	cout << "|     Name:               |";
	GotoXY(27, HEIGH_CONSOLE / 2 + 1);
	cout << "|                         |";
	GotoXY(27, HEIGH_CONSOLE / 2 + 2);
	cout << "|-------------------------|";
	GotoXY(39, HEIGH_CONSOLE / 2);
	gets(nameGame);
	strcat(nameGame, ".txt");
	FILE *f = fopen(nameGame, "w");
	fprintf(f, "%d\n", FOOD_INDEX);
	fprintf(f, "%d %d\n", food[FOOD_INDEX].x, food[FOOD_INDEX].y);
	fprintf(f, "%d\n", SPEED);
	fprintf(f, "%d\n", SIZE_SNAKE);
	for (int i = 0; i < SIZE_SNAKE; i++)
		fprintf(f, "%d %d\n", snake[i].x, snake[i].y);
	for (int i = 0; i < 5; i++)
		fprintf(f, "%d %d\n", gate[i].x, gate[i].y);
	fclose(f);
	GotoXY(0, 0);
}
void cleanSaveGame()
{
	GotoXY(27, HEIGH_CONSOLE / 2 - 2);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2 - 1);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2 + 1);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2 + 2);
	cout << "                           ";
	DrawSnake(nameSnake);
}
void loadGame()
{
	//STATE = 1;
	char space[19];
	for (int i = 0; i < 19; i++)
		space[i] = 32;
	DrawSnake(space);
	GotoXY(food[FOOD_INDEX].x, food[FOOD_INDEX].y); cout << " ";
	char nameGame[40];
	GotoXY(27, HEIGH_CONSOLE / 2 - 2);
	cout << "|-------------------------|";
	GotoXY(27, HEIGH_CONSOLE / 2 - 1);
	cout << "|     Upload Game!        |";
	GotoXY(27, HEIGH_CONSOLE / 2);
	cout << "|     Name:               |";
	GotoXY(27, HEIGH_CONSOLE / 2 + 1);
	cout << "|                         |";
	GotoXY(27, HEIGH_CONSOLE / 2 + 2);
	cout << "|-------------------------|";
	GotoXY(39, HEIGH_CONSOLE / 2);
	gets(nameGame);
	strcat(nameGame, ".txt");
	FILE *f = fopen(nameGame, "r");
	fseek(f, 0, 0L);
	fscanf(f, "%d\n", &FOOD_INDEX);
	fscanf(f, "%d %d\n", &food[FOOD_INDEX].x, &food[FOOD_INDEX].y);
	fscanf(f, "%d\n", &SPEED);
	fscanf(f, "%d\n", &SIZE_SNAKE);
	for (int i = 0; i < SIZE_SNAKE; i++)
		fscanf(f, "%d %d\n", &snake[i].x, &snake[i].y);
	for (int i = 0; i < 5; i++)
		fscanf(f, "%d %d\n", &gate[i].x, &gate[i].y);
	fclose(f);
	if (FOOD_INDEX == MAX_SIZE_FOOD - 1) drawGate();
}
void cleanLoadGame()
{
	GotoXY(27, HEIGH_CONSOLE / 2 - 2);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2 - 1);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2 + 1);
	cout << "                           ";
	GotoXY(27, HEIGH_CONSOLE / 2 + 2);
	cout << "                           ";
	DrawSnake(nameSnake);
}
void NoCursorType()
{
	CONSOLE_CURSOR_INFO Info;
	Info.bVisible = FALSE;
	Info.dwSize = 20;
	SetConsoleCursorInfo(GetStdHandle(STD_OUTPUT_HANDLE), &Info);
}
void main()
{
	int temp;
	FixConsoleWindow();
	NoCursorType();
	StartGame();
	thread t1(ThreadFunc);
	HANDLE handle_t1 = t1.native_handle();
	while (1)
	{
		temp = toupper(getch());
		if (STATE == 1) {
			if (temp == 'P'){
				PauseGame(handle_t1);
			}
			else
			if (temp == 27) {
				ExitGame();
				return;
			}
			else
			if (temp == 'L') {
				PauseGame(handle_t1); //pause rồi chơi tiếp thôi
				saveGame();
				Sleep(500);
				cleanSaveGame();
			}
			else
			if (temp == 'T') {
				PauseGame(handle_t1);
				loadGame();
				Sleep(500);
				cleanLoadGame();
			}

			else{
				ResumeThread(handle_t1);
				if ((temp != CHAR_LOCK) && (temp == 'D' || temp == 'A' || temp ==
					'W' || temp == 'S'))
				{
					if (temp == 'D') CHAR_LOCK = 'A';
					else if (temp == 'W') CHAR_LOCK = 'S';
					else if (temp == 'S') CHAR_LOCK = 'W';
					else CHAR_LOCK = 'D';
					MOVING = temp;
				}
			}
		}
		else {
			if (temp == 'Y') StartGame();
			else
			{
				ExitGame();
				return;
			}
		}
	}
}
