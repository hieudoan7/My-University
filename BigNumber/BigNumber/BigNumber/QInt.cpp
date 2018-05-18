#include "QInt.h"
#include "IntFunction.h"
#include <cstring>

//Lấy 1 bit ở vị trí pos của QInt
int QInt::getBit(int pos){ //pos tinh tu l->r : 0->127
	int data_i = pos / 32;
	pos %= 32;
	int bit = (this->m_data[data_i] >> (31 - pos)) & 1;
	return bit;
}

//Set bit vào vị trí pos của QInt
void QInt::setBit(int pos, int bit){
	int bitpos = this->getBit(pos);
	int data_i = pos / 32;
	pos = pos % 32;
	if (bitpos == 1 && bit == 0){
		this->m_data[data_i] = this->m_data[data_i] - (1 << (31 - pos));
	}
	else{
		this->m_data[data_i] = this->m_data[data_i] | (bit << (31 - pos));
	}
	//or ma bit=1 thi no sẽ set đúng, còn bit=0 thì ko đổi.
}

//default constructor QInt=0
QInt::QInt(){
	m_data[0] = m_data[1] = m_data[2] = m_data[3] = 0;
}
//Copy constructor
QInt::QInt(const QInt& b){
	for (int i = 0; i < 4; i++){
		this->m_data[i] = b.m_data[i];
	}
}
//full constructor
QInt::QInt(int x, int y, int z, int t){
	m_data[0] = x;
	m_data[1] = y;
	m_data[2] = z;
	m_data[3] = t;
}
//dua vao (*this) la default constructor
//toán tử =
QInt& QInt:: operator =(const QInt& b){
	for (int i = 0; i < 4; i++)
		m_data[i] = b.m_data[i];
	return (*this);
}
//Constructor với đối số là dãy bit
QInt::QInt(string binArr){
	*this = QInt();
	int i = binArr.length() - 1;
	int j = 127;
	while (j >= 0 && i >= 0){
		this->setBit(j, binArr[i] - '0');
		j--;
		i--;
	}
}
//trả về chuỗi bit của số QInt
string QInt::binArr(){
	string res = "";
	int i = 127;
	while (i >= 0){
		int bit = this->getBit(i);
		res.insert(res.begin(), bit + '0');
		i--;
	}
	int j = 0;
	while (res[j] == '0' && j<127){
		j++;
	}
	return res.substr(j);
}
//Cac thao tac tren bit, tan dung thao tac co san tren int
QInt QInt:: operator & (const QInt& b){
	QInt res(m_data[0] & b.m_data[0], m_data[1] & b.m_data[1], m_data[2] & b.m_data[2], m_data[3] & b.m_data[3]);
	return res;
}
QInt QInt:: operator | (const QInt& b){
	QInt res(m_data[0] | b.m_data[0], m_data[1] | b.m_data[1], m_data[2] | b.m_data[2], m_data[3] | b.m_data[3]);
	return res;
}
QInt operator ~ (const QInt& b){
	QInt res(~b.m_data[0], ~b.m_data[1], ~b.m_data[2], ~b.m_data[3]);
	return res;
}
QInt QInt:: operator ^ (const QInt& b){
	QInt res(m_data[0] ^ b.m_data[0], m_data[1] ^ b.m_data[1], m_data[2] ^ b.m_data[2], m_data[3] ^ b.m_data[3]);
	return res;
}
//toán tử lấy số đối (1 ngôi)
QInt operator - (QInt b){
	QInt one(0, 0, 0, 1);
	if (b.getBit(0) == 0){
		return ~b + one;
	}
	else return ~(b - one);
}

//Dịch phải QInt 1 vị trí
QInt QInt::rightShift(){ //QInt>>1
	int bit31 = this->getBit(31);
	int bit63 = this->getBit(63);
	int bit95 = this->getBit(95);
	for (int i = 0; i < 4; i++){
		this->m_data[i] >>= 1;
	}
	this->setBit(32, bit31);
	this->setBit(64, bit63);
	this->setBit(96, bit95);
	return (*this);
}

//Dịch trái QInt  1 vị trí
QInt QInt::leftShift(){ //QInt<<1
	int bit32 = this->getBit(32);
	int bit64 = this->getBit(64);
	int bit96 = this->getBit(96);
	for (int i = 0; i < 4; i++){
		this->m_data[i] <<= 1;
	}
	this->setBit(31, bit32);
	this->setBit(63, bit64);
	this->setBit(95, bit96);
	return (*this);
}
//toán tử dịch phải bit
QInt QInt:: operator >> (unsigned int x){
	QInt res;
	while (x--){
		res = this->rightShift();
	}
	return res;
}
//toán tử dịch trái bit
QInt QInt:: operator << (unsigned int x){
	QInt res;
	while (x--){
		res = this->leftShift();
	}
	return res;
}
//toán tử cộng
QInt QInt:: operator + (QInt& b){
	QInt sum;
	int rem = 0;
	for (int i = 127; i >= 0; i--){
		int bitb = b.getBit(i);
		int tmp = this->getBit(i) + b.getBit(i) + rem; //sao const lai sai?
		int bit = tmp % 2;
		rem = tmp / 2;
		sum.setBit(i, bit);
	}
	return sum;
}

