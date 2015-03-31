/********************************************************************
	created:	2006/08/12
	filename: 	PeerList.cpp
	author:		¿Ó¥¥
                http://www.cppblog.com/converse/

	purpose:	
*********************************************************************/

#include "PeerList.h"

Peer_Info::Peer_Info()
	: dwActiveTime(0)
	, nAddrNum(0)
{
	P2PAddr.dwIP = 0;
	P2PAddr.usPort = 0;
	memset(IPAddr, 0, MAX_ADDNUM);
	memset(szUserName, 0, MAX_USERNAME);
}

Peer_Info Peer_Info::operator=(const Peer_Info& rPeerinfo)
{
	if (&rPeerinfo == this)
		return *this;

	P2PAddr = rPeerinfo.P2PAddr;
	dwActiveTime = rPeerinfo.dwActiveTime;
	nAddrNum = rPeerinfo.nAddrNum;
	strcpy(szUserName, rPeerinfo.szUserName);
	for (int i = 0; i < nAddrNum; ++i)
	{
		IPAddr[i] = rPeerinfo.IPAddr[i];
	}

	return *this;
}

PeerList::PeerList()
{

}

PeerList::~PeerList()
{
	DeleteAllPeer();
}

bool PeerList::AddPeer(const Peer_Info& rPeerInfo)
{
	m_PeerInfoList.push_back(rPeerInfo);

	return true;
}

bool PeerList::DeleteAllPeer()
{
	m_PeerInfoList.clear();

	return true;
}

bool PeerList::DeleteAPeer(const char* pszUserName)
{
	PeerInfoListIter Iter1, Iter2;

	for (Iter1 = m_PeerInfoList.begin(), Iter2 = m_PeerInfoList.end();
		 Iter1 != Iter2;
		 ++Iter1)
	{
		if (strcmp((*Iter1).szUserName, pszUserName) == 0)
		{
			m_PeerInfoList.erase(Iter1);
			return true;
		}
	}

	return false;
}

Peer_Info* PeerList::GetAPeer(const char* pszUserName)
{
	PeerInfoListIter Iter1, Iter2;

	for (Iter1 = m_PeerInfoList.begin(), Iter2 = m_PeerInfoList.end();
		Iter1 != Iter2;
		++Iter1)
	{
		if (strcmp((*Iter1).szUserName, pszUserName) == 0)
		{
			return &(*Iter1);
		}
	}

	return NULL;
}

int PeerList::GetCurrentSize()
{
	return (int)m_PeerInfoList.size();
}

Peer_Info* PeerList::operator[](int nIndex)
{
	if (nIndex < 0 || nIndex >= GetCurrentSize())
	{
		return NULL;
	}
	else
	{
		PeerInfoListIter Iter1;
		int i;
		for (i = 0, Iter1 = m_PeerInfoList.begin();
			i < nIndex;
			++Iter1, ++i)
		{
			;
		}
		return &(*Iter1);
	}
}
