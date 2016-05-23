#include <iostream>
#include <cstdio>
using namespace std;
int main()
{
	long long  m, n;
	double ans = 0;
	while (scanf("%lld%lld", &m, &n), (n || m))
	{
		if(n > m/2)
			n = m -n;
		ans = 1;
		for (int i = 1; i <= n; i++)
		{
			ans = ans *(m - i + 1) / i;
		}

		printf("%.0f\n", ans);
	}
	return 0;
}
