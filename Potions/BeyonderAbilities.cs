using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod;
using LordOfTheMysteriesMod.Projectiles;

namespace LordOfTheMysteriesMod.Buffs
{
    public class BeyonderAbilities
    {
        public int EnhancedBreathingTimer = 0;

        public bool RagingBlowHit = false;
        public int RagingBlowCD = 0;
        public int RagingBlowCDMax = 3600;
        public bool RagingBlowInCD = false;
        public int RagingBlowOn = 0;
        public int RagingBlowOnMax = 600;
        public bool InRagingBlow = false;

        public int WaterBallCD = 0;
        public int WaterBallCDMax = 120;
        public bool WaterBallPrepared = false;

        public static Dictionary<string, Action<Player>> Abilities = new Dictionary<string, Action<Player>>();

        /// <summary>
        /// This method Iterates through the player's beyonder abilities and runs the corresponding function every updates.
        /// </summary>
        /// <param name="player"></param>
        public void UpdateBeyonderAbilities(Player player)
        {
            foreach (KeyValuePair<string, Action<Player>> element in player.GetModPlayer<LordOfTheMysteriesModPlayer>().AbilityList) {
                element.Value(player);
            }
        }

        /// <summary>
        /// Tyrant Pathway, Sequence 9, Sailor
        /// Increases breathing time underwater
        /// </summary>
        /// <param name="player"></param>
        public void EnhancedBreathing(Player player)
        {
            int Sequence = player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence;
            if (Sequence < 5) {
                player.gills = true;
            } else {
                int EnhancedBreathingTimerCD = 11 - Sequence;
                EnhancedBreathingTimer++;
                if (EnhancedBreathingTimer >= 2) {
                    player.breathCD--;
                    if (EnhancedBreathingTimer >= EnhancedBreathingTimerCD) {
                        EnhancedBreathingTimer = 0;
                    }   
                }
            }
        }

        //Tyrant Pathway, Sequence 9, Sailor
        //Let the player swim swiftly under water
        public void FastSwimming(Player player)
        {
            player.ignoreWater = true;
            if (player.wet && player.controlJump) {
                player.canFloatInWater = true;
            }
        }

        //Tyrant Pathway, Sequence 9, Sailor
        //The player can see further at night when there is light
        public void NightVision(Player player)
        {
            player.nightVision = true;
        }

        //Tyrant Pathway, Sequence 8, Folk of Rage
        //Increases melee damage and crit chance for 10 seconds 
        //CD is 60 seconds
        public void RagingBlow(Player player)
        {
            if (player.GetModPlayer<LordOfTheMysteriesModPlayer>().RagingBlowHit && !RagingBlowInCD && !InRagingBlow) {
                player.GetModPlayer<LordOfTheMysteriesModPlayer>().RagingBlowHit = false;
                InRagingBlow = true;
                Main.NewText("RagingBlow!", 255, 255, 255);
            }

            if (InRagingBlow) {
                player.GetDamage(DamageClass.Melee).Base += 20;
                player.GetCritChance(DamageClass.Melee) += 0.2f;
                RagingBlowOn++;
            }

            if (RagingBlowOn >= RagingBlowOnMax) {
                InRagingBlow = false;
                RagingBlowInCD = true;
                RagingBlowOn = 0;
                Main.NewText("RagingBlow ends!", 255, 255, 255);
            }

            if (RagingBlowInCD) {
                RagingBlowCD++;
            }

            if (RagingBlowCD >= RagingBlowCDMax) {
                RagingBlowInCD = false;
                RagingBlowCD = 0;
                Main.NewText("RagingBlow prepared!", 255, 255, 255);
            }
        }

        //Tyrant Pathway, Sequence 7, Seafarer
        //Being able to discern weather and direction
        public void SeafarerSense(Player player)
        {
            player.accCompass = 1;
            player.accWeatherRadio = true;
        }

        //Tyrant Pathway, Sequence 7, Seafarer
        //Being able to shoot water balls
        public void WaterBall(Player player)
        {
            Vector2 WaterBallVector = Vector2.Zero; //The initial velocity of water ball projectile
            Vector2 closestWaterTilePosition = Vector2.Zero; //The closest water tile from the player
            Vector2 playerPosition = Main.LocalPlayer.position; //The position of the player
            Vector2 waterTilePosition = Vector2.Zero; //A temporary storage used in the search for the closest water tile from the player
            float DistanceFromNPC = float.MaxValue; //The distance from player to a potential target of the water ball projectile
            float closestWaterTileDistance = float.MaxValue; // The distance from the closest water tile from the player to the player

            //Water ball in CD
            if (!WaterBallPrepared) {
                WaterBallCD++;
            }
            //Water ball is prepared
            if (WaterBallCD >= WaterBallCDMax) {
                WaterBallPrepared = true;
                //Search for the closest active enemy npc
                NPC nearestNPC = SearchEnemy(player, 500);
                //If the closest active enemy npc is not more than 500 units away from the player, we are ready to shoot the water ball projectile
                if (nearestNPC != null) {
                    //Search for the closest water tile
                    for (int x = playerPosition.ToTileCoordinates().X - 5; x <= playerPosition.ToTileCoordinates().X + 5; x++) {
                        for (int y = playerPosition.ToTileCoordinates().Y - 5; y <= playerPosition.ToTileCoordinates().Y + 5; y++) {
                            if (Main.tile[x, y].LiquidType == LiquidID.Water && Main.tile[x, y].LiquidAmount >= 255) {
                                waterTilePosition = new Vector2(x, y).ToWorldCoordinates();
                                float waterTileDistance = Vector2.Distance(playerPosition, waterTilePosition);
                                if (waterTileDistance < closestWaterTileDistance) {
                                    closestWaterTilePosition = waterTilePosition;
                                    closestWaterTileDistance = waterTileDistance;
                                }
                            }
                        }
                    }
                    //If the closest water tile is no more than 100 units from the player, we will transform a water tile into a water ball projectile
                    //and reset CD
                    if (closestWaterTileDistance < 100) {
                        Main.tile[closestWaterTilePosition.ToTileCoordinates().X, closestWaterTilePosition.ToTileCoordinates().Y].LiquidAmount = 0;
                        WorldGen.SquareTileFrame(closestWaterTilePosition.ToTileCoordinates().X, closestWaterTilePosition.ToTileCoordinates().Y, true);
                        Projectile.NewProjectile(null, closestWaterTilePosition, WaterBallVector, ModContent.ProjectileType<WaterBall>(), 10, 10f, player.whoAmI);
                        WaterBallPrepared = false;
                        WaterBallCD = 0;
                    }
                }
            }
        }

        //Tyrant Pathway, Sequence 7, Seafarer
        //Glow in water.
        public void WaterLight(Player player) {
            if (player.wet) {
                Lighting.AddLight(player.position.ToTileCoordinates().X, player.position.ToTileCoordinates().Y, 1f, 1f, 1f);
            }
        }

        //Search for the nearest enemy within a certain distance.
        //return: The nearest enemy NPC within the range.
        public static NPC SearchEnemy(Player player, float maxNPCDistance) {
            float minNPCDistance = maxNPCDistance;
            NPC nearestNPC = null;
            foreach (var npc in Main.npc) {
                if (npc.active && !npc.friendly && npc.type != 488) {
                    Vector2 currVector = npc.Center - player.Center;
                    if (currVector.Length() < minNPCDistance) {
                        minNPCDistance = currVector.Length();
                        nearestNPC = npc;
                    }
                }
            }
            return nearestNPC;
        }
    }
}