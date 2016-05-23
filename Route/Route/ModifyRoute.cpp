// ModifyRoute.cpp : 实现文件
//
#include<stdafx.h>
//#include "WinRouter.h"
#include "ModifyRoute.h"

#include<Windows.h>
// ModifyRoute 对话框
#include<ipmib.h>
#include<cstring>

ModifyRoute::ModifyRoute(): m_subnetMask(_T("")), m_nextHop(_T(""))
{

	m_destination = _T("");
}

ModifyRoute::~ModifyRoute()
{
}



// ModifyRoute 消息处理程序

void ModifyRoute::SetInit(bool blIsAdd, int iRouteIndex, 
	PMIB_IPFORWARDTABLE pIpForwardTable, DWORD dwMaxInterfaceMetric, 
	DWORD dwDefaultInterface)
{
	m_blIsAdd = blIsAdd;
	m_iRouteIndex = iRouteIndex;
	m_pIpForwardTable = pIpForwardTable;
	m_dwMaxInterfaceMetric = dwMaxInterfaceMetric;
	m_dwDefaultInterface = dwDefaultInterface;
}

void ModifyRoute::LoadData()
{
	IN_ADDR inDestination, inSubnetMask, inNextHop;
	inDestination.S_un.S_addr = m_pIpForwardTable->table[m_iRouteIndex].dwForwardDest;
	inSubnetMask.S_un.S_addr = m_pIpForwardTable->table[m_iRouteIndex].dwForwardMask;
	inNextHop.S_un.S_addr = m_pIpForwardTable->table[m_iRouteIndex].dwForwardNextHop;

	wstring wstrTmp;
	Ascii2WideString(inet_ntoa(inDestination), wstrTmp);
	m_destination = wstrTmp.c_str();
	Ascii2WideString(inet_ntoa(inSubnetMask), wstrTmp);
	m_subnetMask = wstrTmp.c_str();
	Ascii2WideString(inet_ntoa(inNextHop), wstrTmp);
	m_nextHop = wstrTmp.c_str();
}

BOOL ModifyRoute::OnInitDialog()
{

	// TODO:  在此添加额外的初始化
	if(m_blIsAdd) {
		//LoadData();
		//SetWindowText(_T("添加路由"));
	} else {
		LoadData();
		//SetWindowText(_T("编辑路由"));
	}
	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常: OCX 属性页应返回 FALSE
}


void ModifyRoute::OnBnClickedOk()
{


	DWORD dwDestination, dwSubnetMask, dwNextHop;
	CStringA csaDestination = CStringA(m_destination);
	CStringA csaSubnetMask = CStringA(m_subnetMask);
	CStringA csaNextHop = CStringA(m_nextHop);
	dwDestination = inet_addr(csaDestination.GetBuffer());
	dwSubnetMask = inet_addr(csaSubnetMask.GetBuffer());
	dwNextHop = inet_addr(csaNextHop.GetBuffer());

	MIB_IPFORWARDROW mibIpForwardRow = {0};
	DWORD e;
	if(m_blIsAdd) {
		mibIpForwardRow.dwForwardMetric1 = m_dwMaxInterfaceMetric;
		mibIpForwardRow.dwForwardDest = dwDestination;
		mibIpForwardRow.dwForwardMask = dwSubnetMask;
		mibIpForwardRow.dwForwardNextHop = dwNextHop;
		mibIpForwardRow.dwForwardIfIndex = m_dwDefaultInterface;
		//mibIpForwardRow.dwForwardType = 4;
		mibIpForwardRow.dwForwardProto = MIB_IPPROTO_NETMGMT;
		e = CreateIpForwardEntry(&mibIpForwardRow);
		/*
		mibIpForwardRow.dwForwardMetric2 = -1;
		mibIpForwardRow.dwForwardMetric3 = -1;
		mibIpForwardRow.dwForwardMetric4 = -1;
		mibIpForwardRow.dwForwardMetric5 = -1;
		*/

	} else {
		
		memcpy(&mibIpForwardRow, &m_pIpForwardTable->table[m_iRouteIndex], 
			sizeof(mibIpForwardRow));
		
		mibIpForwardRow.dwForwardDest = dwDestination;
		mibIpForwardRow.dwForwardMask = dwSubnetMask;
		mibIpForwardRow.dwForwardNextHop = dwNextHop;

		mibIpForwardRow.dwForwardProto = MIB_IPPROTO_NETMGMT;

		e = CreateIpForwardEntry(&mibIpForwardRow);
		if(e == ERROR_ACCESS_DENIED) {
			//MessageBox(_T("Access is denied"), _T("Error"), MB_OK);
			return;
		} else if(e == ERROR_INVALID_PARAMETER) {
			//MessageBox(_T("Parameter is invalid"), _T("Error"), MB_OK);
			return;
		} else if(e) {
			//MessageBox(_T("Unknow Error"), _T("Error"), MB_OK);
			return;
		}

		e = DeleteIpForwardEntry(&m_pIpForwardTable->table[m_iRouteIndex]);
		if(e) {
			//MessageBox(_T("Delete old route fail"), _T("Error"), MB_OK);
			return;
		}
	}
}
