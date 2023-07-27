using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

using LordOfTheMysteriesMod.Buffs;
using LordOfTheMysteriesMod.UI;
using Terraria.GameInput;

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
        /// <para>A dictonary that stores the beyonder abilities of the player.</para>
        /// <para>The keys of the dictionary are strings whose correponding values are beyonder ability functions whose name equals to the value of their keys.</para>
        /// </summary>
        public Dictionary<string, Action<Player>> AbilityList = new();

        bool BeyonderAbilityMode;

        public Dictionary<string, bool> AbilityModeSettings = new();
        public Dictionary<string, bool> AbilityActiveSettings = new();
        public Dictionary<string, int> AbilityTargetSettings = new();

        public int RagingBlowTimer;
        public int RagingBlowCDTimer;
        public bool RagingBlowHit;

        public int WaterBallCount;
        public int WaterBallCapacity;
        public int WaterBallTimer;
        public List<bool> WaterBallPositions;

        public override void Initialize() {
            BeyonderAbilityMode = false;

            RagingBlowTimer = 600;
            RagingBlowCDTimer = 3600;
            RagingBlowHit = false;
            AbilityModeSettings.Add("RagingBlow", false);

			WaterBallCount = 0;
            WaterBallCapacity = 3;
            WaterBallTimer = 120;
            WaterBallPositions = new();
            AbilityModeSettings.Add("WaterBall", false);
            AbilityTargetSettings.Add("WaterBall", 0);

            AbilityModeSettings.Add("Bioluminescence", false);
        }

        public override void SaveData(TagCompound tag) {
			tag["Pathway"] = Pathway;
            tag["SequenceName"] = SequenceName;
            tag["Sequence"] = Sequence;
            tag["BeyonderBuff"] = BeyonderBuff;

            List<string> AbilityStringArray = new();
            foreach (KeyValuePair<string, Action<Player>> element in AbilityList) {
                AbilityStringArray.Add(element.Key);
            }
            tag.Add("AbilityStringArray", AbilityStringArray);

            List<string> ModeSettingArrayKey = new();
            List<bool> ModeSettingArrayValue = new();
            foreach (KeyValuePair<string, bool> element in AbilityModeSettings) {
                ModeSettingArrayKey.Add(element.Key);
                ModeSettingArrayValue.Add(element.Value);
            }
            tag.Add("ModeSettingArrayKey", ModeSettingArrayKey);
            tag.Add("ModeSettingArrayValue", ModeSettingArrayValue);

            List<string> TargetSettingArrayKey = new();
            List<int> TargetSettingArrayValue = new();
            foreach (KeyValuePair<string, int> element in AbilityTargetSettings) {
                TargetSettingArrayKey.Add(element.Key);
                TargetSettingArrayValue.Add(element.Value);
            }
            tag.Add("TargetSettingArrayKey", TargetSettingArrayKey);
            tag.Add("TargetSettingArrayValue", TargetSettingArrayValue);
		}

        public override void LoadData(TagCompound tag) {
			Pathway = (string)tag["Pathway"];
            SequenceName = (string)tag["SequenceName"];
            Sequence = (int)tag["Sequence"];
            BeyonderBuff = (int)tag["BeyonderBuff"];

            List<string> AbilityStringArray = new(tag.GetList<string>("AbilityStringArray"));
            for (int i = 0; i < AbilityStringArray.Count; i++) {
                if (!AbilityList.ContainsKey(AbilityStringArray[i]) && BeyonderAbilities.Abilities.ContainsKey(AbilityStringArray[i])) {
                    AbilityList.Add(AbilityStringArray[i], BeyonderAbilities.Abilities[AbilityStringArray[i]]);
                }
            }    

            List<string> ModeSettingArrayKey = new(tag.GetList<string>("ModeSettingArrayKey"));
            List<bool> ModeSettingArrayValue = new(tag.GetList<bool>("ModeSettingArrayValue"));
            for (int i = 0; i < ModeSettingArrayKey.Count; i++) {
                if (!AbilityModeSettings.ContainsKey(ModeSettingArrayKey[i])) {
                    AbilityModeSettings.Add(ModeSettingArrayKey[i], ModeSettingArrayValue[i]);
                } else {
                    AbilityModeSettings[ModeSettingArrayKey[i]] = ModeSettingArrayValue[i];
                }
            }

            List<string> TargetSettingArrayKey = new(tag.GetList<string>("TargetSettingArrayKey"));
            List<int> TargetSettingArrayValue = new(tag.GetList<int>("TargetSettingArrayValue"));
            for (int i = 0; i < TargetSettingArrayKey.Count; i++) {
                if (!AbilityTargetSettings.ContainsKey(TargetSettingArrayKey[i])) {
                    AbilityTargetSettings.Add(TargetSettingArrayKey[i], TargetSettingArrayValue[i]);
                } else {
                    AbilityTargetSettings[TargetSettingArrayKey[i]] = TargetSettingArrayValue[i];
                }
            }
		}

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (LordOfTheMysteriesMod.BeyonderAbilityScrollLeft.JustPressed) {
                if (BeyonderAbilityMode) {
                    BeyonderAbilityMode = false;
                    Main.NewText("BeyonderAbilityMode: " + BeyonderAbilityMode);
                } else {
                    BeyonderAbilityMode = true;
                    Main.NewText("BeyonderAbilityMode: " + BeyonderAbilityMode);
                }
            }
        }

        public override void Kill (double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
            Pathway = "";
            SequenceName = "";
            Sequence = 10;
            BeyonderBuff = 0;

            AbilityList.Clear();

            AbilityModeSettings["RagingBlow"] = false;
            AbilityModeSettings["WaterBall"] = false;
            AbilityModeSettings["Bioluminescence"] = false;

            AbilityTargetSettings["WaterBall"] = 0;
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
            BeyonderAbilitiesPanelUI.Visible = true;
            base.OnEnterWorld(player);
        }
    }
}
