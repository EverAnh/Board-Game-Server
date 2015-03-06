import pygame, sys

pygame.init()

_display = pygame.display.set_mode((400, 300), 0, 32)
pygame.display.set_caption('Image Button')

allButtons = pygame.image.load('images/button.png')
buttonNormal = allButtons.subsurface(pygame.Rect(0, 0, 158, 30))
buttonPress = allButtons.subsurface(pygame.Rect(0, 60, 158, 30))

brightButtons = pygame.image.load('images/RGB.png')
brightNormal = brightButtons.subsurface(pygame.Rect(0, 0, 59, 50))
brightHighlight = brightButtons.subsurface(pygame.Rect(59, 0, 59, 50))
brightPress = brightButtons.subsurface (pygame.Rect (118, 0, 59, 50))

jump = pygame.mixer.Sound('images/JumpSoundBible.ogg')

score = pygame.image.load('images/counter.png')
scoreX = 40
scoreY = 5
counter = 0

button = buttonNormal
button2 = brightNormal

buttonX = (200-button.get_width())/2
buttonY = (150-button.get_height())/2

print(button.get_width())
print(button.get_height())
print(buttonX)
print(buttonY)

button2X = (350-button2.get_width())
button2Y = (250-button2.get_height())

print(button2.get_width())
print(button2.get_height())
print(button2X)
print(button2Y)

buttonRect = pygame.Rect(buttonX, buttonY, button.get_width(), button.get_height())
button2Rect = pygame.Rect(button2X, button2Y, button2.get_width(), button2.get_height())

myfont = pygame.font.SysFont("Comic Sans MS", 15)
label = myfont.render(str(counter), 1, (0, 0, 0))
mouseX = 10
mouseY = 10

while True:
  _display.fill( (160, 200, 180) )

  for event in pygame.event.get():
    if event.type == pygame.QUIT:
      pygame.quit()
      sys.exit()
    elif event.type == pygame.MOUSEBUTTONDOWN:
      if (buttonRect.collidepoint(event.pos)):
        button = buttonPress
      if (button2Rect.collidepoint(event.pos)):
        button2 = brightPress
    elif event.type == pygame.MOUSEBUTTONUP:
      if (buttonRect.collidepoint(event.pos)):
        button = buttonNormal
        counter += 1
        jump.play()
        label = myfont.render(str(counter), 1, (0, 0, 0))
      if (button2Rect.collidepoint(event.pos)):
        button2 = brightNormal
        counter += 1
        jump.play()
        label = myfont.render(str(counter), 1, (0, 0, 0))
      if mouseX > 290 and mouseX < 349 and mouseY < 250 and mouseY > 200:
        button2 = brightHighlight
    elif event.type == pygame.MOUSEMOTION:
      mouseX = (event.pos[0])
      print(mouseX)
      mouseY = (event.pos[1])
      print(mouseY)
      if mouseX > 290 and mouseX < 349 and mouseY < 250 and mouseY > 200:
        button2 = brightHighlight
      else:
        button2 = brightNormal

  _display.blit(button, (buttonX, buttonY))
  _display.blit(score, (scoreX, scoreY))
  _display.blit(label, (50, 1))
  _display.blit(button2, (button2X, button2Y))
  pygame.display.flip()
