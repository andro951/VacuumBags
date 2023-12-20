﻿using androLib.Common.Utility;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework;
using androLib.Items;
using androLib.Common.Globals;
using androLib;
using System;

namespace VacuumBags.Items
{
	[Autoload(false)]
	public class BagBlack : SimpleBag {
		new public static int BagStorageID;
		public override int MyTileType => ModContent.TileType<Tiles.BagBlack>();
		public static void CloseBag() => StorageManager.CloseBag(BagStorageID);
		new public static SortedSet<int> Blacklist {
			get {
				if (blacklist == null) {
					blacklist = new() {
						ModContent.ItemType<BagBlack>(),
						ModContent.ItemType<PackBlack>(),
					};

					blacklist.UnionWith(StorageManager.GetPlayerBlackListSortedSet(BagStorageID));
				}

				return blacklist;
			}
		}
		private static SortedSet<int> blacklist = null;
		public static SortedSet<int> VacuumWhitelist = new();
		private static bool CanVacuumItem(Item item) => VacuumWhitelist.Contains(item.type);
		private static void UpdateAllowedList(int item, bool add) {
			if (add) {
				VacuumWhitelist.Add(item);
				Blacklist.Remove(item);
			}
			else {
				VacuumWhitelist.Remove(item);
				Blacklist.Add(item);
			}
		}

		public static bool ItemAllowedToBeStored(Item item) => !Blacklist.Contains(item.type);
		new public static Color PanelColor => new Color(20, 20, 20, androLib.Common.Configs.ConfigValues.UIAlpha);
		new public static void RegisterWithAndroLib(Mod mod) {
			if (Main.netMode == NetmodeID.Server)
				return;

			BagStorageID = StorageManager.RegisterVacuumStorageClass(
				mod,//Mod
				typeof(BagBlack),//type 
				ItemAllowedToBeStored,//Is allowed function, Func<Item, bool>
				null,//Localization Key name.  Attempts to determine automatically by treating the type as a ModItem, or you can specify.
				BagSize,//StorageSize
				IsVacuumBag,//Can vacuum
				() => PanelColor, // Get color function. Func<using Microsoft.Xna.Framework.Color>
				() => new Color(30, 30, 30, androLib.Common.Configs.ConfigValues.UIAlpha), // Get Scroll bar color function. Func<using Microsoft.Xna.Framework.Color>
				() => new Color(40, 40, 40, androLib.Common.Configs.ConfigValues.UIAlpha), // Get Button hover color function. Func<using Microsoft.Xna.Framework.Color>
				() => ModContent.ItemType<BagBlack>(),//Get ModItem type
				80,//UI Left
				675,//UI Top
				UpdateAllowedList,
				false,
				canVacuumItem: CanVacuumItem
			);
		}
		public override void AddRecipes() {
			if (!VacuumBags.serverConfig.HarderBagRecipes) {
				CreateRecipe()
				.AddTile(TileID.WorkBenches)
				.AddIngredient(ItemID.Silk, 2)
				.AddIngredient(ItemID.BlackInk, 1)
				.AddIngredient(ItemID.WhiteString, 1)
				.Register();
			}
			else {
				CreateRecipe()
				.AddTile(TileID.WorkBenches)
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.BlackInk, 1)
				.AddIngredient(ItemID.BlackString, 1)
				.AddIngredient(ItemID.CoffeeCup, 1)
				.AddIngredient(ItemID.TopHat, 1)
				.AddIngredient(ItemID.TuxedoShirt, 1)
				.AddIngredient(ItemID.TuxedoPants, 1)
				.AddIngredient(ItemID.BlackPearl, 1)
				.Register();
			}
		}
	}
}