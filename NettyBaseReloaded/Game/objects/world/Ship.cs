﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Game.objects.world
{
    class Ship
    {
        /**********
         * BASICS *
         **********/
        public int Id { get; }

        public string Name { get; set; }

        public string LootId { get; set; }

        /*********
         * STATS *
         *********/
        public int Health { get; }
        public int Nanohull { get; set; }
        public int Shield { get; set; }

        public int Speed { get; }

        public double ShieldAbsorption { get; set; }

        private int MinDamage { get; set; }
        private int MaxDamage { get; set; }

        public int Damage { get; set; }

        public bool IsNeutral { get; set; }

        public int LaserColor { get; set; }

        public int Batteries { get; set; }
        public int Rockets { get; set; }

        public int Cargo { get; set; }

        public Reward Reward { get; set; }

        public DropableRewards CargoDrop { get; set; }

        public int AI { get; set; }

        public Ship(int id, string name, string lootId, int health, int nanohull, int speed, int shield, double shieldAbsorb, int minDamage, int maxDamage, bool neutral, int laserColor,
            int batteries, int rockets, int cargo, Reward reward, DropableRewards cargoDrop, int ai)
        {
            Id = id;
            Name = name;
            LootId = lootId;
            Health = health;
            Nanohull = nanohull;
            Speed = speed;
            Shield = shield;
            ShieldAbsorption = shieldAbsorb;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            IsNeutral = neutral;
            LaserColor = laserColor;
            Batteries = batteries;
            Rockets = rockets;
            Cargo = cargo;
            Reward = reward;
            CargoDrop = cargoDrop;
            AI = ai;
            Damage = CalculateDamage();
        }

        private int CalculateDamage()
        {
            return Damage = (MaxDamage - MinDamage) + MinDamage;
        }

        public double GetHealthBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_goliath_design_saturn":
                    return 1.2;
                case "ship_goliath_design_centaur":
                    return 1.1;
                case "ship_leonov":
                    if (player.State.IsOnHomeMap())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetDamageBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_goliath_design_diminisher":
                case "ship_goliath_design_venom":
                case "ship_goliath_design_referee":
                case "ship_goliath_design_enforcer":
                    return 1.05;
                case "ship_goliath_design_crimson":
                case "ship_goliath_design_independence":
                    return 1.07;
                case "ship_vengeance_design_revenge":
                case "ship_vengeance_design_lightning":
                    return 1.1;
                case "ship_leonov":
                    if (player.State.IsOnHomeMap())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetShieldBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_goliath_design_bastion":
                case "ship_vengeance_design_avenger":
                case "ship_goliath_design_solace":
                case "ship_goliath_design_spectrum":
                case "ship_goliath_design_sentinel":
                case "ship_goliath_design_kick":
                    return 1.1;
                case "ship_leonov":
                    if (player.State.IsOnHomeMap())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetExpBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_vengeance_design_adept":
                case "ship_goliath_design_veteran":
                case "ship_goliath_design_ignite":
                case "ship_goliath_design_goal":
                    return 1.1;
                case "ship_leonov":
                    if (player.State.IsOnHomeMap())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetHonorBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_goliath_design_crimson":
                case "ship_goliath_design_independence":
                    return 1.03;
                case "ship_vengeance_design_corsair":
                case "ship_goliath_design_exalted":
                case "ship_goliath_design_ignite":
                    return 1.1;
                case "ship_leonov":
                    if (player.State.IsOnHomeMap())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public string ToStringLoot()
        {
            if (LootId == "ship_goliath") return "ship_goliath_design_goliath-frost";
            if (LootId != "") return LootId;
            return Id.ToString();
        }
    }
}
