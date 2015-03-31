/********************************************************************
	created:	2006/08/10
	filename: 	CommonDefine.h
	author:		李创
                http://www.cppblog.com/converse/

	purpose:	定义客户端和服务器端都需要知道的一些宏定义
*********************************************************************/

#ifndef __COMMON_DEFINE_H__
#define __COMMON_DEFINE_H__

#include <Windows.h>
#include "PeerList.h"

#pragma comment(lib, "WS2_32")	// 链接到WS2_32.lib

#define MAX_TRY_NUMBER		10

#define SERVER_PORT			4096
#define MAX_PACKET_SIZE		1024

// 各种消息标识宏
#define INVALID_MSG			-1
#define MSG_USERLOGIN		1						// 用户登陆
#define MSG_USERLOGACK		2						// 发送确认用户登陆的信息
#define MSG_GETUSERLIST		3						// 获取用户列表
#define MSG_USERLISTCMP		4						// 用户列表传送完毕
#define MSG_P2PMSG			5						// 发送P2P信息
#define MSG_P2PCONNECT		6						// 有用户请求让另一个用户向它发送打洞消息
#define MSG_P2PMSGACK		7
#define MSG_P2PCONNECTACK	8
#define MSG_USERLOGOUT		9						// 通知server用户退出
#define MSG_USERACTIVEQUERY	10						// 查询用户是否还存在

class MSGDef										// 定义消息的结构体
{
public:
#pragma pack(1)										// 使结构体的数据按照1字节来对齐,省空间

	// 消息头
	struct TMSG_HEADER
	{
		char    cMsgID;								// 消息标识

		TMSG_HEADER(char MsgID = INVALID_MSG)
			: cMsgID(MsgID)
		{
		}
	};

	// 用户登陆
	struct TMSG_USERLOGIN
		: TMSG_HEADER
	{
		Peer_Info PeerInfo;

		TMSG_USERLOGIN(const Peer_Info &rPeerinfo)
			: TMSG_HEADER(MSG_USERLOGIN)
		{
			PeerInfo = rPeerinfo;
		}		
	};

	// 发送确认用户登陆的信息
	struct TMSG_USERLOGACK
		: TMSG_HEADER
	{
		Peer_Info PeerInfo;

		TMSG_USERLOGACK(const Peer_Info& rPeerInfo)
			: TMSG_HEADER(MSG_USERLOGACK)
		{
			PeerInfo = rPeerInfo;
		}
	};

	// 获取用户列表
	struct TMSG_GETUSERLIST
		: TMSG_HEADER
	{
		Peer_Info PeerInfo;

		TMSG_GETUSERLIST(const Peer_Info& rPeerInfo)
			: TMSG_HEADER(MSG_GETUSERLIST)
		{
			PeerInfo = rPeerInfo;
		}
	};

	// 更新用户列表完毕
	struct TMSG_USERLISTCMP
		: TMSG_HEADER
	{
		TMSG_USERLISTCMP()
			: TMSG_HEADER(MSG_USERLISTCMP)
		{

		}
	};

	// 一个client向另一个client发送消息
	struct TMSG_P2PMSG
		: TMSG_HEADER
	{
		Peer_Info	PeerInfo;
		char		szMsg[MAX_PACKET_SIZE - sizeof(TMSG_HEADER) - sizeof(Peer_Info)];

		TMSG_P2PMSG(const Peer_Info& rPeerInfo)
			: TMSG_HEADER(MSG_P2PMSG)
		{
			PeerInfo = rPeerInfo;
			memset(szMsg, 0, MAX_PACKET_SIZE - sizeof(TMSG_HEADER) - sizeof(PeerInfo));
		}
	};

	// 有用户请求让另一个用户向它发送打洞消息
	struct TMSG_P2PCONNECT
		: TMSG_HEADER
	{
		Peer_Info	PeerInfo;
		char		szUserName[MAX_USERNAME];

		TMSG_P2PCONNECT(const Peer_Info& rPeerInfo)
			: TMSG_HEADER(MSG_P2PCONNECT)
		{
			PeerInfo = rPeerInfo;
		}
	};

	// client收到另一个client发送的消息之后的确认
	struct TMSG_P2PMSGACK
		: TMSG_HEADER
	{
		Peer_Info PeerInfo;

		TMSG_P2PMSGACK(const Peer_Info& rPeerInfo)
			: TMSG_HEADER(MSG_P2PMSGACK)
		{
			PeerInfo = rPeerInfo;
		}
	};

	// 接收到节点的打洞消息，在这里设置它的P2P通信地址
	struct TMSG_P2PCONNECTACK
		: TMSG_HEADER
	{
		Peer_Info PeerInfo;

		TMSG_P2PCONNECTACK(const Peer_Info& rPeerInfo)
			: TMSG_HEADER(MSG_P2PCONNECTACK)
		{
			PeerInfo = rPeerInfo;
		}
	};

	// 通知server用户退出
	struct TMSG_USERLOGOUT
		: TMSG_HEADER
	{
		Peer_Info PeerInfo;
		TMSG_USERLOGOUT(const Peer_Info& rPeerInfo)
			: TMSG_HEADER(MSG_USERLOGOUT)
		{
			PeerInfo = rPeerInfo;
		}
	};

	// 查询用户是否还存在
	struct TMSG_USERACTIVEQUERY
		: TMSG_HEADER
	{
		Peer_Info PeerInfo;

		TMSG_USERACTIVEQUERY(const Peer_Info& rPeerInfo = Peer_Info())
			: TMSG_HEADER(MSG_USERACTIVEQUERY)
		{
			PeerInfo = rPeerInfo;
		}
	};

#pragma pack()
};

#endif // __COMMON_DEFINE_H__