#pragma once
#include <iostream>
#include <string>
#include <utility>
#include <stack>
#include <vector>
#include <sstream>
#include <cmath>
using namespace std;

// split floating number
pair <string, string> split_float_num(string input);

// string div and mod 2
pair <string, int> div2(string num);

// string num equal to zero
bool isZero(string s);

// string mul 2
string mul2(string num);

// get bit of part before dot
string getBitPart1(string part1);

// get bit of part after dot
string getBitPart2(string part2);

// count exponent
int countExponent(string bitPart1, string bitPart2);

// convert bias
string cvBias(int a);

//xác định xem 1 đoạn bit cụ thể có tất cả các bit giống nhay hay ko
bool isAllBit(string bit, int val, int s, int e);

// phân loại số chấm động
// 0: số zero
// 1: dạng chuẩn 
// 2: dạng không chuẩn
// 3: Inf
// 4: Số báo lỗi
int Classification(string bit);

// chuyển từ bit sang số (bias)
int cvBiasToNum(string exp_bit);

//nhân 2 ở dạng chuỗi
string F_strx2(string str);

//mũ 2 ở dạng chuỗi
string F_2Expn(int n);

//chèn 0 vào đầu
void F_chen0(string &a, int n);

//cộng 2 số thập phân ở dạng chuỗi
string F_strPlusStr(string a, string b);

// chuyển từ bit sang số thập phân ở phần trước dấu chấm
string BitToNumBeforeDot(string bit);

// chuyển từ bit sang số thập phân ở phần sau dấu chấm
string BitToNumAfterDot(string bit);

//2 mu trừ n
string fstrDiv2v2(string str);
string _2Exp_n(int n);

// cộng chuỗi số sau dấu chấm
string addFrac(string a, string b);
