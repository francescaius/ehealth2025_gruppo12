# Unlinked - Visual Novel
This is the Git repo for the project for the course E-HEALTH METHODS AND APPLICATION of Politecnico di Milano for the aa 2025-2026

## The Game
The game has been build using unity 6000.0.xxxx and is located in the "GiocoDinamico" folder.
The name of the folder came from the initial idea of not building just a game but a dynamic and custom visual novel engines, which, although not perfect because of time constraints helped us a lot in the workload division.

### Run the game
We have included no build to avoid compatibility problem with os but the system easily build and run

 1. Open the folder GiocoDinamico in unity
 2. Open the scene Persistent.unity (located in Assets>Scenes>Persistent.unity)
 3. Start the game from there 

The game can actually be started from any scene in the editor, and it will work.

### Fancy tools for the game
The game have a "DEBUG" mode: 

    Pressing D on your keyword --> enable debug mode

The debug mode offers 2 fancy tools:

 1. Faster (immediate) dialogues
 2. Puzzle pieces - In the game multiple puzzle pieces are acquired and necessary to get to the final scene - in this mode it's sufficient to press a key from 1 to 6 to get the N-th puzzle piece (where N is the pressed key)
 3. Con i pulsanti QWERTYUIOPZXCVB si salta da una scena all'altra del gioco secondo la logica espressa in VisualNovelManager o dalla seguente tabella

| Tasto | Scena |
| :---: | :--- |
| **Q** | `MenuScene` |
| **W** | `SondaggioScene` |
| **E** | `Atto0` |
| **R** | `Atto11` |
| **T** | `CityScene` |
| **Y** | `Atto12` |
| **U** | `Atto2` |
| **I** | `Atto3` |
| **O** | `Atto4` |
| **P** | `Atto51` |
| **Z** | `TrenoScene` |
| **X** | `Atto52` |
| **C** | `Atto53` |
| **V** | `Atto6` |
| **B** | `Atto7` |



### How to read the code
The code is inside Assets>Scripts
Assets>Scripts>Library contains code common to all scenes and useful for our engine

 - VisualNovelManager is our singleton that manage the flow of the game
 - ControllerElementoDiScena is an important class to coordinate what happens on the scene (making scene writing very easy, flexible, straightforward)
 - DialogueManager (and PhoneDialogueManager) enables dialogues
![UML DIAGRAM](https://i.ibb.co/qYhpNnp0/Screenshot-2025-12-11-170133.png)

Assets>Scripts contains just the scenes' scripts 
note that the names of the scripts are not really clear because the game was originally in Italian in our idea and developed simultaneously - but in every scene there is a SceneController GameObject with the script of that scene :)


There is a EmptyScene.cs used as code template (that is not constraining so no one inherit from it)... altough have lot of comments has been written by hands!
LLMs have been used to debug and to get away with annoying and well known mechanics of unity (not for the logic and main piece of codes!)

### Resources (Images)
The resources are located into the Asset folder
There is a Assets>Resource folder but is not used
The Assets>Immagini folder contains all the images
Assets>Immagini>Personaggi contains main SceneElements (which are characters and backgrounds... in our engines they are treated as the same entity type)
Assets>Immagini>Personaggi>Sfondi is specific for the background
Because of time constraints the files are not perfectly named and ordered



Ps in the game there might be an easter egg🕊️(Hatoful)
