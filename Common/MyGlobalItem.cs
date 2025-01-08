using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using UltimateMining.SkillTree;
using Terraria.Utilities;
using Terraria.ModLoader.Core;

namespace RerollTreasureBags.Common
{
    internal class MyGlobalItem : GlobalItem
    {
        public static bool inBossBagMode = false;

        public override bool InstancePerEntity => true;
        public override void Load()
        {
            base.Load();
            On_Item.NewItem_Inner += On_Item_NewItem_Inner;
            On_ItemSlot.RightClick_ItemArray_int_int += On_ItemSlot_RightClick_ItemArray_int_int;
        }

        public override void Unload()
        {
            base.Unload();
            On_Item.NewItem_Inner -= On_Item_NewItem_Inner;
            On_ItemSlot.RightClick_ItemArray_int_int -= On_ItemSlot_RightClick_ItemArray_int_int;
            inBossBagMode = false;
        }

        
        private void On_ItemSlot_RightClick_ItemArray_int_int(On_ItemSlot.orig_RightClick_ItemArray_int_int orig, Item[] inv, int context, int slot)
        {
            bool config = ModContent.GetInstance<RerollTreasureBagsConfig>().rerollAllBagItems;
            bool isValidBagItem = ItemID.Sets.BossBag[inv[slot].type] || (config && ItemID.Sets.OpenableBag[inv[slot].type]);

            if (isValidBagItem && Main.mouseRight && Main.mouseRightRelease && BossBagUI.enabled)
            {
                inv[slot].stack++;
                BossBagUI.buttonItems.Clear();
                inBossBagMode = true;
                MyPlayer.lastBagUsed = inv[slot];//Main.item[Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_DropAsItem(), inv[slot].type)];
            }
            else if (!isValidBagItem || !BossBagUI.enabled)
            {
                inBossBagMode = false;
            }
            orig(inv, context, slot);
        }

        

        private int On_Item_NewItem_Inner(On_Item.orig_NewItem_Inner orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Width, int Height, Item itemToClone, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
        {
            if (source is EntitySource_ItemOpen && inBossBagMode && BossBagUI.enabled)
            {
                int num = 400;
                Main.timeItemSlotCannotBeReusedFor[num] = 0;
                Main.item[num] = new Item();
                Item item = Main.item[num];
                
                item.SetDefaults(Type);
                item.Prefix(pfix);
                item.stack = Stack;
                
                BossBagUI.buttonItems.Add(item);
                return num;
            }
            return orig(source, X, Y, Width, Height, itemToClone, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
        }

        

    }
}
