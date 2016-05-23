#include"route.h"


Router::Router()
{
	pIpRouteTab = NULL;

	/*获取路由表*/
	MyGetIpForwardTable(false);

	/*获取接口信息*/
	GetInterfaceName();	
}

/* 获取路由表
 * bOrder 是否排序
 * return 成功true 失败false
 */
bool Router::MyGetIpForwardTable(bool bOrder)
{
	if(pIpRouteTab != NULL)
	{
		free(pIpRouteTab);
		pIpRouteTab = NULL;
	}
	DWORD dwActualSize = 0;

	// 查询所需缓冲区的大小
	if(::GetIpForwardTable(pIpRouteTab, &dwActualSize, bOrder) == ERROR_INSUFFICIENT_BUFFER)
	{
		// 为MIB_IPFORWARDTABLE结构申请内存
		pIpRouteTab = (PMIB_IPFORWARDTABLE)::GlobalAlloc(GPTR, dwActualSize);
		// 获取路由表
		if(::GetIpForwardTable(pIpRouteTab, &dwActualSize, bOrder) == NO_ERROR)
			return true;
		::GlobalFree(pIpRouteTab);
	}
	return false;
}


void Router::showRouteTab()
{
	if(pIpRouteTab == NULL)
		return;
	printf("DestIP\t\tMask\t\tNextHop\t\tforwardType\tInterFaceIndex Met\n");
	for(DWORD i = 0; i < pIpRouteTab->dwNumEntries; i++)
	{
		IN_ADDR inDest, inMask, inNextHop;
		wchar_t wtIfName[1024], wtMetric[1024];
		ULONG ulInterfaceIndex, ulMetric;

		inDest.S_un.S_addr = pIpRouteTab->table[i].dwForwardDest;
		inMask.S_un.S_addr = pIpRouteTab->table[i].dwForwardMask;
		inNextHop.S_un.S_addr = pIpRouteTab->table[i].dwForwardNextHop;
		_itow(pIpRouteTab->table[i].dwForwardType, wtIfName, 10);

		ulInterfaceIndex = pIpRouteTab->table[i].dwForwardIfIndex;
		ulMetric = pIpRouteTab->table[i].dwForwardMetric1;

		printf("%-18s %-18s %-18s %s %8ld %ld\n",inet_ntoa(inDest), inet_ntoa(inMask), inet_ntoa(inNextHop), wtIfName, ulInterfaceIndex, ulMetric);
	}

}

/*添加路由表项
 * dest: 目标IP
 * mask: 掩码
 * nextHop： 下一跳IP
 * metric:   xxxxx
 * InterfaceIndex： 接口索引号
 * return 成功0 失败不是0    ERROR_ACCESS_DENIED  ERROR_INVALID_PARAMETER
 */
DWORD Router::AddRoute(string dest, string mask, string nextHop, DWORD metric, DWORD InterfaceIndex)
{
	DWORD dwDestination, dwSubnetMask, dwNextHop;

	dwDestination = inet_addr(dest.c_str());
	dwSubnetMask = inet_addr(mask.c_str());
	dwNextHop = inet_addr(nextHop.c_str());

	MIB_IPFORWARDROW mibIpForwardRow = {0};
	DWORD e;
	mibIpForwardRow.dwForwardMetric1 = metric;//m_dwMaxInterfaceMetric;
	mibIpForwardRow.dwForwardDest = dwDestination;
	mibIpForwardRow.dwForwardMask = dwSubnetMask;
	mibIpForwardRow.dwForwardNextHop = dwNextHop;
	mibIpForwardRow.dwForwardIfIndex = InterfaceIndex;//m_dwDefaultInterface;
	//mibIpForwardRow.dwForwardType = 4;
	mibIpForwardRow.dwForwardProto = MIB_IPPROTO_NETMGMT;
	mibIpForwardRow.dwForwardPolicy = 0;
	//e = CreateIpForwardEntry(&rt.pIpRouteTab->table[0]);
	e = CreateIpForwardEntry(&mibIpForwardRow);

	//日志记录
	return e;
}
DWORD Router::DelRoute(string dest, string mask, string nextHop, DWORD metric, DWORD InterfaceIndex)
{
	DWORD dwDestination, dwSubnetMask, dwNextHop;

	dwDestination = inet_addr(dest.c_str());
	dwSubnetMask = inet_addr(mask.c_str());
	dwNextHop = inet_addr(nextHop.c_str());

	MIB_IPFORWARDROW mibIpForwardRow = {0};
	DWORD e;
	mibIpForwardRow.dwForwardMetric1 = metric;//m_dwMaxInterfaceMetric;
	mibIpForwardRow.dwForwardDest = dwDestination;
	mibIpForwardRow.dwForwardMask = dwSubnetMask;
	mibIpForwardRow.dwForwardNextHop = dwNextHop;
	mibIpForwardRow.dwForwardIfIndex = InterfaceIndex;//m_dwDefaultInterface;
	//mibIpForwardRow.dwForwardType = 4;
	mibIpForwardRow.dwForwardProto = MIB_IPPROTO_NETMGMT;
	mibIpForwardRow.dwForwardPolicy = 0;
	//e = CreateIpForwardEntry(&rt.pIpRouteTab->table[0]);
	e = DeleteIpForwardEntry(&mibIpForwardRow);
	return e;
}

/* 获取接口信息 接口IP----接口索引号
 */
void Router::GetInterfaceName()
{

	m_mpInterfaceName.clear();
	PIP_ADAPTER_INFO pIpAdapterInfo = NULL;
	ULONG ulBufLen = 0;

	GetAdaptersInfo(pIpAdapterInfo, &ulBufLen);
	pIpAdapterInfo = (PIP_ADAPTER_INFO)malloc(ulBufLen);
	if(GetAdaptersInfo(pIpAdapterInfo, &ulBufLen) != ERROR_NO_DATA) {
		PIP_ADAPTER_INFO pTmp = pIpAdapterInfo;
		while(pTmp != NULL) {
			m_mpInterfaceName[ string(pTmp->IpAddressList.IpAddress.String) ] = pTmp->Index;
			//printf("%s %s %d \n", pTmp->Description, pTmp->IpAddressList.IpAddress.String, m_mpInterfaceName[string(pTmp->IpAddressList.IpAddress.String)]);
			pTmp = pTmp->Next;
		}
	}
	free(pIpAdapterInfo);
}