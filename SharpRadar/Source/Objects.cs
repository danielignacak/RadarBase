using System;
using System.Collections.Concurrent;
using System.Drawing;

namespace SharpRadar
{

    /// <summary>
    /// GUI Testing Structures, may change
    /// </summary>
    public class Game
    {
        public Player CurrentPlayer;
        public ConcurrentBag<PMC> PMCs = new ConcurrentBag<PMC>();
        public ConcurrentBag<Scav> Scavs = new ConcurrentBag<Scav>();
    }

    public class Unit
    {
        public int X;
        public int Y;
        public int Group;
        public bool? IsAlive = true;
        public Point Position
        {
            get
            {
                return new Point(X, Y);
            }
        }
    }
    public class Player : Unit
    {
        // ToDo

    }

    public class PMC : Unit
    {
    }

    public class Scav : Unit
    {
        public bool IsPlayerScav;
        public bool IsBoss;
    }

    /// <summary>
    /// EFT/Unity Structures (WIP)
    /// </summary>
    public struct GameObjectManager
    {
        public ulong LastTaggedNode; // 0x0

        public ulong TaggedNodes; // 0x8

        public ulong LastMainCameraTaggedNode; // 0x10

        public ulong MainCameraTaggedNodes; // 0x18

        public ulong LastActiveNode; // 0x20

        public ulong ActiveNodes; // 0x28

    }

    public struct BaseObject
    {
        public ulong previousObjectLink; //0x0000
        public ulong nextObjectLink; //0x0008
        public ulong obj; //0x0010
	};

    public struct ListInternal // Not sure if this is correct
    {
		public unsafe fixed byte pad_0x0000[0x20]; //0x0000
        public ulong firstEntry; //0x0020 
    }; //Size=0x0028

    public struct List // Not sure if this is correct
    {
		public unsafe fixed byte pad_0x0000[0x10]; //0x0000
        public ulong listBase; //0x0010    to ListInternal
        public int itemCount; //0x0018 
    }; //Size=0x001C

}
