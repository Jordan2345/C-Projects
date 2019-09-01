import matplotlib.pyplot as plt 
import csv
import numpy as np

x = []
y = []
xGreen=[]
yGreen=[]
with open('RouletteData.txt','r') as csvfile:
    plots = csv.reader(csvfile, delimiter=',')
    for row in plots:
        x.append(int(row[0]))
        y.append(int(row[1]))
with open('RouletteDataGreen.txt','r') as csvfile:
    plots = csv.reader(csvfile, delimiter=',')
    for row in plots:
        xGreen.append(int(row[0]))
        yGreen.append(int(row[1]))

plt.xticks(np.arange(00, 38, step=1))

plt.bar(x, y, width = 0.8, color = ['black', 'red']) 
plt.bar(xGreen,yGreen, width=0.8,color=['green'])
plt.xlabel('Roulette Number') 
plt.ylabel('Number of Occurences') 
plt.title('Roulette Data') 

# function to show the plot 
plt.show() 
