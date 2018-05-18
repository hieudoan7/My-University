// FTP_Client.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "FTP_Client.h"
#include "afxsock.h"
#include <string>
#include <cstring>
#include <string.h>
#include <vector>
#include <sstream>
#include <fstream>
#include <Windows.h>

//#include <iostream>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif
#define auxDIBImageLoad auxDIBImageLoadW
#define MAX_BUF_LEN 1000
#define MAXLINE 1000

#define DEFAULT_PORT 27015;

// The one and only application object

CWinApp theApp;

using namespace std;
// function support
struct portNumber{
	unsigned char p[2];
};
struct ipNumber{
	unsigned int ip[4];
};
const wchar_t *GetWC(const char *c)
{
	const size_t cSize = strlen(c) + 1;
	wchar_t* wc = new wchar_t[cSize];
	mbstowcs(wc, c, cSize);

	return wc;
}
void sendCmd(CSocket& c, string msg){
	msg += "\r\n";
	c.Send(msg.c_str(), msg.length(), 0);
}
string printReply(CSocket& c){

	char response[MAX_BUF_LEN]={0};
	int byte=c.Receive(response, MAX_BUF_LEN, 0);
	response[byte] = '\0';
	string responseCode = string(response, 3);
	string response2 = string(response);
	cout << response;
	int pos = response2.find("\r\n");
	if (pos != response2.length()-2){
		responseCode = string(response2,pos+2, 3);
	}
	return responseCode;
	//return string(response);
}

portNumber getPort(CSocket& c){ //get port and ip
	UINT port;
	CString tmp;
	c.GetSockName(tmp, port);
	portNumber p;
	p.p[0] = port / 256;
	p.p[1] = port % 256;
	return p;
}
string normalizeIP(string IP){
	string res = IP;
	for (int i = 0; i <res.length(); i++)
	if (res[i] == '.') res[i] = ',';
	return res;
}
bool login(CSocket& c){
	string request, responseCode;
	cout << "username: ";
	cin >> request;
	sendCmd(c, "user " + request);
	responseCode=printReply(c);
	cout << "password: ";
	cin >> request;
	sendCmd (c,"PASS " + request);
	responseCode = printReply(c);
	if (responseCode == "230") {
		return true;
	}
	return false;

}

string ip;  //global ip
portNumber port; //global port

//2) Liệt kê được danh sách các thư mục, tập tin trên Server (ls,dir)
void ls(CSocket& c){
	//normalize IP
	for (int i = 0; i < ip.length(); i++){
		if (ip[i] == '.') ip[i] = ',';
	}
	if (port.p[1] < 255) port.p[1]++;
	else port.p[0]++;
	CSocket datasock, serverConnect;
	int portN = port.p[0] * 256 + port.p[1];
	datasock.Create(portN);
	datasock.Listen(); //be on a temporoty server
	// create data connection
	string cmd = "PORT " + ip + "," + to_string(port.p[0]) + "," + to_string(port.p[1])+"\r\n";
	sendCmd(c, cmd);
	printReply(c);
	sendCmd(c,"NLST");
	string _226=printReply(c);
	//
	if (datasock.Accept(serverConnect)){
		printReply(serverConnect);
		serverConnect.Close();
		//delay
		if (_226 != "226"){
			string tmp2 = printReply(c);
		}
	}
	else{  ls(c); }
	
	datasock.Close();
}
void dir(CSocket& c){
	//normalize IP
	for (int i = 0; i < ip.length(); i++){
		if (ip[i] == '.') ip[i] = ',';
	}
	if (port.p[1] < 255) port.p[1]++;
	else port.p[0]++;
	CSocket datasock, serverConnect;
	int portN = port.p[0] * 256 + port.p[1];
	datasock.Create(portN);
	datasock.Listen(); //be on a temporoty server
	// create data connection
	string cmd = "PORT " + ip + "," + to_string(port.p[0]) + "," + to_string(port.p[1]) + "\r\n";
	sendCmd(c, cmd);
	printReply(c);
	sendCmd(c, "LIST");
	string _226 = printReply(c);
	if (datasock.Accept(serverConnect)){
		printReply(serverConnect);
		serverConnect.Close();
		//delay
		if (_226 != "226"){
			string tmp2 = printReply(c);
		}
	}
	else{ ls(c); }

	datasock.Close();
}

