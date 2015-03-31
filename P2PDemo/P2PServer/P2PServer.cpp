/********************************************************************
	created:	2006/08/11
	filename: 	P2PServer.cpp
	author:		李创
                http://www.cppblog.com/converse/

	purpose:	
*********************************************************************/

#include <Winsock2.h>
#include <assert.h>
#include <stdio.h>
#include "P2PServer.h"

P2PServer::P2PServer()
	: m_sSocket(INVALID_SOCKET)
	, m_hThread(NULL)
{
	Initialize();
}

P2PServer::~P2PServer()
{
	printf("P2P Server shutdown. \n");

	// 通知接收线程退出
	if(m_hThread != NULL)
	{
		::WaitForSingleObject(m_hThread, 300);
		::CloseHandle(m_hThread);
	}

	if(INVALID_SOCKET != m_sSocket)
	{
		::closesocket(m_sSocket);
	}

	::DeleteCriticalSection(&m_PeerListLock);
	::WSACleanup();
}

bool P2PServer::Initialize()
{
	if (INVALID_SOCKET != m_sSocket)
	{
		printf("Error: Socket Already Been Initialized!\n");
		return false;
	}

	// 初始化WS2_32.dll
	WSADATA wsaData;
	WORD sockVersion = MAKEWORD(2, 2);
	if(::WSAStartup(sockVersion, &wsaData) != 0)
	{
		printf("Error: Initialize WS2_32.dll Failed!\n");
		exit(-1);
	}

	// 创建Socket
	m_sSocket = ::WSASocket(AF_INET, SOCK_DGRAM , IPPROTO_UDP, NULL, 0, WSA_FLAG_OVERLAPPED);
	if (INVALID_SOCKET == m_sSocket)
	{
		printf("Error: Initialize Socket Failed!\n");
		return false;
	}

	//
	sockaddr_in sockAddr = {0};
	sockAddr.sin_family = AF_INET;
	sockAddr.sin_addr.S_un.S_addr = INADDR_ANY;
	sockAddr.sin_port = htons(SERVER_PORT);
	if (SOCKET_ERROR == (::bind(m_sSocket, (sockaddr*)(&sockAddr), sizeof(sockaddr_in))))
	{
		printf("Error: Bind Socket Failed!\n");
		::closesocket(m_sSocket);
		return false;
	}

	// 
	char szHostName[256];
	::gethostname(szHostName, 256);
	hostent* pHost = ::gethostbyname(szHostName);
	in_addr addr;
	char *p;
	for (int i = 0; ; ++i)
	{
		p = pHost->h_addr_list[i];
		if (NULL == p)
		{
			break;
		}

		memcpy(&addr.S_un.S_addr, p, pHost->h_length);
		printf("Bind To Local Address -> %s:%ld \n", ::inet_ntoa(addr), SERVER_PORT);
	}

	// 创建接受线程
	if (NULL == (m_hThread = ::CreateThread(NULL, 0, RecvThreadProc, this, 0, NULL)))
	{
		printf("Error: Create Thread Failed!\n");
		return false;
	}

	// 创建临界区对象
	::InitializeCriticalSection(&m_PeerListLock);

	printf("P2P Server Running...\n");
	return true;
}

bool P2PServer::ProcMsg()
{
	int nRet;
	while (true)
	{
		nRet = ::WaitForSingleObject(m_hThread, 1000 * 15);

		if (WAIT_TIMEOUT == nRet)		// 等待超时
		{
			DWORD dwTick = ::GetTickCount();
			Peer_Info* pPeerInfo;
			int nCurrentsize = m_PeerList.GetCurrentSize();
			for(int i = 0; i < nCurrentsize; ++i)
			{				
				if (NULL != (pPeerInfo = m_PeerList[i]))
				{
					if (dwTick - pPeerInfo->dwActiveTime >= 2*15*1000 + 600)
					{
						printf("Delete A Non-Active User: %s \n", pPeerInfo->szUserName);
						::EnterCriticalSection(&m_PeerListLock);
						m_PeerList.DeleteAPeer(pPeerInfo->szUserName);
						::LeaveCriticalSection(&m_PeerListLock);
						--i;
					}
					else
					{
						MSGDef::TMSG_USERACTIVEQUERY tUserActiveQuery;
						int nAddrNum = pPeerInfo->nAddrNum - 1;
						sockaddr_in peerAddr = {0};
						peerAddr.sin_family = AF_INET;
						peerAddr.sin_addr.S_un.S_addr = pPeerInfo->IPAddr[nAddrNum].dwIP;
						peerAddr.sin_port = htons(pPeerInfo->IPAddr[nAddrNum].usPort);
						::sendto(m_sSocket, (char*)(&tUserActiveQuery), sizeof(tUserActiveQuery), 0, (sockaddr*)&peerAddr, sizeof(peerAddr));
						printf("Sending Active Ack Message To %s (%s:%ld) \n",
								tUserActiveQuery.PeerInfo.szUserName, ::inet_ntoa(peerAddr.sin_addr), ntohs(peerAddr.sin_port));
					}
				}
			}
		}
		else
		{
			break;
		}
	}

	return true;
}

