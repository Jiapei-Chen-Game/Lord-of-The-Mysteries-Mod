using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LordOfTheMysteriesMod.Projectiles;

namespace LordOfTheMysteriesMod.Buffs
{
    public class BeyonderAbilities
    {
        private int EnhancedBreathingTimer = 0;

        public static Dictionary<string, Action<Player>> Abilities = new();

        /// <summary>
        /// This method Iterates through the player's beyonder abilities and runs the corresponding function every updates.
        /// </summary>
        /// <param name="player"></param>
        public static void UpdateBeyonderAbilities(Player player)
        {
            foreach (KeyValuePair<string, Action<Player>> element in player.GetModPlayer<LordOfTheMysteriesModPlayer>().AbilityList) {
                element.Value(player);
            }
        }

        /// <summary>
        /// Tyrant Pathway, Sequence 9, Sailor,
        /// Allows the player to move swiftly in water and increase their breathing time underwater.
        /// </summary>
        /// <param name="player"></param>
        public void Swimmer(Player player)
        {
            int Sequence = player.GetModPlayer<LordOfTheMysteriesModPlayer>().Sequence;
            int EnhancedBreathingTimerCD = 11 - Sequence;

            if (Sequence < 5) {
                player.gills = true;
            } else {
                EnhancedBreathingTimer++;
                if (EnhancedBreathingTimer >= 2) {
                    player.breathCD--;
                    if (EnhancedBreathingTimer >= EnhancedBreathingTimerCD) {
                        EnhancedBreathingTimer = 0;
                    }   
                }
            }

            player.ignoreWater = true;
            if (player.wet && player.controlJump) {
                player.canFloatInWater = true;
            }
        }

        /// <summary>
        /// Tyrant Pathway, Sequence 9, Sailor, 
        /// The player can see further at night when there is light
        /// </summary>
        /// <param name="player"></param>
        public void NightVision(Player player)
        {
            player.nightVision = true;
        }

        /// <summary>
        /// Tyrant Pathway, Sequence 8, Folk of Rage, 
        /// Increases melee damage and crit chance for 10 seconds 
        /// CD: 60 seconds
        /// </summary>
        /// <param name="player"></param>
        public void RagingBlow(Player player)
        {
            LordOfTheMysteriesModPlayer modPlayer = player.GetModPlayer<LordOfTheMysteriesModPlayer>();

            if (modPlayer.RagingBlowTimer == 600 && modPlayer.RagingBlowCDTimer == 3600) {
                if ((!modPlayer.AbilityModeSettings["RagingBlow"] && Main.keyState.IsKeyDown(Keys.Z)) || 
                    (modPlayer.AbilityModeSettings["RagingBlow"] && modPlayer.RagingBlowHit)) {
                    modPlayer.RagingBlowHit = false;
                    modPlayer.RagingBlowTimer--;
                    player.GetDamage(DamageClass.Melee).Base += 20;
                    player.GetCritChance(DamageClass.Melee) += 0.2f;
                    Main.NewText("RagingBlow!", 255, 255, 255);
                }
            }

            if (modPlayer.RagingBlowTimer < 600) {
                player.GetDamage(DamageClass.Melee).Base += 20;
                player.GetCritChance(DamageClass.Melee) += 0.2f;
                modPlayer.RagingBlowTimer--;
            }

            if (modPlayer.RagingBlowTimer == 0) {
                modPlayer.RagingBlowTimer = 600;
                modPlayer.RagingBlowCDTimer--;
                Main.NewText("RagingBlow ends!", 255, 255, 255);
            }

            if (modPlayer.RagingBlowCDTimer < 3600) {
                modPlayer.RagingBlowCDTimer--;
            }

            if (modPlayer.RagingBlowCDTimer == 0) {
                modPlayer.RagingBlowCDTimer = 3600;
                Main.NewText("RagingBlow prepared!", 255, 255, 255);
            }
        }

        /// <summary>
        /// Tyrant Pathway, Sequence 7, Seafarer, 
        /// Being able to discern weather and direction
        /// </summary>
        /// <param name="player"></param>
        public void SeafarerSense(Player player)
        {
            player.accCompass = 1;
            player.accWeatherRadio = true;
        }

