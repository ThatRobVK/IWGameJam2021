# IWGameJam2021
Entry for the Infinity Works Game Jam 2021 by team Four Devs and a Game Funeral

## Where to find it
- Github: https://github.com/ThatRobVK/IWGameJam2021
- Builds: https://github.com/ThatRobVK/IWGameJam2021/tree/main/Build

## About the Game
You are the Chief of a Mesoamerican village. Your village Priest is __ascending the temple__, seeking the blessing of your Gods for your village. Your neighbouring villages are doing the same, and __only the first Priest__ to reach the top will be blessed!

This is a 2-4 player multiplayer game, supporting play over local networks and the internet (see instructions below). You are not able to play this on your own.

The game __plays in turns__ and involves bluffing, strategy and betting. You win a turn by making an __offering greater than your rival Chiefs__. You can direct your villagers each turn to make the items you need.

__Your village__ is at the bottom of the screen. Here you can see your __current resource stockpile__. You can also see other players' stockpiles for a tactical advantage.

Top right you see the __resource the Gods want__ an offering of this turn, and the __resource they'll want next turn__. Keep an eye on this panel to direct your villagers.

__Worker placement:__ You can move your workers at the beginning of each turn. 
You get __1 resource for every worker__ at the end of the turn. You can send 1 villager out to __recruit another villager__.

__Offering:__ Select how much you are willing to offer, trying to beat your opponents. You lose your offering, regardless of whether you won. To get a boost, you could __sacrifice one of your workers__. However, if you're too bloodthirsty there is a price to pay...

Keep an eye on the temple and everyone's position. __The game ends when a Priest reaches the top!__

## Running the Game
Pre-made builds of the game can be found in the Build folder. Or you can clone the repo and open it in Unity 2019.4.17f1 and run it directly.
### Windows
1. Download the 32 or 64 bit package, whichever is right for your version of Windows.
2. Right click on the file _SacrificesMustBeMade.exe_ and select properties. On the first panel at the bottom, it tells you this is a downloaded executable and is blocked from being executed. Tick the "Unblock" tickbox and apply the changes.
3. Run _SacrificesMustBeMade.exe_
4. When hosting or connecting, you are likely to get a Windows Firewall notification, allow the application to continue.
### Linux
1. Download the Linux folder
2. Execute _SacrificesMustBeMade.x86_64_
3. Enjoy
### Mac
1. Download the Mac folder
2. Do your Maccy thing, seriously I have no idea

### Networking
As the game plays over a network, you may have port forwarding and firewalls to contend with. Please note the following details to help with any network setup issues:
- Protocol: KCP (UDP under the hood)
- Port: 7777

For example to make it work locally, I had to:
1. Open UDP port 7777 from the internet on my modem/router
2. Set up port forwarding on UDP port 7777 to my dedicated router on my modem/router
3. Set up port forwarding on UDP port 7777 to my computer on my dedicated router

## What We Did vs What We Borrowed
- Game design is our original idea.
- All game programming is our own.
- Intro video: Video creation and sound editing by us. Images from the internet, speech edited from a YouTube video, music is a license free piece we found.
- UI and in-game graphics: Mix of custom made and found graphics, edited and put together by us. Map custom made by us using DungeonFog.
- In-game sound: License free sounds used from various sources. License free music as background.

## Trivia
Did you find the [Wilhelm Scream](https://en.wikipedia.org/wiki/Wilhelm_scream)?
