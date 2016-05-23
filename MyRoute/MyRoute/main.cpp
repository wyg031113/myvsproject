#include"route.h"
int main()
{
	
	Router rt;
	
	rt.showRouteTab();
	DWORD e;
	e = rt.DelRoute("192.168.1.0", "255.255.255.0", "192.168.0.1", 20, 11);
	 e = rt.AddRoute("192.168.1.0", "255.255.255.0", "192.168.0.1", 20, 11);
	//e = DeleteIpForwardEntry(&mibIpForwardRow);
	printf("e = %d\n", e);
	switch(e)
	{
	case ERROR_ACCESS_DENIED:
		printf("Error_ACCESS_DENIED\n");
		break;
	case ERROR_INVALID_PARAMETER:
		printf("ERROR_INVALID_PARAMETER\n");
		break;
	}

	map<string, DWORD>::iterator ite = rt.m_mpInterfaceName.begin();
	for(; ite!=rt.m_mpInterfaceName.end(); ite++)
		cout<<ite->first<<"  "<<ite->second<<endl;

	cout<<rt.m_mpInterfaceName["192.168.0.15"]<<endl;
	return 0;
}