        /// <summary>
        /// Tyrant Pathway, Sequence 7, Seafarer, 
        /// <para>turn a water tile into a water ball and let it rotate around the player for 30s.</para>
        /// <para>When there is an enemy nearby, shoot it to the enemy.</para>
        /// CD: 2 seconds
        /// </summary>
        /// <param name="player"></param>
        public void WaterBall(Player player) {

            LordOfTheMysteriesModPlayer modPlayer = player.GetModPlayer<LordOfTheMysteriesModPlayer>();

            //Water ball in CD
            if (modPlayer.WaterBallTimer > 0) {
                modPlayer.WaterBallTimer--;
            }

            if (modPlayer.WaterBallTimer == 0) {
                Point nearestWaterTile = SearchWaterTile(player, 300);
                if (nearestWaterTile.X != -1 && nearestWaterTile.Y != -1 && modPlayer.WaterBallCount < modPlayer.WaterBallCapacity && Main.myPlayer == player.whoAmI) {
                    if ((!modPlayer.AbilityModeSettings["WaterBall"] && Main.keyState.IsKeyDown(Keys.X)) || 
                        modPlayer.AbilityModeSettings["WaterBall"]) {
                        Main.tile[nearestWaterTile.X, nearestWaterTile.Y].LiquidAmount = 0;
                        WorldGen.SquareTileFrame(nearestWaterTile.X, nearestWaterTile.Y, true);
                        Projectile.NewProjectile(null, nearestWaterTile.ToWorldCoordinates(), Vector2.Zero, ModContent.ProjectileType<WaterBall>(), 10, 10f, player.whoAmI, ai1:GetWaterBallPosition(player));
                        modPlayer.WaterBallCount++;
                        modPlayer.WaterBallTimer = 120;
                    }
                }
            }
        }

        /// <summary>
        /// Search for the nearest avaliable water tile for making water ball.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="maxWaterTileDistance"></param>
        /// <returns>A Point that represents the nearest water tile.</returns>
        public Point SearchWaterTile(Player player, float maxWaterTileDistance) {
            Point start = player.position.ToTileCoordinates();
            Point curr = new(-1, -1);
            
            Queue<Point> queue = new();
            queue.Enqueue(start);

            Dictionary<(int, int), bool> visited = new();

            bool found = false;

            while(!found && queue.Count > 0) {
                curr = queue.Dequeue();

                if (Vector2.Distance(curr.ToWorldCoordinates(), start.ToWorldCoordinates()) <= maxWaterTileDistance) {
                    if (curr.X < Main.tile.Width - 1 && !visited.ContainsKey((curr.X + 1, curr.Y))) {
                        queue.Enqueue(new(curr.X + 1, curr.Y));
                        visited[(curr.X + 1, curr.Y)] = true;
                    }

                    if (curr.X > 0 && !visited.ContainsKey((curr.X - 1, curr.Y))) {
                        queue.Enqueue(new Point(curr.X - 1, curr.Y));
                        visited[(curr.X - 1, curr.Y)] = true;
                    }
                    
                    if (curr.Y < Main.tile.Height - 1 && !visited.ContainsKey((curr.X, curr.Y + 1))) {
                        queue.Enqueue(new Point(curr.X, curr.Y + 1));
                        visited[(curr.X, curr.Y + 1)] = true;
                    }

                    if (curr.Y > 0 && !visited.ContainsKey((curr.X, curr.Y - 1))) {
                        queue.Enqueue(new Point(curr.X, curr.Y - 1));
                        visited[(curr.X, curr.Y - 1)] = true;
                    }
                }

                if (Main.tile[curr.X, curr.Y].LiquidType == LiquidID.Water && Main.tile[curr.X, curr.Y].LiquidAmount >= 255) {
                    found = true;
                }
            }

            if (found) {
                return curr;
            } else {
                return new Point(-1, -1);
            }
            
        }
        
        /// <summary>
        /// Get an integer for a water ball to calculate the position of water balls.
        /// </summary>
        /// <returns>Return the index where the value of element is false. If there doesn't exist any, return -1.</returns>
        public int GetWaterBallPosition(Player player) {

            LordOfTheMysteriesModPlayer modPlayer = player.GetModPlayer<LordOfTheMysteriesModPlayer>();

            if (modPlayer.WaterBallCount < modPlayer.WaterBallCapacity) {
                modPlayer.WaterBallPositions.Add(false);
            }

            for (int i = 0; i < player.GetModPlayer<LordOfTheMysteriesModPlayer>().WaterBallPositions.Count; i++) {
                if (!modPlayer.WaterBallPositions[i]) {
                    modPlayer.WaterBallPositions[i] = true;
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Tyrant Pathway, Sequence 7, Seafarer, 
        /// Glow in water.
        /// </summary>
        /// <param name="player"></param>
        public void Bioluminescence(Player player) {

            LordOfTheMysteriesModPlayer modPlayer = player.GetModPlayer<LordOfTheMysteriesModPlayer>();

            if (player.wet) {
                if ((!modPlayer.AbilityModeSettings["Bioluminescence"] && Main.keyState.IsKeyDown(Keys.C)) || modPlayer.AbilityModeSettings["Bioluminescence"]) {
                    Lighting.AddLight(player.position.ToTileCoordinates().X, player.position.ToTileCoordinates().Y, 1f, 1f, 1f);
                }
            }
        }
    }
}