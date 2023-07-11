using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using System.Linq;

namespace LordOfTheMysteriesMod.UI
{
    public class NotebookLeftPage : UIElement
    {
        readonly string texturePath = "LordOfTheMysteriesMod/UI/UIImages/NotebookLeftPage";
        public NotebookLeftPage()
        {
            Width.Set(440f, 0f);
            Height.Set(580f, 0f);
            Left.Set(-440f, 0.5f);
            Top.Set(10f, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(texturePath).Value, GetDimensions().Position(), Color.White);
        }
    }

    public class NotebookRightPage : UIElement
    {
        readonly string texturePath = "LordOfTheMysteriesMod/UI/UIImages/NotebookRightPage";
        public NotebookRightPage()
        {
            Width.Set(440f, 0f);
            Height.Set(580f, 0f);
            Left.Set(0f, 0.5f);
            Top.Set(10f, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>(texturePath).Value, GetDimensions().Position(), Color.White);
        }
    }

    public class NoteBookButton : UIState
    {
        public override void OnInitialize()
        {
            UIImageButton noteBookButton = new UIImageButton(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonPlay"));

            noteBookButton.SetVisibility(1f, 0.8f);
            noteBookButton.Width.Set(96f, 0f);
            noteBookButton.Height.Set(96f, 0f);
            noteBookButton.Left.Set(-200f, 0.5f);
            noteBookButton.Top.Set(30f, 0f);

            noteBookButton.OnClick += (evt, element) => {
                NoteBookUI.currentPageNumber = 0;
                NoteBookUI.Visible = true;
            };

            Append(noteBookButton);
        }
    }

    public class NoteBookBeyonderAbilityThumbNail : UIPanel
    {
        public string imagePath;
        public string text;
        public int DetailedPageIndex;
        public NoteBookBeyonderAbilityThumbNail(string text, int DetailedPageIndex, string imagePath = "LordOfTheMysteriesMod/UI/UIImages/TestImage")
        {
            this.imagePath = imagePath;
            this.text = text;
            this.DetailedPageIndex = DetailedPageIndex;
        }

        public string GetText() {
            return text;
        }
    }

    public class NoteBookUI : UIState
    {
        public static bool DEBUG = false;

        public static bool Visible = false;
        public static bool[] PageVisible = new bool[256];
        public static List<UIElement>[] Pages = new List<UIElement>[512];
        public static int currentPageNumber = 0;
        public static int PageNumber = 0;

        public static int PlayerAbilitySectionTitleIndex = 2;
        public static int PlayerAbilitySectionBegin = PlayerAbilitySectionTitleIndex + 1;
        public static int PlayerAbilitySectionEnd = PlayerAbilitySectionBegin + 1;
        public static int PlayerAbilitySectionDetailedBegin = PlayerAbilitySectionBegin + 1;
        public static int PlayerAbilitySectionDetailedEnd = PlayerAbilitySectionDetailedBegin + 1;
        public static int PotionFormulaSectionTitleIndex = PlayerAbilitySectionDetailedEnd;
        public static int PotionFormulaSectionBegin = PotionFormulaSectionTitleIndex + 1;
        public static int PotionFormulaSectionEnd = PotionFormulaSectionBegin + 1;

        UIImage Cover = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/NotebookCover"));

        NotebookLeftPage LeftPage = new();
        NotebookRightPage RightPage = new();
        UIText LeftPageNumber = new("1");
        UIText RightPageNumber = new("2");

        UIImage Page1PlayerPathSymbol = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/TestImage"));
        UIText Page1PlayerPathInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceInformation = new("To Be Loaded");
        UIText Page1PlayerSequenceNameInformation = new("To Be Loaded");

        UIText Page2MenuTitle = new("Menu", large: true);
        UIText Page2MenuPlayerBeyonderAbilities = new("1. My Beyonder Abilities");
        UIText Page2MenuPotionFormula = new("2. Potion Formulas and Advancement Rituals");
        UIText Page2MenuPlayerNotes = new("3. My Notes");

