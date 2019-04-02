import heapq
import math

class PriorityQueue:
	def __init__(self):
		self.elements=[]

	def empty(self):
		return len(self.elements)==0
	def put(self,item,value):
		heapq.heappush(self.elements,(value,item)) 
	def get(self):
		return heapq.heappop(self.elements)[1]  
	def topValue(self):
		return self.elements[0][0]
	def topItem(self):
		return self.elements[0][1]

def h(a,b):  #euclid distance heuristic 
	(x1,y1) =a
	(x2,y2)=b
	return math.sqrt((x2-x1)**2+(y2-y1)**2)

def fvalue(state,g,E,goal):
	# print("E ne ne:",E,"\n")
	return g[state]+E*h(state,goal)

def neighbor(robot,sizeOfGrid,grid):
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

def reconstruct_path(came_from, start, goal):
    current = goal
    if came_from == -1:
        return -1
    if came_from[goal] == None:
        return -1
    path = []
    while current != start:
        path.append(current)
        current = came_from[current]
    path.append(start)
    path.reverse()  # optional
    return path
