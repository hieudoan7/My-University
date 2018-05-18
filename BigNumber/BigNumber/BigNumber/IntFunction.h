//chuyen string giua cac co so 2,10,16
#pragma once
#include <iostream>
#include <string>
#include <vector>
using namespace std;
//convert giữa các cơ số 10 (dec), 2(bin), 16(hex)
string DecToBin(string Dec);
string BinToDec(string Bin);
string HexToBin(string Hex);
string BinToHex(string Bin);
string DecToHex(string Dec);
string HexToDec(string Hex);
//doc input
vector<string> splitLine(string line);
//doc line tra ve ket qua
string readLine(string line);
