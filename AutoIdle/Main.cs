using UnityEngine;


namespace AutoIdle
{
    class Main : MonoBehaviour
    {
        public bool AutoLoadLayoutToggle;
		public bool KeepBossRushEnabledToggle;
		public int wait;
		public void Start()
        {
        }
        public void Update()
        {
			if (Input.GetKeyDown(KeyCode.U))
			{
				windowOpen = !windowOpen;
				Cursor.visible = windowOpen;
			}
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
			if (KeepBossRushEnabledToggle)
            {
				if (!UIManager.Instance.bossRushActive)
				{
					UIManager.Instance.clickedBossRushButton();
				}
			}
			if (/* GameManager.Instance.statCurrRunWavesFailed > 0 && */GameManager.Instance.currHighestWave >= GameManager.Instance.prestigeReqWave)
            {
				wait++;
				if (wait > 1000)
				{
                    UIManager.Instance.prestigeButtonDo(UIManager.Instance.mapSelect);
					wait = 0;
				}
			}
			if (GameManager.Instance.droneSwarmNextTime <= 0)
            {
                UIManager.Instance.droneSwarmPlay();
            }
			if (AutoLoadLayoutToggle)
            {
				if (TowerManager.Instance.usePlacements[0].thisTower == null)
				{
					LayoutPanel layoutPanel = UIManager.Instance.layoutsPanel.GetComponent<LayoutPanel>();
					layoutPanel.loadMapLayout(3);
				}
			}
			if (UIManager.Instance.ccHudButtonText.text == "Ready!")
            {
				UIManager.Instance.cargoCarrierPlay();

			}
		}

		public bool windowOpen = false;
		public Rect windowRect = new Rect(20, 20, 200, 100);
		public void OnGUI()
        {
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 150f, 50f), UIManager.Instance.ccHudButtonText.text);
			if (windowOpen)
			{
				windowRect = GUI.Window(0, windowRect, DoMyWindow, "Menu <3");
			}
		}

		private void DoMyWindow(int windowID)
		{
			AutoLoadLayoutToggle = GUI.Toggle(new Rect(10, 15, 200, 20), AutoLoadLayoutToggle, "Auto Load Layout");
			KeepBossRushEnabledToggle = GUI.Toggle(new Rect(10, 35, 200, 20), KeepBossRushEnabledToggle, "Keep Boss Rush Enabled");
			GUI.DragWindow(new Rect(0, 0, 10000, 200));
		}
	}
}