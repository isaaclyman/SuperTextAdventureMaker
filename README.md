# SuperTextAdventureMaker
A super simple tool for creating text adventures. Currently under construction.

## What is it?
SuperTextAdventureMaker (STAM) is a tool that lets you create retro-style text adventures (interactive fiction) that can
be played on Windows computers.

Text adventures are a style of video game where on every turn the player sees a scene description and a set of actions.
Each action can lead to a different scene. The player reads scenes and chooses actions until the game is over.

If you've ever read a Choose Your Own Adventure book or played Strongbad's game "Thy Dungeonman", you already know how to
play.

## Is it hard to use?
Nope. The grammar is simple and you don't need to know how to code. But you do need to find three symbols on your keyboard 
that you may not use a lot:

`>`: The right caret. You usually press Shift+. (shift-period) to type this.

`` ` ``: The backtick. This is not an apostrophe! It's usually on the upper-left corner of your keyboard.

`|`: The pipe. This is usually above the Enter key. It may look like two vertical lines stacked on top of each other.

## How does it work?
To begin, create a new folder on your computer. You can create more folders inside of this folder, to as many levels as
you want; STAM will read everything. Then create a text file (the file name can end in the extensions ".txt", ".text" or
".stam"). This text file will contain part (or all) of your game content.

STAM text files are composed of *blocks*. Each block has a scene, a set of actions, and results for each action. A block 
looks like this:

```
enter forest > You enter a dark, forboding forest with an overgrown path.
The wind chills you to the bone and it seems that you can hear howling 
in the distance. You need to find Meg quickly and get out of here.

|s| Sneak carefully along the path, staying in the shadows.
> You move cautiously, hoping not to become some creature's lunch.
>> forest sneak

|r| Run toward the center of the forest.
> You break into a sprint, twigs snapping underfoot.
>> forest run

|c| Call Meg's name and listen for a response.
> The howling stops suddenly, but you do not hear Meg.
>> forest yell

|cr| Cry softly to yourself.
> You hear a rustle in the trees and your breath catches.
>> forest cry
```

The player will see something like this:

```
You enter a dark, forboding forest with an overgrown path. The wind 
chills you to the bone and it seems that you can hear howling in the 
distance. You need to find Meg quickly and get out of here.

What do you do?

(s) Sneak carefully along the path, staying in the shadows.
(r) Run toward the center of the forest.
(c) Call Meg's name and listen for a response.
(cr) Cry softly to yourself.

stam> 
```

Suppose the player wants to sneak. They'll type "s" and press Enter:

```
stam> s

You move cautiously, hoping not to become some creature's lunch.

[Press ENTER to continue.]
```

When they press ENTER, they'll go to the next scene.

Here's how blocks work:

- The first part of the block is a "scene name". It's not case sensitive (capitalization doesn't matter). The first
scene in the adventure will *not* have a scene name, but all the others will. The scene name can't have a `>` in it. The
player will never see the scene name.
- At the end of the scene name, put a `>` and then type the scene description. This is what the player will see when they
arrive at the scene. The scene description can't have a `|` in it.
- At the end of the scene description, on a new line, create actions.
  - Before the list of actions, the player will see the text `What do you do?`
  - You can create as many actions per scene as you want, but more than five is probably too many.
  - Actions start with an abbreviation inside of pipes, `|a|` for example. The abbreviation is what the player will type 
to perform the action. An abbreviation can be as long as you want, but short ones are recommended.
  - After the abbreviation, type the action description. The action description can't have a `>` in it.
  - After the action description, on a new line, put a `>` and then an action result. This is usually a short description of
what happens immediately after the player's action. This is optional.
  - After the action description and the action result (if there is one), on a new line, put a `>>` and then a scene name.
This should match the name of the scene you want the action to lead to. This is optional.
  - Each action must have either an `>` action result, `>>` a scene name, or both. If there is an action result but no scene
name, the player will see the action result and then be allowed to choose another action in the same scene. If there is a 
scene name but no action result, the player will go to the next scene as soon as the action is chosen. If there is both an
action result and a scene name, the player will see the action result, followed by the text `[Press ENTER to continue.]`.
When they press Enter, the window will be cleared and they will go to the next scene.
  - If you only create one action and the abbreviation is empty (`||`), the action description will not be shown. The
player will see `[Press ENTER to continue.]` and when they press Enter, they'll automatically go to the next scene.
  - If there are no actions in a scene, that scene will be a Game Over.

You can put multiple blocks in the same text file, or split them between files. It doesn't matter. For bigger adventures,
you'll want to organize your scenes into multiple files (one scene per file is best) and multiple folders.

## Technical stuff

### Goals
This project should produce a Windows executable that is usable by people with no programming knowledge. It will 
do the following:
- Recursively search text files in a given folder. All files ending in ".txt", ".text", or ".stam" will be concatenated 
together in memory.
- Parse these files using a simple, proprietary syntax.
- Provide human-readable error messages when unexpected syntax is discovered.
- Interpret named scene descriptions, actions (with shortcut keys) and action results from the concatenated files.
- Present these in a procedural format in the console, providing a retro text gaming experience.
- Allow the user to choose actions for each scene.
- Allow the user to backtrack, reset the game, or save the game and resume it later.
- When the creator is satisfied with the adventure, allow them to package the concatenated game file into a single file for
distribution. This file will be Base64 encoded (but not encrypted), to add a slight layer of protection against players
spoiling the ending. The file extension will be ".stam.game".

### Roadmap for the future
Some nice-to-haves:
- Allow the player to configure the `What do you do?` text on each scene.
- Variable storage, manipulation and response (so actions can have different results based on variable values, e.g. 
`You don't have enough mana!`).
- Dice roll to determine if an action succeeds or fails.
- Config file that determines things like the symbols used, default `What do you do?` text, and
`[Press ENTER to continue.]` text. Maybe backtracking can be disallowed?
- Allow pictures to be displayed for each scene.
http://stackoverflow.com/questions/33538527/display-a-image-in-a-console-application
- Go platform-agnostic.