#pragma once
#include "QInt.h"
#include "FloatTools.h"

class QFloat
{
private:
	int m_data[4];
public:
	int getBit(int pos);
	void setBit(int pos, int bit);
	QFloat();
	QFloat(int, int, int, int);
	QFloat(string binArr);
	QFloat(const QFloat& b);

	QFloat add(const QFloat &);
	QFloat substract(const QFloat &);

	// set bit ở phần dấu
	void setSign(string &part1);
	
	// set bit ở phần mũ
	void setExp(string exp_bit);
	
	// set bit ở phần trị
	void setFraction(string fraction);
	
	// scan chuỗi input từ stream
	void ScanQFloat(string input, int base);

	//trả ra chuỗi kết quả với base tương ứng
	string PrintQFloat(int base);
	
	// trường hợp là số dạng chuẩn
	string printNormalize(string bit);

	// trường hợp là số dạng không chuẩn
	string printDeNormalize(string bit);

	//Kiem tra QFloat co bang 0 hay khong?
	bool checkZero();

	//Kiem tra so mu cua 2 so co bang nhau hay khong
	bool checkExpEqual( QFloat&);
	
	// kiểm tra 2 số có bằng nhau hay không
	bool checkEqual(QFloat&);
	
	// kiểm tra số có phần mũ lớn hơn
	bool checkBiggerExp(QFloat &x, QFloat &y);
	
	//Dich trai logic tu vi tri start den vi tri end mot so bit nhat dinh
	void shiftLeftLogical(int start, int end, int bit);
	
	//Dich phai logic tu vi tri start den vi tri end mot so bit nhat dinh
	void shiftRightLogical(int start, int end, int bit);
	
	//Kiem tra phan tri co bang 0 hay khong?
	bool isSignificandZero();
	
	//Tang 1 doan bit tu start den end len 1 don vi 
	void IncrementOne(int start, int end);
	
	//Giam 1 doan bit tu start den end di 1 don vi 
	void DecrementOne(int start, int end);
	
	/*Ham cong 2 so cham dong thuc hien 4 cong viec:
	1. Kiem tra cac so co bang 0 hay khong
	2. Dua ve cung so mu
	3. Thuc hien phep cong
	4. Dua ve dang chuan va lam tron*/

	QFloat operator+(const QFloat &);
	QFloat operator-(const QFloat &);
	QFloat operator*(QFloat &);
	QFloat operator/(QFloat &);

	// kiểm tra bằng 0
	bool is0();

	// get bit từ x -> y
	string getFromTo(int x, int y);
};
