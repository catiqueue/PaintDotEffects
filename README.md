# What is this?
Hi. This repository contains my Paint.NET plugins, which are awesome.
Come take a look.

## Paint.XOR
What happens, if you XOR X and Y coordinates of an image?  
You'll get a number.  
If you filter out the numbers, that are not primes, an interesting pattern emerges.  
You can also filter by divisibility, which is cool and produces more interesting results.   
You can configure zoom, offset, filter and operation.
### Example output:
![Paint.XOR Image](.examples/XOR_What_Are_You_Doing_0x0.png)
### More at: [flickr.com](https://flic.kr/s/aHBqjCqGVB)  
An important note:  
My examples are not "pure", meaning that the pictures are not raw outputs of the plugin, but a result of an artistic process applied to the raw images.  
But you can get the idea, the pattern is recognizable across all the pictures.  
(The example above is pure)

---------------------------------------

## Paint.RND
Just a configurable random noise generator.  
You can configure zoom and detailing (how much colors are available)
### Example output:
![Paint.RND Image](.examples/15px_noise_FullHD.png)

---------------------------------------

## Paint.ECA
Elementary Cellular Automaton implementation as a Paint.NET plugin.  
Currently under reconstruction, but is working.  
You can draw on a transparent image and then run the automaton on it. Cool stuff.  
Currently you can only configure the rule, which defines the behavior of the automaton.
### Example output:
![Paint.ECA Image](.examples/ECA_What_Are_You_Doing_0x0.png)
### More at: [flickr.com](https://flic.kr/s/aHBqjCqMcp)

---------------------------------------

## Paint.Basket 
Just a cute pattern. Inspired by the baskets... :D  
You can configure spacing and thickness of the lines,
although applying spacing together with thickness does not produce great results.

### Example output:
![Paint.Basket Image](.examples/Basket_What_Are_You_Doing_Example.png)

---------------------------------------

## Paint.GEN
Basically, it randomly stacks up mathematical functions on the X and/or Y coordinate(-s) of the image.  
The old version produced more interesting patterns, but was unusable, because I used C# Expressions,
which then compiled to Func-s. Cool, but impossible to control. You can't know the limits of those functions,
so you can't reliably call them.  
The new version requires me to specify a function's return value range, 
which I can then use to calculate ranges up to the root expression.  
With some limitations.

### Example output (from the old version):
![Paint.GEN Image](.examples/GEN_What_Are_You_Doing_0x1F.png)
### More at: [flickr.com](https://flic.kr/s/aHBqjCqHx4)