//7. Thay đổi đường dẫn trên server

void cd(CSocket& c, string& path){
	string cmd = "CWD " + path;
	sendCmd(c, cmd);
	printReply(c);
}

//8. Thay đổi đường dẫn dưới client
string localDir;    //always two backslash  "//"
void lcd(const string& path){
	//handle path by duplicate backslash
	if (CreateDirectory(GetWC(path.c_str()), NULL) == TRUE)
	{
		cout << "Local directory now "+path+"\n";
		localDir = path;
	}
	else if (GetLastError() == ERROR_ALREADY_EXISTS)
	{
		cout << "Already! Local directory now " + path + "\n";
		localDir = path;
	}
	else
		cout << "Cannot change directory\n";
}

//6. Download 1 file từ server
void get(CSocket& c, string filename){
	//normalize IP
	for (int i = 0; i < ip.length(); i++){
		if (ip[i] == '.') ip[i] = ',';
	}
	if (port.p[1] < 255) port.p[1]++;
	else port.p[0]++;
	CSocket datasock, serverConnect;
	int portN = port.p[0] * 256 + port.p[1];
	datasock.Create(portN);
	datasock.Listen(); //be on a temporoty server
	// create data connection
	string cmd = "PORT " + ip + "," + to_string(port.p[0]) + "," + to_string(port.p[1]) + "\r\n";
	sendCmd(c, cmd);
	printReply(c);
	sendCmd(c, "RETR "+filename);
	string _226 = printReply(c);
	if (datasock.Accept(serverConnect)){
		ofstream fo((localDir + "\\" + filename).c_str(), ios::out | ios::binary); //create file already to write
		char buf[1024];
		int byteNum = 0;
		do{
			byteNum = serverConnect.Receive(buf, 1024, 0);
			fo.write(buf, byteNum);
		} while (byteNum > 0);

		fo.close();
		serverConnect.Close();
		//print reply after
		if(_226!="226") printReply(c);
	}
	else{ get(c, filename); }

	datasock.Close();
}

//7. Upload 1 file đến server
void put(CSocket& c, string filename){   //ko duoc put file đã tồn tại
	//normalize IP
	for (int i = 0; i < ip.length(); i++){
		if (ip[i] == '.') ip[i] = ',';
	}
	if (port.p[1] < 255) port.p[1]++;
	else port.p[0]++;
	CSocket datasock, serverConnect;
	int portN = port.p[0] * 256 + port.p[1];
	datasock.Create(portN);
	datasock.Listen(); //be on a temporoty server
	// create data connection
	string cmd = "PORT " + ip + "," + to_string(port.p[0]) + "," + to_string(port.p[1]) + "\r\n";
	sendCmd(c, cmd);
	printReply(c);
	sendCmd(c, "STOR " + filename);
	string _226=printReply(c);
	//
	if (datasock.Accept(serverConnect)){
		ifstream fi((localDir + "\\" + filename).c_str(), ios::in | ios::binary); //create file already to write
		char buf[1024];

		fi.seekg(ios::beg);
		if (fi.is_open()) {
			while (!fi.eof()){
				fi.read(buf, 1024); //1024 chars ~ 1024 bytes
				int numChar = fi.gcount();  //the number of chars (bytes) already transferred from file fi to buf 
				while (numChar > 0){			// send immediately after read from fi file
					int lenBufSend = min(1024, numChar);   //trách ghi dư 
					numChar -= serverConnect.Send(buf, lenBufSend);
				}
			}
		}
		else cout<<"Can't open the file!";
		fi.close();
		//print reply after
		serverConnect.Close();   //đóng để client nó nhận msg từ server, avoid conflicting
		if(_226!="226") printReply(c);
	}
	else{put(c, filename); }

	datasock.Close();
}

//9. Xóa 1 file trên server
void del(CSocket& c, string filename){
	sendCmd(c,"DELE " + filename);
	printReply(c);
}

