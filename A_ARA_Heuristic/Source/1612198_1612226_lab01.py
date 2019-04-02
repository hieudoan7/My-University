from Mylibrary import *

#Astar Search
def Astar_search(start, goal,sizeOfGrid,grid):
	frontier = PriorityQueue()
	frontier.put(start,0)
	came_from={}
	cost_so_far={}
	came_from[start]= None
	came_from[goal]=None #neo lai
	cost_so_far[start]=0
	while not frontier.empty():
		current=frontier.get()
		if current==goal:
			break

		for next in neighbor(current,sizeOfGrid,grid):
			new_cost = cost_so_far[current]+1 #chi tinh tren edges thoi
			if next not in cost_so_far or new_cost<cost_so_far[next]:
				cost_so_far[next]=new_cost
				value = new_cost+h(goal,next)
				frontier.put(next,value)
				came_from[next]=current
	return came_from #, cost_so_far

import sys
if __name__=='__main__':
	fileInput = sys.argv[1]
	fileOutput = sys.argv[2]
	grid =[]
	sizeOfGrid=0

	#read input
	file_obj = open(fileInput,"r")
	sizeOfGrid = int(file_obj.readline())
	iS, jS = map(int,file_obj.readline().split()); #iStart, jStart
	iG, jG = map(int,file_obj.readline().split()); #iGoal, jGoal
	for line in file_obj:
		grid.append(line.strip().split(' '))
	file_obj.close()

	#write output
	g_start = (iS,jS) #g_ mean global_
	g_goal = (iG,jG)
	g_came_from = Astar_search(g_start,g_goal,sizeOfGrid,grid)  
	g_path = reconstruct_path(g_came_from,g_start,g_goal)

	file_out = open(fileOutput,"w")
	if g_path==-1:
		file_out.write("-1")
	else:
		numberOfStep = len(g_path)
		for i in range(sizeOfGrid):
			for j in range(sizeOfGrid):
				if (i,j) in g_path:
					grid[i][j]='x'
				elif grid[i][j]=='1':
					grid[i][j]='o'
				else:
					grid[i][j]='-'
		grid[iS][jS]='S'
		grid[iG][jG]='G'

		file_out.write(str(numberOfStep)+'\n')
		for i in range(numberOfStep-1):
			file_out.write(str(g_path[i])+" ")
		file_out.write(str(g_path[numberOfStep-1])+'\n')
		for i in range(sizeOfGrid):
			for j in range(sizeOfGrid):
				file_out.write(grid[i][j])
				if j<sizeOfGrid-1: file_out.write(' ')
			file_out.write('\n')
	file_out.close()