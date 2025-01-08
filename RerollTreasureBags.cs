using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace RerollTreasureBags
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class RerollTreasureBags : Mod
	{

	}

	public class RerollTreasureBagsConfig : ModConfig
	{
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Configuration")]
        [Label("Allow Rerolling All Bag Items")]
        [Tooltip("Enables reroll functionality for ALL openable bag items, such as Crates, Presents, Goodie Bags, and Oysters.\nThis is very overpowered and not recommended for normal play.")]
        [DefaultValue(false)]
        public bool rerollAllBagItems;
    }
}
