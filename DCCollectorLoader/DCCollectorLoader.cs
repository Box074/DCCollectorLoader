using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DCCollectorLoader
{
    public class DCCollectorLoader : Mod
    {
        public static AssetBundle ab = null;
        public static GameObject dc_scene = null;
        public override void Initialize()
        {
            ab = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(typeof(DCCollectorLoader).Assembly.Location)
                , "dccollector"));
            dc_scene = ab.LoadAsset<GameObject>("HKCollectorScene");
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode _)
        {
            if (arg0.name.StartsWith("GG_Collector"))
            {
                new GameObject().AddComponent<CollectorFinder>();
                var w = UnityEngine.Object.Instantiate(dc_scene);
                w.GetComponentInChildren<CollectorControl>().OnDeath += () =>
                {
                    BossSceneController.Instance.EndBossScene();
                };

            }
        }
    }
    class CollectorFinder : MonoBehaviour
    {

        float lt = 0;
        void Update()
        {
            if (Time.time - lt < 0.5f) return;
            lt = Time.time;
            foreach (var v in FindObjectsOfType<HealthManager>())
            {
                if (v.gameObject.name.StartsWith("Jar Collector"))
                {
                    Destroy(v.gameObject);
                }
                if (v.transform.gameObject.GetComponent<CollectorControl>())
                {
                    if (v.isDead && v.enabled)
                    {
                        BossSceneController.Instance.EndBossScene();
                        v.enabled = false;
                    }
                }
            }
        }
    }
}
