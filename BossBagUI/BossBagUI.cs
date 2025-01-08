using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using SteelSeries.GameSense;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ID;
using Terraria.Physics;
using System;
using static System.Net.Mime.MediaTypeNames;
using ReLogic.Content;
using System.Drawing.Imaging;
using System.Configuration;


namespace UltimateMining.SkillTree
{
    public class BossBagUI : ModSystem
    {
        internal UserInterface MyInterface;
        internal BossBagUIState MyUI;

        public static bool enabled = false;
        public static List<Item> buttonItems = new List<Item>();

        public override void Load()
        {
            if (!Main.dedServ)
            {
                MyUI = new();
                MyInterface = new();
                MyInterface.SetState(MyUI);
                buttonItems.Clear();
            }
        }

        public override void Unload()
        {
            enabled = false;
        }

        public override void OnWorldLoad()
        {
            enabled = false;
            buttonItems.Clear();
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            MyInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (layerIndex != -1)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                    "Treasure Bag Preview",
                    delegate {
                        MyInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

    }

    public class BossBagUIKeybinds : ModSystem
    {
        public static ModKeybind ShowUIKeybind;

        public override void Load()
        {
            ShowUIKeybind = KeybindLoader.RegisterKeybind(Mod, "ShowTreasureBagPreview", "O");
        }

        public override void Unload()
        {
            ShowUIKeybind = null;
        }
    }

    public class BossBagUIState : UIState
    {

        private UIText header;



        public override void OnInitialize()
        {
            UIPanel panel = new UIPanel();
            panel.Width.Set(500, 0);
            panel.Height.Set(500, 0);
            panel.HAlign = 0.6f;
            panel.VAlign = 0.5f;
            Append(panel);

            header = new UIText("default", 1f, false);
            header.HAlign = 0.0f;
            header.Top.Set(15, 0);
            panel.Append(header);
            BossBagUI.buttonItems.Clear();


            for (int i = 0; i < 60; i++)
            {
                ItemButton button = new ItemButton(-200 + 40 * (i % 10), 50 + 40 * (i / 10), i);
                panel.Append(button);
                button.stackText = new UIText("", 0.5f, false);
                button.Append(button.stackText);
                button.stackText.HAlign = 0.8f;
                button.stackText.VAlign = 0.8f;
            }

            
        }

        public override void OnDeactivate()
        {
            BossBagUI.buttonItems.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (BossBagUI.enabled) base.Draw(spriteBatch);
            header.SetText("Items in this Treasure Bag:");
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            
        }



    }

    public class ItemButton : UIPanel
    {
        public int buttonWidth = 40;
        public int buttonHeight = 40;
        public int item;
        public int stack;
        public int index;
        public UIText stackText;

        public ItemButton() { }
        public ItemButton(int offsetX, int offsetY, int index)
        {

            Width.Set(buttonWidth, 0);
            Height.Set(buttonHeight, 0);
            HAlign = 0.5f; // centered in the panel
            Top.Set(offsetY, 0);
            Left.Set(offsetX, 0);
            SetPadding(0);
            this.index = index;

            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {



            if (index < BossBagUI.buttonItems.Count)
            {
                base.DrawSelf(spriteBatch);

                Item myItem = BossBagUI.buttonItems[index];
                if (myItem == null)
                {
                    return;
                }
                item = myItem.type;
                stack = myItem.stack;

                if (TextureAssets.Item[item].State == AssetState.NotLoaded) Main.Assets.Request<Texture2D>(TextureAssets.Item[item].Name, AssetRequestMode.AsyncLoad);
                Texture2D texture = TextureAssets.Item[item].Value;

                base.DrawSelf(spriteBatch);
                Rectangle rect = texture.Frame();
                if (item < Main.itemAnimations.Length && Main.itemAnimations[item] != null)
                {
                    rect = Main.itemAnimations[item].GetFrame(texture);
                }
                float drawScale = 1f;
                if (rect.Width > buttonWidth) drawScale = buttonWidth / (float)rect.Width;
                Vector2 origin = rect.Size() * drawScale;
                Vector2 position = GetInnerDimensions().Position() + rect.Size() * drawScale * drawScale + new Vector2(buttonWidth, buttonHeight) / 2f - origin / 2f;
                spriteBatch.Draw(texture, position, rect, Color.White, 0f, origin, drawScale, SpriteEffects.None, 1f);

                stackText.SetText(stack.ToString());
            }
            else
            {
                stackText.SetText("");
            }
            

            // tooltip //
            if (IsMouseHovering && index < BossBagUI.buttonItems.Count && BossBagUI.buttonItems[index] != null && Main.HoverItem != BossBagUI.buttonItems[index])
            {
                Main.HoverItem = BossBagUI.buttonItems[index].Clone();


                Main.hoverItemName = Main.HoverItem.Name;
            }

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }



        }



    }

    



}
