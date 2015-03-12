import pygame, sys
import random

pygame.init()
mainClock = pygame.time.Clock()
pygame.key.set_repeat(30, 30)

_display = pygame.display.set_mode( (450, 306), 0, 32)
pygame.display.set_caption('Johnny Spaceship')

background = pygame.image.load('images/balloon-background.png')
bgSize = background.get_rect()

balloon = pygame.image.load('images/balloon.png')
balloon.set_colorkey( (0, 128, 255) )
balloonX = background.get_width() - balloon.get_width()
balloonY = background.get_height()/2

balloonMotionX = -1
balloonMotionY = random.randint(-3, 8)
# randomly decides balloon motion properties each time the game is launched

spaceship = pygame.image.load('images/joe.png')
spaceship.set_colorkey( (187, 221, 255) )
spaceshipX = background.get_width()/2
# spaceshipX = background.get_width() / 2

spaceshipY = background.get_height() - spaceship.get_height()*2
# spaceshipY = 50
spaceshipMotionY = 0.0

while True:

  balloonX = balloonX + balloonMotionX
  balloonY = balloonY + balloonMotionY

  if balloonX <= 0 or balloonX > (450-balloon.get_width()):
    balloonMotionX = -balloonMotionX
  if balloonY <= 0 or balloonY > (306-balloon.get_height()):
    balloonMotionY = -balloonMotionY

  spaceshipY = spaceshipY - spaceshipMotionY
  if spaceshipY <= 0: spaceshipY = 0
  if spaceshipY >= (background.get_height()-spaceship.get_height()): 
    spaceshipY = (background.get_height()-spaceship.get_height())

  for event in pygame.event.get():
    if event.type == pygame.QUIT:
      pygame.quit()
      sys.exit()
    if event.type == pygame.KEYDOWN:
      if event.key == pygame.K_UP:
        if spaceshipMotionY < 15:
          spaceshipMotionY = spaceshipMotionY + 1.50
          print(spaceshipMotionY)
      elif event.key == pygame.K_DOWN:
        if spaceshipMotionY > -10:
          spaceshipMotionY = spaceshipMotionY - 0.50
          print(spaceshipMotionY)
        # higher number is faster acceleration for the spaceship
        # negative numbers cause him to accelerate in the opposite direction

  if spaceshipY < (background.get_height() - spaceship.get_height() ):
    spaceshipMotionY = spaceshipMotionY - 0.25
    # print(spaceshipY)
    # print(background.get_height())
    print(spaceshipMotionY)
        
  _display.blit(background, bgSize)
  
  _display.blit(balloon, (balloonX, balloonY))
  _display.blit(spaceship, (spaceshipX, spaceshipY))

  #_display.blit (balloon, (spaceshipX, spaceshipY))
  # _display.blit (spaceship, (balloonX, balloonY))
  # swapping the two statements puts the balloon in the bottom corner
  # and the spaceship traveling a straight line across the field
  # the up arrow now causes the balloon to move instead of the ship
  # with no display blit spaceship (x, y) there is no spaceship

  pygame.display.flip()
  mainClock.tick(32)  
