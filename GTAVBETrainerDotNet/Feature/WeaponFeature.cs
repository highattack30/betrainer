﻿//////////////////////////////////////////////
//   BE Trainer.NET for Grand Theft Auto V
//             by BE.Tenner
//      Copyright (c) BE Group 2015
//               Thanks to
//    ScriptHookV & ScriptHookVDotNet
//  Native Trainer & Enhanced Native Trainer
//////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAVBETrainerDotNet.Weapon;
using GTAVBETrainerDotNet.Menu;
using GTA;
using GTA.Native;

namespace GTAVBETrainerDotNet
{
    public static partial class Feature
    {
        /// <summary>
        /// Weapon features
        /// </summary>
        public static class Weapon
        {
            public static bool InfiniteAmmo = false;
            public static bool PermanentParachute = false;
            public static bool NoReload = false;
            public static bool FireAmmo = false;
            public static bool ExplosiveAmmo = false;
            public static bool ExplosiveMelee = false;
            public static bool VehicleRocket = false;

            private static int _vehicleRocketFrames = 0;
            private const int VEHICLE_ROCKET_FRAME_INTERVAL = 10;

            /// <summary>
            /// Sets infinite ammo
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void SetInfiniteAmmo(MenuItem sender)
            {
                InfiniteAmmo = sender.On;
                Config.DoAutoSave();
            }

            /// <summary>
            /// Sets permanent parachute
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void SetPermanentParachute(MenuItem sender)
            {
                PermanentParachute = sender.On;
                Config.DoAutoSave();
            }

            /// <summary>
            /// Sets no reload
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void SetNoReload(MenuItem sender)
            {
                NoReload = sender.On;
                Config.DoAutoSave();
            }

            /// <summary>
            /// Sets fire ammo
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void SetFireAmmo(MenuItem sender)
            {
                FireAmmo = sender.On;
                Config.DoAutoSave();
            }

            /// <summary>
            /// Sets explosive ammo
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void SetExplosiveAmmo(MenuItem sender)
            {
                ExplosiveAmmo = sender.On;
                Config.DoAutoSave();
            }

            /// <summary>
            /// Sets explosive melee
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void SetExplosiveMelee(MenuItem sender)
            {
                ExplosiveMelee = sender.On;
                Config.DoAutoSave();
            }

            /// <summary>
            /// Sets vehicle rocket
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void SetVehicleRocket(MenuItem sender)
            {
                VehicleRocket = sender.On;
                Config.DoAutoSave();
            }

            /// <summary>
            /// Gets all weapons
            /// </summary>
            /// <param name="sender">Source menu item</param>
            public static void GetAllWeapons(MenuItem sender)
            {

            }

            /// <summary>
            /// Updates weapon features
            /// </summary>
            public static void UpdateFeatures()
            {
                int player = Game.Player.Character.Handle;
                bool hasPlayer = Function.Call<bool>(Hash.DOES_ENTITY_EXIST, player);
                if (!hasPlayer) return;
                
                bool inVehicle = Game.Player.Character.IsInVehicle();
                int vehicle = Game.Player.Character.CurrentVehicle.Handle;

                // Fire ammo
                if (FireAmmo)
                {
                    Function.Call(Hash.SET_FIRE_AMMO_THIS_FRAME, player);
                }

                // Explosive ammo
                if (ExplosiveAmmo)
                {
                    Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, player);
                }

                // Explosive melee
                if (ExplosiveMelee)
                {
                    Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, player);
                }

                // Infinite ammo
                if (InfiniteAmmo)
                {
                    for (int i = 0; i < WeaponStorage.WEAPON_CATEGORY_COUNT; i++)
                    {
                        for (int j = 0; j < WeaponStorage.WEAPONS[i].Length; j++)
                        {
                            int weapon = Function.Call<int>(Hash.GET_HASH_KEY, WeaponStorage.WEAPONS[i][j].InternalValue);
                            if (Function.Call<bool>(Hash.IS_WEAPON_VALID, weapon) && Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, player, weapon, 0))
                            {
                                unsafe
                                {
                                    int ammo = 0;
                                    if (Function.Call<bool>(Hash.GET_MAX_AMMO, player, weapon, &ammo))
                                    {
                                        Function.Call(Hash.SET_PED_AMMO, player, weapon, ammo);
                                    }
                                }
                            }
                        }
                    }
                }

                // Permanent parachute
                if (PermanentParachute)
                {
                    int paraState = Function.Call<int>(Hash.GET_PED_PARACHUTE_STATE, player);

                    // Thanks to Enhanced Native Trainer, this is to prevent crashing the game
                    if (paraState == -1 || paraState == 3)
                    {
                        Function.Call(Hash.GIVE_DELAYED_WEAPON_TO_PED, player, WeaponStorage.PARACHUTE_HASH, 1, 0);
                    }
                }