//toán tử trừ
QInt QInt::operator - (const QInt& b){ //b>0
	return (*this) + (-b);
}

//toán tử nhân
QInt QInt::  operator * (/*const*/ QInt& M){
	QInt A;
	int Q_1 = 0;
	QInt Q = (*this);
	int lastA, lastQ;
	int i = 128;
	//freopen("output.txt", "w", stdout);
	while (i--){
		lastQ = Q.getBit(127);
		if (lastQ > Q_1){
			A = A - M;
		}
		else if (lastQ < Q_1) {
			A = A + M;
		}
		//dich phai
		lastA = A.getBit(127);
		Q_1 = lastQ;
		Q.rightShift();
		Q.setBit(0, lastA);

		A.rightShift();
		//cout << A.binArr() << endl << Q.binArr() << endl << Q_1 << endl;
	}
	return Q;
}

//thuật toán chia 2 số nguyên
QInt QInt::operator /(/*const*/ QInt& M){
	QInt A;
	QInt Q = (*this);
	int dauQ = Q.getBit(0);
	int dauM = M.getBit(0);
	//cout << dauQ << " " << dauM << endl;
	M = abs(M);
	Q = abs(Q);
	int firstQ;
	int i = 128;
	while (i--){
		firstQ = Q.getBit(0);
		A.leftShift();
		Q.leftShift();
		A.setBit(127, firstQ);
		A = A - M;
		if (A.m_data[0] < 0) {
			A = A + M;
			Q.setBit(127, 0);
		}
		else Q.setBit(127, 1);
	}
	if (dauM != dauQ) return -Q;
	return Q;
}
//nhap vao voi string la chuoi bat ki kèm theo cơ số
void QInt::scan(string str, int base){
	switch (base){
	case 2:
		*this = QInt(str);
		break;
	case 10:{
				str = DecToBin(str);
				*this = QInt(str);
				break;
	}
	case 16:{
				str = HexToBin(str);
				*this = QInt(str);
				break;
	}
	}
}
//hàm xuất theo cơ số
string QInt::print(int base){
	string res = "";
	switch (base){
	case 2:
		res = this->binArr();
		break;
	case 10: {
				 res = BinToDec(this->binArr());
				 break;
	}
	case 16:{
				res = BinToHex(this->binArr());
				break;
	}
	}
	return res;
}
//trị tuyệt đối QInt
QInt QInt::abs(QInt a){
	if (a.getBit(0) == 0) return a;
	QInt one(0, 0, 0, 1);
	a = a - one;
	QInt res = ~a;
	return res;
}

string QInt::nhan(QInt& M){ //viêt thêm hàm nhân trong QInt để lấy 256bit answer
	QInt A;
	int Q_1 = 0;
	QInt Q = (*this);
	int lastA, lastQ;
	int i = 128;
	//freopen("output.txt", "w", stdout);
	while (i--){
		lastQ = Q.getBit(127);
		if (lastQ > Q_1){
			A = A - M;
		}
		else if (lastQ < Q_1) {
			A = A + M;
		}
		//dich phai
		lastA = A.getBit(127);
		Q_1 = lastQ;
		Q.rightShift();
		Q.setBit(0, lastA);

		A.rightShift();
		//cout << A.binArr() << endl << Q.binArr() << endl << Q_1 << endl;
	}
	return A.binArrFull() + Q.binArrFull();
}

string QInt::binArrFull(){ //chuỗi 128 bit đầy đủ, ko bỏ số 0 đầu
	string res = "";
	int i = 127;
	while (i >= 0){
		int bit = this->getBit(i);
		res.insert(res.begin(), bit + '0');
		i--;
	}
	return res;
}

//thêm phương thức chia trả về chuỗi quotient lượt bỏ 0 đầu
string QInt::chia(QInt& M){
	QInt A;
	QInt Q = (*this);
	int dauQ = Q.getBit(0);
	int dauM = M.getBit(0);
	M = abs(M);
	Q = abs(Q);
	int firstQ;
	int i = 128;
	while (i--){
		firstQ = Q.getBit(0);
		A.leftShift();
		Q.leftShift();
		A.setBit(127, firstQ);
		A = A - M;
		if (A.m_data[0] < 0) {
			A = A + M;
			Q.setBit(127, 0);
		}
		else Q.setBit(127, 1);
	}
	return Q.binArr();
}
string QInt::getFromTo(int x, int y) {
	string res = "";
	for (int i = x; i <= y; i++) {
		res += getBit(i) + '0';
	}
	return res;
}

bool QInt::operator==(QInt &q){
	for (int i = 0; i < 4; i++) {
		if (m_data[i] != q.m_data[i])
			return false;
	}
	return true;
}
