#include <Windows.h>
#include <iostream>
#include <cstdio>
#include <string>
#include <sstream>
#include <vector>
#include <io.h>
using namespace std;
#include <direct.h>
#include "tinyxml/tinyxml.h"
#include "Str.h"
#include "string.h"
#define BUF_SIZE    256
//移除相对路径
const char * rmredir(const char *pDir){
	int i;
	const char* pszDir =NULL;
	int iLen = strlen(pDir);
	for (i = 0; i < iLen; i++){
		if (pDir[i] == '.' || pDir[i] == '\\' || pDir[i] == '/')
		{
			i++;
		}
		else
		{
			pszDir = &pDir[i];
			break;
		}

	}
	return pszDir;
}
int CreatDir(const char *pDir)
{
	int i = 0;
	int iRet;
	int iLen;
	char* pszDir;

	if (NULL == pDir)
	{
		return 0;
	}
	iLen = strlen(pDir);
	//pszDir = strdup(pDir);
	//移除 
	pszDir =(char *) rmredir(pDir);
	//
	

	// 创建中间目录  
	for (i = 0; i < iLen; i++)
	{
		if (pszDir[i] == '\\' || pszDir[i] == '/')
		{
			pszDir[i] = '\0';

			//如果不存在,创建  
			iRet = _access(pszDir, 0);
			if (iRet != 0)
			{
				iRet = _mkdir(pszDir);
				if (iRet != 0)
				{
					return -1;
				}
			}
			//支持linux,将所有\换成/  
			pszDir[i] = '/';
		}
	}

	iRet = _mkdir(pszDir);
	//free(pszDir);
	return iRet;
}
char filecpy(const char* src,char *dst){
	char * rsrc0;
	char * rsrc;
	const char * rdst;
	FILE *in_file, *out_file;
	char data[BUF_SIZE];
	size_t bytes_in, bytes_out;
	long len = 0;

	if ((in_file = fopen(src, "rb")) == NULL)
	{
		
		return 2;
	}

	strcpy(data, src);
	strrchr(data, '.')[0] = 0;//移除扩展名

	strrchr(data, '\\')[0] = 0;//移除文件名

	CreatDir(data);
	if (_access("src", 0) == -1){
		printf("file not exist");
		_mkdir("src");
		_mkdir("src\\src0");
		_mkdir("src\\src0\\src1");
	}

	//移除相对路径
	rdst = rmredir(src);


	if ((out_file = fopen(rdst, "wb+")) == NULL)
	{
		
		return 3;
	}


	while ((bytes_in = fread(data, 1, BUF_SIZE, in_file)) > 0)
	{
		bytes_out = fwrite(data, 1, bytes_in, out_file);
		if (bytes_in != bytes_out)
		{
			perror("Fatal write error.\n");
			return 4;
		}
		len += bytes_out;
		printf("copying file .... %d bytes copy\n", len);
	}

	fclose(in_file);
	fclose(out_file);
}

bool make_dsw_file(const char* project_name)
{
	const char* dsw_content = 
		"Microsoft Developer Studio Workspace File, Format Version 6.00\r\n"
		"# WARNING: DO NOT EDIT OR DELETE THIS WORKSPACE FILE!\r\n"
		"\r\n"
		"###############################################################################\r\n"
		"\r\n"
		"Project: \"%s\"=\".\\%s.dsp\" - Package Owner=<4>\r\n"
		"\r\n"
		"Package=<5>\r\n"
		"{{{\r\n"
		"}}}\r\n"
		"\r\n"
		"Package=<4>\r\n"
		"{{{\r\n"
		"}}}\r\n"
		"\r\n"
		"###############################################################################\r\n"
		"\r\n"
		"Global:\r\n"
		"\r\n"
		"Package=<5>\r\n"
		"{{{\r\n"
		"}}}\r\n"
		"\r\n"
		"Package=<3>\r\n"
		"{{{\r\n"
		"}}}\r\n"
		"\r\n"
		"###############################################################################\r\n"
		"\r\n"
		"";
	
	char dsw[1024] = {0};
	string name(project_name);
	name += ".dsw";
	FILE* fp_dsw = fopen(name.c_str(),"wb");
	if(fp_dsw == NULL){
		cout<<"错误:无法打开 "<<name<<" 用于写!"<<endl;
		return false;
	}

	int len = _snprintf(dsw,sizeof(dsw),dsw_content,project_name,project_name);
	if(fwrite(dsw,1,len,fp_dsw) == len){
		cout<<"已创建 "<<name<<" 工作空间文件!"<<endl;
		fclose(fp_dsw);
		return true;
	}else{
		cout<<"错误:文件写入错误!"<<endl;
		fclose(fp_dsw);
		remove(name.c_str());
		return false;
	}
}

