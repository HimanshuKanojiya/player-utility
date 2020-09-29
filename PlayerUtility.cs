//Required reference files
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using System.Windows.Forms;
using System.Drawing;
using GTA.Native;
using NativeUI;
using NativeUI.PauseMenu;

namespace PlayerUtility
{
    //inheritance
    public class PlayerUtility : Script
    {
        //constructor
        MenuPool PlayerUtliPool;
        UIMenu plUiMenu;
        UIMenu weatherset;
        UIMenu weaponMenu;
        UIMenu bikesMenu;
        UIMenuItem IncreaseHealth, IncreaseArmor, IncreaseWantedLevel, ResetWantedLevel, SetWeather, weapoMenuItem,bikeMenuItem;
        
        //Weather Menu Item

        public PlayerUtility()
        {
            PlayerUtliPool = new MenuPool();
            PlayerMenuSetup();
            KeyUp += PlayerUtility_KeyUp;
            Tick += Player_tick;
            plUiMenu.OnItemSelect += OnitemSelectorEvent;
            weatherset.OnItemSelect += OnitemSelectorEvent;
            weaponMenu.OnItemSelect += WeaponMenu_OnItemSelect;
            bikesMenu.OnItemSelect += BikesMenu_OnItemSelect;
        }

        private void BikesMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (selectedItem.Text == "Akuma")
            {
                GetBikes(VehicleHash.Akuma);
            }
            else if (selectedItem.Text == "Avarus")
            {
                GetBikes(VehicleHash.Avarus);
            }
            else if (selectedItem.Text == "Enduro")
            {
                GetBikes(VehicleHash.Enduro);
            }
            else if (selectedItem.Text == "Vador")
            {
                GetBikes(VehicleHash.Vader);
            }
        }

