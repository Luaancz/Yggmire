Yggmire
=======

An crafting MMO platform built on top of Microsoft Research's Project "Orleans". The objective is to create a simple, open and extensible platform that scales well, and serve as a proof of concept.

You can follow my (Luaan's) blog for updates at http://www.luaan.cz/

To set this up, you have to install the Orleans SDK CTP. Follow this guide to do that - https://orleans.codeplex.com/wikipage?title=Orleans%20Setup%20for%20Developers&referringTitle=Getting%20Started%20with%20Orleans.

Make sure that the environment variable for the OrleansSDK is properly setup. Also, you'll need to install SharpDX SDK, and add that path to your enviornmental variables as well (SharpDXSdk). I've got this setup from a clean slate on my build server, so it shouldn't be all that hard anymore.

I've added a simple test silo host, so you no longer need to do any more manual configuration - just make sure the storage provider path in TestServerConfiguration.xml points to a valid path on your computer.

There's usually a server running on the address `yggmire.net`. You can see the Git revision used to build the server on http://www.yggmire.net/. Using the same client, you should be able to connect
and test that everything is working fine.
