#global variable
grid =[]
sizeOfGrid=0


#function and class
import heapq
class PriorityQueue:
	def __init__(self):
		self.elements=[]

	def emtpy(self):
		return len(self.elements)==0
	def put(self,item,value):
		heapq.heappush(self.elements,(value,item)) #tham số đầu là cái xét độ ưu tiên
	def get(self):
		return heapq.heappop(self.elements)[1]  #ko chứa 2 phần tử giống nhau và tự update cái nào có value nhỏ hơn

#heuristic 
import math
def h(a,b):  #euclid distance heuristic 
	(x1,y1) =a
	(x2,y2)=b
	return math.sqrt((x2-x1)**2+(y2-y1)**2)

def neighbor(robot):
	(x,y)=robot
	xplus=[-1,-1,-1,0,1,1,1,0]
	yplus=[-1,0,1,1,1,0,-1,-1]
	listNeighbor = []
	for i in range(8):
		xx=x+xplus[i]
		yy=y+yplus[i]
		if 0<=xx<sizeOfGrid and 0<=yy<sizeOfGrid and grid[xx][yy]!='1':
			listNeighbor.append((xx,yy))
	return listNeighbor

#Astar Search
def Astar_search(start, goal):
	frontier = PriorityQueue()
	frontier.put(start,0)
	came_from={}
	cost_so_far={}
	came_from[start]= None
	cost_so_far[start]=0
	while not frontier.emtpy():
		current=frontier.get()
		if current==goal:
			break

		for next in neighbor(current):
			new_cost = cost_so_far[current]+1 #chi tinh tren edges thoi
			if next not in cost_so_far or new_cost<cost_so_far[next]:
				cost_so_far[next]=new_cost
				value = new_cost+h(goal,next)
				frontier.put(next,value)
				came_from[next]=current
	return came_from #, cost_so_far

def reconstruct_path(came_from, start, goal):
    current = goal
    path = []
    while current != start:
        path.append(current)
        current = came_from[current]
    path.append(start) # optional
    path.reverse() # optional
    return path
#read input
file_obj = open("input.txt","r")
sizeOfGrid = int(file_obj.readline())
iS, jS = map(int,file_obj.readline().split()); #iStart, jStart
iG, jG = map(int,file_obj.readline().split()); #iGoal, jGoal
for line in file_obj:
	grid.append(line.strip().split(' '))
file_obj.close()

#process
g_start = (iS,jS) #g_ mean global_
g_goal = (iG,jG)
g_came_from = Astar_search(g_start,g_goal)  
g_path = reconstruct_path(g_came_from,g_start,g_goal)
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

#write output
file_out = open("output.txt","w")
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