﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Checkers : IAbstractCharacter, ITick
    {
        public int VisibilityRange { get; set; }

        //public int PacketSendRange => 1000;

        public bool InVisibleZone => !Character.Range.Zones.Any(x => x.Value is PalladiumZone);

        public Checkers(AbstractCharacterController controller) : base(controller)
        {
            VisibilityRange = 2000;//900
        }

        public void Start()
        {
            //Global.TickManager.Add(this);
        }

        private DateTime LastTick = new DateTime();

        public override void Tick()
        {
            EntityChecker();
            ZoneChecker();
            ObjectChecker();
            LastTick = DateTime.Now;
        }

        public override void Stop()
        {
            ResetEntityRange();
            //Controller.StopController = true;
            //Global.TickManager.Remove(this);
        }

        #region Character related
        public ConcurrentDictionary<int, Character> DisplayedRangeCharacters => Controller.Character.Range.Entities;

        public ConcurrentDictionary<int, Character> SpacemapEntities => Controller.Character.Spacemap.Entities;

        public void EntityChecker()
        {
            var allEntities = DisplayedRangeCharacters.Concat(SpacemapEntities.Where( x=> !DisplayedRangeCharacters.Keys.Contains(x.Key)));
    
            foreach (var entity in allEntities)
            {
                var eValue = entity.Value;
                if (eValue.InRange(Character) && !DisplayedRangeCharacters.ContainsKey(entity.Key))
                {
                    AddCharacterToDisplay(eValue);
                }
                else if (!eValue.InRange(Character) && DisplayedRangeCharacters.ContainsKey(entity.Key))
                {
                    RemoveCharacterFromDisplay(eValue);
                }
            }
        }
        
        public void AddCharacterToDisplay(Character character)
        {
            if (DisplayedRangeCharacters.TryAdd(character.Id, character) && Character is Player player)
            {
                var gameSession = player.GetGameSession();
                Packet.Builder.ShipCreateCommand(gameSession, character);
                Packet.Builder.DronesCommand(gameSession, character);

                //Send movement
                var timeElapsed = (DateTime.Now - character.MovementStartTime).TotalMilliseconds;
                Packet.Builder.MoveCommand(gameSession, character, (int) (character.MovementTime - timeElapsed));                
            }
        }

        public void RemoveCharacterFromDisplay(Character character)
        {
            Character removed;
            if (DisplayedRangeCharacters.TryRemove(character.Id, out removed))
            {
                if (Character.Selected == character)
                {
                    Character.RemoveSelection();
                }

                if (Character is Player player)
                {
                    Packet.Builder.ShipRemoveCommand(player.GetGameSession(), character);
                }
            }
        }

        public void ResetEntityRange()
        {
            foreach (var displayed in DisplayedRangeCharacters)
            {
                RemoveCharacterFromDisplay(displayed.Value);
            }

        }
        
        #endregion
        #region Zone related
        private void ZoneChecker()
        {
            try
            {
                foreach (var zone in Character.Spacemap.Zones.Values.ToList())
                {
                    if ((Character.Position.X >= zone.TopLeft.X && Character.Position.X <= zone.BottomRight.X) &&
                        (Character.Position.Y <= zone.TopLeft.Y && Character.Position.Y >= zone.BottomRight.Y))
                    {
                        if (!Character.Range.Zones.ContainsKey(zone.Id))
                        {
                            Character.Range.Zones.Add(zone.Id, zone);
                        }
                    }
                    else
                    {
                        if (Character.Range.Zones.ContainsKey(zone.Id)) Character.Range.Zones.Remove(zone.Id);
                    }
                }
            }
            catch (Exception e)
            {
                if (Character.Position == null || Character.Spacemap == null) return;
            }
        }
        #endregion
        #region Object related
        private void ObjectChecker()
        {
            if (!(Character is Player)) return;
            try
            {
                foreach (var obj in Character.Spacemap.Objects.Values)
                {
                    if (obj == null) continue;
                    if (Vector.IsInRange(obj.Position, Character.Position, obj.Range))
                    {
                        if (!Character.Range.Objects.ContainsKey(obj.Id))
                        {
                            Character.Range.AddObject(obj);
                            obj.execute(Character);
                            (Character as Player)?.ClickableCheck(obj);
                        }
                    }
                    else
                    {
                        if (Character.Range.Objects.ContainsKey(obj.Id))
                        {
                            Character.Range.RemoveObject(obj);
                            (Character as Player)?.ClickableCheck(obj);
                        }
                    }
                }
                if (Character.Range.Objects.Count != Character.Spacemap.Objects.Count)
                {
                    var diff = Character.Range.Objects.Except(Character.Spacemap.Objects).Concat(Character.Spacemap.Objects.Except(Character.Range.Objects));
                    foreach (var objDiff in diff)
                    {
                        if (objDiff.Value == null) continue;
                        if (objDiff.Value.Position == null)
                        {
                            Character.Range.RemoveObject(objDiff.Value);
                            (Character as Player)?.ClickableCheck(objDiff.Value);
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (Character?.Position == null || Character?.Spacemap == null) return;
                //new ExceptionLog("checkers", "Object Checker", e);
                //Error in checkers->Disconnecting player
               // World.StorageManager.GetGameSession(Character.Id)?.Disconnect(GameSession.DisconnectionType.ERROR);
            }
        }
        #endregion
    }
}