//11. Tạo thư mục trên server (mkdir)
void mkdir(CSocket& c, string& foldername){
	sendCmd(c, "XMKD " + foldername);
	printReply(c);
}

//12. Xóa thư mục rỗng trên server (rmkdir)
void rmkdir(CSocket& c,string& foldername){
	sendCmd(c, "XRMD " + foldername);
	printReply(c);
}

//13. Hiển thị đường dẫn hiện tại trên server (pwd)
void pwd(CSocket& c){
	sendCmd(c, "XPWD");
	printReply(c);
}


vector<string> split(string s){
	vector<string> res;
	char tmp[100];
	int j = 0;
	for (int i = 0; i<s.length(); i++){
		if (s[i] == '\r') {
			tmp[j] = '\0';
			j = 0;
			res.push_back(string(tmp));
			i++; continue;
		}
		tmp[j] = s[i];
		j++;
	}
	return res;
}
//6. Download nhiều file từ server (mget)
void mget(CSocket& c, string filename){  //example filename: *.txt
	sendCmd(c, "TYPE A");
	printReply(c);
	for (int i = 0; i < ip.length(); i++){
		if (ip[i] == '.') ip[i] = ',';
	}
	if (port.p[1] < 255) port.p[1]++;
	else port.p[0]++;
	CSocket datasock, serverConnect;
	int portN = port.p[0] * 256 + port.p[1];
	datasock.Create(portN);
	datasock.Listen(); //be on a temporoty server
	// create data connection
	string cmd = "PORT " + ip + "," + to_string(port.p[0]) + "," + to_string(port.p[1]) + "\r\n";
	sendCmd(c, cmd);
	printReply(c);
	sendCmd(c, "NLST "+filename);
	string _226=printReply(c);
	//
	vector<string> fname;
	if (datasock.Accept(serverConnect)){
		char response[MAX_BUF_LEN];
		int byte = serverConnect.Receive(response, MAX_BUF_LEN, 0);
		response[byte] = '\0';
		fname = split(string(response));
		serverConnect.Close();
		//delay
		if(_226!="226") printReply(c);
	}
	else{ mget(c, filename); }
	sendCmd(c, "TYPE A");
	printReply(c);
	for (int i = 0; i < fname.size(); i++){
		cout << "mget " + fname[i] + "?";
		char ch = getchar();
		if (ch == 10){				 //enter là 10, đkm mất 10 mins
			get(c, fname[i]);
		}
	}
	datasock.Close();

}

void getDir(string d, vector<string> & f)
{
	FILE* pipe = NULL;
	string pCmd = "dir /B /S " + d;
	char buf[256];

	if (NULL == (pipe = _popen(pCmd.c_str(), "rt")))
	{
		return;
	}

	while (!feof(pipe))
	{
		if (fgets(buf, 256, pipe) != NULL)
		{
			f.push_back(string(buf));
		}

	}
	_pclose(pipe);
	vector<string> v;  //only get part filename
	for (int i = 0; i < f.size(); i++){
		//erase ending character
		f[i].erase(f[i].end() - 1);
		int j = f[i].length() - 1;
		while (f[i][j] != '\\') j--;
		string filename = f[i].substr(j + 1);
		v.push_back(filename);
	}
	f = v;
}

//5. Upload nhiều file đến server (mput) 
void mput(CSocket& c, string filename){  //filename like *.txt
	vector<string> fname;
	getDir(localDir, fname);
	filename.erase(filename.begin());
	for (int i = 0; i < fname.size(); i++){
		if (fname[i].find(filename) != -1){
			cout << "mput " + fname[i] + "?";
			char ch = getchar();
			if (ch == 10){
				put(c, fname[i]);
			}
		}
	}
}


