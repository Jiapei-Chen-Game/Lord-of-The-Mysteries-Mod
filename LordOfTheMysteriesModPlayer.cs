using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

using LordOfTheMysteriesMod.Buffs;
using LordOfTheMysteriesMod.UI;

namespace LordOfTheMysteriesMod
{
    public class LordOfTheMysteriesModPlayer : ModPlayer
    {
        /// <summary>
        /// <para>The pathway of the player. It should be one of the 22 pathways.</para> 
        /// <para>Example: Sailor/Tyrant(changes to latter after the player knows the sequence 0 of the path)</para>
        /// </summary>
        public string Pathway = "";
        /// <summary>
        /// <para>The sequence name of the player. It should be one of the 10 sequences of the pathway the player belongs to.</para> 
        /// <para>Example: Seafarer(the player belongs to the Tyrant pathway and is sequence 7)</para>
        /// </summary>
        public string SequenceName = "";
        /// <summary>
        /// <para>The sequence of the player. It should be an integer from 0 to 10.</para> 
        /// <para>Example: 7(the player is currently sequence 7)</para>
        /// </summary>
        public int Sequence = 10;
        /// <summary>
        /// <para>The index number of the buff that gives the player beyonder abilities.</para> 
        /// </summary>
        public int BeyonderBuff = 0; 
        /// <summary>
        /// <para>The sanity of the player.</para> 
        /// </summary>
        public int Sanity = 100; 
        /// <summary>
        /// <para>The abilities of the player.</para> 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Action<Player>> AbilityList = new();
        
        public bool RagingBlowHit = false;

        public override void SaveData(TagCompound tag) {
			tag["Pathway"] = Pathway;
            tag["SequenceName"] = SequenceName;
            tag["Sequence"] = Sequence;
            tag["BeyonderBuff"] = BeyonderBuff;

            List<string> AbilityStringArray = new();
            foreach (KeyValuePair<string, Action<Player>> element in AbilityList) {
                AbilityStringArray.Add(element.Key);
            }
            tag.Add("AbilityStringArray", AbilityStringArray);;
		}

        public override void LoadData(TagCompound tag) {
			Pathway = (string)tag["Pathway"];
            SequenceName = (string)tag["SequenceName"];
            Sequence = (int)tag["Sequence"];
            BeyonderBuff = (int)tag["BeyonderBuff"];

            List<string> AbilityStringArray = new(tag.GetList<string>("AbilityStringArray"));
            for (int i = 0; i < AbilityStringArray.Count; i++) {
                if (!AbilityList.ContainsKey(AbilityStringArray[i])) {
                    AbilityList.Add(AbilityStringArray[i], BeyonderAbilities.Abilities[AbilityStringArray[i]]);
                }
            }    
		}

        public override void Kill (double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
            Pathway = "";
            SequenceName = "";
            Sequence = 10;
            BeyonderBuff = 0;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if ((Pathway.Equals("Tyrant") || Pathway.Equals("Sailor")) && AbilityList.ContainsKey("RagingBlow") && !item.noMelee) {
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