        private void WeaponMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (selectedItem.Text == "ASSAULT GUNS")
            {
                GetGuns("Assault");
            }
            else if (selectedItem.Text == "Pistols")
            {
                GetGuns("Pistol");
            }
            else if (selectedItem.Text == "Snipers")
            {
                GetGuns("Snipers");
            }
            else if (selectedItem.Text == "Shotguns")
            {
                GetGuns("Shotguns");
            }
            else if (selectedItem.Text == "Reset Weapons")
            {
                GetGuns("No guns");
            }
            else if (selectedItem.Text == "Rifles")
            {
                GetGuns("Rifles");
            }
        }

        void OnitemSelectorEvent(UIMenu sender, UIMenuItem item, int index)
        {
            if (item == IncreaseHealth)
            {
                PlayerIncreaseHealth();
            }
            else if (item == IncreaseArmor)
            {
                EquipArmor();
            }
            else if (item == IncreaseWantedLevel)
            {
                GetWanted(5);
            }
            else if (item == ResetWantedLevel)
            {
                UI.ShowSubtitle(item.Text,15);
                GetWanted(0);
            }
            else if (item.Description.Contains("Weather"))
            {
                WeatherCall(item.Text);
            }
        }

        private void PlayerUtility_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.B)
            {
                plUiMenu.Visible = !plUiMenu.Visible;
            }
            
        }

        private void Player_tick(object sender, System.EventArgs e)
        {
            if (PlayerUtliPool != null)
            {
                PlayerUtliPool.ProcessMenus();
            }
        }

        void PlayerMenuSetup()
        {
            //Subitems on Parent Menu
            plUiMenu = new UIMenu("Player Utility","Created by Himanshu.anon");
            PlayerUtliPool.Add(plUiMenu);

            IncreaseHealth = new UIMenuItem("Increase Health", "Health to 100%");
            IncreaseArmor = new UIMenuItem("Equip Armor","GET ARMOR");
            IncreaseWantedLevel = new UIMenuItem("Become Most Wanted", "Police Department After You!");
            ResetWantedLevel = new UIMenuItem("Reset Wanted Level", "Free From Police");

            //Menu which has sub menu's
            SetWeather = new UIMenuItem("Set Weather","Choose Your Favorite Weather");
            weapoMenuItem = new UIMenuItem("Weapons", "Choose Your Weapons");
            bikeMenuItem = new UIMenuItem("Spawn Bike", "Bring Bike to Ride");


            plUiMenu.AddItem(IncreaseHealth);
            plUiMenu.AddItem(IncreaseArmor);
            plUiMenu.AddItem(IncreaseWantedLevel);
            plUiMenu.AddItem(ResetWantedLevel);
            plUiMenu.AddItem(SetWeather);
            plUiMenu.AddItem(weapoMenuItem);
            plUiMenu.AddItem(bikeMenuItem);

            //Weather Menu Begin
            weatherset = new UIMenu("Player Utility","SET WEATHER NOW");
            PlayerUtliPool.Add(weatherset);
            var weatherCollection = new string[]
            {
                "CLEAR", "EXTRASUNNY", "CLOUDS", "OVERCAST", "RAIN", "CLEARING", "THUNDER", "SMOG", "FOGGY", "XMAS",
                "SNOWLIGHT", "BLIZZARD"
            };
            foreach (string weather_name in weatherCollection)
            {
                weatherset.AddItem(new UIMenuItem(weather_name, "Set " + weather_name + " Weather"));
            }
            
            weatherset.RefreshIndex();
            //End of Weather Menu End

            //Weapon Menu
            weaponMenu = new UIMenu("Player Utility","Select Weapons");
            PlayerUtliPool.Add(weaponMenu);
            weaponMenu.AddItem(new UIMenuItem("Assault GUNS", "Get All Assault Guns"));
            weaponMenu.AddItem(new UIMenuItem("Pistols","Get All Pistols"));
            weaponMenu.AddItem(new UIMenuItem("Snipers", "Get All Sniper Guns"));
            weaponMenu.AddItem(new UIMenuItem("Shotguns", "Get All ShotGuns"));
            weaponMenu.AddItem(new UIMenuItem("Rifles", "Get All Rifles"));
            weaponMenu.AddItem(new UIMenuItem("Reset Weapons","Remove All Weapons"));
            weaponMenu.RefreshIndex();
            //End of Weapon Menu

            //Bike Menu
            bikesMenu = new UIMenu("Player Utility","Choose Your Bike");
            PlayerUtliPool.Add(bikesMenu);
            bikesMenu.AddItem(new UIMenuItem("Akuma"));
            bikesMenu.AddItem(new UIMenuItem("Daemon"));
            bikesMenu.AddItem(new UIMenuItem("Avarus"));
            bikesMenu.AddItem(new UIMenuItem("Enduro"));
            bikesMenu.AddItem(new UIMenuItem("Vador"));
            bikesMenu.RefreshIndex();
            //End of Bike Menu

            //Menu Binding with Parent Menu
            plUiMenu.BindMenuToItem(weatherset, SetWeather);
            plUiMenu.BindMenuToItem(weaponMenu,weapoMenuItem);
            plUiMenu.BindMenuToItem(bikesMenu,bikeMenuItem);
        }

        void PlayerIncreaseHealth()
        {
            Game.Player.Character.Health = 100;

        }

        void EquipArmor()
        {
            Game.Player.Character.Armor = 100;
        }

        void GetWanted(int wanted)
        {
            //This function is to handle wanted levels from 5 to 0
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL,Game.Player.GetHashCode(),wanted,true);
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW,Game.Player.GetHashCode());

            if (wanted > 0)
            {
                UI.ShowSubtitle("COPS ARE COMING READY, BE PREPARED",5);
            }
            else
            {
                UI.ShowSubtitle("NO MORE COPS, ENJOY!");
            }
                
        }

        void WeatherCall(string WeatherType)
        {
            //This function is to manage weather
            Function.Call(Hash.SET_WEATHER_TYPE_NOW, WeatherType);
            UI.ShowSubtitle($"{WeatherType} IS COMING!",5);
        }

        void GetGuns(string guntype)
        {
            //This method is to handle weapon functions
            if (guntype == "Assault")
            {
                Game.Player.Character.Weapons.Give(WeaponHash.AssaultSMG, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.AssaultRifle, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.AssaultShotgun, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.AssaultrifleMk2, 1000, true, true);
            }
            else if (guntype == "Pistol")
            {
                Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.Pistol50, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.PistolMk2, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.APPistol, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.CombatPistol, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.HeavyPistol, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.MachinePistol, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.MarksmanPistol, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.SNSPistol, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.SNSPistolMk2, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.VintagePistol, 1000, true, true);
            }
            else if (guntype == "Snipers")
            {
                Game.Player.Character.Weapons.Give(WeaponHash.SniperRifle, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.HeavySniper, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.HeavySniperMk2, 1000, true, true);
            }
            else if (guntype == "Shotguns")
            {
                Game.Player.Character.Weapons.Give(WeaponHash.AssaultShotgun, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.BullpupShotgun, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.HeavyShotgun, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.PumpShotgun, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.PumpShotgunMk2, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.SawnOffShotgun, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.SweeperShotgun, 1000, true, true);
            }
            else if (guntype == "No guns")
            {
                Game.Player.Character.Weapons.RemoveAll();
            }
            else if (guntype == "Rifles")
            {
                Game.Player.Character.Weapons.Give(WeaponHash.AdvancedRifle, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.BullpupRifle, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.CarbineRifle, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.BullpupRifleMk2, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.CarbineRifleMk2, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.CompactRifle, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.MarksmanRifle, 1000, true, true);
                Game.Player.Character.Weapons.Give(WeaponHash.MarksmanRifleMk2, 1000, true, true);
            }
        }

        void GetBikes(Model Bikes_Name)
        {
            var bike = World.CreateVehicle(Bikes_Name,
                Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 5, 0)));
        }
    }

}
