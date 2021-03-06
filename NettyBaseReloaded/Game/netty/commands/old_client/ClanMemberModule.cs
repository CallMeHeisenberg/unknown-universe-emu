﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class ClanMemberModule
    {
        public const short ID = 17584;

        public int userId;
        public string userName;

        public ClanMemberModule(int userId, string userName)
        {
            this.userId = userId;
            this.userName = userName;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.UTF(userName);
            return cmd.Message.ToArray();
        }
    }
}
