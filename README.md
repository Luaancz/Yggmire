Yggmire
=======

An crafting MMO platform built on top of Microsoft Research's Project "Orleans". The objective is to create a simple, open and extensible platform that scales well, and serve as a proof of concept.

You can follow my (Luaan's) blog for updates at http://www.luaan.cz/

To set this up, you have to install the Orleans SDK CTP. Follow this guide to do that - https://orleans.codeplex.com/wikipage?title=Orleans%20Setup%20for%20Developers&referringTitle=Getting%20Started%20with%20Orleans.
Next, you're going to have to setup your silo. At this point, this is tricky, since I'm using a few hard-coded paths in the projects. I'll try to solve this ASAP, for now, you have to play around with it a bit.

To launch the server, you just have to open the solution, build it (make sure everything actually worked) and then just use the StartLocalSilo.cmd script in your Orleans SDK directory, and that should do it.
Make sure that the OrleansConfiguration.XML in the LocalSilo folder is actually setup properly. You have to add a storage provider like this:

<Provider Type="Luaan.Yggmire.OrleansStorage.OrleansFileStorage" Name="Default" RootDirectory="P:\Software\Yggmire\Data\"/>