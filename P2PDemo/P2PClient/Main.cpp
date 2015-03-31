/********************************************************************
	created:	2006/08/11
	filename: 	Main.cpp
	author:		李创
                http://www.cppblog.com/converse/

	purpose:	
*********************************************************************/

#include "P2PClient.h"
#include <assert.h>
#include <stdio.h>

#define MAX_INPUT_LEN			256

// 删除输入中多余的行
void static EraseNewLine(char szChar[]);
// 输入用户名和server IP地址
void static InputNameAndServerIP(char szUserName[], char szServerIP[], int nLen);
// 处理用户输入的命令
void static ProcCommand(P2PClient& rP2PClient);

int main()
{
	P2PClient p2pClient;

	char szUserName[MAX_INPUT_LEN], szServerIP[MAX_INPUT_LEN];

	InputNameAndServerIP(szUserName, szServerIP, MAX_INPUT_LEN);

	if (false == p2pClient.Login(szUserName, szServerIP))
	{
		exit(-1);
	}
	p2pClient.GetUserList();

	printf("%s Has Successfully Logined Server \n", szUserName);
	printf("\n Commands are: \"getu\", \"send\", \"exit\" \n");

	ProcCommand(p2pClient);

	return 0;
}

void InputNameAndServerIP(char szUserName[], char szServerIP[], int nLen)
{
	assert(NULL != szUserName);
	assert(NULL != szServerIP);

	printf("Input User Name: ");
	fgets(szUserName, nLen, stdin);
	EraseNewLine(szUserName);
	printf("Input Server IP: ");
	fgets(szServerIP, nLen, stdin);
	EraseNewLine(szServerIP);

	int nServerIP = (int)strlen(szUserName);
}

void ProcCommand(P2PClient& rP2PClient)
{
	// 解析输入的命令
	char szCommandLine[256];
	char szCommand[10];
	char szUserName[MAX_USERNAME];
	int i;
	while (true)
	{
		fgets(szCommandLine, 256, stdin);

		if (strlen(szCommandLine) < 4)
		{
			continue;
		}
		strncpy(szCommand, szCommandLine, 4);
		szCommand[4] = '\0';

		if (strcmp(szCommand, "getu") == 0)
		{
			// 
			if (true == rP2PClient.GetUserList())
			{
				rP2PClient.DisplayUserList();
			}
			else
			{
				printf(" Get User List Failure !\n");
			}
		}
		else if (strcmp(szCommand, "send") == 0)
		{
			char c;
			
			// 解析出用户名
			for (i = 5; ; ++i)
			{
				if (' ' != (c = szCommandLine[i]))
				{
					szUserName[i - 5] = c;
				}
				else
				{
					szUserName[i - 5] = '\0';
					break;
				}
			}

			char szMsg[256];
			strcpy(szMsg, &szCommandLine[i + 1]);

			if (rP2PClient.SendText(szUserName, szMsg, (int)strlen(szMsg)))
			{
				printf("Send Text To %s Success!\n", szUserName);
			}
			else
			{
				printf("Send Text To %s Failed!\n", szUserName);
			}
		}
		else if (strcmp(szCommand, "exit") == 0)
		{
			break;
		}
		else
		{
			printf("Error: Invalid Command!\n");
			printf("Commands are: \"getu\", \"send\", \"exit\" \n");
		}
	}
}

void static EraseNewLine(char szChar[])
{
	for (int i = 0; i < strlen(szChar); ++i)
	{
		if (szChar[i] == '\n')
		{
			szChar[i] = '\0';
			break;
		}
	}
}