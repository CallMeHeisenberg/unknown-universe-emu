﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;

namespace NettyBaseReloaded.Game.objects.world.map.mines
{
    class DDM01 : Mine
    {
        public override int MineType => 4;

        public DDM01(int id, string hash, Vector pos, Spacemap map) : base(id, hash, pos, map)
        {
        }

        public override void Effect()
        {
            Damage.Area(Spacemap, Position, 1000, 25, Damage.Types.MINE, DamageType.PERCENTAGE);
        }
    }
}
