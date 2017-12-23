﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.controllers.pet.gears
{
    class Passive : Gear
    {
        internal Passive(PetController controller) : base(controller, true, 1)
        {
            Type = GearType.PASSIVE;
        }

        public override void Activate()
        {

        }

        public override void Check()
        {
            Follow(baseController.Pet.GetOwner());
        }
    }
}
