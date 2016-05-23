#include <cstdio>
#include <iostream>
#include <cstdlib>
#include <queue>
using namespace std;
#define N 31
int L, R, C;
char maze[N][N][N];
int  dis[N][N][N];
class Point
{
public:
	Point(int a=0, int b=0, int c=0):x(a), y(b), z(c){}
	Point operator+(const Point p)
	{
		return Point(x + p.x, y + p.y, z + p.z);
	}
	bool operator==(const Point p)
	{
		return (x == p.x && y == p.y && z == p.z);
	}
	int x;
	int y;
	int z;
};
Point start;
Point endpt;
Point dir[6] = {Point(0,1,0), Point(0,-1, 0), Point(0,0,1), Point(0,0,-1), Point(1,0,0), Point(-1,0,0)};

bool check(const Point &p)
{
	if (p.x < 0 || p.y < 0 || p.z < 0 ||
		p.x >= L || p.y >= R || p.z >= C)
		return false;
	return true;
}
void bfs()
{
	memset(dis, 0x7f, sizeof(dis));
	dis[start.x][start.y][start.z] = 0;
	queue<Point> Q;
	Q.push(start);
	Point t;
	Point p;
	int i;
	while (!Q.empty())
	{
		t = Q.front();
		Q.pop();
		if (t == endpt)
			break;
		for (i = 0; i < 6; i++)
		{
			p = t + dir[i];
			if (!check(p) || (maze[p.x][p.y][p.z] != '.' && maze[p.x][p.y][p.z] != 'E'))
				continue;
			if (dis[p.x][p.y][p.z] > dis[t.x][t.y][t.z] + 1)
			{
				dis[p.x][p.y][p.z] = dis[t.x][t.y][t.z] + 1;
				Q.push(p);
				//printf("(%d, %d, %d)\n", p.x, p.y, p.z);
			}

		}
	}
	if (dis[endpt.x][endpt.y][endpt.z] < 0x7f7f7f7f)
		printf("Escaped in %d minute(s).\n", dis[endpt.x][endpt.y][endpt.z]);
	else
		printf("Trapped!\n");
}
int main()
{
	int i, j, k;
	//freopen("input.txt", "r", stdin);
	while (scanf("%d%d%d", &L, &R, &C) == 3 && (L != 0 && R != 0 && N != 0))
	{
		for (i = 0; i < L; i++)
		{
			for (j = 0; j < R; j++)
			{
				scanf("%s", maze[i][j]);
				for (k = 0; maze[i][j][k]; k++)
				{
					if (maze[i][j][k] == 'S')
					{
						start.x = i;
						start.y = j;
						start.z = k;
					}
					else if (maze[i][j][k] == 'E')
					{
						endpt.x = i;
						endpt.y = j;
						endpt.z = k;
					}
				}
			}
		}
		bfs();
	}
	return 0;
}