DWORD WINAPI P2PServer::RecvThreadProc(LPVOID lpParam)
{
	P2PServer* pThisP2PServer = (P2PServer*)lpParam;

	char szBuff[MAX_PACKET_SIZE];
	MSGDef::TMSG_HEADER *pMsgHeader = (MSGDef::TMSG_HEADER *)szBuff;
	sockaddr_in remoteAddr;
	int nRecv, nAddrLen = sizeof(sockaddr_in);

	while (true)
	{
		nRecv = ::recvfrom(pThisP2PServer->m_sSocket, szBuff, MAX_PACKET_SIZE, 0, (sockaddr*)(&remoteAddr), &nAddrLen);
		if (SOCKET_ERROR == nRecv)
		{
			printf("Error: Receive Message From Client Failed!\n");
			continue;
		}

		switch (pMsgHeader->cMsgID)
		{
		case MSG_USERLOGIN:					// 用户登陆
			{
				pThisP2PServer->ProcUserLoginMsg(pMsgHeader, remoteAddr);
			}
			break;
		case MSG_GETUSERLIST:				// 发出当前的用户列表
			{
				pThisP2PServer->ProcGetUserListMsg(pMsgHeader, remoteAddr);
			}
			break;
		case MSG_P2PCONNECT:				// 有用户请求让另一个用户向它发送打洞消息
			{
				pThisP2PServer->ProcP2PConnectMsg(pMsgHeader, remoteAddr);
			}
			break;
		case MSG_USERLOGOUT:				// 用户退出
			{
				pThisP2PServer->ProcUserLogoutMsg(pMsgHeader, remoteAddr);
			}
			break;
		case MSG_USERACTIVEQUERY:			// 查询用户是否存在的答复
			{
				pThisP2PServer->ProcUserActiveQueryMsg(pMsgHeader, remoteAddr);
			}
			break;
		}
	}

	return 0;
}

// 用户登陆
bool P2PServer::ProcUserLoginMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& remoteAddr)
{
	MSGDef::TMSG_USERLOGIN *pUserLogin = (MSGDef::TMSG_USERLOGIN*)pMsgHeader;
	
	int nAddrNum = pUserLogin->PeerInfo.nAddrNum;
	// IPAddr数组的最后一个元素保存的是公有的端口和IP地址
	pUserLogin->PeerInfo.IPAddr[nAddrNum].dwIP = remoteAddr.sin_addr.S_un.S_addr;
	pUserLogin->PeerInfo.IPAddr[nAddrNum].usPort = ntohs(remoteAddr.sin_port);
	++pUserLogin->PeerInfo.nAddrNum;
	pUserLogin->PeerInfo.dwActiveTime = GetTickCount();	// 登陆的时间为活跃时间

	::EnterCriticalSection(&m_PeerListLock);
	bool bRet = m_PeerList.AddPeer(pUserLogin->PeerInfo);
	::LeaveCriticalSection(&m_PeerListLock);

	if (true == bRet)
	{
		MSGDef::TMSG_USERLOGACK tMsgUserLogAck(pUserLogin->PeerInfo);
		::sendto(m_sSocket, (char*)(&tMsgUserLogAck), sizeof(MSGDef::TMSG_USERLOGACK), 0, (sockaddr*)(&remoteAddr), sizeof(sockaddr_in));
		printf("%s Login: (%s:%ld) \n", pUserLogin->PeerInfo.szUserName, ::inet_ntoa(remoteAddr.sin_addr), ntohs(remoteAddr.sin_port));
		return true;
	}
	else
	{
		return false;
	}
}