        UIImageButton closeButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonDelete"));
        UIImageButton nextPageButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonDelete"));
        UIImageButton previousPageButton = new(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/ButtonDelete"));

        public override void OnInitialize()
        {
            Player player = Main.CurrentPlayer;

            PageVisible[0] = true;

            for (int i = 0; i < Pages.Length; i++) {
                Pages[i] = new List<UIElement>();
            }

            Cover.Width.Set(900f, 0f);
            Cover.Height.Set(600f, 0f);
            Cover.Left.Set(-450f, 0.5f);
            Cover.Top.Set(-300f, 0.5f);
            Append(Cover);

            Cover.Append(LeftPage);

            LeftPage.Append(LeftPageNumber);
            LeftPageNumber.SetText((2 * currentPageNumber + 1).ToString());
            LeftPageNumber.HAlign = 0.5f;
            LeftPageNumber.Top.Set(-60f, 1f);

            Pages[0].Add(Page1PlayerPathSymbol);
            Page1PlayerPathSymbol.Width.Set(160f, 0f);
            Page1PlayerPathSymbol.Height.Set(160f, 0f);
            Page1PlayerPathSymbol.HAlign = 0.5f;
            Page1PlayerPathSymbol.Top.Set(120f, 0f);

            Pages[0].Add(Page1PlayerPathInformation);
            Page1PlayerPathInformation.HAlign = 0.5f;
            Page1PlayerPathInformation.Top.Set(0, 0.5f);

            Pages[0].Add(Page1PlayerSequenceInformation);
            Page1PlayerSequenceInformation.HAlign = 0.5f;
            Page1PlayerSequenceInformation.Top.Set(30f, 0.5f);

            Pages[0].Add(Page1PlayerSequenceNameInformation);
            Page1PlayerSequenceNameInformation.HAlign = 0.5f;
            Page1PlayerSequenceNameInformation.Top.Set(60f, 0.5f);

            for (int i = 0; i < Pages[0].Count; i++) {
                LeftPage.Append(Pages[0][i]);
            }

            Cover.Append(RightPage);

            RightPage.Append(RightPageNumber);
            RightPageNumber.SetText((2 * currentPageNumber + 2).ToString());
            RightPageNumber.HAlign = 0.5f;
            RightPageNumber.Top.Set(-60f, 1f);

            Pages[1].Add(Page2MenuTitle);
            Page2MenuTitle.Top.Set(80f, 0);
            Page2MenuTitle.HAlign = 0.5f;

            Pages[1].Add(Page2MenuPlayerBeyonderAbilities);
            Page2MenuPlayerBeyonderAbilities.Top.Set(150f, 0);
            Page2MenuPlayerBeyonderAbilities.MarginLeft = 30f;
            Page2MenuPlayerBeyonderAbilities.OnClick += (evt, element) => {
                currentPageNumber = 1;
            };
            
            Pages[1].Add(Page2MenuPotionFormula);
            Page2MenuPotionFormula.Top.Set(200f, 0);
            Page2MenuPotionFormula.MarginLeft = 30f;
            Page2MenuPotionFormula.OnClick += (evt, element) => {
                currentPageNumber = (PlayerAbilitySectionEnd + 1) / 2;
            };

            Pages[1].Add(Page2MenuPlayerNotes);
            Page2MenuPlayerNotes.Top.Set(250f, 0);
            Page2MenuPlayerNotes.MarginLeft = 30f;
            Page2MenuPlayerNotes.OnClick += (evt, element) => {
                Main.NewText("Beyonder Abilities!", 255, 255, 255);
            };

            for (int i = 0; i < Pages[1].Count; i++) {
                RightPage.Append(Pages[1][i]);
            }

            UIText PlayerAbilitySectionTitleText = new("Beyonder Abilities", 1.5f);
            Pages[PlayerAbilitySectionTitleIndex].Add(PlayerAbilitySectionTitleText);
            PlayerAbilitySectionTitleText.HAlign = 0.5f;
            PlayerAbilitySectionTitleText.VAlign = 0.5f;

            UIText PotionFormulaSectionTitleText = new("Potion Formulas", 1.5f);
            Pages[PotionFormulaSectionTitleIndex].Add(PotionFormulaSectionTitleText);
            PotionFormulaSectionTitleText.HAlign = 0.5f;
            PotionFormulaSectionTitleText.VAlign = 0.5f;
 
            closeButton.Width.Set(32f, 0f);
            closeButton.Height.Set(32f, 0f);
            closeButton.Left.Set(-90f, 1f);
            closeButton.Top.Set(30f, 0f);
            closeButton.OnClick += (evt, element) => {
                Visible = false;
            };
            Cover.Append(closeButton);

            nextPageButton.Width.Set(32f, 0f);
            nextPageButton.Height.Set(32f, 0f);
            nextPageButton.Left.Set(-90f, 1f);
            nextPageButton.Top.Set(-60f, 1f);
            nextPageButton.OnClick += (evt, element) => {
                if (currentPageNumber < PageVisible.Length - 1) {
                    PageVisible[currentPageNumber] = false;
                    currentPageNumber += 1;
                    PageVisible[currentPageNumber] = true;
                }
            };
            Cover.Append(nextPageButton);

            previousPageButton.Width.Set(32f, 0f);
            previousPageButton.Height.Set(32f, 0f);
            previousPageButton.Left.Set(58f, 0f);
            previousPageButton.Top.Set(-60f, 1f);
            previousPageButton.OnClick += (evt, element) => {
                if (currentPageNumber > 0) {
                    PageVisible[currentPageNumber] = false;
                    currentPageNumber -= 1;
                    PageVisible[currentPageNumber] = true;
                }
            };
            Cover.Append(previousPageButton);
        }

        public override void Update(GameTime gametime) {

            /// <summary>
            /// When there is a change on the page that is displayed, set this value to true. Otherwise false.
            /// </summary>
            bool needsUpdate = false;

            /// <summary>
            /// The player who is using this UI system
            /// </summary>
            LordOfTheMysteriesModPlayer player = Main.LocalPlayer.GetModPlayer<LordOfTheMysteriesModPlayer>();

            // If the player is viewing page 1 and page 2, then update the player's information based on
            // player status accordingly.
            if (PageVisible[0]) {
                Page1PlayerPathInformation.SetText("Pathway: " + player.Pathway);
                Page1PlayerSequenceInformation.SetText("Sequence: " + player.Sequence.ToString());
                Page1PlayerSequenceNameInformation.SetText(player.SequenceName);
                if (!player.Pathway.Equals("")) {
                    Page1PlayerPathSymbol.SetImage(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/Buffs/" + CapitalizeAfterSpace(player.SequenceName) + "Buff"));
                } else {
                    Page1PlayerPathSymbol.SetImage(ModContent.Request<Texture2D>("LordOfTheMysteriesMod/UI/UIImages/TestImage"));
                }
            }

            // If the player is viewing beyonder abilities thumbnail/menu section, then update the corresponding
            // information based on player beyonder abilities accordingly
            if (CheckSectionPageVisibility(PlayerAbilitySectionBegin / 2, (PlayerAbilitySectionEnd + 1) / 2)) {
                Dictionary<string, Action<Player>> playerAbilityDictionary = player.AbilityList;
                int currentAbilitySectionIndex = PlayerAbilitySectionBegin;
                int currentDetailedAbilitySectionIndex = PlayerAbilitySectionDetailedBegin;

                // Check every beyonder ability thumbnail. If there exists a thumbnail whose corresponding beyonder
                // ability is not currently owned by the player, then remove the thumbnail from Pages.
                for (int i = PlayerAbilitySectionBegin; i < PlayerAbilitySectionEnd; i++) {
                    int removeNumber = Pages[i].RemoveAll(item =>
                    {
                        NoteBookBeyonderAbilityThumbNail textItem = (NoteBookBeyonderAbilityThumbNail)item;
                        // When the thumbnail is removed from Pages, its corresponding detailed page should also
                        // be removed.
                        if (!playerAbilityDictionary.ContainsKey(textItem.GetText())) {
                            Pages[textItem.DetailedPageIndex].Clear();
                            return true;
                        }
                        return false;
                    });
                    // If there is any thumbnail removed, then we need an update
                    if (removeNumber > 0) {
                        needsUpdate = true;
                        if (Pages[i].Count == 0) {
                            RemovePage(i);
                        }
                    }
                }

                if (DEBUG) {
                    Main.NewText("PlayerAbilitySectionBegin: " + PlayerAbilitySectionBegin + " PlayerAbilitySectionEnd: " + PlayerAbilitySectionEnd);
                }

                foreach (string key in playerAbilityDictionary.Keys) {
                    bool containsKey = false;
                    
                    for (int i = PlayerAbilitySectionBegin; i < PlayerAbilitySectionEnd; i++) {
                        foreach (NoteBookBeyonderAbilityThumbNail element in Pages[i].Cast<NoteBookBeyonderAbilityThumbNail>()) {
                            if (element is not null && element.GetText().Equals(key)) {
                                containsKey = true;
                                break;
                            }
                        }

                        if (containsKey) {
                            break;
                        }

                    }

                    if (!containsKey) {
                        
                        if (Pages[currentAbilitySectionIndex].Count == 4) {
                            currentAbilitySectionIndex += 1;
                            InsertPage(currentAbilitySectionIndex);
                        }

                        NoteBookBeyonderAbilityThumbNail AbilityTextPanel = new(key, currentDetailedAbilitySectionIndex);
                        AbilityTextPanel.Width.Set(320f, 0f);
                        AbilityTextPanel.Height.Set(100f, 0f);
                        
                        AbilityTextPanel.OnClick += (evt, element) => {
                            currentPageNumber = AbilityTextPanel.DetailedPageIndex / 2;
                        };

                        UIImage image = new(ModContent.Request<Texture2D>(AbilityTextPanel.imagePath));
                        AbilityTextPanel.Append(image);
                        image.Height.Set(64f, 0f);
                        image.Width.Set(64f, 0f);
                        image.ScaleToFit = true;
                        image.Left.Set(10f, 0f);
                        image.VAlign = 0.5f;

                        UIText title = new(AbilityTextPanel.text);
                        AbilityTextPanel.Append(title);
                        title.Left.Set(100f, 0f);
                        title.VAlign = 0.5f;

                        AbilityTextPanel.Top.Set(100f * Pages[currentAbilitySectionIndex].Count + 90f, 0);
                        Pages[currentAbilitySectionIndex].Add(AbilityTextPanel);
                        AbilityTextPanel.MarginLeft = 60f;

                        currentDetailedAbilitySectionIndex += 1;
                        needsUpdate = true;
                    }
                }
            }

            // Update the displayed page when the current page is changed.
            if (PageNumber != currentPageNumber || needsUpdate) {
                LeftPage.RemoveAllChildren();
                RightPage.RemoveAllChildren();

                for (int i = 0; i < Pages[2 * currentPageNumber].Count; i++) {
                    LeftPage.Append(Pages[2 * currentPageNumber][i]);
                }
                LeftPage.Append(LeftPageNumber);
                LeftPageNumber.SetText((2 * currentPageNumber + 1).ToString());
                LeftPageNumber.HAlign = 0.5f;
                LeftPageNumber.Top.Set(-60f, 1f);

                for (int i = 0; i < Pages[2 * currentPageNumber + 1].Count; i++) {
                    RightPage.Append(Pages[2 * currentPageNumber + 1][i]);
                }
                RightPage.Append(RightPageNumber);
                RightPageNumber.SetText((2 * currentPageNumber + 2).ToString());
                RightPageNumber.HAlign = 0.5f;
                RightPageNumber.Top.Set(-60f, 1f);

                PageNumber = currentPageNumber;
            }                
        }

        /// <summary>
        /// Insert a page into Pages.
        /// </summary>
        /// <param name="index">The index where the page inserts</param>
        /// <returns>Returns true if insertion is successful. Otherwise return false.</returns>
        static public bool InsertPage(int index) {
            // If index is out of the range of the notebook, return false;
            if (index < 0 || index >= Pages.Length) {
                return false;
            }
            // If the notebook is filled, return false.
            if (Pages[^1].Count > 0) {
                return false;
            }
            // Move every element from index one position backward.
            for (int i = index; i < Pages.Length - 1; i++) {
                List<UIElement> list = Pages[i];
                Pages[i + 1] = list;
            }
            // Change the indices that mark the start and end of sections accordingly.
            ManageSectionIndex(index, 1);
            Main.NewText("Potion Formula Title Page Index: " + PotionFormulaSectionTitleIndex);
            Main.NewText("Potion Formula Title Page UIElement number: " + Pages[PotionFormulaSectionTitleIndex].Count);
            // Insert an empty page at index.
            Pages[index] = new();
            return true;
        }

        /// <summary>
        /// Remove a page from Pages.
        /// </summary>
        /// <param name="index">The index where the page is removed</param>
        /// <returns>Returns true if remove is successful. Otherwise return false.</returns>
        static public bool RemovePage(int index) {
            // If index is out of the range of the notebook, return false;
            if (index < 0 || index >= Pages.Length) {
                return false;
            }
            // Move every element from index one position forward.
            for (int i = index + 1; i < Pages.Length - 1; i++) {
                Pages[i] = Pages[i + 1];
            }
            // Change the indices that mark the start and end of sections accordingly.
            ManageSectionIndex(index, -1);
            return true;
        }

        /// <summary>
        /// Given a range from StartIndex to EndIndex, check whether there exists an element in this range such that 
        /// it is true.
        /// </summary>
        /// <param name="StartIndex"></param>
        /// <param name="EndIndex"></param>
        /// <returns>Returns true if one of the pages from StartIndex to EndIndex is visible.</returns>
        static public bool CheckSectionPageVisibility(int StartIndex, int EndIndex) {
            for (int i = StartIndex; i < EndIndex; i++) {
                if (PageVisible[i]) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Capitalize every letter in the given string that is after a space and removed the spaces.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public string CapitalizeAfterSpace(string input) {
            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++) {
                if (string.IsNullOrEmpty(words[i])) continue;
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
           return string.Join("", words);
        }

        /// <summary>
        /// <para>When a new page is inserted into the notebook or deleted from the notebook at newPageIndex, increase or decrease</para>
        /// <para>corresponding fields that records the start index and end index of each section accordingly. At the same time, </para>
        /// <para>increase or decrease the index of detailed beyonder ability pages accordingly. </para>
        /// </summary>
        /// <param name="index">The index where there is a change in page</param>
        /// <param name="increase">If we want to increase a page, it should be 1. Otherwise, it should be -1</param>
        static public void ManageSectionIndex(int index, int increment) {
            if (increment >= 0) {
                increment = 1;
                if (index <= PotionFormulaSectionEnd) {
                    PotionFormulaSectionEnd += increment;
                    if (index <= PotionFormulaSectionBegin) {
                        PotionFormulaSectionBegin += increment;
                        PotionFormulaSectionTitleIndex += increment;
                        PlayerAbilitySectionDetailedEnd += increment;
                        if (index <= PlayerAbilitySectionDetailedBegin) {
                            //Update Detailed Beyonder Ability Indices.
                            for (int i = PlayerAbilitySectionBegin; i < index; i++) {
                                foreach (NoteBookBeyonderAbilityThumbNail element in Pages[i].Cast<NoteBookBeyonderAbilityThumbNail>()) {
                                    element.DetailedPageIndex += increment;
                                }
                            }
                            PlayerAbilitySectionDetailedBegin += increment;
                            PlayerAbilitySectionEnd += increment;
                            if (index <= PlayerAbilitySectionBegin) {
                                PlayerAbilitySectionBegin += increment;
                            }
                        }
                    }
                }
            } else {
                increment = -1;
                if (index < PotionFormulaSectionEnd) {
                    PotionFormulaSectionEnd += increment;
                    if (index < PotionFormulaSectionBegin) {
                        if (PlayerAbilitySectionDetailedEnd - PlayerAbilitySectionDetailedBegin > 1) {
                            PotionFormulaSectionBegin += increment;
                            PotionFormulaSectionTitleIndex += increment;
                            PlayerAbilitySectionDetailedEnd += increment;
                        }
                        if (index < PlayerAbilitySectionDetailedBegin) {
                            if (PlayerAbilitySectionEnd - PlayerAbilitySectionBegin > 1) {
                                for (int i = PlayerAbilitySectionBegin; i < index; i++) {
                                    foreach (NoteBookBeyonderAbilityThumbNail element in Pages[i].Cast<NoteBookBeyonderAbilityThumbNail>()) {
                                        element.DetailedPageIndex += increment;
                                    }
                                }
                                PlayerAbilitySectionDetailedBegin += increment;
                                PlayerAbilitySectionEnd += increment;
                            }
                            if (index < PlayerAbilitySectionBegin) {
                                PlayerAbilitySectionBegin += increment;
                                PlayerAbilitySectionTitleIndex += increment;
                            }
                        }
                    }
                }
            }
            
        }

    }
}