using BepInEx;
using BepInEx.Configuration;
using System;
using System.Diagnostics;
using UnityEngine;

namespace BepInEx.ForceQuit {
    [BepInPlugin("eu.haruka.bepinex.forcequit", "Unity Force Quit", "1.0")]
    public class Plugin : BaseUnityPlugin {

        private ConfigEntry<bool> Enable;
        private ConfigEntry<MethodEnum> Method;
        private bool triggered;

        private void Awake() {
            Enable = Config.Bind("General", "Enable", true, "Kill the game process when the game is closed");
            Method = Config.Bind("General", "Method", MethodEnum.ProcessKill, "Method that should be used to kill the game process");
            Logger.LogInfo("Plugin is loaded!");
        }

        public void OnApplicationQuit() {
            if (Enable.Value) {
                if (triggered) {
                    return;
                }
                triggered = true;
                Logger.LogInfo("Force quitting");
                if (Method.Value == MethodEnum.EnvironmentExit) {
                    Environment.Exit(0);
                } else if (Method.Value == MethodEnum.ProcessKill) {
                    Process.GetCurrentProcess().Kill();
                } else if (Method.Value == MethodEnum.ExternalTaskKill) {
                    string args = "/F /IM " + Process.GetCurrentProcess().ProcessName + ".exe";
                    Logger.LogDebug("Running taskkill " + args);
                    Process.Start("taskkill", args);
                }
            }
        }


    }

    internal enum MethodEnum {
        EnvironmentExit,
        ProcessKill,
        ExternalTaskKill
    }
}
