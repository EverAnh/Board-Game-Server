

import socket
import sys

host = '127.0.0.1'
port = 54389

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect( (host, port) )

print ('socket test')

print (s.recv(4096))

'''
hello = 'I say hello \n'

try :
    #Set the whole string
    # s.sendall(hello)
except socket.error:
    #Send failed
    print ('Send failed')
    sys.exit()
'''

print ('Message recieve successfully')

'''
while True:
    print (s.recv(4096))
    m = raw_input('type a message:')
    mess = str(m) + '\n'
    s.sendall(mess)
'''
