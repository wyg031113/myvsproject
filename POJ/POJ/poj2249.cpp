#include <iostream>
#include <cstdio>
using namespace std;
int main()
{
	long long  m, n;
	long long ans, prev;
	while (scanf("%lld%lld", &m, &n), (n || m))
	{
		if(n > m/2)
			n = m -n;
		prev = 1;
		ans = 1;
		for (int i = 1; i <= n; i++)
		{
			ans = prev * (m - i + 1)/i;
			prev = ans;
		}

		printf("%lld\n", ans);
	}
	return 0;
}
