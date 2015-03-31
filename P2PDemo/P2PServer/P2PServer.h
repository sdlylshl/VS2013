/********************************************************************
	created:	2006/08/11
	filename: 	P2PServer.h
	author:		李创
                http://www.cppblog.com/converse/

	purpose:	
*********************************************************************/

#ifndef __P2P_SERVER_H__
#define __P2P_SERVER_H__

#include "../CommonDefine.h"

class P2PServer
{
public:
	P2PServer();
	~P2PServer();

	bool ProcMsg();

private:
	bool Initialize();
	static DWORD WINAPI RecvThreadProc(LPVOID lpParam);

	// 处理各个消息的函数
	bool ProcUserLoginMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& remoteAddr);
	bool ProcGetUserListMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& remoteAddr);
	bool ProcP2PConnectMsg(MSGDef::TMSG_HEADER *pMsgHeader, sockaddr_in& remoteAddr);
	bool ProcUserLogoutMsg(MSGDef::TMSG_HEADER *pMsgHeader, sockaddr_in& remoteAddr);
	bool ProcUserActiveQueryMsg(MSGDef::TMSG_HEADER *pMsgHeader, const sockaddr_in& remoteAddr);

private:
	SOCKET				m_sSocket;			// 套接字
	HANDLE				m_hThread;			// 接收消息的线程
	CRITICAL_SECTION	m_PeerListLock;		// 读写m_PeerList的临界区对象
	PeerList			m_PeerList;			// 所有与server相连的节点组成的链表
};

#endif	// __P2P_SERVER_H__