bool make_dsp_file(const char* project_name,vector<string>& groups,string& define,string& includepath)
{
	const char* dsp_content = 
		"# Microsoft Developer Studio Project File - Name=\"%s\" - Package Owner=<4>\r\n"
		"# Microsoft Developer Studio Generated Build File, Format Version 6.00\r\n"
		"# ** DO NOT EDIT **\r\n"
		"\r\n"
		"# TARGTYPE \"Win32 (x86) Console Application\" 0x0103\r\n"
		"\r\n"
		"CFG=%s - Win32 Debug\r\n"
		"!MESSAGE This is not a valid makefile. To build this project using NMAKE,\r\n"
		"!MESSAGE use the Export Makefile command and run\r\n"
		"!MESSAGE \r\n"
		"!MESSAGE NMAKE /f \"%s.mak\".\r\n"
		"!MESSAGE \r\n"
		"!MESSAGE You can specify a configuration when running NMAKE\r\n"
		"!MESSAGE by defining the macro CFG on the command line. For example:\r\n"
		"!MESSAGE \r\n"
		"!MESSAGE NMAKE /f \"%s.mak\" CFG=\"%s - Win32 Debug\"\r\n"
		"!MESSAGE \r\n"
		"!MESSAGE Possible choices for configuration are:\r\n"
		"!MESSAGE \r\n"
		"!MESSAGE \"%s - Win32 Release\" (based on \"Win32 (x86) Console Application\")\r\n"
		"!MESSAGE \"%s - Win32 Debug\" (based on \"Win32 (x86) Console Application\")\r\n"
		"!MESSAGE \r\n" /*在这之前有7个项目名*/
		"\r\n"
		"# Begin Project\r\n"
		"# PROP AllowPerConfigDependencies 0\r\n"
		"# PROP Scc_ProjName \"\"\r\n"
		"# PROP Scc_LocalPath \"\"\r\n"
		"CPP=cl.exe\r\n"
		"RSC=rc.exe\r\n"
		"\r\n"
		"!IF  \"$(CFG)\" == \"%s - Win32 Release\"\r\n" //项目名
		"\r\n"
		"# PROP BASE Use_MFC 0\r\n"
		"# PROP BASE Use_Debug_Libraries 0\r\n"
		"# PROP BASE Output_Dir \"Release\"\r\n"
		"# PROP BASE Intermediate_Dir \"Release\"\r\n"
		"# PROP BASE Target_Dir \"\"\r\n"
		"# PROP Use_MFC 0\r\n"
		"# PROP Use_Debug_Libraries 0\r\n"
		"# PROP Output_Dir \"Release\"\r\n"
		"# PROP Intermediate_Dir \"Release\"\r\n"
		"# PROP Target_Dir \"\"\r\n"
		"# ADD BASE CPP /nologo /W3 /GX /O2 %s /YX /FD /c\r\n" //宏定义:/D \"WIN32\" /D \"NDEBUG\"
		"# ADD CPP /nologo /W3 /GX /O2 %s %s /YX /FD /c\r\n" //目录+宏定义
		"# ADD BASE RSC /l 0x804 /d \"NDEBUG\"\r\n"
		"# ADD RSC /l 0x804 /d \"NDEBUG\"\r\n"
		"BSC32=bscmake.exe\r\n"
		"# ADD BASE BSC32 /nologo\r\n"
		"# ADD BSC32 /nologo\r\n"
		"LINK32=link.exe\r\n"
		"# ADD BASE LINK32 user32.lib /nologo /subsystem:console /machine:I386\r\n"
		"# ADD LINK32 user32.lib /nologo /subsystem:console /machine:I386\r\n"
		"\r\n"
		"!ELSEIF  \"$(CFG)\" == \"%s - Win32 Debug\"\r\n"     //项目名
		"\r\n"
		"# PROP BASE Use_MFC 0\r\n"
		"# PROP BASE Use_Debug_Libraries 1\r\n"
		"# PROP BASE Output_Dir \"Debug\"\r\n"
		"# PROP BASE Intermediate_Dir \"Debug\"\r\n"
		"# PROP BASE Target_Dir \"\"\r\n"
		"# PROP Use_MFC 0\r\n"
		"# PROP Use_Debug_Libraries 1\r\n"
		"# PROP Output_Dir \"Debug\"\r\n"
		"# PROP Intermediate_Dir \"Debug\"\r\n"
		"# PROP Target_Dir \"\"\r\n"
		"# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od %s /YX /FD /GZ  /c\r\n" //宏定义
		"# ADD CPP /nologo /W3 /Gm /GX /ZI /Od %s %s /YX /FD /GZ  /c\r\n" //目录+宏定义
		"# ADD BASE RSC /l 0x804 /d \"_DEBUG\"\r\n"
		"# ADD RSC /l 0x804 /d \"_DEBUG\"\r\n"
		"BSC32=bscmake.exe\r\n"
		"# ADD BASE BSC32 /nologo\r\n"
		"# ADD BSC32 /nologo\r\n"
		"LINK32=link.exe\r\n"
		"# ADD BASE LINK32 user32.lib /nologo /subsystem:console /debug /machine:I386 /pdbtype:sept\r\n"
		"# ADD LINK32 user32.lib /nologo /subsystem:console /debug /machine:I386 /pdbtype:sept\r\n"
		"\r\n"
		"!ENDIF \r\n" /*前面只有2个项目名*/
		"\r\n"
		"# Begin Target\r\n"
		"\r\n"
		"# Name \"%s - Win32 Release\"\r\n"    //这里有两个
		"# Name \"%s - Win32 Debug\"\r\n"
		;

	char dsp[50*1024] = {0};
	string name(project_name);
	name += ".dsp";
	FILE* fp_dsp = fopen(name.c_str(),"wb");
	if(fp_dsp == NULL){
		cout<<"错误:无法打开 "<<name<<" 用于写!"<<endl;
		return false;
	}

	// 定义是逗号分隔的
	char* zDefine = new char[define.length()+2];
	memset(zDefine,0,define.length()+2);
	strcpy(zDefine,define.c_str());
	for(int iii=0; iii<define.length()+2; iii++){
		if(zDefine[iii]==','){
			zDefine[iii] = '\0';
		}
	}
	string strDefine("");
	for(const char* pp=zDefine;*pp;){
		strDefine += "/D \"";
		strDefine += pp;
		strDefine += "\" ";

		while(*pp) pp++;
		pp++;
	}

	// includepath是分号分隔的
	char* zInclude = new char[includepath.length()+2];
	memset(zInclude,0,includepath.length()+2);
	strcpy(zInclude,includepath.c_str());
	for(int jjj=0; jjj<includepath.length()+2; jjj++){
		if(zInclude[jjj]==';'){
			zInclude[jjj] = '\0';
		}
	}
	
	string strInclude("");
	for(const char* qq=zInclude;*qq;){
		strInclude += "/I \"";
		strInclude += qq;
		strInclude += "\" ";
		
		while(*qq) qq++;
		qq++;
	}

	int len=0;
	len += _snprintf(dsp,sizeof(dsp)-len,dsp_content,
		project_name,project_name,project_name,project_name,project_name,project_name,project_name,
		
		project_name,
		strDefine.c_str(),
		strInclude.c_str(),strDefine.c_str(),
		
		project_name,
		strDefine.c_str(),
		strInclude.c_str(),strDefine.c_str(),

		project_name,
		project_name
	);

	delete[] zDefine;
	delete[] zInclude;

	for(vector<string>::iterator it=groups.begin(); it!=groups.end(); it++){
		len += _snprintf(dsp+len,sizeof(dsp)-len,"%s",it->c_str());
	}

	len += _snprintf(dsp+len,sizeof(dsp)-len,"%s","# End Target\r\n# End Project\r\n");

	if(fwrite(dsp,1,len,fp_dsp) == len){
		cout<<"已创建 "<<name<<" 项目文件!"<<endl;
		fclose(fp_dsp);
	}else{
		cout<<"错误:文件写入错误!"<<endl;
		fclose(fp_dsp);
		remove(name.c_str());
		return false;
	}
	return true;
}

