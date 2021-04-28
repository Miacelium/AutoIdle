using UnityEngine;

namespace AutoIdle
{
    public class Loader
    {
        public static void Init()
        {
            _Load = new GameObject();
            _Load.AddComponent<Main>();
            GameObject.DontDestroyOnLoad(_Load);
        }
        public static void Unload()
        {
            GameObject.Destroy(_Load);
        }
        private static GameObject _Load;
    }
}