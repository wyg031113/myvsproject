

#include<Windows.h>
// ModifyRoute �Ի���
#include<ipmib.h>
#include<atlstr.h>
#include<string>
using namespace std;
class ModifyRoute
{
	

public:
	ModifyRoute();   // ��׼���캯��
	void SetInit(bool blIsAdd, int iRouteIndex, PMIB_IPFORWARDTABLE pIpForwardTable,
		DWORD dwMaxInterfaceMetric, DWORD dwDefaultInterface);
	virtual ~ModifyRoute();

	void OnBnClickedOk();
private:
	bool m_blIsAdd;
	int m_iRouteIndex;
	PMIB_IPFORWARDTABLE m_pIpForwardTable;
	DWORD m_dwMaxInterfaceMetric;
	DWORD m_dwDefaultInterface;

	void LoadData();

public:
//	CString m_distination;
	CString m_subnetMask;
	CString m_nextHop;
	virtual BOOL OnInitDialog();
	CString m_destination;
};
