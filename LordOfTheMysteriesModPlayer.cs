using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

using LordOfTheMysteriesMod.Potions;
using LordOfTheMysteriesMod.Buffs;
using LordOfTheMysteriesMod.UI;

namespace LordOfTheMysteriesMod
{
    public class LordOfTheMysteriesModPlayer : ModPlayer
    {
        public string Pathway = "Empty"; //The current Pathway of the player.
        public int Sequence = 10; //The current Sequence of the player.
        public int BeyonderBuff = 0; //The current Beyonder Buff on the player.
        public int Sanity = 100; //The Sanity of the player.
        public Dictionary<string, Action<Player>> AbilityList = new Dictionary<string, Action<Player>>(); //The Abilities of the player
        
        public bool RagingBlowHit = false;

        public override void SaveData(TagCompound tag) {
			tag["Pathway"] = Pathway;
            tag["Sequence"] = Sequence;
            tag["BeyonderBuff"] = BeyonderBuff;

            List<string> AbilityStringArray = new List<string>();
            foreach (KeyValuePair<string, Action<Player>> element in AbilityList) {
                AbilityStringArray.Add(element.Key);
            }
            tag.Add("AbilityStringArray", AbilityStringArray);;
		}

        public override void LoadData(TagCompound tag) {
			Pathway = (string)tag["Pathway"];
            Sequence = (int)tag["Sequence"];
            BeyonderBuff = (int)tag["BeyonderBuff"];
            List<string> AbilityStringArray = new List<string>(tag.GetList<string>("AbilityStringArray"));
            for (int i = 0; i < AbilityStringArray.Count; i++) {
                if (!this.AbilityList.ContainsKey(AbilityStringArray[i])) {
                    this.AbilityList.Add(AbilityStringArray[i], BeyonderAbilities.Abilities[AbilityStringArray[i]]);
                }
            }    
		}

        public override void Kill (double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
            Pathway = "Empty";
            Sequence = 10;
            BeyonderBuff = 0;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (Pathway.Equals("Tyrant") && Sequence <= 8 && !item.noMelee) {
                RagingBlowHit = true;
            }
        }

        public override void OnEnterWorld(Player player)
        {
            NoteBookUI.Visible = false;
            base.OnEnterWorld(player);
        }
    }
}
