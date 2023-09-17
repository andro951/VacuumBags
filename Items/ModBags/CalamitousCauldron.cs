﻿using androLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Default;

namespace VacuumBags.Items
{
	[Autoload(false)]
	public class CalamitousCauldron : ModBag {
		public override string ModDisplayNameTooltip => "Calamity";
		public override string LocalizationDisplayName => "Calamitous Cauldron";
		new public static int BagStorageID;
		public static SortedSet<int> Blacklist {
			get {
				if (blacklist == null) {
					blacklist = new() {

					};
				}

				return blacklist;
			}
		}
		private static SortedSet<int> blacklist = null;
		public static bool ItemAllowedToBeStored(Item item) => item?.ModItem != null && (item.ModItem is UnloadedItem unloadedItem ? unloadedItem.ModName : item.ModItem.Mod.Name) == AndroMod.calamityModName && !Blacklist.Contains(item.type);
		new public static Color PanelColor => new Color(45, 25, 33, androLib.Common.Configs.ConfigValues.UIAlpha);
		new public static Color ScrollBarColor => new Color(166, 32, 53, androLib.Common.Configs.ConfigValues.UIAlpha);
		new public static Color ButtonHoverColor => new Color(89, 50, 70, androLib.Common.Configs.ConfigValues.UIAlpha);
		public static void RegisterWithAndroLib(Mod mod) {
			BagStorageID = StorageManager.RegisterVacuumStorageClass(
				mod,//Mod
				typeof(CalamitousCauldron),//type 
				ItemAllowedToBeStored,//Is allowed function, Func<Item, bool>
				null,//Localization Key name.  Attempts to determine automatically by treating the type as a ModItem, or you can specify.
				200,//StorageSize
				true,//Can vacuum
				() => PanelColor, // Get color function. Func<using Microsoft.Xna.Framework.Color>
				() => ScrollBarColor, // Get Scroll bar color function. Func<using Microsoft.Xna.Framework.Color>
				() => ButtonHoverColor, // Get Button hover color function. Func<using Microsoft.Xna.Framework.Color>
				() => ModContent.ItemType<CalamitousCauldron>(),//Get ModItem type
				80,//UI Left
				675//UI Top
			);
		}
		public override void AddRecipes() {
			if (AndroMod.calamityEnabled) {
				if (AndroMod.calamityMod.TryFind("WulfrumMetalScrap", out ModItem wulfrumMetalcrap)
						&& AndroMod.calamityMod.TryFind("EnergyCore", out ModItem energyCore)
						&& AndroMod.calamityMod.TryFind("SeaPrism", out ModItem seaPrism)
						) {
					if (!VacuumBags.serverConfig.HarderBagRecipes) {
						CreateRecipe()
						.AddTile(TileID.WorkBenches)
						.AddIngredient(wulfrumMetalcrap.Type, 10)
						.AddIngredient(energyCore.Type, 2)
						.AddIngredient(seaPrism.Type, 10)
						.Register();
					}
					else {
						if (AndroMod.calamityMod.TryFind("SulphurousSand", out ModItem sulphurousSand)
							&& AndroMod.calamityMod.TryFind("Acidwood", out ModItem acidwood)
							&& AndroMod.calamityMod.TryFind("AerialiteOre", out ModItem aerialiteOre)
							) {
							CreateRecipe()
							.AddTile(TileID.WorkBenches)
							.AddIngredient(wulfrumMetalcrap.Type, 50)
							.AddIngredient(energyCore.Type, 5)
							.AddIngredient(seaPrism.Type, 50)
							.AddIngredient(sulphurousSand.Type, 50)
							.AddIngredient(acidwood.Type, 20)
							.AddIngredient(aerialiteOre.Type, 5)
							.Register();
						}
					}
				}
			}
		}
	}
}