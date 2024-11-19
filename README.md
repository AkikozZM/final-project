# Final Project - Feel the 3D Dungeon!

This is it! The culmination of your procedural graphics experience this semester. For your final project, we'd like to give you the time and space to explore a topic of your choosing. You may choose any topic you please, so long as you vet the topic and scope with an instructor or TA. We've provided some suggestions below. The scope of your project should be roughly 1.5 homework assignments). To help structure your time, we're breaking down the project into 4 milestones:

## Project planning: Design Doc (due 11/6)
TODO: get your project idea and scope approved by Rachel, Adam or a TA.

# Design Doc

## Introduction
#### What motivates your project?

With the CGGT SIGLAB has been moved to a "Dungeon", and as a gameplay engineer, after taking the procedure generation course, I was thinking: what if I build a procedure dungeon game level in Unity3D? Therefore, I want to create an infinite-level puzzle game. The gameplay should be very easy to understand and get started. Once the player starts the game, it will automatically generate a character in a random starting place, player needs to find all the stars that randomly dropped in the dungeon. If the player can collect all the stars on the current level, the system will move the player to the next level, the entire dungeon map will be shuffled and the player needs to collect all the stars again to get into the next level. As the level increases, the player needs to collect more and more stars (maximum 5 stars).

|Dungeon Map|
|:-:|
|<img src="img/idea01.png" width=500>|

## Goal
#### What do you intend to achieve with this project?

