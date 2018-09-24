#pragma once
#include "IntFunction.h"
#include "QInt.h"

//chuỗi = chuỗi chia 2
string strDiv2(string str){
	string res = "";
	int i = 0;
	int soDu = 0;
	while (i < str.length()){
		soDu = 10 * soDu + (str[i] - '0');
		res += (soDu / 2) + '0';
		soDu %= 2;
		i++;
	}
	int l = res.length() - 1;
	for (int i = 0; i < res.length() - 1; i++){
		if (res[i] != '0'){
			l = i;
			break;
		}
	}
	return res.substr(l);
}

//Chuyển thập phân sang dãy bit nhị phân
string DecToBin(string Dec){
	string res = "";
	int minus = 0;
	if (Dec[0] == '-') {
		Dec.erase(Dec.begin());
		minus = 1;
	}
	while (Dec != "0"){
		int bit = (Dec[Dec.length() - 1] - 48) % 2;
		res.insert(res.begin(), bit + '0');
		Dec = strDiv2(Dec);
	}
	if (minus){
		QInt temp(res);
		temp = -temp;
		res = temp.binArr();
	}
	return res;
}
//chuỗi nhân 2 trả về chuỗi
string strx2(string str){
	int nho = 0;
	string res = "";
	for (int i = str.length() - 1; i >= 0; i--){
		int tmp = (str[i] - '0') * 2 + nho;
		if (tmp >= 10) {
			nho = 1; tmp -= 10;
		}
		else nho = 0;
		res.insert(res.begin(), tmp + '0');
	}
	if (nho) res.insert(res.begin(), nho + '0');
	return res;
}

//chuỗi * 2^n, return chuỗi
string _2Expn(int n){
	string str = "1";
	for (int i = 0; i < n; i++){
		str = strx2(str);
	}
	return str;
}

//chèn bit 0 vào đầu
void chen0(string &a, int n){
	while (n--){
		a.insert(a.begin(), '0');
	}
}

//cộng 2 chuỗi, trả về chuỗi
string strPlusStr(string a, string b){
	int lenA = a.length();
	int lenB = b.length();
	if (lenA > lenB) chen0(b, lenA - lenB);
	else chen0(a, lenB - lenA);
	int i = a.length() - 1;
	string res = "";
	int nho = 0;
	while (i >= 0){
		int tmp = (a[i] - '0') + (b[i] - '0') + nho;
		if (tmp >= 10) {
			nho = 1; tmp -= 10;
		}
		else nho = 0;
		res.insert(res.begin(), tmp + '0');
		i--;
	}
	if (nho) res.insert(res.begin(), nho + '0');
	return res;
}
//Chuyển từ chuỗi nhị phân sang số thập phân
string BinToDec(string Bin){

	int minus = 0;
	if (Bin.length() == 128 && Bin[0] == '1') minus = 1;
	if (minus){
		QInt temp(Bin);
		QInt one(0, 0, 0, 1);
		temp = temp - one;
		temp = ~temp;
		Bin = temp.binArr();
	}
	int i = 127;
	string res = "";
	int j = Bin.length() - 1;
	while (i > 0 && j >= 0){
		int bit = Bin[j] - '0';
		if (bit){
			string mu2 = _2Expn(127 - i);
			res = strPlusStr(res, mu2);
		}
		i--;
		j--;
	}
	if (res == "") res = "0";
	if (minus) res.insert(res.begin(), '-');
	return res;
}

//Chuyển dãy bit nhị phân sang cơ số 16
string BinToHex(string bin){
	int len = bin.length();
	if (len % 4 != 0){
		for (int k = 0; k < (4 - len % 4); k++)
			bin.insert(bin.begin(), '0');
	}
	string res = "";
	int i = bin.length() - 1;
	while (i >= 0){
		int num = 0;
		for (int j = 0; j < 4; j++){
			num += pow(2, j)*(bin[i] - '0');
			i--;
		}
		if (num >= 10) num += 55;
		else num += '0';
		res.insert(res.begin(), num);
	}
	return res;
}

//chuyển cơ số 16 sang dãy bit nhị phân
string HexToBin(string Hex){
	int i = Hex.length() - 1;
	string res = "";
	while (i >= 0){
		if (Hex[i] >= 65) Hex[i] -= 55;
		for (int j = 0; j < 4; j++){
			int bit_j = (Hex[i] >> j) & 1;
			res.insert(res.begin(), bit_j + '0');
		}
		i--;
	}
	while (res[0] == '0'){
		res.erase(res.begin());
	}
	return res;
}

//chuyển base: 10->16
string DecToHex(string Dec){
	return BinToHex(DecToBin(Dec));
}
//chuyển base: 16->10
string HexToDec(string Hex){
	return BinToDec(HexToBin(Hex));
}

//tách dòng lệnh
vector<string> splitLine(string line){
	vector<string> res;
	char* s = strdup(line.c_str());
	char* token = strtok(s, " ");
	while (token != NULL){
		res.push_back((string)token);
		token = strtok(NULL, " ");
	}
	return res;
}
//Đọc từng dòng theo format file input.txt
string readLine(string line){
	vector<string> phan = splitLine(line);
	switch (phan.size()){
	case 3:{
			   int base1 = stoi(phan[0]);
			   int base2 = stoi(phan[1]);
			   QInt num;
			   num.scan(phan[2], base1);
			   return num.print(base2);
	}
	case 4:{
			   int base = stoi(phan[0]);
			   QInt a;
			   QInt res;
			   a.scan(phan[1], base);
			   if (phan[2] == "<<"){
				   int b = stoi(phan[3]);
				   res = a << b;
			   }
			   else if (phan[2] == ">>"){
				   int b = stoi(phan[3]);
				   res = a >> b;
			   }
			   else {
				   QInt b;
				   b.scan(phan[3], base);
				   if (phan[2] == "+") res = a + b;
				   else if (phan[2] == "-") res = a - b;
				   else if (phan[2] == "*") res = a*b;
				   else if (phan[2] == "/") {
					   QInt zero(0, 0, 0, 0);
					   if (b == zero)
					   {
						   if (a.getBit(0) == 1) 
							   return "-Inf";
						   return "Inf";
					   }
					   res = a / b;
				   }
				   else if (phan[2] == "&") res = a&b;
				   else if (phan[2] == "|") res = a | b;
				   else if (phan[2] == "^") res = a^b;
			   }
			   return res.print(base);
	}
	}
}

//int main(){
//	string Dec = "-1234567789101112131415";
//	string Bin = DecToBin(Dec);
//	cout << Bin << endl;
//	string Dec2 = BinToDec(Bin);
//	cout << Dec2 << endl;
//	string hex = BinToHex(Bin);
//	cout << hex << endl;
//	string bin2 = HexToBin(hex);
//	cout << bin2 << endl;
//	string Hex = DecToHex(Dec);
//	cout << Hex << endl;
//	string Dec3 = HexToDec(Hex);
//	cout << Dec3 << endl;
//	return 0;
//}