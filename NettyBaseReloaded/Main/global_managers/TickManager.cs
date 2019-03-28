﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Main.interfaces;
using System.Diagnostics;
using System.Threading;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.global_managers
{
    class TickManager
    {
        public static short TICKS_PER_SECOND = 64;

        /// <summary>
        /// ITick, Delay *TODO* 
        /// </summary>
        private ConcurrentDictionary<int, ITick> Tickables = new ConcurrentDictionary<int, ITick>();

        private int GetNextTickId()
        {
            var i = 0;
            while (true)
            {
                if (Tickables.ContainsKey(i))
                    i++;
                else return i;
            }
        }
        
        public void Add(ITick tick, out int id)
        {
            id = -1;
            if (/*Tickables.Values.Contains(tick) ||*/ Tickables.ContainsKey(id))
            {
                return;
            }

            id = GetNextTickId();
            Tickables.TryAdd(id, tick);
        }

        public void Remove(ITick tick)
        {
            ITick output;
            if (!Tickables.ContainsKey(tick.GetId()))
            {
                return;
            }
            Tickables.TryRemove(tick.GetId(), out output);
        }

        public bool Exists(ITick tickable)
        {
            if (Tickables.Count == 0) return false;
            if (Tickables.ContainsKey(tickable.GetId())) return true;
            return false;
        }

        public void Tick()
        {
            while (true)
            {
                foreach (var tickable in Tickables)
                {
                    tickable.Value.Tick();
                }
                Thread.Sleep(1);
            }
        }
    }
}
