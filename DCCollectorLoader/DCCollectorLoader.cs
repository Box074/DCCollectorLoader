﻿using System;
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
        public override string GetVersion()
        {
            return "1.0.0";
        }
        public override void Initialize()
        {
            ab = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(typeof(DCCollectorLoader).Assembly.Location)
                , "dccollector"));
            dc_scene = ab.LoadAsset<GameObject>("HKCollectorScene");
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            if (arg1.name.StartsWith("GG_Collector"))
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
                if (v.transform.root.gameObject.GetComponent<CollectorControl>())
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
