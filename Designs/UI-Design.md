# UI Design

Jiapei Chen

## Notebook

When the player enters the world for the first time, he should have a consumable item "notebook" in his bag. He can consume the notebook to get a notebook UI.

The button that can open the notebook UI should be at the top left of the screen. The button should look like a book. The current notebook button looks like the following:

![Notebook Button](../UI/ButtonPlay.png)

When the user click the button, the notebook UI should show up at the middle of the screen. The notebook is open and shows two sides at a time.

The notebook should be able to display the following information:

Section 1:

- [x] The player's current path
- [x] The player's current sequence
- [x] The player's current sequence name

  The above information should be syncronized with the player's path and sequence so that the player can check the changes as soon as something is changed.

  The path name will change as soon as the player hears the name of sequence 0 from some NPC. For example: Sailor --> Tyrant

- [x] A picture of the symbol of the pathway the player belongs to
  
- [ ] Add a frame to the picture

Section 2:

- [ ] The menu of the book that can lead the player to different pages

Section 3:

- [ ] The beyonder abilities of the player

  The player can go over detailed information of his beyonder abilities by hovering his mouse over the corresponding icon of a certain ability. If he wishes to change the setting of the ability, he can click on the icon to the setting page of the ability.

Section 4:

- [ ] The potion formula and advancement rituals recorded by the player

Section 5:

- [ ] The rest of the notebook is blank and can be written by the player

The notebook should have the following features:

- [x] The player can turn to the next page by clicking the bottom right corner of the right page

- [x] The player can turn to the previous page by clicking the bottom left corner of the left page

- [ ] The player can go to different sections by clicking the topics in the menu

- [ ] The player can go back to the the menu by clicking the bookmark at the top left of the book

- [ ] The player can leave the notebook by clicking the delete button at the top right of the book at each page