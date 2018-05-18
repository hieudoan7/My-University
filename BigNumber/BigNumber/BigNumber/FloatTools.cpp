#include "FloatTools.h"

// split floating number
pair <string, string> split_float_num(string input) {
	pair <string, string> res;

	int k = input.find('.');

	if (k == -1) {
		res.first = input;
		res.second = "0";
	}
	else {
		res.first = input.substr(0, input.find('.'));
		res.second = "0" + input.substr(input.find('.') + 1);
	}
	return res;
}

// string div and mod 2
pair <string, int> div2(string num) {
	string quotient = "";
	int val = num[0] - '0';

	for (int i = 0; i < num.size(); i++) {
		quotient += (val / 2 + '0');
		if (i + 1 < num.size())
			val = (val % 2) * 10 + (num[i + 1] - '0');
	}

	pair<string, int> res;
	res.first = quotient;
	res.second = ((num[num.size() - 1] - '0') % 2) ? 1 : 0;

	return res;
}

// string num equal to zero
bool isZero(string s)
{
	for (int i = 0; i < s.size(); i++) {
		if (s[i] != '0')
			return false;
	}
	return true;
}

// string mul 2
string mul2(string num)
{
	int sz = num.size();
	int val = 0, mem = 0;
	for (int i = sz - 1; i >= 0; i--)
	{
		val = num[i] - '0';
		num[i] = (val * 2 + mem) % 10 + '0';
		mem = (val * 2 + mem) / 10;
	}

	return num;
}

// get bit of part before dot
string getBitPart1(string part1)
{
	stack <int> st;
	pair <string, int> psi;

	while (!isZero(part1))
	{
		psi = div2(part1);
		st.push(psi.second);
		part1 = psi.first;
	}
	string res;
	while (st.empty() == false)
	{
		res.push_back(st.top() + '0');
		st.pop();
	}

	return res;
}

// get bit of part after dot
string getBitPart2(string part2)
{
	string res;
	int max_bit = 0;
	while (!isZero(part2) && max_bit < 112)
	{
		part2 = mul2(part2);
		res.push_back(part2[0]);
		if (part2[0] == '1')
		{
			part2[0] = '0';
		}
		max_bit++;
	}

	return res;
}

// count exponent
int countExponent(string bitPart1, string bitPart2)
{
	int dot_pos = bitPart1.size();
	int exp = 0;
	for (int i = 0; i < bitPart1.size(); i++)
		if (bitPart1[i] == '1')
		{
			exp = dot_pos - i - 1;
			return exp;
		}

	for (int i = 0; i < bitPart2.size(); i++)
		if (bitPart2[i] == '1')
		{
			exp = -(i + 1);
			break;
		}
	return exp;
}

//convert bias
string cvBias(int a)
{
	string bit;
	if (a <= 0) {

		a = -a;
		stringstream ss;
		ss << a;

		string str_a;
		ss >> str_a;
		bit = getBitPart1(str_a);

		for (int i = bit.size() - 1; i >= 0; i--) {
			if (bit[i] == '1')
				bit[i] = '0';
			else bit[i] = '1';
		}

		int tmp = 14 - bit.size();
		for (int i = 1; i <= tmp; i++)
			bit.insert(bit.begin(), '1');
		bit.insert(bit.begin(), '0');
	}
	else {
		a = a - 1; // because 100000000000..00

		stringstream ss;
		ss << a;

		string str_a;
		ss >> str_a;
		bit = getBitPart1(str_a);

		for (int i = bit.size() - 1; i >= 0; i--) {
			if (bit[i] == '1')
				bit[i] = '1';
			else bit[i] = '0';
		}
		int tmp = 14 - bit.size();
		for (int
			i = 1; i <= tmp; i++)
			bit.insert(bit.begin(), '0');
		bit.insert(bit.begin(), '1');
	}

	return bit;
}
//xác định xem 1 đoạn bit cụ thể có tất cả các bit đều giống nhau hay ko
bool isAllBit(string bit, int val, int s, int e) {
	for (int i = s; i <= e; i++) {
		if (bit[i] != val)
			return false;
	}
	return true;
}
//phân loại số chấm động
//0: số zero
//1: dạng chuẩn
//2: dạng không chuẩn
//3: Inf
//4: số báo lỗi
int Classification(string bit)
{
	if (!isAllBit(bit, '0', 1, 15) && !isAllBit(bit, '1', 1, 15))
		return 1;

	if (isAllBit(bit, '0', 1, 127))
		return 0;

	if (isAllBit(bit, '0', 1, 15) && !isAllBit(bit, '0', 16, 127))
		return 2;

	if (isAllBit(bit, '1', 1, 15) && isAllBit(bit, '0', 16, 127))
		return 3;

	return 4;
}