bool get_uv_info(const char* uvproj,vector<string>& groups,string& define,string& includepath)
{
	const char* ret_str=NULL;
	bool bXmlUtf8 = true;
	const char* src;
	TiXmlDocument doc(uvproj);
	if(!doc.LoadFile()){
		cout<<"错误:TiXmlDocument.LoadFile()"<<endl;
		return false;
	}
	//doc.Print();
	//TiXmlElement* rootElement = doc.RootElement();
	//TiXmlElement* classElement = rootElement->FirstChildElement();  // Class元素
	//TiXmlElement* studentElement = classElement->FirstChildElement();  //Students 
	bXmlUtf8 = stricmp(doc.FirstChild()->ToDeclaration()->Encoding(),"UTF-8")==0;
 

	TiXmlNode* nodeTarget = doc.FirstChild("Project")->FirstChild("Targets")->FirstChild("Target");
	TiXmlNode* nodeVariousControls = nodeTarget->FirstChild("TargetOption")->FirstChild("TargetArmAds")->FirstChild("Cads")->FirstChild("VariousControls");
	//取得宏定义 Define
	TiXmlElement* cDefine = nodeVariousControls->FirstChild("Define")->ToElement();
	ret_str = cDefine->GetText();
	if(ret_str){
		if(bXmlUtf8){
			define = AStr(ret_str,true).toAnsi();
		}else{
			define = ret_str;
		}
	}
	//取得包含路径 IncludePath
	TiXmlElement* cIncludePath = nodeVariousControls->FirstChild("IncludePath")->ToElement();
	ret_str = cIncludePath->GetText();
	if(ret_str){
		if(bXmlUtf8){
			includepath = AStr(ret_str,true).toAnsi();
		}else{
			includepath = ret_str;
		}
	}

	cout<<cDefine->GetText()<<endl;
	cout<<cIncludePath->GetText()<<endl;

	TiXmlNode* nodeGroups = nodeTarget->FirstChild("Groups");
	//Goups包含源代码分组信息,遍历
	for(TiXmlNode* group=nodeGroups->FirstChild(); group!=NULL; group=group->NextSibling()){
		stringstream strGroup("");
		//取得分组名称
		TiXmlElement* eleGroupName = group->FirstChild()->ToElement();
		
		strGroup<<"# Begin Group \""<<AStr(eleGroupName->GetText(),bXmlUtf8).toAnsi()<<"\"\r\n\r\n";
		strGroup<<"# PROP Default_Filter \"\"\r\n";
		//cout<<eleGroupName->GetText()<<endl;

		//Files包含所有的文件列表
		TiXmlNode* nodeFiles = eleGroupName->NextSibling();
		if(nodeFiles){
			for(TiXmlNode* file=nodeFiles->FirstChild(); file!=NULL; file=file->NextSibling()){
				//文件名
				TiXmlElement* eleFileName = file->FirstChild()->ToElement();
				//文件路径
				TiXmlElement* eleFilePath = file->FirstChild()->NextSibling()->NextSibling()->ToElement();
				cout<<eleFileName->GetText()<<","<<eleFilePath->GetText()<<endl;
				//system("xcopy eleFilePath->GetText()  1.txt /e /i");
				src = eleFilePath->GetText();
				filecpy(src, "1.txt");
				strGroup<<"# Begin Source File\r\n\r\nSOURCE=\""<<AStr(eleFilePath->GetText(),bXmlUtf8).toAnsi()<<"\"\r\n"<<"# End Source File\r\n";
			}
		}
		strGroup<<"# End Group\r\n";
		groups.push_back(string(strGroup.str()));
	}
	return true;
}
//DEQUOTE
//去除字符串中的配对引号，且该字符串是以该引号开始的，并且去掉与之配对的后引号之后的全部内容。
//啰嗦了，举个例子：y = dequote(x);
//
//"我是'我'的"我""    我是'我'的
//"你"是你的'你'      你
//他是他的"他"        他是他的"他"
//她们还是她们        她们还是她们
//'它是'它'的它'      它是
void dequote(char* str)
{
	char* di = str;
	char* si = str;
	if(*si!=' ' && *si!='\t' && *si!='\"') return;

	while(*si && *si!='\"'){
		si++;
	}

	if(!*si) return;
	else si++;

	while(*si && *si!='\"'){
		*di++ = *si++;
	}
	*di = '\0';
}

