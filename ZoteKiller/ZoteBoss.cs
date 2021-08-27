﻿using System.Collections;
using UnityEngine;
using ModCommon.Util;

namespace ZoteKiller
{
    public class ZoteBoss : MonoBehaviour
    {
        public static ZoteKillerMod ZoteKillerMod => (ZoteKillerMod)Modding.ModHooks.GetMod("ZoteKillerMod");
        void Awake()
        {
            DamageHero dh = GetComponent<DamageHero>();
            if (dh != null) Destroy(dh);
            foreach (var v in GetComponentsInChildren<DamageHero>()) Destroy(v);

            HealthManager hm = GetComponent<HealthManager>();
            hm.IsInvincible = false;
            hm.isDead = false;
            hm.OnDeath += Hm_OnDeath;
        }

        private void Hm_OnDeath()
        {
            ZoteData zoteData = ZoteKillerMod.zoteData;
            GameObject czo = Instantiate(ZoteKillerMod.zoteCor);
            czo.transform.parent = transform;
            czo.transform.position = transform.position;
            czo.SetActive(true);
            czo.LocateMyFSM("Control").InsertMethod("End", 0, () =>
            {
                for (int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
                GameObject d = Instantiate(ZoteKillerMod.zoteDead);
                d.transform.position = transform.position;
                d.SetActive(true);

                zoteData.KilledZote = true;
                zoteData.DeadScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                zoteData.DeadX = d.transform.position.x;
                zoteData.DeadY = d.transform.position.y;

                if (PlayerData.instance.brettaRescued && PlayerData.instance.zoteDefeated)
                {
                    ZoteKillerMod.zoteData.GrilFan = true;
                }
                else
                {
                    PlayerData.instance.zoteDefeated = false;
                    PlayerData.instance.zoteDead = true;
                }
                GameObject dead = Instantiate(ZoteKillerMod.zoteDead);
                dead.transform.position = transform.position;
                dead.SetActive(true);

                Destroy(gameObject);
            });
        }
    }
}