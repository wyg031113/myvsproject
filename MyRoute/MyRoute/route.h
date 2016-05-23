#ifndef __ROUTE_H__
#define __ROUTE_H__

#include<iostream>
#include <WinSock2.h>
#include <IPHlpApi.h>
#include <Windows.h>
#include <string>
#pragma comment(lib, "Iphlpapi.lib")
#pragma comment(lib, "Ws2_32.lib")
#include <map>
using namespace std;

class Router
{
public:
	
	Router();
	
	bool MyGetIpForwardTable(bool bOrder);
	
	void showRouteTab();
	
	DWORD AddRoute(string dest, string mask, string nextHop, DWORD metric, DWORD InterfaceIndex);
	
	DWORD DelRoute(string dest, string mask, string nextHop, DWORD metric, DWORD InterfaceIndex);

	void GetInterfaceName();

	PMIB_IPFORWARDTABLE pIpRouteTab;
	map <string, ULONG> m_mpInterfaceName;  //map[IP]==Index
};

#endif