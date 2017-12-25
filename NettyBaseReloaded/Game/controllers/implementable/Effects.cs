﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Effects : IAbstractCharacter
    {
        public bool SlowedDown { get; set; }

        public Effects(AbstractCharacterController controller) : base(controller)
        {
        }

        public override void Tick()
        {
            //throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public void Slowdown(Character targetCharacter)
        {
            //TODO
            GameClient.SendToSpacemap(targetCharacter.Spacemap, netty.commands.new_client.LegacyModule.write("0|n|fx|start|GRAPHIC_FX_SABOTEUR_DEBUFF|" + targetCharacter.Id));
            GameClient.SendToSpacemap(targetCharacter.Spacemap, netty.commands.old_client.LegacyModule.write("0|n|fx|start|GRAPHIC_FX_SABOTEUR_DEBUFF|" + targetCharacter.Id));
        }

        public void SetInvincible(int time, bool showEffect = false)
        {
            if (Character.Cooldowns.Exists(x => x is InvincibilityCooldown)) return;

            var cooldown = new InvincibilityCooldown(showEffect, DateTime.Now.AddMilliseconds(time));
            Character.Cooldowns.Add(cooldown);
            cooldown.OnStart(Character);
        }

        public void NotTargetable(int time)
        {
            if (Character.Cooldowns.Exists(x => x is NonTargetableCooldown)) return;

            var cooldown = new NonTargetableCooldown(DateTime.Now.AddMilliseconds(time));
            Character.Cooldowns.Add(cooldown);
            cooldown.OnStart(Character);
        }

        public void UpdatePlayerVisibility()
        {
            GameClient.SendPacketSelected(Controller.Character,
                netty.commands.old_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Invisible)));
            GameClient.SendPacketSelected(Controller.Character,
                netty.commands.new_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Invisible)));
            GameClient.SendRangePacket(Controller.Character,
                netty.commands.old_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Invisible)), true);
            GameClient.SendRangePacket(Controller.Character,
                netty.commands.new_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Invisible)), true);
        }
    }
}