                // No reload
                if (NoReload)
                {
                    unsafe
                    {
                        int current = 0;
                        Function.Call(Hash.GET_CURRENT_PED_WEAPON, player, &current, 1);
                        if (Function.Call<bool>(Hash.IS_WEAPON_VALID, current))
                        {
                            int maxAmmo = 0;
                            if (Function.Call<bool>(Hash.GET_MAX_AMMO, player, current, &maxAmmo))
                            {
                                Function.Call(Hash.SET_PED_AMMO, player, current, maxAmmo);
                                maxAmmo = Function.Call<int>(Hash.GET_MAX_AMMO_IN_CLIP, player, current, 1);
                                if (maxAmmo > 0) Function.Call(Hash.SET_AMMO_IN_CLIP, player, current, maxAmmo);
                            }
                        }
                    }
                }

                // Vehicle rocket
                if (VehicleRocket && inVehicle && Game.IsKeyPressed(Configuration.InputKey.VehicleRocket))
                {
                    if (Trainer.FrameCounter - _vehicleRocketFrames >= VEHICLE_ROCKET_FRAME_INTERVAL)
                    {
                        GTA.Math.Vector3 v0 = GTA.Math.Vector3.Zero, v1 = GTA.Math.Vector3.Zero;

                        unsafe
                        {
                            //Function.Call(Hash.GET_MODEL_DIMENSIONS, Function.Call<int>(Hash.GET_ENTITY_MODEL), &v0, &v1);//
                        }


                        _vehicleRocketFrames = Trainer.FrameCounter;
                    }
                }
            }

            /// <summary>
            /// Gets the current player's weapon set
            /// </summary>
            /// <returns></returns>
            public static unsafe WeaponSet GetCurrentWeaponSet()
            {
                WeaponSet weaponSet = new WeaponSet();
                List<SavedWeaponItem> items = new List<SavedWeaponItem>(WeaponStorage.TOTAL_WEAPONS_SLOTS);
                for (int i = 0; i < WeaponStorage.WEAPON_CATEGORY_COUNT; i++)
                {
                    for (int j = 0; j < WeaponStorage.WEAPONS[i].Length; j++)
                    {
                        WeaponData weapon = WeaponStorage.WEAPONS[i][j];
                        int weaponHash = Function.Call<int>(Hash.GET_HASH_KEY, weapon.InternalValue);
                        if (Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character.Handle, weaponHash, 0))
                        {
                            SavedWeaponItem item = new SavedWeaponItem();
                            item.Data = weapon;
                            item.Hash = weaponHash;
                            item.AmmoInWeapon = Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, Game.Player.Character.Handle, weaponHash);
                            int ammoInClip = 0;
                            Function.Call(Hash.GET_AMMO_IN_CLIP, Game.Player.Character.Handle, weaponHash, &ammoInClip);
                            item.AmmoInClip = ammoInClip;
                            if (weapon.HasTint)
                            {
                                item.HasTint = true;
                                item.Tint = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character.Handle, weaponHash);
                            }
                            if (weapon.AttachmentCount > 0)
                            {
                                item.HasAttachment = new bool[weapon.AttachmentCount];
                                for (int k = 0; k < weapon.AttachmentCount; k++)
                                {
                                    WeaponAttachmentData attachment = weapon.GetAttachmentData(k);
                                    int attachmentHash = Function.Call<int>(Hash.GET_HASH_KEY, attachment.InternalValue);
                                    item.HasAttachment[k] = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, Game.Player.Character.Handle, weaponHash, attachmentHash);
                                }
                            }
                            items.Add(item);
                        }
                    }
                }
                weaponSet.Weapons = items.ToArray();
                weaponSet.HasParachute = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character.Handle, WeaponStorage.PARACHUTE_HASH, 0);
                weaponSet.Armor = Function.Call<int>(Hash.GET_PED_ARMOUR, Game.Player.Character.Handle);

                return weaponSet;
            }

            /// <summary>
            /// Applies weapon set to player
            /// </summary>
            /// <param name="set">Weapon set</param>
            public static void ApplyWeaponSet(WeaponSet set)
            {
                foreach (SavedWeaponItem item in set.Weapons)
                {
                    Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character.Handle, item.Hash, 1000, 0, 0);
                    Function.Call(Hash.SET_PED_AMMO, Game.Player.Character.Handle, item.Hash, item.AmmoInWeapon);
                    Function.Call(Hash.SET_AMMO_IN_CLIP, Game.Player.Character.Handle, item.Hash, item.AmmoInClip);
                    if (item.HasTint)
                    {
                        Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character.Handle, item.Hash, item.Tint);
                    }
                    if (item.HasAttachment != null)
                    {
                        for (int i = 0; i < item.HasAttachment.Length; i++)
                        {
                            bool has = item.HasAttachment[i];
                            int hash = Function.Call<int>(Hash.GET_HASH_KEY, item.Data.GetAttachmentData(i).InternalValue);
                            if (has)
                            {
                                Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, Game.Player.Character.Handle, item.Hash, hash);
                            }
                            else
                            {
                                Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, Game.Player.Character.Handle, item.Hash, hash);
                            }
                        }
                    }
                }
                if (set.HasParachute)
                {
                    Function.Call(Hash.GIVE_DELAYED_WEAPON_TO_PED, Game.Player.Character.Handle, WeaponStorage.PARACHUTE_HASH, 1, 0);
                }
                Function.Call(Hash.SET_PED_ARMOUR, Game.Player.Character.Handle, set.Armor);
            }
        }
    }
}
