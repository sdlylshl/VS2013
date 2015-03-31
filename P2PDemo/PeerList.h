/********************************************************************
	created:	2006/08/12
	filename: 	PeerList.h
	author:		李创
                http://www.cppblog.com/converse/

	purpose:	
*********************************************************************/

#ifndef __PEER_LIST_H__
#define __PEER_LIST_H__

#include <map>
#include <string>
#include <list>
#include <Windows.h>

#define	MAX_USERNAME		15
#define MAX_ADDNUM			5

struct Addr_Info
{
	unsigned short	usPort;							// 端口号
	DWORD			dwIP;							// IP地址
	
	Addr_Info operator = (const Addr_Info& rAddrInfo)
	{
		usPort = rAddrInfo.usPort;
		dwIP   = rAddrInfo.dwIP;

		return *this;
	}
};

struct Peer_Info
{
	Addr_Info		IPAddr[MAX_ADDNUM];				// 本机所有适配器的IP地址和端口号,
													// 数组的第nAddrNum + 1个元素是本次通讯server端分配的IP地址和端口号
	char			szUserName[MAX_USERNAME];		// 用户名
	DWORD			dwActiveTime;					// 活跃时间
	int				nAddrNum;						// 适配器数目
	Addr_Info		P2PAddr;						// 
	Peer_Info();

	Peer_Info operator=(const Peer_Info& rPeerinfo);
};

class PeerList
{
public:
	PeerList();
	~PeerList();

	bool		AddPeer(const Peer_Info& rPeerInfo);
	bool		DeleteAPeer(const char* pszUserName);
	bool		DeleteAllPeer();
	Peer_Info*	GetAPeer(const char* pszUserName);
	int			GetCurrentSize();
	Peer_Info*	operator[](int nIndex);

private:
	typedef std::list<Peer_Info> PeerInfoList;
	typedef PeerInfoList::iterator PeerInfoListIter;
	PeerInfoList	m_PeerInfoList;
};

#endif // __PEER_LIST_H__
