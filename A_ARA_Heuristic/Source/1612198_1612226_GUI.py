import tkinter as tk
import math
import heapq
from tkinter import *
from tkinter import messagebox
from tkinter import simpledialog
from time import sleep

class PriorityQueue:
    def __init__(self):
        self.elements = []

    def empty(self):
        return len(self.elements) == 0

    def put(self, item, value):
        heapq.heappush(self.elements, (value, item))

    def get(self):
        return heapq.heappop(self.elements)[1]

    def topValue(self):
        return self.elements[0][0]

    def topItem(self):
        return self.elements[0][1]


class GUI(tk.Tk):
    def __init__(self):
        self.isExistGoal = False
        self.isExistStart = False
        self.isDrawableCanvas = True
        self.isSearched = False
        self.openAStar = []
        self.openARA = []
        self.start = None
        self.goal = None
        self.iPathARA = 0
        super().__init__()
        self.title("Search Heuristic")
        # Create a list of button
        self.initButton()
        # Create a 2D array of canvas rectangle
        self.initCanvas()

    def initButton(self):
        # Make a frame and put 4 four button (prev, pause, start, continue)
        frame_btn = tk.Frame(self)
        frame_btn.pack(side=tk.BOTTOM, fill=tk.X)
        self.prev_btn = tk.Button(
            frame_btn, text="Previous", command=self.prevAction)
        self.prev_btn.pack(side=tk.LEFT)
        self.return_btn = tk.Button(
            frame_btn, text="Return", command=self.returnAction)
        self.return_btn.pack(side=tk.LEFT)
        self.start_btn = tk.Button(
            frame_btn, text="Start", command=self.startAction)
        self.start_btn.pack(side=tk.LEFT)
        self.continue_btn = tk.Button(
            frame_btn, text="Continue", command=self.continueAction)
        self.continue_btn.pack(side=tk.LEFT)
        self.createNew_btn = tk.Button(
            frame_btn, text="Create New", command=self.createNewAction)
        self.createNew_btn.pack(side=tk.LEFT)
        self.checkVal = IntVar()
        self.checkVal.set(1)
        self.radio_btnAStar = Radiobutton(
            frame_btn, text="A*", variable=self.checkVal, value=1)
        self.radio_btnAStar.pack(side=tk.LEFT)
        self.radio_btnARA = Radiobutton(
            frame_btn, text="ARA", variable=self.checkVal, value=2)
        self.radio_btnARA.pack(side=tk.LEFT)

    def initCanvas(self):
        # Create a empty canvas
        self.canvas = tk.Canvas(self)
        self.canvas.pack(side=tk.TOP, fill=tk.BOTH, expand=tk.TRUE)
        self.rows = 25
        self.columns = 25
        # Make the canvas update their winfos
        self.update_idletasks()
        self.cellwidth = self.canvas.winfo_width() / self.columns
        self.cellheight = self.canvas.winfo_height() / self.rows
        for column in range(self.columns):
            for row in range(self.rows):
                x1 = column*self.cellwidth
                y1 = row * self.cellheight
                x2 = x1 + self.cellwidth
                y2 = y1 + self.cellheight
                self.canvas.create_rectangle(x1, y1, x2, y2, fill="blue")
        self.canvas.bind("<B1-Motion>", self.onCreateBarrier)
        self.canvas.bind("<B3-Motion>", self.onDeleteBarrier)
        self.canvas.bind("<Configure>", self.onResize)
        self.canvas.bind("<Double-Button-1>", self.onCreateStart)
        self.canvas.bind("<Double-Button-3>", self.onCreateGoal)

    def onCreateBarrier(self, event):
        if self.isDrawableCanvas == False:
            return
        else:
            item = self.canvas.find_closest(event.x, event.y)
            current_color = self.canvas.itemcget(item, 'fill')

            if current_color == 'blue':
                self.canvas.itemconfig(item, fill='#32323e')

    def onDeleteBarrier(self, event):
        if self.isDrawableCanvas == False:
            return
        else:
            item = self.canvas.find_closest(event.x, event.y)
            current_color = self.canvas.itemcget(item, 'fill')

            if current_color == '#32323e':
                self.canvas.itemconfig(item, fill='blue')

    def onCreateGoal(self, event):
        if self.isDrawableCanvas == False:
            return
        else:
            item = self.canvas.find_closest(event.x, event.y)
            current_color = self.canvas.itemcget(item, 'fill')
            if current_color == 'blue' and self.isExistGoal == False:
                self.canvas.itemconfig(item, fill='green')
                self.isExistGoal = True
                self.goal = (math.floor(event.x/self.cellwidth),
                             math.floor(event.y/self.cellheight))
            if current_color == 'green' and self.isExistGoal == True:
                self.canvas.itemconfig(item, fill='blue')
                self.isExistGoal = False
                self.goal = None

    def onCreateStart(self, event):
        if self.isDrawableCanvas == False:
            return
        else:
            item = self.canvas.find_closest(event.x, event.y)
            current_color = self.canvas.itemcget(item, 'fill')
            if current_color == 'blue' and self.isExistStart == False:
                self.canvas.itemconfig(item, fill='yellow')
                self.isExistStart = True
                self.start = (math.floor(event.x/self.cellwidth),
                              math.floor(event.y/self.cellheight))
            if current_color == 'yellow' and self.isExistStart == True:
                self.canvas.itemconfig(item, fill='blue')
                self.isExistStart = False
                self.start = None

    def onResize(self, event):
        widthRatio = self.canvas.winfo_width() / (self.cellwidth * self.columns)
        heightRatio = self.canvas.winfo_height() / (self.cellheight * self.rows)
        self.cellwidth = self.canvas.winfo_width() / self.columns
        self.cellheight = self.canvas.winfo_height() / self.rows
        self.canvas.scale("all", 0, 0, widthRatio, heightRatio)

    def prevAction(self):
        if self.checkVal.get() == 1:
            if self.isSearched:
                if self.pos == 0:
                    tk.messagebox.showinfo(
                        "Thông báo:", "Không thể lùi lại quá đỉnh xuất phát!")
                else:
                    if self.pos == len(self.openAStar) and self.path != -1:
                        for p in self.path:
                            if p != self.goal and p != self.start:
                                (x, y) = p
                                self.setColor(x, y, "white")
                    if self.openAStar[self.pos - 1] == self.goal and self.path != -1:
                        (x, y) = self.goal
                        self.setColor(x, y, "green")
                    else:
                        (x, y) = self.openAStar[self.pos - 1]
                        self.setColor(x, y, "blue")
                    self.pos -= 1
            else:
                tk.messagebox.showinfo("Thông báo:", "Bắt đầu tìm đường!")
                self.searchPath()
            self.radio_btnARA.config(state=DISABLED)
        elif self.checkVal.get() == 2:
            if self.isSearched:
                ColorPath = ["#ff0000", "#ff3232",
                             "#ff6666", "#ff9999", "#ffcccc"]
                ColorOpen = ["#99999f", "#b2b2b7",
                             "#cccccf", "#e5e5e7", "#ffffff"]
                if self.iPathARA == -1:
                    self.iPathARA += 1
                if self.pos == 0:
                    tk.messagebox.showinfo(
                        "Thông báo:", "Không thể lùi lại quá đỉnh xuất phát!")
                else:
                    if self.path[self.iPathARA][1] != -1:  
                        if self.pos == len(self.openARA):
                            self.prevUtil(ColorPath, ColorOpen)
                        elif self.openARA[self.pos][0] != self.openARA[self.pos - 1][0]:                     
                            self.prevUtil(ColorPath, ColorOpen)
                    if self.openARA[self.pos-1][1] != self.goal:
                        (x, y) = self.openARA[self.pos-1][1]
                        self.setColor(x, y, "blue")
                    self.pos -= 1
            else:
                tk.messagebox.showinfo("Thông báo:", "Bắt đầu tìm đường!")
                self.searchPath()
            self.radio_btnAStar.config(state=DISABLED)

    def prevUtil(self, colorpath, coloropen):
        while self.path[self.iPathARA][0] <= self.openARA[self.pos - 1][0]:
            tk.messagebox.showinfo(
                "Thông báo:", "Đường đi của E = " + str(self.path[self.iPathARA][0]))
            for i in range(len(self.path[self.iPathARA][1]) - 1, 0, -1):
                (x, y) = self.path[self.iPathARA][1][i]
                self.update_idletasks()
                self.setColor(
                    x, y, colorpath[int((self.path[self.iPathARA-1][0] - 1.0)/0.5)])
                sleep(0.01)
            if self.iPathARA == 0:
                break
            self.iPathARA -= 1
        if self.iPathARA == 0:
            self.iPathARA = -1
        for i in range(len(self.path[self.iPathARA+1][1]) - 1, 0, -1):
            (x, y) = self.path[self.iPathARA+1][1][i]
            self.update_idletasks()
            self.setColor(
                x, y, coloropen[int((self.openARA[self.pos-1][0] - 1.0)/0.5)])
            sleep(0.01)
        (x, y) = self.start
        self.setColor(x, y, "yellow")
        (x, y) = self.goal
        self.setColor(x, y, "green")

    def startAction(self):
        if self.checkVal.get() == 1:
            if self.isSearched:
                while self.pos < len(self.openAStar):
                    self.update_idletasks()
                    self.continueAction()
                    sleep(0.01)
                tk.messagebox.showinfo("Thông báo:", "Đã mở rộng hết!")
                if self.path != -1:
                    (x, y) = self.goal
                    self.setColor(x, y, "green")
                    for p in self.path:
                        if p != self.goal and p != self.start:
                            (x, y) = p
                            self.setColor(x, y, "red")
                else:
                    tk.messagebox.showinfo(
                        "Thông báo:", "Không tìm thấy đường đi!")
            else:
                tk.messagebox.showinfo("Thông báo:", "Bắt đầu tìm đường!")
                self.searchPath()
            self.radio_btnARA.config(state=DISABLED)
        elif self.checkVal.get() == 2:
            if self.isSearched:
                while self.pos < len(self.openARA):
                    self.update_idletasks()
                    self.continueAction()
                    sleep(0.01)
                self.continueAction()
            else:
                tk.messagebox.showinfo("Thông báo:", "Bắt đầu tìm đường!")
                self.searchPath()
            self.radio_btnAStar.config(state=DISABLED)
            
    def returnAction(self):
        if self.isSearched:
            while self.pos >= 0:
                self.update_idletasks()
                if self.pos == 0:
                    break
                self.prevAction()
                sleep(0.01)
        else:
            tk.messagebox.showinfo("Thông báo:", "Bắt đầu tìm đường!")
            self.searchPath()
        if self.checkVal.get() == 1:
            self.radio_btnARA.config(state=DISABLED)
        else:
            self.radio_btnAStar.config(state=DISABLED)
        
    def continueAction(self):
        if self.checkVal.get() == 1:
            if self.isSearched:
                if self.pos == len(self.openAStar):
                    tk.messagebox.showinfo("Thông báo:", "Đã mở rộng hết!")
                    if self.path != -1:
                        (x, y) = self.goal
                        self.setColor(x, y, "green")
                        for p in self.path:
                            if p != self.goal and p != self.start:
                                (x, y) = p
                                self.setColor(x, y, "red")
                    else:
                        tk.messagebox.showinfo(
                            "Thông báo:", "Không tìm thấy đường đi!")
                else:
                    (x, y) = self.openAStar[self.pos]
                    self.setColor(x, y, "white")
                    self.pos += 1
            else:
                tk.messagebox.showinfo("Thông báo:", "Bắt đầu tìm đường!")
                self.searchPath()
            self.radio_btnARA.config(state=DISABLED)
        elif self.checkVal.get() == 2:
            ColorPath = ["#ff0000", "#ff3232", "#ff6666", "#ff9999", "#ffcccc"]
            ColorOpen = ["#99999f", "#b2b2b7", "#cccccf", "#e5e5e7", "#ffffff"]
            if self.isSearched:
                if self.pos == len(self.openARA):
                    tk.messagebox.showinfo("Thông báo:", "Đã mở rộng hết!")
                    if self.path[self.iPathARA][1] != -1:
                        while self.iPathARA < len(self.path):
                            self.update_idletasks()
                            self.contUtil(ColorPath, ColorOpen)
                            if(self.iPathARA == len(self.path) - 1):
                                break
                            self.iPathARA += 1
                    else:
                        tk.messagebox.showinfo(
                            "Thông báo:", "Không tìm thấy đường đi!")
                else:
                    (x, y) = self.openARA[self.pos][1]
                    self.setColor(
                        x, y, ColorOpen[int((self.openARA[self.pos][0] - 1.0)/0.5)])
                    if self.pos < len(self.openARA) - 1 and self.openARA[self.pos][0] != self.openARA[self.pos + 1][0]:
                        while self.path[self.iPathARA][0] != self.openARA[self.pos + 1][0]:
                            self.update_idletasks()
                            self.contUtil(ColorPath, ColorOpen)
                            if(self.iPathARA == len(self.path) - 1):
                                break
                            self.iPathARA += 1
                    self.pos += 1
            else:
                tk.messagebox.showinfo("Thông báo:", "Bắt đầu tìm đường!")
                self.searchPath()
            self.radio_btnAStar.config(state=DISABLED)

    def contUtil(self, colorpath, coloropen):
        tk.messagebox.showinfo(
            "Thông báo:", "Đường đi của E = " + str(self.path[self.iPathARA][0]))
        for p in self.path[self.iPathARA][1]:
            self.update_idletasks()
            (x, y) = p
            self.setColor(
                x, y, colorpath[int((self.path[self.iPathARA][0]-1.0)/0.5)])
            (x, y) = self.start
            self.setColor(x, y, "yellow")
            (x, y) = self.goal
            self.setColor(x, y, "green")
            sleep(0.01)

    def createNewAction(self):
        for column in range(self.columns):
            for row in range(self.rows):
                self.setColor(column, row, "blue")
        self.isExistGoal = False
        self.isExistStart = False
        self.isDrawableCanvas = True
        self.isSearched = False
        self.openAStar = []
        self.openAStar = []
        self.openARA = []
        self.start = None
        self.goal = None
        self.pos = None
        self.path = []
        self.iPathARA = 0
        self.radio_btnARA.config(state=NORMAL)
        self.radio_btnAStar.config(state=NORMAL)

    def searchPath(self):
        if self.isExistGoal and self.isExistStart:
            if self.checkVal.get() == 1:
                self.path = self.reconstruct_path(
                    self.Astar_search(self.start, self.goal), self.start, self.goal)
            elif self.checkVal.get() == 2:
                self.path = self.ARA(self.start, self.goal, 3.0)
            self.isSearched = True
            self.pos = 0
            self.isDrawableCanvas = False
        else:
            tk.messagebox.showerror(
                "Lỗi", "Chưa nhập vào điểm đầu và điểm cuối!")

    def getColor(self, x, y):
        xPos = (x + 0.5) * self.cellwidth
        yPos = (y + 0.5) * self.cellheight
        item = self.canvas.find_closest(xPos, yPos)
        return self.canvas.itemcget(item, 'fill')

    def setColor(self, x, y, colour):
        xPos = (x + 0.5) * self.cellwidth
        yPos = (y + 0.5) * self.cellheight
        item = self.canvas.find_closest(xPos, yPos)
        self.canvas.itemconfig(item, fill=colour)

    def h(self, a, b):  # euclid distance heuristic
        (x1, y1) = a
        (x2, y2) = b
        return math.sqrt((x2-x1)**2+(y2-y1)**2)

    def neighbor(self, robot):
        (x, y) = robot
        xplus = [-1, -1, -1, 0, 1, 1, 1, 0]
        yplus = [-1, 0, 1, 1, 1, 0, -1, -1]
        listNeighbor = []
        for i in range(8):
            xx = x+xplus[i]
            yy = y+yplus[i]
            if 0 <= xx < self.columns and 0 <= yy < self.rows and self.getColor(xx, yy) != '#32323e':
                listNeighbor.append((xx, yy))
        return listNeighbor

    def Astar_search(self, start, goal):
        frontier = PriorityQueue()
        frontier.put(start, 0)
        came_from = {}
        cost_so_far = {}
        came_from[start] = None
        came_from[goal] = None
        cost_so_far[start] = 0
        while not frontier.empty():
            current = frontier.get()
            if current == goal:
                break
            for next in self.neighbor(current):
                new_cost = cost_so_far[current] + 1
                if next not in cost_so_far or new_cost < cost_so_far[next]:
                    self.openAStar.append(next)
                    cost_so_far[next] = new_cost
                    value = new_cost + self.h(goal, next)
                    frontier.put(next, value)
                    came_from[next] = current
        return came_from

    def fvalue(self, state, g, E):
        return g[state] + E*self.h(state, self.goal)

    def ImprovePath(self, start, goal, g, E, OPEN, CLOSED, INCONS, came_from):
        if OPEN.empty():
            return -1
        while self.fvalue(goal, g, E) > OPEN.topValue():
            current = OPEN.get()
            CLOSED.append(current)
            for next in self.neighbor(current):
                if next not in g:
                    g[next] = 10**9
                    came_from[next] = current
                if g[next] > g[current]+1:
                    
                    g[next] = g[current]+1
                    came_from[next] = current
                    if next not in CLOSED:
                        self.openARA.append((E, next))
                        OPEN.put(next, self.fvalue(next, g, E))
                    else:
                        INCONS.put(next, g[next]+self.h(next, goal))
            if OPEN.empty():
                return -1
        if came_from[goal] == None:
            return -1
        else:
            return came_from

    def ARA(self, start, goal, E):
        g = {}
        came_from = {}
        came_from[start] = None
        came_from[goal] = None
        path = []
        CLOSED = []
        INCONS = PriorityQueue()
        OPEN = PriorityQueue()
        g[start] = 0
        g[goal] = 10**9
        OPEN.put(start, self.fvalue(start, g, E))
        CameFrom = self.ImprovePath(start, goal, g, E, OPEN,
                                    CLOSED, INCONS, came_from)
        path.append((E, self.reconstruct_path(CameFrom, start, goal)))
        while E > 1:
            E -= 0.5
            INCONS.elements += OPEN.elements
            OPEN.elements.clear()
            while not INCONS.empty():
                state = INCONS.get()
                OPEN.put(state, self.fvalue(state, g, E))
            CLOSED.clear()  
            CameFrom2 = self.ImprovePath(
                start, goal, g, E, OPEN, CLOSED, INCONS, came_from)
            path.append((E, self.reconstruct_path(CameFrom2, start, goal)))
        return path

    def reconstruct_path(self, came_from, start, goal):
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
        path.reverse()
        return path


if __name__ == "__main__":
    app = GUI()
    app.mainloop()
