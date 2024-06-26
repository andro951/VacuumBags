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
using static Terraria.ID.ContentSamples.CreativeHelper;
using Terraria.GameContent.ItemDropRules;
using System;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace VacuumBags.Items
{
    [Autoload(false)]
	public  class SlayersSack : AllowedListBagModItem_VB {
		public static BagModItem Instance {
			get {
				if (instance == null)
					instance = new SlayersSack();

				return instance;
			}
		}
		private static BagModItem instance;
		public override string Texture => (GetType().Namespace + ".Sprites." + Name).Replace('.', '/');
		public override void SetDefaults() {
            Item.maxStack = 1;
            Item.value = 100000;
			Item.rare = ItemRarityID.Blue;
			Item.width = 26;
            Item.height = 32;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 1;
		}
		public override int GetBagType() => ModContent.ItemType<SlayersSack>();
		public override void AddRecipes() {
			if (!VacuumBags.serverConfig.HarderBagRecipes) {
				CreateRecipe()
				.AddIngredient(ItemID.Silk, 4)
				.AddIngredient(ItemID.Rope, 20)
				.Register();
			}
			else {
				CreateRecipe()
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.Rope, 50)
				.Register();
			}
		}
		public override Color PanelColor => new Color(167, 162, 164, androLib.Common.Configs.ConfigValues.UIAlpha);
		public override Color ScrollBarColor => new Color(65, 0, 0, androLib.Common.Configs.ConfigValues.UIAlpha);
		public override Color ButtonHoverColor => new Color(255, 248, 252, androLib.Common.Configs.ConfigValues.UIAlpha);

		public static Item ChooseRopeFromSack(Player player) => ChooseFromBag(Instance.BagStorageID, ItemSets.IsRope, player);
		public static Item ChooseTorchFromSack(Player player, Func<Item, bool> torchCondition) {
			Item heldItem = torchCondition(player.HeldItem) ? player.HeldItem : null;
			return ChooseFromBagOnlyIfFirstInInventory(
				heldItem, 
				player,
				Instance.BagStorageID,
				torchCondition
			);
		}
		internal static void OnTileInteractionsUse(ILContext il) {
			var c = new ILCursor(il);

			//IL_2532: ldloc.s 102
			//IL_2534: brtrue.s IL_2541

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchLdloc(100),
				i => i.MatchBrtrue(out _)
			)) { throw new Exception("Failed to find instructions OnGUIChatDrawInner 1/2"); }

			if (!c.TryGotoNext(MoveType.Before,
				i => i.MatchBrtrue(out _)
			)) { throw new Exception("Failed to find instructions OnGUIChatDrawInner 2/2"); }

			c.Emit(OpCodes.Ldloc, 100);
			c.Emit(OpCodes.Ldloc, 96);
			c.Emit(OpCodes.Ldloc, 97);
			c.Emit(OpCodes.Ldloc, 102);
			c.Emit(OpCodes.Ldarg_0);

			c.EmitDelegate((bool flag14, int num69, int num70, int num72, Player player) => {
				if (!flag14 || !Chest.IsLocked(num69, num70))
					return;

				Item[] inv = StorageManager.GetItems(Instance.BagStorageID);
				Item foundItem = null;
				for (int i = 0; i < inv.Length; i++) {
					if (inv[i].netID == num72) {
						foundItem = inv[i];
						break;
					}
				}

				if (foundItem == null || foundItem.stack <= 0 || !Chest.Unlock(num69, num70))
					return;

				bool consumable = num72 != ItemID.ShadowKey;
				if (consumable) {
					if (ItemLoader.ConsumeItem(foundItem, player))
						foundItem.stack--;

					if (foundItem.stack <= 0)
						foundItem.TurnToAir();
				}

				if (Main.netMode == NetmodeID.MultiplayerClient)
					NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, num69, num70);
			});
		}

		public override bool? DevCheck(ItemSetInfo info, SortedSet<ItemGroup> itemGroups, SortedSet<string> endWords, SortedSet<string> searchWords) {
			if (info.Rope || info.Torch || info.WaterTorch || info.Glowstick || info.FlairGun)
				return true;

			if (enemyDropItems.Contains(info.Type))
				return true;

			if (info.Equipment)
				return false;

			return null;
		}
		private static SortedSet<int> enemyDropItems = null;
		public override void PostSetup() {
			enemyDropItems = null;
		}
		public override SortedSet<int> DevWhiteList() {
			SortedSet<int> devWhiteList = new() {
				ItemID.Torch,
				ItemID.Lens,
				ItemID.RottenChunk,
				ItemID.WormTooth,
				ItemID.ShadowScale,
				ItemID.ShadowOrb,
				ItemID.Book,
				ItemID.Bone,
				ItemID.Stinger,
				ItemID.Vine,
				ItemID.WhoopieCushion,
				ItemID.BlackLens,
				ItemID.TopHat,
				ItemID.TuxedoShirt,
				ItemID.TuxedoPants,
				ItemID.BunnyHood,
				ItemID.PlumbersHat,
				ItemID.PlumbersShirt,
				ItemID.PlumbersPants,
				ItemID.ArchaeologistsHat,
				ItemID.ArchaeologistsJacket,
				ItemID.ArchaeologistsPants,
				ItemID.Leather,
				ItemID.RedHat,
				ItemID.Robe,
				ItemID.RobotHat,
				ItemID.Glowstick,
				ItemID.SharkFin,
				ItemID.Feather,
				ItemID.AntlionMandible,
				ItemID.GoldenKey,
				ItemID.ShadowKey,
				ItemID.TatteredCloth,
				ItemID.MiningHelmet,
				ItemID.MiningShirt,
				ItemID.MiningPants,
				ItemID.Hook,
				ItemID.PixieDust,
				ItemID.UnicornHorn,
				ItemID.DarkShard,
				ItemID.LightShard,
				ItemID.EskimoHood,
				ItemID.EskimoCoat,
				ItemID.EskimoPants,
				ItemID.MummyMask,
				ItemID.MummyShirt,
				ItemID.MummyPants,
				ItemID.PinkEskimoHood,
				ItemID.PinkEskimoCoat,
				ItemID.PinkEskimoPants,
				ItemID.RainHat,
				ItemID.RainCoat,
				ItemID.TempleKey,
				ItemID.BoneKey,
				ItemID.LizardEgg,
				ItemID.WispinaBottle,
				ItemID.UmbrellaHat,
				ItemID.Skull,
				ItemID.SailorHat,
				ItemID.EyePatch,
				ItemID.SailorShirt,
				ItemID.SailorPants,
				ItemID.EyeSpring,
				ItemID.ToySled,
				ItemID.TurtleShell,
				ItemID.TissueSample,
				ItemID.Vertebrae,
				ItemID.Ichor,
				ItemID.SWATHelmet,
				ItemID.GiantHarpyFeather,
				ItemID.BoneFeather,
				ItemID.FireFeather,
				ItemID.IceFeather,
				ItemID.BrokenBatWing,
				ItemID.TatteredBeeWing,
				ItemID.BrokenHeroSword,
				ItemID.ButterflyDust,
				ItemID.FartinaJar,
				ItemID.BlackFairyDust,
				ItemID.JackOLanternMask,
				ItemID.Present,
				ItemID.Coal,
				ItemID.BeetleHusk,
				ItemID.SpiderFang,
				ItemID.LunarTabletFragment,
				ItemID.MoonMask,
				ItemID.SunMask,
				ItemID.MartianCostumeMask,
				ItemID.MartianCostumeShirt,
				ItemID.MartianCostumePants,
				ItemID.MartianUniformHelmet,
				ItemID.MartianUniformTorso,
				ItemID.MartianUniformPants,
				ItemID.WhiteLunaticHood,
				ItemID.BlueLunaticHood,
				ItemID.WhiteLunaticRobe,
				ItemID.BlueLunaticRobe,
				ItemID.BlessedApple,
				ItemID.BuccaneerBandana,
				ItemID.BuccaneerShirt,
				ItemID.BuccaneerPants,
				ItemID.TheBrideHat,
				ItemID.TheBrideDress,
				ItemID.PedguinHat,
				ItemID.PedguinShirt,
				ItemID.PedguinPants,
				ItemID.AncientBattleArmorMaterial,
				ItemID.LamiaPants,
				ItemID.LamiaShirt,
				ItemID.LamiaHat,
				ItemID.AncientCloth,
				ItemID.CarbonGuitar,
				ItemID.DrManFlyMask,
				ItemID.DrManFlyLabCoat,
				ItemID.ButcherMask,
				ItemID.ButcherMask,
				ItemID.ButcherMask,
				ItemID.RockGolemHead,
				ItemID.WolfMountItem,
			};

			enemyDropItems = new();
			foreach (KeyValuePair<int, NPC> npcPair in ContentSamples.NpcsByNetId) {
				int netID = npcPair.Key;
				NPC npc = npcPair.Value;
				List<IItemDropRule> dropRules = Main.ItemDropsDB.GetRulesForNPCID(netID, false).ToList();
				if (npc.boss || npc.townNPC)
					continue;

				foreach (IItemDropRule dropRule in dropRules) {
					List<DropRateInfo> dropRates = new();
					DropRateInfoChainFeed dropRateInfoChainFeed = new(1f);
					dropRule.ReportDroprates(dropRates, dropRateInfoChainFeed);
					foreach (DropRateInfo dropRate in dropRates) {
						if (dropRate.itemId <= ItemID.None || dropRate.itemId >= ItemLoader.ItemCount)
							continue;

						//Vanilla item from modded enemy
						if (netID >= NPCID.Count && dropRate.itemId < ItemID.Count)
							continue;

						ItemSetInfo info = new(dropRate.itemId);
						if (info.Equipment || info.CreateWall || info.CreateTile || info.Coin)
							continue;

						enemyDropItems.Add(info.Type);
					}
				}
			}

			foreach (int itemType in RecipeGroup.recipeGroups[RecipeGroupID.Birds].ValidItems) {
				devWhiteList.Add(itemType);
			}

			foreach (int itemType in RecipeGroup.recipeGroups[RecipeGroupID.Ducks].ValidItems) {
				devWhiteList.Add(itemType);
			}

			foreach (int itemType in RecipeGroup.recipeGroups[RecipeGroupID.Squirrels].ValidItems) {
				devWhiteList.Add(itemType);
			}

			foreach (int itemType in RecipeGroup.recipeGroups[RecipeGroupID.Turtles].ValidItems) {
				devWhiteList.Add(itemType);
			}

			foreach (int itemType in RecipeGroup.recipeGroups[RecipeGroupID.Macaws].ValidItems) {
				devWhiteList.Add(itemType);
			}

			foreach (int itemType in RecipeGroup.recipeGroups[RecipeGroupID.Cockatiels].ValidItems) {
				devWhiteList.Add(itemType);
			}

			return devWhiteList;
		}
		public override SortedSet<int> DevBlackList() {
			SortedSet<int> devBlackList = new() {
				ItemID.TikiTorch,
				ItemID.Vine,
				ItemID.MagicMirror
			};

			return devBlackList;
		}
		public override SortedSet<ItemGroup> ItemGroups() {
			SortedSet<ItemGroup> itemGroups = new() {
				ItemGroup.GoodieBags,
				ItemGroup.Critters,
				ItemGroup.Keys
			};
			
			return itemGroups;
		}
		public override SortedSet<string> EndWords() {
			SortedSet<string> endWords = new() {
				"present"
			};

			return endWords;
		}
		public override SortedSet<string> SearchWords() {
			SortedSet<string> searchWords = new() {
				"soulof",
			};

			return searchWords;
		}

		#region AndroModItem attributes that you don't need.

		public override string SummaryOfFunction => "Rope, torches, glowsticks, flair guns, keys";
		public override string LocalizationDisplayName => "Slayer's Sack";
		public override string LocalizationTooltip =>
			$"Automatically stores materials dropped by enemies and other items found while adventuring such as torches, rope and keys.\n" +
			$"When in your inventory, the contents of the sack are available for crafting.\n" +
			$"Right click to open the sack.\n" +
			$"Rope can be placed from the sack by left clicking with the sack.  If any rope in the sack is favorited, only favorited rope will be used.\n" +
			$"Torches and glowsticks can be used from the bag when holding shift as if they were in your inventory.";
		public override string Artist => "anodomani";
		public override string ArtModifiedBy => "@kingjoshington";
		public override string Designer => "andro951";

		#endregion
	}
}
