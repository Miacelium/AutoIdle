using System;
using System.Reflection;
using UnityEngine;
 
namespace AutoIdle
{
    class Main : MonoBehaviour
    {
        // GUI stuff
        public Rect windowRect = new Rect(20, 20, 300, 700);
        private Vector2 scrollViewVector = Vector2.zero;
        public static string[] TierSelection = { "Tier1", "Tier2", "Tier3", "Tier4", "Tier5", "Tier6", "Tier7", };
        bool show = false;

        // Information Variables
        public int prestigeCounter = 0;
        public string nextFlyingChestReward;
        public int flyingChestClickedCounter = 0;

        // Setting Variables
        public bool windowOpen = false;
        public bool AutoLoadLayoutToggle = true;
        public int SelectedLayout = 1;
        public bool KeepBossRushEnabledToggle = true;
        public bool AutoPrestigeToggle = true;
        public bool FastPrestigeToggle = false;
        public bool AutoDroneSwarmToggle = true;
        public bool AutoBuyMonsterToggle = true;
        public int SelectedTier = 3;
        public bool BuyOnlyNeededToggle = true;
        public bool AutoClickFlyingChest = true;
        public bool AutoLevelTwoTowersToggle = true;
        public bool AutoCargoShipToggle = true;

        // Info Variables
        public float WaitAfterPrestige;
        public int currentReward = GameManager.Instance.videoAdRotation;
        public int LevelCounter = 0;
        public double currHighestDps;

        public void Start()
        {
            GetNextFlyingChestReward();
        }

        public void Update()
        {
            
            Cursor.visible = true;
            if (Input.GetKeyDown(KeyCode.U))
            {
                windowOpen = !windowOpen;
                Cursor.visible = true;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Cursor.visible = true;
                Loader.Unload();
            }
            // Good stuff
            ClickFlyingChest();
            CheckPrestige();
            KeepBossRushEnabled();
            CheckDroneSwarm();
            CheckAutoBuy();
            AutoLayout();
            LevelUpHighestDPS();
            
        }

        public void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            if (windowOpen)
            {
                windowRect = GUI.Window(0, windowRect, DoMyWindow, "Menu <3");
            }
        }

