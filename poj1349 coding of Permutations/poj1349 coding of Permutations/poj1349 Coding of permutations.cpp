#include <cstdio>
#include <iostream>
#include <vector>



using namespace std;
extern vector<vector<double>> vd;
vector< vector<double> > vd(10, vector<double>(20));
vector<int> vi(10, 10);
int main()
{
	int n;
	scanf("%d", &n);
	cout << "hello world!" << endl;
	cout << vd.size() << " " << vd[0].size() << endl;
	for (int i = 0; i < vi.size(); i++)
		cout << vi[i] << endl;
}