﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luaan.Yggmire.OrleansInterfaces.Structures;
using Luaan.Yggmire.OrleansServer.Actors;
using Luaan.Yggmire.OrleansServer.Behaviours;

namespace Luaan.Yggmire.OrleansServer.Actors
{
    public static class WorldItemCreator
    {
        public static WorldItem CreateWorldItem(int id, int prototypeId)
        {
            // This should be built based on configuration. For now, we'll just hardcode this.
            switch (prototypeId)
            {
                // Box
                case 1:
                    {
                        var box = new WorldItem();
                        box.Id = id;
                        box.PrototypeId = prototypeId;

                        box.Behaviours.Add(new ItemStorageBehaviour { Id = "contents", IsContainer = true });

                        return box;
                    }
                // Tree
                case 2:
                    {
                        var tree = new WorldItem();
                        tree.Id = id;
                        tree.PrototypeId = prototypeId;

                        tree.Behaviours.Add(new ItemStorageBehaviour { Id = "1", IsContainer = false });
                        tree.Behaviours.Add(new ItemGeneratorBehaviour());

                        return tree;
                    }
            }

            throw new Exception("Invalid prototype Id.");
        }
    }
}
