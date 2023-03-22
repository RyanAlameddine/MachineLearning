# MachineLearning
This repository is simply a collection of a few of my experiments with AI. 

It includes the following:
* A perceptron with hill climbing and gradient descent
* A basic Feedforward Nerual Network with genetics and gradient descent
* Flappy Bird AI implemented with Genetic Learning on my feedforward neural net
* A TicTacToe AI implemented with a Minimax tree
* A Connect4 AI implemented with a Minimax tree with Alpha Beta pruning and monte-carlo tree search on the value
* A Checkers AI implemented with a Minimax tree with Alpha Beta pruning a feed forward value Neural Network which is trained from monte-carlo tree search
* A Markov Chain which can analyze scripts or books and then recreate stories from said scripts (including some sample Game of Thrones scripts).
* A genetic algorithm which tries to replicate a source image using only non-overlapping triangles and quads, described in more detail below.

Projects I have finished but are not included in this repository for different reasons:
* A Snake AI that plays in a 20x20 board and is trained with a Feed Forward Neural Network by taking in two 1x10 slices of the board around it's head as inputs

## Genetic Quad-Based Abstract Art Generator AI
<img src="https://github.com/RyanAlameddine/MachineLearning/blob/master/AiArt/ExampleResources/bird.gif" width=400> -> <img src="https://github.com/RyanAlameddine/MachineLearning/blob/master/AiArt/ExampleResources/bird-final.png" width=200>

<img src="https://github.com/RyanAlameddine/MachineLearning/blob/master/AiArt/ExampleResources/weirdpanda.jpg" width=200> -> <img src="https://github.com/RyanAlameddine/MachineLearning/blob/master/AiArt/ExampleResources/panda.bmp" width=200>

### The Algorithm

The generator starts with a single quad with the size of the screen, with a random color. At a high level, this algorithm works by mutating the color and verticies of quads, and by splitting quads into smaller sub-quads every once in a while. 

Mutations are only accepted if the average "color difference" between each pixel in the source image and each pixel in the newly generated image (the loss) is lower with the mutation than without.

Each iteration until complete (by a timer), it does the following. For each member in the population (`populationSize` times).

* Randomly mutate zero or more verticies by pushing them a random small distance in a random direction. Some extra trigonometry logic is included to prevent the quads from pushing out of bounds of their neighbors. Impacted by the `positionMutationRate` parameter.

* Randomly mutate the color of zero or more of the quads. This is accomplished by randomly offsetting the RGB values. Impacted by the `colorMutationRate`.

* If a current split threshold has been reached, split each quad perfectly into 4 sub-quads (with the same colors).

The mutation with the lowest loss in the population is selected as the basis for the population in the next round.