I want to create a high-freedom 3D puzzle game with the implementation of Wave Function Collapse [Algorithm reference](https://github.com/mxgmn/WaveFunctionCollapse)

## Inspiration:
- You must have some form of reference material for your final project. Your reference may be a research paper, a blog post, some artwork, a video, another class at Penn, etc.  
- Include in your design doc links to and images of your reference material.

In the beginning, I was inspired by this project: [3D Wavefunction Collapse Dungeon Generator](https://whaoran0718.github.io/3dDungeonGeneration/)

|Dungeon Generator|
|:-:|
|<img src="img/dungeon.gif" width=500>|

The most important part of the implementation is the Wave Function Collapse algorithm, I found this: [Algorithm reference](https://github.com/mxgmn/WaveFunctionCollapse) as a reference.

## Specification:
#### Outline the main features of your project.
##### 1. 3D Character Movement
##### 2. Procedure Dungeon Generator
##### 3. Random Stars Generator


## Techniques:
#### What are the main technical/algorithmic tools you’ll be using? Give an overview, citing specific papers/articles.
[Wave Function Collapse Algorithm reference](https://github.com/mxgmn/WaveFunctionCollapse)
[Procedural Generation with Wave Function Collapse](https://www.gridbugs.org/wave-function-collapse/)

## Design:
#### How will your program fit together? Make a simple free-body diagram illustrating the pieces.
![](img/idea02.png)

## Timeline:
#### Create a week-by-week set of milestones for each person in your group. Make sure you explicitly outline what each group member's duties will be.

|            | MileStone 1                                                                     | MileStone 2                                                                      | MileStone 3 |
|  ----      | ----                                                                            | ----                                                                             | ----  |
| Akiko Zhu  | Setup develop environment                                          | Finish the tile map generator, make WFC algorithm working in 3D scene            | Polish the procedural generation        | 
|            | Complete 3D character basic movement                               | Finish the random stars generator                                                | Fix all the bugs                        |
|            | Implement basic data structure of wave function collapse algorithm | Create basic materials, Finish the level generator                               | Polish the materials                    |



## Milestone 1: Implementation part 1 (DONE: due 11/13)
#### 1. Set up everything for the game development
#### 2. Implement C# script to achieve 3D character movement, including vertical and horizontal movement, and jump
#### 3. Implement a spherical camera such that the camera will always look at the character and rotate by the cursor

|Character & Camera Controller|
|:-:|
|<img src="img/m1_controller.gif" width=500>|

#### 4. Create some basic tiles for the wave function collapse algorithm, and implement the algorithm framework

|Basic tiles|
|:-:|
|<img src="img/m1_tiles.png" width=500>|


## Milestone 2: Implementation part 2 (due 11/25)

#### 1. Finish the tile map generator, make the WFC algorithm work in 3D scene

|3D WFC algorithm|
|:-:|
|<img src="img/m2_wfc.png" width=500>|

#### 2. Implement C# script to achieve random star generation
#### 3. Implement basic materials, Finish the level generator

Submission: Add a new section to your README titled: Milestone #3, which should include
- written description of progress on your project goals. If you haven't hit all your goals, what did you have to cut and why? 
- Detailed output from your generator, images, video, etc.
We'll check your repository for updates. No need to create a new pull request.

Come to class on the due date with a WORKING COPY of your project. We'll be spending time in class critiquing and reviewing your work so far.

## Final submission (due 12/2)
Time to polish! Spen this last week of your project using your generator to produce beautiful output. Add textures, tune parameters, play with colors, play with camera animation. Take the feedback from class critques and use it to take your project to the next level.

Submission:
- Push all your code / files to your repository
- Come to class ready to present your finished project
- Update your README with two sections 
  - final results with images and a live demo if possible
  - post mortem: how did your project go overall? Did you accomplish your goals? Did you have to pivot?

## Topic Suggestions

### Create a generator in Houdini

### A CLASSIC 4K DEMO
- In the spirit of the demo scene, create an animation that fits into a 4k executable that runs in real-time. Feel free to take inspiration from the many existing demos. Focus on efficiency and elegance in your implementation.
- Example: 
  - [cdak by Quite & orange](https://www.youtube.com/watch?v=RCh3Q08HMfs&list=PLA5E2FF8E143DA58C)

### A RE-IMPLEMENTATION
- Take an academic paper or other pre-existing project and implement it, or a portion of it.
- Examples:
  - [2D Wavefunction Collapse Pokémon Town](https://gurtd.github.io/566-final-project/)
  - [3D Wavefunction Collapse Dungeon Generator](https://github.com/whaoran0718/3dDungeonGeneration)
  - [Reaction Diffusion](https://github.com/charlesliwang/Reaction-Diffusion)
  - [WebGL Erosion](https://github.com/LanLou123/Webgl-Erosion)
  - [Particle Waterfall](https://github.com/chloele33/particle-waterfall)
  - [Voxelized Bread](https://github.com/ChiantiYZY/566-final)

### A FORGERY
Taking inspiration from a particular natural phenomenon or distinctive set of visuals, implement a detailed, procedural recreation of that aesthetic. This includes modeling, texturing and object placement within your scene. Does not need to be real-time. Focus on detail and visual accuracy in your implementation.
- Examples:
  - [The Shrines](https://github.com/byumjin/The-Shrines)
  - [Watercolor Shader](https://github.com/gracelgilbert/watercolor-stylization)
  - [Sunset Beach](https://github.com/HanmingZhang/homework-final)
  - [Sky Whales](https://github.com/WanruZhao/CIS566FinalProject)
  - [Snail](https://www.shadertoy.com/view/ld3Gz2)
  - [Journey](https://www.shadertoy.com/view/ldlcRf)
  - [Big Hero 6 Wormhole](https://2.bp.blogspot.com/-R-6AN2cWjwg/VTyIzIQSQfI/AAAAAAAABLA/GC0yzzz4wHw/s1600/big-hero-6-disneyscreencaps.com-10092.jpg)

### A GAME LEVEL
- Like generations of game makers before us, create a game which generates an navigable environment (eg. a roguelike dungeon, platforms) and some sort of goal or conflict (eg. enemy agents to avoid or items to collect). Aim to create an experience that will challenge players and vary noticeably in different playthroughs, whether that means procedural dungeon generation, careful resource management or an interesting AI model. Focus on designing a system that is capable of generating complex challenges and goals.
- Examples:
  - [Rhythm-based Mario Platformer](https://github.com/sgalban/platformer-gen-2D)
  - [Pokémon Ice Puzzle Generator](https://github.com/jwang5675/Ice-Puzzle-Generator)
  - [Abstract Exploratory Game](https://github.com/MauKMu/procedural-final-project)
  - [Tiny Wings](https://github.com/irovira/TinyWings)
  - Spore
  - Dwarf Fortress
  - Minecraft
  - Rogue

### AN ANIMATED ENVIRONMENT / MUSIC VISUALIZER
- Create an environment full of interactive procedural animation. The goal of this project is to create an environment that feels responsive and alive. Whether or not animations are musically-driven, sound should be an important component. Focus on user interactions, motion design and experimental interfaces.
- Examples:
  - [The Darkside](https://github.com/morganherrmann/thedarkside)
  - [Music Visualizer](https://yuruwang.github.io/MusicVisualizer/)
  - [Abstract Mesh Animation](https://github.com/mgriley/cis566_finalproj)
  - [Panoramical](https://www.youtube.com/watch?v=gBTTMNFXHTk)
  - [Bound](https://www.youtube.com/watch?v=aE37l6RvF-c)

### YOUR OWN PROPOSAL
- You are of course welcome to propose your own topic . Regardless of what you choose, you and your team must research your topic and relevant techniques and come up with a detailed plan of execution. You will meet with some subset of the procedural staff before starting implementation for approval.