//10) Xóa nhiều file trên server (mdelete)  //inhert mget
void mdelete(CSocket& c, string filename){  //example filename: *.txt
	sendCmd(c, "TYPE A");
	printReply(c);
	for (int i = 0; i < ip.length(); i++){
		if (ip[i] == '.') ip[i] = ',';
	}
	if (port.p[1] < 255) port.p[1]++;
	else port.p[0]++;
	CSocket datasock, serverConnect;
	int portN = port.p[0] * 256 + port.p[1];
	datasock.Create(portN);
	datasock.Listen(); //be on a temporoty server
	// create data connection
	string cmd = "PORT " + ip + "," + to_string(port.p[0]) + "," + to_string(port.p[1]) + "\r\n";
	sendCmd(c, cmd);
	printReply(c);
	sendCmd(c, "NLST " + filename);
	string _226 = printReply(c);
	vector<string> fname;
	if (datasock.Accept(serverConnect)){
		cout << "OK";
		char response[MAX_BUF_LEN];
		int byte = serverConnect.Receive(response, MAX_BUF_LEN, 0);
		response[byte] = '\0';
		fname = split(string(response));

		serverConnect.Close();
		//delay
		if (_226 != "226") printReply(c);
	}
	else{ mdelete(c, filename); }
	sendCmd(c, "TYPE A");
	printReply(c);
	for (int i = 0; i < fname.size(); i++){
		cout << "mdelete " + fname[i] + "?";
		char ch = getchar();
		if (ch == 10){				 //enter là 10, đkm mất 10 mins
			del(c, fname[i]);
		}
	}

	datasock.Close();

}


int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	int nRetCode = 0;

	HMODULE hModule = ::GetModuleHandle(NULL);

	if (hModule != NULL)
	{
		// initialize MFC and print and error on failure
		if (!AfxWinInit(hModule, NULL, ::GetCommandLine(), 0))
		{
			// TODO: change error code to suit your needs
			_tprintf(_T("Fatal Error: MFC initialization failed\n"));
			nRetCode = 1;
		}
		else
		{
			// TODO: code your application's behavior here.
			//Khai báo sử dụng socket trong windows
			AfxSocketInit(NULL);
			CSocket client;
			client.Create();
			//display
			do{
				cout << "Enter IP server: ";
				cin >> ip;
			} while (!client.Connect(GetWC(ip.c_str()), 21)); //port 21
			string tmp;
			tmp = printReply(client); //get hết mấy cái tào lao 
			//log in
			while (!login(client)){};
			port = getPort(client);
			int k = 1;
			while (1){
				cout << "ftp> ";
				vector<string> token;
				token.clear();
				string word;
				string line;
				if(k) cin.ignore(),k=0;
				getline(cin, line);
				stringstream ss;
				ss << line;
				while (getline(ss, word, ' ')){
					token.push_back(word);
				}

				if (token[0] == "!ls") { system("ls"); cout << endl; }			//system("command"); gọi vào command line
				if (token[0] == "!dir") { system("dir"); cout << endl; }
				if (token[0] == "ls") 
					ls(client);
				if (token[0] == "dir") dir(client);
				if (token[0] == "put"){
					put(client, token[1]); //token[1] is filename from local computer
				}
				if (token[0] == "get"){
					get(client, token[1]); //token[1] is filename
				}
				if (token[0] == "mput"){
					mput(client, token[1]);
				}
				if (token[0] == "mget"){
					mget(client, token[1]);
				}
				if (token[0] == "cd") 
					cd(client, token[1]);
				if (token[0] == "lcd"){   //temporary don't use path contain space because delimination is space in getline(...)
					if (token.size() == 1) {
						string defaultpath = "C:\\Windows\\System32";
						lcd(defaultpath);
					}
					else lcd(token[1]);
				}
				if (token[0] == "delete"){
					del(client, token[1]);  //token[1] is filename need to delete
				}
				if (token[0] == "mdelete"){
					mdelete(client, token[1]);
				}
				if (token[0] == "mkdir") {
					mkdir(client,token[1]);
				}
				if (token[0] == "rmkdir") {
					rmkdir(client,token[1]);
				}
				if (token[0] == "pwd"){
					pwd(client);
				}
				
				if (token[0] == "exit" || token[0] == "quit") {
					cout << "Goodbye!";
					break;
				}
			}
			client.Close();
			//serverConnect.Close();
		}
	}
	else
	{
		// TODO: change error code to suit your needs
		_tprintf(_T("Fatal Error: GetModuleHandle failed\n"));
		nRetCode = 1;
	}

	return nRetCode;
}
