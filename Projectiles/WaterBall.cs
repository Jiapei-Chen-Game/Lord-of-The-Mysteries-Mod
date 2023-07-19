using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LordOfTheMysteriesMod.Projectiles
{
    public class WaterBall : ModProjectile
    {
        public NPC nearestEnemyNPC = null;
        private Texture2D texture;

        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Water Ball");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "水球");
		}

        public override void SetDefaults() {
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 1000;
            Projectile.alpha = 100;

            texture = ModContent.Request<Texture2D>("LordOfTheMysteriesMod/Projectiles/WaterBall").Value;

            DrawOffsetX = -16;
            DrawOriginOffsetY = -16;
		}

        private enum AttackState : int {
            Ready,
            Dash,
        };
        private AttackState State {
            get { return (AttackState)(int)Projectile.ai[0]; }
            set { Projectile.ai[0] = (int)value; }
        }

        public void SearchNearestEnemy(Player player, float MaxNPCDistance) 
        {
            float DistanceFromNPC = MaxNPCDistance;

            foreach (var npc in Main.npc) {
                if (npc.active && !npc.friendly && npc.type != 488) {
                    float currDistance = (npc.Center - player.Center).Length();
                    if (currDistance < DistanceFromNPC) {
                        DistanceFromNPC = currDistance;
                        nearestEnemyNPC = npc;
                    }
                }
            }
        }

        public override void AI()
        {
            Dust Dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Dust.dustWater(), 0f, 0f, 100, default, 3f);
            var player = Main.player[Projectile.owner];
            float t = (float)Main.time * 0.1f + (float)(2 * Math.PI * Projectile.ai[1] / player.GetModPlayer<LordOfTheMysteriesModPlayer>().WaterBallCount);

            if ((player.Center - Projectile.Center).Length() > 1000) {
                Projectile.Kill();
            }

            switch (State) {
                case AttackState.Ready: {
                    var targetPos = player.Center + new Vector2(8f * (float)Math.Cos(t), 1f * (float)Math.Sin(t)) * 5f;
                    Projectile.velocity = Vector2.Normalize(targetPos - Projectile.Center) * 5f + player.velocity;

                    SearchNearestEnemy(player, 500);
                    if (nearestEnemyNPC != null) {
                        State = AttackState.Dash;
                    }
                }
                    break;
                case AttackState.Dash: {
                    var targetPos = nearestEnemyNPC.position;
                    Vector2 targetVel = Vector2.Normalize(targetPos - Projectile.Center);
                    targetVel *= 6f;
                    float accX = 0.2f;
                    float accY = 0.1f;
                    Projectile.velocity.X += (Projectile.velocity.X < targetVel.X ? 1 : -1) * accX;
                    Projectile.velocity.Y += (Projectile.velocity.Y < targetVel.Y ? 1 : -1) * accY;

                    if (nearestEnemyNPC == null || !nearestEnemyNPC.active) {
                        State = AttackState.Ready;
                    }
                }
                    break;
                default:
                    break;
            }
        }

        public override void Kill(int timeLeft) {
            var player = Main.player[Projectile.owner];
            player.GetModPlayer<LordOfTheMysteriesModPlayer>().WaterBallCount -= 1;
            player.GetModPlayer<LordOfTheMysteriesModPlayer>().WaterBallPositions[(int)Projectile.ai[1]] = false;
            Main.tile[Projectile.position.ToTileCoordinates().X, Projectile.position.ToTileCoordinates().Y].LiquidAmount = 255;
            WorldGen.SquareTileFrame(Projectile.position.ToTileCoordinates().X, Projectile.position.ToTileCoordinates().Y, true);
        }

        public override bool PreDraw(ref Color lightColor) {
            return false;
        }

        public override void PostDraw(Color lightColor) {
            Color WaterColor = Colors.CurrentLiquidColor;
            if (WaterColor == new Color(168, 106, 32)) {
                WaterColor = new Color(7, 145, 142);
            }

            Color DarkColor = Color.Black;
            float LightLevel = Lighting.GetColor(Projectile.Center.ToTileCoordinates().X, Projectile.Center.ToTileCoordinates().Y).ToVector3().Length();
            Color FinalColor = Projectile.GetAlpha(Color.Lerp(DarkColor, WaterColor, LightLevel));

            Main.EntitySpriteDraw(texture, Projectile.position - Main.screenPosition, null, FinalColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
        }
    }
}