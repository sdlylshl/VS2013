/********************************************************************
	created:	2006/08/07
	filename: 	P2PClient.h
	author:		李创
                http://www.cppblog.com/converse/

	purpose:	
*********************************************************************/

#ifndef __P2P_CLIENT_H__
#define __P2P_CLIENT_H__

#include <WinSock2.h>
#include "../CommonDefine.h"

class P2PClient
{
public:
	P2PClient();
	~P2PClient();

	bool Login(char *pszUserName, char *pszServerIP);
	bool Logout();
	bool GetUserList();
	bool SendText(char *pszUserName, char* pszText, int nTextLen);
	void DisplayUserList();

private:
	bool Initialize();
	static DWORD WINAPI RecvThreadProc(LPVOID lpParam);

	// 处理各个消息的函数
	bool ProcUserLogAckMsg(MSGDef::TMSG_HEADER *pMsgHeader);
	bool ProcGetUserList(MSGDef::TMSG_HEADER *pMsgHeader);
	bool ProcUserListCmpMsg();
	bool ProcP2PMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& sockAddr);
	bool ProcP2PConnectMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& sockAddr);
	bool ProcP2PConnectAckMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& sockAddr);
	bool ProcUserActiveQueryMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& sockAddr);

private:
	SOCKET				m_sSocket;				// 套接字
	Peer_Info			m_PeerInfo;				// 本机信息
	PeerList			m_PeerList;				// 与本机相连的节点链表
	HANDLE				m_hThread;				// 线程句柄
	DWORD				m_dwServerIP;			// server IP地址
	WSAOVERLAPPED		m_ol;					// 用于等待网络事件的重叠结构
	CRITICAL_SECTION	m_PeerListLock;			// 用于读取m_PeerList的临界区对象
	bool				m_bExitThread;			// 是否退出线程
	bool				m_bLogin;				// 是否已经登陆服务器了
	bool				m_bUserListCmp;			// 是否得到了用户列表
	bool				m_bMessageACK;			// 是否接收到消息应答
};

#endif // __P2P_CLIENT_H__