void dereturn(char* str)
{
	char* p = str+strlen(str)-1;
	if(*p=='\n') *p = '\0';
}


void setdir(char* location)
{
	char tmp;
	char* p = location+strlen(location)-1;
	while(*p!='\\' && p>=location) p--;
	if(p>=location){
		tmp = *p;
		*p = '\0';
		SetCurrentDirectory(location);
		*p = tmp;
	}
}

char *strstr1(const char*s1, const char*s2)
{
	int n;
	if (*s2)
	{
		while (*s1)
		{
			for (n = 0; *(s1 + n) == *(s2 + n); n++)
			{
				if (!*(s2 + n + 1))
					return(char*)s1;
			}
			s1++;
		}
		return NULL;
	}
	else
		return (char*)s1;
}
bool is_file_present(const char* fn)
{
	DWORD dwAttributes = GetFileAttributes(fn);
	return dwAttributes!=INVALID_FILE_ATTRIBUTES &&
		!(dwAttributes&FILE_ATTRIBUTE_DIRECTORY);
}

void about(void)
{
	const char* help = 
		"   欢迎使用 Keil uVision stm32 项目文件转 Visual Studio 项目小工具\n"
		"如果你厌倦了Keil那糟糕的代码编辑器, 试试回到VS的怀抱吧~ VC6也远比Keil好用哦~\n"
		"\n"
		"使用方法:\n"
		"  项目名称:你的项目的名称\n"
		"  项目路径:Keil uVision stm32 .uvproj 文件绝对路径\n"
		"  系统路径:Keil uVision MDK 提供的头文件所在的绝对路径(Keil/ARM/RV31)\n"
		"\n"
		"关于程序:\n"
		"  作者:女孩不哭 编写时间:2013-10-29 联系:anhbk@qq.com\n"
		"  源码下载:http://www.cnblogs.com/nbsofer/p/keil2vs.html\n\n"
	;
	printf(help);
}
//获取当前运行文件路径
string GetFilePath(char * filepath)
{
	//char szFilePath[MAX_PATH + 1] = { 0 };
	GetModuleFileNameA(NULL, filepath, MAX_PATH);
	(strrchr(filepath, '\\'))[0] = 0; // 删除文件名，只获得路径字串  
	string path = filepath;
	cout << path << endl;
	return path;

}
void make_full_path(char* s, int nLen, const char *file_name, const char*file_ext)
{
	char szPath[MAX_PATH] = { 0 };
	GetModuleFileNameA(NULL, szPath, MAX_PATH);
	char cDir[100] = "";
	char cDrive[10] = "";
	char cf[20] = "";
	char cExt[10] = "";
	_splitpath_s(szPath, cDrive, cDir, cf, cExt);
	_makepath_s(s, nLen, cDrive, cDir, file_name, file_ext);
}
int main(int argc,char** argv)
{
	FILE *in_file, *out_file;
	vector<string> groups;
	string define;
	string includepath;
	string runpath;
	WIN32_FIND_DATA FindFileData;
	char project[128]={0};
	char filename[MAX_PATH] = { 0 };
	char path[MAX_PATH] = { 0 };
	char location[260]={0};
	char compiler[260]={0};
	about();
	char tmp;
	runpath = GetFilePath(location);

	//strrchr(location, '.\\');
	for (;;){

		printf("输入keil工程文件名:");
		//获取工程文件 
		fgets(filename, sizeof(filename), stdin);
		if (*filename == '\n'){
			printf("错误:项目名称不合法!\n");
			continue;
		}
		else{			
			dequote(filename);
			dereturn(filename);
			printf("%s", filename);
			// 判断 相对路径 绝对路径
			if (NULL == strstr((const char *)filename, ":\\"))	{
				
				strcat(location, "\\");
				strcat(location, filename);
				strcpy(filename, location);
			}
			if (GetFileAttributes(filename) != -1)
			{
				printf("File exists ");
			
			break;
			}

		}
	}
	/*
	for(;;){
		printf("项目名称:");
		fgets(project,sizeof(project),stdin);
		if(*project=='\n'){
			printf("错误:项目名称不合法!\n");
			continue;
		}else{
			dequote(project);
			dereturn(project);
			break;
		}
	}

	for(;;){
		printf("项目路径:");
		fgets(location,sizeof(location),stdin);
		if(*location=='\n'){
			printf("错误:路径不合法!\n");
			continue;
		}else{
			dequote(location);
			dereturn(location);
			if(!is_file_present(location)){
				printf("错误:不存在的 .uvproj 文件\n");
				continue;
			}
			break;
		}
	}
	
	for(;;){
		printf("系统路径:");
		fgets(compiler,sizeof(compiler),stdin);
		if(*compiler=='\n'){
			*compiler = '\0';
		}
		dequote(compiler);
		dereturn(compiler);
		break;
	}
*/
	setdir(location);

	try{
		if (make_dsw_file(filename)){
			if(get_uv_info(location,groups,define,includepath)){
				includepath += ";";
				//includepath += compiler;
				if (make_dsp_file(filename, groups, define, includepath)){

				}
			}
		}
	}
	catch(...){
		
	}
	system("pause");

	return 0;
}
