using UnityEngine;
using System;
using System.Reflection;


namespace AutoIdle
{
	class Main : MonoBehaviour
	{
        public Rect windowRect = new Rect(20, 20, 300, 400);
		public string[] toolbarStrings = new string[] { "Layout 1", "Layout 2", "Layout 3" };
		// Information Variables
		public int prestigeCounter = 0;
		public string nextFlyingChestReward;
        public int flyingChestClickedCounter = 0;

        // Setting Variables
        public bool AutoLoadLayoutToggle;
		public int SelectedLayout;
        public bool KeepBossRushEnabledToggle;
		public bool AutoPrestigeToggle;
		public bool FastPrestigeToggle;
		public bool AutoDroneSwarm;

        // Help Variables
        public float WaitAfterPrestige;
		//public float WaitForAutoLayout;
		public int currentReward = GameManager.Instance.videoAdRotation;
		public int mainWidth = 300;
		public void Start()
		{
            GetNextFlyingChestReward();
		}
		public void Update()
		{
			ClickFlyingChest();
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
            if (KeepBossRushEnabledToggle)
            {
                if (!UIManager.Instance.bossRushActive)
                {
                    UIManager.Instance.clickedBossRushButton();
                }
            }
			if (AutoPrestigeToggle)
            {
				CheckPrestige();
			}
			CheckDroneSwarm();
            if (UIManager.Instance.ccHudButtonText.text == "Ready!" && GameManager.Instance.mCargoKillsLevel < 3)
            {
                UIManager.Instance.cargoCarrierPlay();
            }
			if (AutoLoadLayoutToggle)
			{
				if (TowerManager.Instance.usePlacements[0].thisTower == null)
				{
					LayoutPanel layoutPanel = UIManager.Instance.layoutsPanel.GetComponent<LayoutPanel>();
					layoutPanel.loadMapLayout(0);
				}
			}
		}

        public bool windowOpen = false;
		public void OnGUI()
		{
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 150f, 50f), "!!!INJECTED!!!");
			if (windowOpen)
			{
				windowRect = GUI.Window(0, windowRect, DoMyWindow, "Menu <3");
			}
		}

		private void DoMyWindow(int windowID)
		{
			Rect settings = new Rect(10, 10, windowRect.width - 20, 200);
            AutoLoadLayoutToggle = GUI.Toggle(new Rect(settings.x, settings.y + 10, windowRect.width - 20, 20), AutoLoadLayoutToggle, "Auto Load Layout");
			SelectedLayout = GUI.Toolbar(new Rect(settings.x, settings.y + 30, windowRect.width - 20, 20), SelectedLayout, toolbarStrings);
            KeepBossRushEnabledToggle = GUI.Toggle(new Rect(settings.x, settings.y + 50, windowRect.width - 20, 20), KeepBossRushEnabledToggle, "Keep Boss Rush Enabled");
			AutoPrestigeToggle = GUI.Toggle(new Rect(settings.x, settings.y + 70, windowRect.width - 20, 20), AutoPrestigeToggle, "Enable Auto Prestige");
			FastPrestigeToggle = GUI.Toggle(new Rect(settings.x, settings.y + 90, windowRect.width - 20, 20), FastPrestigeToggle, "Enable Fast Prestige");
			AutoDroneSwarm = GUI.Toggle(new Rect(settings.x, settings.y + 110, windowRect.width - 20, 20), AutoDroneSwarm, "Auto Drone Swarm: ");
            Rect information = new Rect(10, 140, windowRect.width - 20, 200);
			GUI.Label(new Rect(information.x, information.y, windowRect.width - 20, 25), "Information:");
            GUI.Label(new Rect(information.x, information.y + 20, windowRect.width - 20, 25), "Flying Chest Clicked: " + flyingChestClickedCounter.ToString());
            GUI.Label(new Rect(information.x, information.y + 40, windowRect.width - 20, 25), "Next Chest Reward: " + nextFlyingChestReward);
            GUI.Label(new Rect(information.x, information.y + 60, windowRect.width - 20, 25), "Prestiges Done: " + prestigeCounter.ToString());
			GUI.Label(new Rect(information.x, information.y + 100, windowRect.width - 20, 25), GetSize().ToString());
			/*if (GUI.Button(new Rect(10, 350, 180, 30), "Nothing"))
			{
				
			}*/
			GUI.DragWindow(new Rect(0, 0, 10000, 200));
		}
		public void CheckDroneSwarm()
        {
			if (AutoDroneSwarm)
			{
				if (GameManager.Instance.droneSwarmNextTime <= 0)
				{
					UIManager.Instance.droneSwarmPlay();
				}
			}
		}
        public void LevelUpHighestDPS()
        {
            Tower thisTower = TowerManager.Instance.usePlacements[0].thisTower;
            if (thisTower != null)
            {
                // Going to find the tower with the highest DPS and upgrade it

            }
        }
        public void ClickFlyingChest()
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
		public void CheckPrestige()
        {
			if (GameManager.Instance.currHighestWave >= GameManager.Instance.prestigeReqWave)
			{
				if (FastPrestigeToggle)
				{
                    WaitAfterPrestige -= Time.deltaTime;
					if (WaitAfterPrestige <= 0.0f)
					{
                        UIManager.Instance.prestigeButtonDo(UIManager.Instance.mapSelect);
						prestigeCounter++;
                        WaitAfterPrestige = 6.0f;
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
                windowRect.Set(20, 20, GetSize() + 20, 400);
				currentReward = GameManager.Instance.videoAdRotation;
			}

		}
        public int GetSize()
        {
            GUIContent content = new GUIContent("Next Chest Reward: " + nextFlyingChestReward);
            GUIStyle style = GUI.skin.box;
            Vector2 size = style.CalcSize(content);

            return Convert.ToInt32(size.x);
        }
		public void AutoLayout(int option)
        {
			int i = option;
			LayoutPanel layoutPanel = UIManager.Instance.layoutsPanel.GetComponent<LayoutPanel>();
			layoutPanel.loadMapLayout(i);
		}
	}
}