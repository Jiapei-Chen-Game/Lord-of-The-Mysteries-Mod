using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace LordOfTheMysteriesMod.Projectiles
{
    public class WaterBall : ModProjectile
    {
        public NPC TargetNPC = null;
        public bool dash = false;
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
            Projectile.tileCollide = true;
			Projectile.timeLeft = 600;
            Projectile.alpha = 100;
            texture = ModContent.Request<Texture2D>("LordOfTheMysteriesMod/Projectiles/WaterBall").Value;

            DrawOffsetX = -16;
            DrawOriginOffsetY = -16;
		}

        public void searchTarget() 
        {
            Vector2 WaterBallVector = new Vector2(0, 0);
            float DistanceFromNPC = 500;

            foreach (var npc in Main.npc) {
                if (npc.active && !npc.friendly && npc.type != 488) {
                    Vector2 currVector = npc.Center - Main.player[Projectile.owner].Center;
                    if (currVector.Length() < DistanceFromNPC) {
                        TargetNPC = npc;
                        WaterBallVector = currVector;
                        DistanceFromNPC = currVector.Length();
                    }
                }
            }
        }

        public override void AI()
        {
            Dust Dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Dust.dustWater(), 0f, 0f, 100, default(Color), 3f);
            
            if (TargetNPC == null || !TargetNPC.active) {
                searchTarget();
                if (TargetNPC == null || !TargetNPC.active) {
                    Projectile.Kill();
                }
            }

            Vector2 TargetVel = Vector2.Normalize(TargetNPC.Center - Projectile.Center);
            Vector2 UpdateVelocity = Vector2.Normalize(Projectile.velocity + TargetVel * 5.5f) * 5f;
            
            //Get three tiles that lie in front of the projectile.
            Point ProjectileFront1 = (Projectile.Center + Vector2.Normalize(UpdateVelocity) * 23f).ToTileCoordinates();
            Point ProjectileFront2 = (Projectile.Center + Vector2.Transform(Vector2.Normalize(UpdateVelocity), Matrix.CreateRotationZ(MathHelper.ToRadians(-60))) * 16f).ToTileCoordinates();
            Point ProjectileFront3 = (Projectile.Center + Vector2.Transform(Vector2.Normalize(UpdateVelocity), Matrix.CreateRotationZ(MathHelper.ToRadians(60))) * 16f).ToTileCoordinates();

            //Decide the velocity and rotation of the projectile based on whether there exist blocks in the three tiles.
            if ((Main.tile[ProjectileFront1.X, ProjectileFront1.Y].HasTile && !Main.tile[ProjectileFront1.X, ProjectileFront1.Y].SkipLiquid) ||
                (Main.tile[ProjectileFront2.X, ProjectileFront2.Y].HasTile && !Main.tile[ProjectileFront2.X, ProjectileFront2.Y].SkipLiquid) ||
                (Main.tile[ProjectileFront3.X, ProjectileFront3.Y].HasTile && !Main.tile[ProjectileFront3.X, ProjectileFront3.Y].SkipLiquid)) {
                if (!Main.tile[ProjectileFront3.X, ProjectileFront3.Y].HasTile) {
                    Projectile.velocity = Vector2.Transform(UpdateVelocity, Matrix.CreateRotationZ(MathHelper.ToRadians(60)));
                } else {
                    Projectile.velocity = Vector2.Transform(UpdateVelocity, Matrix.CreateRotationZ(MathHelper.ToRadians(-60)));
                }
            } else {
                Projectile.velocity = UpdateVelocity;
            }
        }    

        public override void Kill(int timeLeft) 
        {
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