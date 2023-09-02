﻿using androLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using VacuumBags.Localization;

namespace VacuumBags.Items
{
	public abstract class VBModItem : AndroModItem
	{
		protected override Action<ModItem, string, string> AddLocalizationTooltipFunc => VacuumBagsLocalizationDataStaticMethods.AddLocalizationTooltip;
	}
}
