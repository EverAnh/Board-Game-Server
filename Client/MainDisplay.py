import pygame
import os, sys
import Tkinter as tk
from Tkinter import *

class MainDisplay:
    
   
    def __init__(self):
        win = Tk()
        
        L1 = Label(win, text="User Name")
        L1.grid(row=0, column=0)
        E1 = Entry(win, bd =5)
        E1.grid(row=0, column=1)

        L2 = Label(win, text="Password")
        L2.grid(row=1, column=0)
        E2 = Entry(win, bd=5)
        E2.grid(row=1, column=1)


        ## radiobutton
        R1 = Radiobutton(win, text="OTHELLO", value=StringVar())
        R1.grid(row=2, column=0)
        R2 = Radiobutton(win, text="BATTLESHIP", value=StringVar())
        R2.grid(row=2, column=1)
        R3 = Radiobutton(win, text="CONNECTFOUR", value=StringVar())
        R3.grid(row=2, column=2)

        submitButton = Button(win, text="Submit")
        submitButton.grid(row=3)


        win.mainloop()


    def retrieve_user_info(self):
        #retrieve user info

        pass


        

##if __name__ == 'main'



mainDisplay = MainDisplay()
#e = Entry(mainDisplay)
#while not mainDisplay._should_quit:
    ## clock.tick(30)
#    mainDisplay.update()
#pygame.quit()
