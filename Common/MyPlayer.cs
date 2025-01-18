using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using UltimateMining.SkillTree;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RerollTreasureBags.Common
{
    internal class MyPlayer : ModPlayer
    {

        public static Item lastBagUsed;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BossBagUIKeybinds.ShowUIKeybind.JustPressed)
            {
                BossBagUI.enabled = !BossBagUI.enabled;
                if (!BossBagUI.enabled)
                {
                    MyGlobalItem.inBossBagMode = false;

                    int inventorySlot = Player.FindItem(lastBagUsed.netID);
                    if (lastBagUsed != null && inventorySlot >= 0 && inventorySlot <= 49 && Player.inventory[Player.FindItem(lastBagUsed.netID)] != null &&
                        BossBagUI.buttonItems.Count > 0)
                    {   
                        foreach (Item item in BossBagUI.buttonItems)
                        {
                            Item newItem = new Item();
                            newItem.SetDefaults(item.type);
                            newItem.type = item.type;
                            newItem.stack = item.stack;
                            newItem.Prefix(item.prefix);
                            int index = Item.NewItem(Player.GetSource_ItemUse(lastBagUsed), Player.Center, newItem);
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f);
                            }
                        }
                        Item bag = Player.inventory[Player.FindItem(lastBagUsed.netID)];
                        bag.stack--;
                        if (bag.stack == 0) bag.SetDefaults();
                    }
                    
                }
                BossBagUI.buttonItems.Clear();

            }
        }

    }
}
