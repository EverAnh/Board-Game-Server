import pygame
import os, sys
import Tkinter as tk
from Tkinter import *

class MainDisplay:

    def __init__(self):
        self._username = "NONE"
        self._password = "NONE"
        self._game_choice = "NONE"
    
   
    def _run_window(self, message):
        self._win = Tk()
        self._win.wm_title("Welcome!")
        
        self._L1 = Label(self._win, text="User Name")
        self._L1.grid(row=0, column=0, sticky = E)
        self._E1 = Entry(self._win, bd =5)
        self._E1.grid(row=0, columnspan=2, column=1, sticky = EW, padx=4, pady=4)

        self._L2 = Label(self._win, text="Password")
        self._L2.grid(row=1, column=0, sticky = E)
        self._E2 = Entry(self._win, bd=5)
        self._E2.grid(row=1, columnspan=2, column=1, sticky = EW, padx=4, pady=4)

        self._choice = StringVar()
        self._choice.set("OTHELLO")

        ## radiobutton
        self._R1 = Radiobutton(self._win, text="Othello", variable=self._choice, value="OTHELLO")
        self._R1.grid(row=2, column=0)
        self._R2 = Radiobutton(self._win, text="Chutes & Ladders", variable=self._choice, value="CHUTES_LADDERS")
        self._R2.grid(row=2, column=1)
        self._R3 = Radiobutton(self._win, text="Connect Four", variable=self._choice, value="CONNECT_FOUR")
        self._R3.grid(row=2, column=2)

        self._L1 = Label(self._win, text=message, fg="red")
        self._L1.grid(row=3, column=0, columnspan=3, sticky=W, padx=4, pady=4)

        self._submitButton = Button(self._win, text="Connect", command=self._submit)
        self._submitButton.grid(row=3, column=2, sticky=E, padx=4, pady=4)

        self._win.mainloop()


    def _submit(self):
        self._username = self._E1.get()
        self._password = self._E2.get()
        self._game_choice = self._choice.get()
        self._win.destroy()


    def retrieve_user_info(self, message):
        self._run_window(message)
        return (self._username, self._password, self._game_choice)
        
        

##if __name__ == 'main'



#mainDisplay = MainDisplay()
#print mainDisplay.retrieve_user_info()
#e = Entry(mainDisplay)
#while not mainDisplay._should_quit:
    ## clock.tick(30)
#    mainDisplay.update()
#pygame.quit()
