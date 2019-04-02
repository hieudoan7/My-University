import time
from Mylibrary import *


def ImprovePath(start, goal, g, E, OPEN, CLOSED, INCONS, came_from, sizeOfGrid, grid):
    tmp = fvalue(goal, g, E, goal)
    if OPEN.empty():
        return -1
    while fvalue(goal, g, E, goal) > OPEN.topValue():
        current = OPEN.get()
        CLOSED.append(current)
        for nxt in neighbor(current, sizeOfGrid, grid):
            if nxt not in g:
                g[nxt] = 10**9
                came_from[nxt] = current
            if g[nxt] > g[current]+1:  # dung roi, g[current]+1 = new_cost
                g[nxt] = g[current]+1
                came_from[nxt] = current
                if nxt not in CLOSED:
                    print((E, nxt))
                    OPEN.put(nxt, fvalue(nxt, g, E, goal))
                else:
                    INCONS.put(nxt, g[nxt]+h(nxt, goal))
        if OPEN.empty():
            return -1
    if came_from[goal] == None:  # lan sau ko tim duoc duong nao tot hon lan dau
        return -1
    else:
        return came_from


def ARA(start, goal, E, sizeOfGrid, grid, interval):
    g = {}
    came_from = {}
    came_from[start] = None
    came_from[goal] = None
    OPEN = PriorityQueue()
    CLOSED = []
    INCONS = PriorityQueue()
    g[start] = 0
    g[goal] = 10**9
    startTime = time.time()
    OPEN.put(start, fvalue(start, g, E, goal))
    CameFrom = ImprovePath(start, goal, g, E, OPEN, CLOSED,
                           INCONS, came_from, sizeOfGrid, grid)
    duongDi = reconstruct_path(CameFrom, start, goal)
    finishTime = time.time()
    if finishTime-startTime <= interval:
        print("Duong di vs E=", E, "\n", duongDi, "\n")

    while E > 1:
        E -= 0.5
        INCONS.elements += OPEN.elements
        OPEN.elements.clear()
        while not INCONS.empty():
            state = INCONS.get()
            OPEN.put(state, fvalue(state, g, E, goal))
        CLOSED.clear()
        CameFrom2 = ImprovePath(start, goal, g, E, OPEN,
                                CLOSED, INCONS, came_from, sizeOfGrid, grid)
        duongDi2 = reconstruct_path(CameFrom2, start, goal)
        finishTime = time.time()
        if finishTime-startTime <= interval:
            print("Duong di vs E=", E, ":\n", duongDi2, "\n")
        else:
            break
if __name__=='__main__':
    #global variable
    grid = []
    sizeOfGrid = 0

    # read input
    file_obj = open("input.txt", "r")
    sizeOfGrid = int(file_obj.readline())
    iS, jS = map(int, file_obj.readline().split())  # iStart, jStart
    iG, jG = map(int, file_obj.readline().split())  # iGoal, jGoal
    for line in file_obj:
        grid.append(line.strip().split(' '))
    file_obj.close()

    # write output
    start = (iS, jS)  # g_ mean global_
    goal = (iG, jG)
    E = 3
    print("Nhập thời gian chạy cho thuật toán ARA* (Ex: 0.0015 second):")
    inputTime = float(input())

    ARA(start, goal, E, sizeOfGrid, grid, inputTime)