        private void DoMyWindow(int windowID)
        {
            Rect settings = new Rect(10, 10, windowRect.width - 20, 200);
            AutoLoadLayoutToggle = GUI.Toggle(new Rect(settings.x, settings.y + 10, windowRect.width - 20, 20), AutoLoadLayoutToggle, "Auto Load Layout: " + SelectedLayout);
            if (GUI.Button(new Rect(settings.x, settings.y + 30, (windowRect.width - 20) / 3, 20), "Layout 1"))
            {
                SelectedLayout = 1;
                LayoutPanel layoutPanel = UIManager.Instance.layoutsPanel.GetComponent<LayoutPanel>();
                layoutPanel.loadMapLayout(SelectedLayout);
            }
            if (GUI.Button(new Rect(settings.x + (windowRect.width - 20) / 3, settings.y + 30, (windowRect.width - 20) / 3, 20), "Layout 2"))
            {
                SelectedLayout = 2;
                LayoutPanel layoutPanel = UIManager.Instance.layoutsPanel.GetComponent<LayoutPanel>();
                layoutPanel.loadMapLayout(SelectedLayout);
            }
            if (GUI.Button(new Rect(settings.x + 2 * (windowRect.width - 20) / 3, settings.y + 30, (windowRect.width - 20) / 3, 20), "Layout 3"))
            {
                SelectedLayout = 3;
                LayoutPanel layoutPanel = UIManager.Instance.layoutsPanel.GetComponent<LayoutPanel>();
                layoutPanel.loadMapLayout(SelectedLayout);
            }
            KeepBossRushEnabledToggle = GUI.Toggle(new Rect(settings.x, settings.y + 50, windowRect.width - 20, 20), KeepBossRushEnabledToggle, "Keep Boss Rush Enabled");
            AutoPrestigeToggle = GUI.Toggle(new Rect(settings.x, settings.y + 70, windowRect.width - 20, 20), AutoPrestigeToggle, "Enable Auto Prestige");
            FastPrestigeToggle = GUI.Toggle(new Rect(settings.x, settings.y + 90, windowRect.width - 20, 20), FastPrestigeToggle, "Enable Fast Prestige");
            AutoDroneSwarmToggle = GUI.Toggle(new Rect(settings.x, settings.y + 110, windowRect.width - 20, 20), AutoDroneSwarmToggle, "Auto Drone Swarm");
            AutoBuyMonsterToggle = GUI.Toggle(new Rect(settings.x, settings.y + 130, windowRect.width - 20, 20), AutoBuyMonsterToggle, "Auto Buy Monster");
            Rect dropDownRect = new Rect(settings.x + 100, settings.y + 150, 125, 300);
            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y, dropDownRect.width, 25), ""))
            {
                if (!show)
                {
                    show = true;
                }
                else
                {
                    show = false;
                }
            }
            if (show)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect((dropDownRect.x - 100), (dropDownRect.y + 25), dropDownRect.width, dropDownRect.height), scrollViewVector, new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (TierSelection.Length * 25))));

                GUI.Box(new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (TierSelection.Length * 25))), "");

                for (int index = 0; index < TierSelection.Length; index++)
                {

                    if (GUI.Button(new Rect(0, (index * 25), dropDownRect.height, 25), ""))
                    {
                        show = false;
                        SelectedTier = index;
                    }

                    GUI.Label(new Rect(5, (index * 25), dropDownRect.height, 25), TierSelection[index]);

                }

                GUI.EndScrollView();
            }
            else
            {
                GUI.Label(new Rect((dropDownRect.x - 95), dropDownRect.y, 300, 25), TierSelection[SelectedTier]);
            }
            AutoClickFlyingChest = GUI.Toggle(new Rect(settings.x, settings.y + 195, windowRect.width - 20, 20), AutoClickFlyingChest, "Auto Click Flying Chest");
            AutoLevelTwoTowersToggle = GUI.Toggle(new Rect(settings.x, settings.y + 215, windowRect.width - 20, 20), AutoLevelTwoTowersToggle, "Auto Level Two Towers");
            AutoCargoShipToggle = GUI.Toggle(new Rect(settings.x, settings.y + 235, windowRect.width - 20, 20), AutoCargoShipToggle, "Auto Cargo Ship");
            Rect information = new Rect(10, 270, windowRect.width - 20, 300);
            GUI.Label(new Rect(information.x, information.y, windowRect.width - 20, 25), "Information:");
            GUI.Label(new Rect(information.x, information.y + 20, windowRect.width - 20, 25), "Flying Chest Clicked: " + flyingChestClickedCounter.ToString());
            GUI.Label(new Rect(information.x, information.y + 40, windowRect.width - 20, 25), "Next Chest Reward: " + nextFlyingChestReward);
            GUI.Label(new Rect(information.x, information.y + 60, windowRect.width - 20, 25), "Prestiges Done: " + prestigeCounter.ToString());
            GUI.Label(new Rect(information.x, information.y + 80, windowRect.width - 20, 25), "Times leveled: " + LevelCounter);

            if (GUI.Button(new Rect(information.x, information.y + 165, windowRect.width - 20, 25), "Do it!"))
            {
                double num = GameManager.Instance.baseEnemyHp;
                num *= (double)(1f - (float)GameManager.Instance.resLvlEnemyHp * 0.02f);
                num *= (double)(1f - (float)GameManager.Instance.artEnemyHp * 0.05f);
                num *= (double)(1f - (float)GameManager.Instance.relicEnemyHp * 0.02f);
                num *= (double)(1f - (float)GameManager.Instance.tournEnemyHp * 0.02f);
                num = Math.Round(num, 0);
                double num2 = currHighestDps;
                if (num * 2 > num2)
                {
                    
                }
            }
            GUI.DragWindow(new Rect(0, 0, 10000, 200));
        }

        public void CheckDroneSwarm()
        {
            if (AutoDroneSwarmToggle)
            {
                if (GameManager.Instance.droneSwarmNextTime <= 0)
                {
                    UIManager.Instance.droneSwarmPlay();
                }
            }
        }

        public void LevelUpHighestDPS()
        {
            if (AutoLevelTwoTowersToggle)
            {
                double highestDps = 0;
                Tower highestDpsTower = TowerManager.Instance.usePlacements[0].thisTower;
                Tower secondTower = TowerManager.Instance.usePlacements[0].thisTower;
                for (int i = 0; i < TowerManager.Instance.usePlacements.Count; i++)
                {
                    Tower thisTower = TowerManager.Instance.usePlacements[i].thisTower;

                    if (thisTower.getDps(true, false, thisTower.baseDps, thisTower.milestoneBonus) > highestDps)
                    {
                        highestDps = thisTower.getDps(true, false, thisTower.baseDps, thisTower.milestoneBonus);
                        highestDpsTower = thisTower;
                        currHighestDps = highestDps;
                    }
                }
                highestDps = 0;
                for (int i = 0; i < TowerManager.Instance.usePlacements.Count; i++)
                {
                    Tower thisTower = TowerManager.Instance.usePlacements[i].thisTower;
                    if (thisTower.getDps(true, false, thisTower.baseDps, thisTower.milestoneBonus) > highestDps && thisTower.subClass.element != highestDpsTower.subClass.element)
                    {
                        highestDps = thisTower.getDps(true, false, thisTower.baseDps, thisTower.milestoneBonus);
                        secondTower = thisTower;
                    }
                }
                if (secondTower.levelCost < GameManager.Instance.resGoldObs)
                {
                    TowerManager.Instance.selectedTower = secondTower;
                    TowerManager.Instance.selectedTowerPanel.SetActive(true);
                    TowerUpgradePanel towerUpgradePanel = TowerManager.Instance.selectedTowerPanel.GetComponent<TowerUpgradePanel>();
                    towerUpgradePanel.clickedLevelButton();
                    TowerManager.Instance.selectedTowerPanel.SetActive(false);
                    TowerManager.Instance.selectedTower = null;
                    LevelCounter++;
                }
                if (highestDpsTower.levelCost < GameManager.Instance.resGoldObs)
                {
                    TowerManager.Instance.selectedTower = highestDpsTower;
                    TowerManager.Instance.selectedTowerPanel.SetActive(true);
                    TowerUpgradePanel towerUpgradePanel = TowerManager.Instance.selectedTowerPanel.GetComponent<TowerUpgradePanel>();
                    towerUpgradePanel.clickedLevelButton();
                    TowerManager.Instance.selectedTowerPanel.SetActive(false);
                    TowerManager.Instance.selectedTower = null;
                    LevelCounter++;
                }
            }
        }

        public void ClickFlyingChest()
        {
            if (AutoClickFlyingChest)
            {
                FlyingChest flyingChest = (FlyingChest)FindObjectOfType(typeof(FlyingChest));
                if (flyingChest != null)
                {
                    UIManager.Instance.videoAdHudButtonClicked();
                    flyingChest.GetType().GetMethod("initObj", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(flyingChest, null);
                    flyingChestClickedCounter++;
                    UIManager.Instance.videoAdPanelMain.SetActive(false);
                    GetNextFlyingChestReward();
                }
            }

        }

        public void CheckPrestige()
        {
            if (AutoPrestigeToggle && GameManager.Instance.currHighestWave >= GameManager.Instance.prestigeReqWave)
            {
                if (FastPrestigeToggle)
                {
                    double num = GameManager.Instance.baseEnemyHp;
                    num *= (double)(1f - (float)GameManager.Instance.resLvlEnemyHp * 0.02f);
                    num *= (double)(1f - (float)GameManager.Instance.artEnemyHp * 0.05f);
                    num *= (double)(1f - (float)GameManager.Instance.relicEnemyHp * 0.02f);
                    num *= (double)(1f - (float)GameManager.Instance.tournEnemyHp * 0.02f);
                    num = Math.Round(num, 0);
                    double num2 = currHighestDps;
                    if (num * 2 > num2 || GameManager.Instance.statCurrRunWavesFailed > 0)
                    {
                        WaitAfterPrestige -= Time.deltaTime;
                        if (WaitAfterPrestige <= 0.0f)
                        {
                            UIManager.Instance.prestigeButtonDo(UIManager.Instance.mapSelect);
                            prestigeCounter++;
                            WaitAfterPrestige = 6.0f;
                        }
                    }

                }
                else if (GameManager.Instance.mPrestigesLevel < 3)
                {
                    WaitAfterPrestige -= Time.deltaTime;
                    if (WaitAfterPrestige <= 0.0f)
                    {
                        UIManager.Instance.prestigeButtonDo(UIManager.Instance.mapSelect);
                        prestigeCounter++;
                        WaitAfterPrestige = 6.0f;
                    }
                }
                else if (GameManager.Instance.statCurrRunWavesFailed > 0)
                {
                    WaitAfterPrestige -= Time.deltaTime;
                    if (WaitAfterPrestige <= 0.0f)
                    {
                        UIManager.Instance.prestigeButtonDo(UIManager.Instance.mapSelect);
                        prestigeCounter++;
                        WaitAfterPrestige = 6.0f;
                    }
                }
                
            }
        }

        public void GetNextFlyingChestReward()
        {
            if (currentReward != GameManager.Instance.videoAdRotation)
            {
                int rotation = GameManager.Instance.videoAdRotation;
                if (rotation == 2 && (GameManager.Instance.iap2xGameSpeed || GameManager.Instance.temp2xGameSpeed > 0f))
                {
                    rotation++;
                }
                switch (rotation)
                {
                    case 0:
                        nextFlyingChestReward = "2x Damage";
                        break;
                    case 1:
                        nextFlyingChestReward = "2x Gold Drop";
                        break;
                    case 2:
                        nextFlyingChestReward = "2x Game Speed";
                        break;
                    case 3:
                        nextFlyingChestReward = "2x Kill Exp";
                        break;
                    case 4:
                        nextFlyingChestReward = "Gold";
                        break;
                    case 5:
                        nextFlyingChestReward = "Essence";
                        break;
                    case 6:
                        nextFlyingChestReward = "Gems";
                        break;

                }
                currentReward = GameManager.Instance.videoAdRotation;
            }

        }

        public void AutoLayout()
        {
            if (AutoLoadLayoutToggle && TowerManager.Instance.usePlacements[0].thisTower == null)
            {
                LayoutPanel layoutPanel = UIManager.Instance.layoutsPanel.GetComponent<LayoutPanel>();
                layoutPanel.loadMapLayout(SelectedLayout);
            }
        }

        public void CheckAutoBuy()
        {
            if (AutoBuyMonsterToggle)
            {
                if (BuyOnlyNeededToggle)
                {
                    if (GameManager.Instance.mMultiCardsValue < 100)
                    {
                        BuyMonster();
                    }
                    return;
                }
                else
                {
                    BuyMonster();
                }
            }
        }

        public void BuyMonster()
        {
            MonstersNewPanel monstersNewPanel = UIManager.Instance.monstersNewPanel.GetComponent<MonstersNewPanel>();
            int amount;
            switch (SelectedTier)
            {
                case 0:
                    amount = GameManager.Instance.resEssObs / 10;
                    if (BuyOnlyNeededToggle)
                    {
                        if (amount > 100 - GameManager.Instance.mMultiCardsValue)
                        {
                            amount = 100 - GameManager.Instance.mMultiCardsValue;
                        }
                    }
                    if (amount > 0)
                    {
                        Reflection.SetField(monstersNewPanel, "CNFCHEFDGGM", 10 * amount);
                        Reflection.SetField(monstersNewPanel, "JLHCNIAKJFK", amount);
                        if (GameManager.Instance.achHighestWave >= 0)
                        {
                            monstersNewPanel.buyMonster(1);
                        }
                    }
                    break;
                case 1:
                    amount = GameManager.Instance.resEssObs / 15;
                    if (BuyOnlyNeededToggle)
                    {
                        if (amount > 100 - GameManager.Instance.mMultiCardsValue)
                        {
                            amount = 100 - GameManager.Instance.mMultiCardsValue;
                        }
                    }
                    if (amount > 0)
                    {
                        Reflection.SetField(monstersNewPanel, "JJLHDEGJPDH", 15 * amount);
                        Reflection.SetField(monstersNewPanel, "JLHCNIAKJFK", amount);
                        if (GameManager.Instance.achHighestWave >= 40)
                        {
                            monstersNewPanel.buyMonster(2);
                        }
                    }
                    break;
                case 2:
                    amount = GameManager.Instance.resEssObs / 25;
                    if (BuyOnlyNeededToggle)
                    {
                        if (amount > 100 - GameManager.Instance.mMultiCardsValue)
                        {
                            amount = 100 - GameManager.Instance.mMultiCardsValue;
                        }
                    }
                    if (amount > 0)
                    {
                        Reflection.SetField(monstersNewPanel, "AKBJOBIKFOI", 25 * amount);
                        Reflection.SetField(monstersNewPanel, "JLHCNIAKJFK", amount);
                        if (GameManager.Instance.achHighestWave >= 120)
                        {
                            monstersNewPanel.buyMonster(3);
                        }
                    }
                    break;
                case 3:
                    amount = GameManager.Instance.resEssObs / 40;
                    if (BuyOnlyNeededToggle)
                    {
                        if (amount > 100 - GameManager.Instance.mMultiCardsValue)
                        {
                            amount = 100 - GameManager.Instance.mMultiCardsValue;
                        }
                    }
                    if (amount > 0)
                    {
                        Reflection.SetField(monstersNewPanel, "GKLHPKIKMEO", 40 * amount);
                        Reflection.SetField(monstersNewPanel, "JLHCNIAKJFK", amount);
                        if (GameManager.Instance.achHighestWave >= 250)
                        {
                            monstersNewPanel.buyMonster(4);
                        }
                    }
                    break;
                case 4:
                    amount = GameManager.Instance.resEssObs / 70;
                    if (BuyOnlyNeededToggle)
                    {
                        if (amount > 100 - GameManager.Instance.mMultiCardsValue)
                        {
                            amount = 100 - GameManager.Instance.mMultiCardsValue;
                        }
                    }
                    if (amount > 0)
                    {
                        Reflection.SetField(monstersNewPanel, "NIKFNINBCJC", 70 * amount);
                        Reflection.SetField(monstersNewPanel, "JLHCNIAKJFK", amount);
                        if (GameManager.Instance.achHighestWave >= 500)
                        {
                            monstersNewPanel.buyMonster(5);
                        }
                    }
                    break;
                case 5:
                    amount = GameManager.Instance.resEssObs / 150;
                    if (BuyOnlyNeededToggle)
                    {
                        if (amount > 100 - GameManager.Instance.mMultiCardsValue)
                        {
                            amount = 100 - GameManager.Instance.mMultiCardsValue;
                        }
                    }
                    if (amount > 0)
                    {
                        Reflection.SetField(monstersNewPanel, "MKOOAPHPNJJ", 125 * amount);
                        Reflection.SetField(monstersNewPanel, "JLHCNIAKJFK", amount);
                        if (GameManager.Instance.achHighestWave >= 800)
                        {
                            monstersNewPanel.buyMonster(6);
                        }
                    }
                    break;
                case 6:
                    amount = GameManager.Instance.resEssObs / 250;
                    if (BuyOnlyNeededToggle)
                    {
                        if (amount > 100 - GameManager.Instance.mMultiCardsValue)
                        {
                            amount = 100 - GameManager.Instance.mMultiCardsValue;
                        }
                    }
                    if (amount > 0)
                    {
                        Reflection.SetField(monstersNewPanel, "FHLFLCEGENP", 250 * amount);
                        Reflection.SetField(monstersNewPanel, "JLHCNIAKJFK", amount);
                        if (GameManager.Instance.achHighestWave >= 1500)
                        {
                            monstersNewPanel.buyMonster(7);
                        }
                    }
                    break;

            }
            monstersNewPanel.newCardPanel.gameObject.SetActive(false);
            monstersNewPanel.newCardMultiPanel.gameObject.SetActive(false);
        }

        public void KeepBossRushEnabled()
        {
            if (KeepBossRushEnabledToggle && !UIManager.Instance.bossRushActive)
            {
                UIManager.Instance.clickedBossRushButton();
            }
        }

        public void AutoCargoShip()
        {
            if (AutoCargoShipToggle && UIManager.Instance.ccHudButtonText.text == "Ready!" && GameManager.Instance.mCargoKillsLevel < 3)
            {
                UIManager.Instance.cargoCarrierPlay();
            }
        }

    }
}