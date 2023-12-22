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
using VacuumBags.Items;

namespace VacuumBags.Tiles
{
    public class PackOrange : BagOrange {
		protected override BagModItem ModBag => Items.PackOrange.Instance;
	}
}
