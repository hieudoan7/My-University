#include <iostream>
#include <fstream>
#include <cstring>
#include <sstream>
#include "QInt.h"
#include "QFloat.h"
#include "IntFunction.h"
using namespace std;

int main(int argc,char* argv[]) {

	fstream fi, fo;
	fi.open(argv[1], ios::in);
	fo.open(argv[2], ios::out);

	int choice;
	do {
		cout << "Enter 1: QInt";
		cout << "\nEnter 2: QFloat\n";
		cin >> choice;
	} while (choice != 1 && choice != 2);

	if (choice == 1)
	{
		while (!fi.eof()) {
			string tmp = "";
			getline(fi, tmp);
			string res = readLine(tmp);
			fo << res << endl;
		}
	}
	else {
		while (!fi.eof())
		{
			string tmp = "";
			getline(fi, tmp);
			stringstream ss(tmp);
			vector <string> line;
			//
			QFloat f1,f2,f3;
			int base;

			while (ss >> tmp)
			{
				line.push_back(tmp);
			}

			if (line.size() == 3)
			{
				stringstream sstr1(line[0]);
				sstr1 >> base;

				f1.ScanQFloat(line[2], base);

				stringstream sstr2(line[1]);
				sstr2 >> base;

				fo << f1.PrintQFloat(base) << endl;
			}
			else if (line.size() == 4){
				stringstream sstr1(line[0]);
				sstr1 >> base;
			
				f1.ScanQFloat(line[1], base);
				f2.ScanQFloat(line[3], base);

				char opr = line[2][0];
				if (opr == '+')
					f3 = f1 + f2;
				else if (opr == '-')
					f3 = f1 - f2;
				else if (opr == '*')
					f3 = f1 * f2;
				else if (opr == '/')
					f3 = f1 / f2;
				else
					fo << "invalid operator";

				fo << f3.PrintQFloat(base) << endl;
			}
		}
	}

	return 0;
}