// 发出当前的用户列表
bool P2PServer::ProcGetUserListMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& remoteAddr)
{
	MSGDef::TMSG_GETUSERLIST *pMsgGetUserList = (MSGDef::TMSG_GETUSERLIST *)pMsgHeader;
	printf("Sending User List Information To %s (%s: %ld)...\n",
		pMsgGetUserList->PeerInfo.szUserName, ::inet_ntoa(remoteAddr.sin_addr), ::ntohs(remoteAddr.sin_port));

	// 把当前用户链表中的用户信息发送出去
	for (int i = 0, nPeerListSize = m_PeerList.GetCurrentSize(); i < nPeerListSize; ++i)
	{
		pMsgGetUserList->PeerInfo = *(m_PeerList[i]);
		::sendto(m_sSocket, (char*)(pMsgGetUserList), sizeof(MSGDef::TMSG_GETUSERLIST), 0, (sockaddr*)(&remoteAddr), sizeof(remoteAddr));
	}

	// 发送结束封包
	MSGDef::TMSG_USERLISTCMP tMsgUserListCmp;
	::sendto(m_sSocket, (char*)(&tMsgUserListCmp), sizeof(tMsgUserListCmp), 0, (sockaddr*)(&remoteAddr), sizeof(remoteAddr));
	return true;
}

// 有用户请求让另一个用户向它发送打洞消息
bool P2PServer::ProcP2PConnectMsg(MSGDef::TMSG_HEADER *pMsgHeader, sockaddr_in& remoteAddr)
{
	MSGDef::TMSG_P2PCONNECT* pP2PConnect = (MSGDef::TMSG_P2PCONNECT*)pMsgHeader;
	printf("%s Wants To Connect To %s \n", pP2PConnect->PeerInfo.szUserName, pP2PConnect->szUserName);	

	::EnterCriticalSection(&m_PeerListLock);
	Peer_Info* pPeerInfo = m_PeerList.GetAPeer(pP2PConnect->szUserName);
	::LeaveCriticalSection(&m_PeerListLock);
	
	if (NULL != pPeerInfo)
	{
		int nAddrNum = pPeerInfo->nAddrNum;
		remoteAddr.sin_addr.S_un.S_addr = pPeerInfo->IPAddr[nAddrNum - 1].dwIP;
		remoteAddr.sin_port = htons(pPeerInfo->IPAddr[nAddrNum - 1].usPort);
		::sendto(m_sSocket, (char*)(pP2PConnect), sizeof(MSGDef::TMSG_P2PCONNECT), 0, (sockaddr*)(&remoteAddr), sizeof(remoteAddr));
	}

	return true;
}

// 有用户退出
bool P2PServer::ProcUserLogoutMsg(MSGDef::TMSG_HEADER *pMsgHeader, sockaddr_in& remoteAddr)
{
	MSGDef::TMSG_USERLOGOUT *pUserLogout = (MSGDef::TMSG_USERLOGOUT *)pMsgHeader;

	::EnterCriticalSection(&m_PeerListLock);
	m_PeerList.DeleteAPeer(pUserLogout->PeerInfo.szUserName);
	::LeaveCriticalSection(&m_PeerListLock);

	printf("%s Logout : (%s:%ld) \n", pUserLogout->PeerInfo.szUserName,
		::inet_ntoa(remoteAddr.sin_addr), ntohs(remoteAddr.sin_port));

	return true;
}

// 用户对服务器轮询的应答
bool P2PServer::ProcUserActiveQueryMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& remoteAddr)
{
	MSGDef::TMSG_USERACTIVEQUERY *pUserActiveQuery = (MSGDef::TMSG_USERACTIVEQUERY *)pMsgHeader;

	printf("Receive Active Ack Message from %s (%s:%ld) \n",
				pUserActiveQuery->PeerInfo.szUserName, ::inet_ntoa(remoteAddr.sin_addr), ntohs(remoteAddr.sin_port));

	::EnterCriticalSection(&m_PeerListLock);
	Peer_Info* pPeerInfo = m_PeerList.GetAPeer(pUserActiveQuery->PeerInfo.szUserName);
	// 更新用户的激活时间
	if (NULL != pPeerInfo)
	{
		pPeerInfo->dwActiveTime = ::GetTickCount();
	}
	::LeaveCriticalSection(&m_PeerListLock);

	return true;
}