// chuyển từ bit sang số (bias)
int cvBiasToNum(string exp_bit)
{
	int res = 0;
	bool positve = false;
	if (exp_bit[0] == '0') {
		for (int i = 1; i < exp_bit.size(); i++) {
			if (exp_bit[i] == '1')
				exp_bit[i] = '0';
			else
				exp_bit[i] = '1';
		}
	}
	else {
		exp_bit[0] = '0';
		positve = true;
	}

	for (int i = 0; i < exp_bit.size(); i++) {
		res += (exp_bit[i] - '0') * pow(2, 14 - i);
	}

	if (positve) res++;
	else res = -res;

	return res;
}

//nhân 2 ở dạng chuỗi
string F_strx2(string str) {
	int nho = 0;
	string res = "";
	for (int i = str.length() - 1; i >= 0; i--) {
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

//mũ 2 ở dạng chuỗi
string F_2Expn(int n) {
	string str = "1";
	for (int i = 0; i < n; i++) {
		str = F_strx2(str);
	}
	return str;
}

//chèn 0 vào đầu
void F_chen0(string &a, int n) {
	while (n--) {
		a.insert(a.begin(), '0');
	}
}

//cộng 2 số thập phân ở dạng chuỗi
string F_strPlusStr(string a, string b) {
	int lenA = a.length();
	int lenB = b.length();
	if (lenA > lenB) F_chen0(b, lenA - lenB);
	else F_chen0(a, lenB - lenA);
	int i = a.length() - 1;
	string res = "";
	int nho = 0;
	while (i >= 0) {
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

string BitToNumBeforeDot(string bit)
{
	int cur_exp = bit.size() - 1;
	string res = "0";
	for (int i = 0; i < bit.size(); i++) {
		if (bit[i] == '1')
			res = F_strPlusStr(res, F_2Expn(cur_exp));
		cur_exp--;
	}
	return res;
}

//2 mu trừ n
string fstrDiv2v2(string str) { //str div 2 version 2 ko bỏ "0" đầu
	string res = "";
	int i = 0;
	int soDu = 0;
	while (i < str.length()) {
		soDu = 10 * soDu + (str[i] - '0');
		res += (soDu / 2) + '0';
		soDu %= 2;
		i++;
	}
	return res;
}

//trả về kiểu chuỗi 2^(-n)
string _2Exp_n(int n) {
	string _2Exp_i = "5";
	for (int i = 2; i <= n; i++) {
		_2Exp_i = fstrDiv2v2(_2Exp_i + "0");
	}
	return _2Exp_i;
}

// cộng chuỗi số sau dấu chấm
string addFrac(string a, string b) {
	while (a.length() < b.length()) a += "0";
	return F_strPlusStr(a, b);
}


string BitToNumAfterDot(string bit)
{
	int cur_exp = 1;
	string res = "0";
	for (int i = 0; i < bit.size(); i++) {
		if (bit[i] == '1')
			res = addFrac(res, _2Exp_n(cur_exp));
		cur_exp++;
	}
	return res;
}