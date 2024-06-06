﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using androLib;
using VacuumBags.Items;
using androLib.Items;

namespace VacuumBags.Tiles
{
    public class BagWhite : SimpleBagTile
    {
		protected override IBagModItem ModBag => Items.BagWhite.Instance;
	}
}
