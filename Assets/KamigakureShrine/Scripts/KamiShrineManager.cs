using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Persistence;
using VRC.SDKBase;

namespace KamigakureShrine
{
    /// <summary>
    /// Central idle-game manager for Kamigakure Shrine.
    /// Progress is per-player (local) and saved with VRChat Persistence (PlayerData).
    ///
    /// Performance notes (Udon):
    ///  - No Update(). A 1 s tick via SendCustomEventDelayedSeconds.
    ///  - Production rate is cached; recomputed only when levels/buffs change.
    ///  - Kami data lives in parallel arrays here; statues only forward Interact with an index.
    ///  - Animator parameters are written only when the value changes.
    ///  - Public methods are entry points only, '_'-prefixed so they cannot be fired as network events.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class KamiShrineManager : UdonSharpBehaviour
    {
        // ---------- Lesser kami (all arrays same length) ----------
        [Header("Lesser kami (parallel arrays)")]
        [SerializeField] private string[] kamiNames;
        [Tooltip("0 Earth, 1 Water, 2 Fire, 3 Wind, 4 Void")]
        [SerializeField] private int[] kamiDomain;
        [Tooltip("Reiryoku per second per level")]
        [SerializeField] private double[] baseRate;
        [Tooltip("Cost of the first level")]
        [SerializeField] private double[] baseCost;
        [SerializeField] private Animator[] kamiAnimators;
        [Tooltip("First-awakening line per kami (see STORY.md). Empty entry → generic \"{name} awakens!\"")]
        [SerializeField] private string[] kamiAwakenLines;

        // ---------- Greater kami (length 5, one per domain) ----------
        [Header("Greater kami (one per domain)")]
        [SerializeField] private string[] domainNames = { "地 Earth", "水 Water", "火 Fire", "風 Wind", "空 Void" };
        [Tooltip("Shown on first manifestation. Empty entry → generic \"The greater kami of {domain} manifests!\"")]
        [SerializeField] private string[] greaterManifestLines =
        {
            "山守 The Mountain Guardian: “The path is open. Let them climb.”",
            "川龍 The River Dragon: “Clean. Finally. You may look at me.”",
            "狐王 The Fox Lord: “Nine tails. Nine lanterns. I remember my duty now.”",
            "嵐天狗 The Storm Tengu: “…You re-hung the bell. Then I will ring it.”",
            "天鏡 The Celestial Mirror clears… and the patron steps out."
        };
        [SerializeField] private Animator[] greaterAnimators;
        [SerializeField] private int greaterThreshold1 = 25;
        [SerializeField] private int greaterThreshold2 = 50;
        [SerializeField] private int greaterThreshold3 = 100;

        // ---------- Tuning ----------
        [Header("Tuning")]
        [SerializeField] private double costGrowth = 1.15;
        [SerializeField] private double startingReiryoku = 10.0;
        [SerializeField] private double offlineEfficiency = 0.5;
        [SerializeField] private double offlineCapSeconds = 8.0 * 3600.0;
        [SerializeField] private double faithPerMultiplier = 0.1;
        [SerializeField] private double faithDivisor = 1e6;
        [SerializeField] private float bellDuration = 30f;
        [SerializeField] private float bellCooldown = 120f;
        [SerializeField] private double bellMultiplier = 2.0;
        [Tooltip("Kohaku's remark when the bell is rung. Empty → \"The kami stir\"")]
        [SerializeField] private string bellLine = "Kohaku: “Not so loud. …Again, though.”";
        [SerializeField] private float omikujiDuration = 600f;
        [SerializeField] private float saveInterval = 30f;

        // ---------- UI ----------
        [Header("UI (TextMeshPro, font needs kanji glyphs)")]
        [SerializeField] private TextMeshProUGUI reiryokuText;
        [SerializeField] private TextMeshProUGUI rateText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI faithText;

        // ---------- Persistence keys ----------
        private const string KEY_REIRYOKU = "ks_reiryoku";
        private const string KEY_EARNED = "ks_earned";
        private const string KEY_FAITH = "ks_faith";
        private const string KEY_LEVELS = "ks_levels";
        private const string KEY_TIME = "ks_utcTicks";

        // ---------- Runtime state ----------
        private int[] levels;
        private int[] lastAwakeTier;
        private int[] greaterTier;       // 0 none, 1..3 milestones
        private double reiryoku;
        private double earnedThisRun;
        private int faith;

        private double cachedRate;
        private double[] domainMult;

        private float blessingEndTime = -1f;
        private float bellReadyTime;
        private float omikujiEndTime = -1f;
        private double omikujiMult = 1.0;
        private bool omikujiDrawnThisSession;
        private bool blessingWasActive;

        private bool loaded;
        private bool dirty;
        private float saveTimer;
        private float lastTickTime;

        private VRCPlayerApi localPlayer;

        private readonly string[] JpUnits = { "", "万", "億", "兆", "京", "垓", "秭", "穣", "溝", "澗", "正", "載", "極" };

        // ======================================================
        // Lifecycle
        // ======================================================

        private void Start()
        {
            localPlayer = Networking.LocalPlayer;

            int n = kamiNames.Length;
            levels = new int[n];
            lastAwakeTier = new int[n];
            for (int i = 0; i < n; i++) lastAwakeTier[i] = -1;
            greaterTier = new int[5];
            domainMult = new double[5];
            reiryoku = startingReiryoku;

            SetStatus("Restoring shrine…");
        }

        public override void OnPlayerRestored(VRCPlayerApi player)
        {
            if (player == null || !player.isLocal) return;
            LoadFromPlayerData(player);
            loaded = true;

            RecomputeRate();
            UpdateAllVisuals();
            ApplyOfflineProgress(player);

            lastTickTime = Time.realtimeSinceStartup;
            SendCustomEventDelayedSeconds(nameof(_Tick), 1f);
        }

        // ======================================================
        // Tick (1 s)
        // ======================================================

        public void _Tick()
        {
            float now = Time.realtimeSinceStartup;
            float dt = now - lastTickTime;
            lastTickTime = now;

            // Buff expiry → recompute rate once, only on change.
            bool blessingActive = now < blessingEndTime;
            if (blessingWasActive && !blessingActive) RecomputeRate();
            blessingWasActive = blessingActive;

            if (omikujiEndTime > 0f && now >= omikujiEndTime)
            {
                omikujiEndTime = -1f;
                omikujiMult = 1.0;
                RecomputeRate();
            }

            double gain = cachedRate * dt;
            reiryoku += gain;
            earnedThisRun += gain;
            if (gain > 0.0) dirty = true;

            UpdateCounterUI();

            saveTimer += dt;
            if (dirty && saveTimer >= saveInterval) Save();

            SendCustomEventDelayedSeconds(nameof(_Tick), 1f);
        }

        // ======================================================
        // Player actions (entry points)
        // ======================================================

        public void _OnStatueInteract(int index)
        {
            if (!loaded || index < 0 || index >= levels.Length) return;

            double cost = GetCost(index);
            if (reiryoku < cost)
            {
                SetStatus(kamiNames[index] + " needs " + FormatJp(cost) + " 霊力");
                return;
            }

            reiryoku -= cost;
            levels[index]++;
            dirty = true;

            UpdateKamiVisual(index);
            CheckGreaterKami(kamiDomain[index]);
            RecomputeRate();
            UpdateCounterUI();

            if (levels[index] == 1)
            {
                string line = GetLine(kamiAwakenLines, index);
                SetStatus(line != null ? kamiNames[index] + ": " + line : kamiNames[index] + " awakens!");
            }
            else
            {
                SetStatus(kamiNames[index] + " Lv " + levels[index] + "  (next " + FormatJp(GetCost(index)) + ")");
            }
        }

        public void _RingBell()
        {
            if (!loaded) return;
            float now = Time.realtimeSinceStartup;
            if (now < bellReadyTime)
            {
                SetStatus("The bell still hums… " + Mathf.CeilToInt(bellReadyTime - now) + "s");
                return;
            }
            blessingEndTime = now + bellDuration;
            bellReadyTime = now + bellCooldown;
            blessingWasActive = true;
            RecomputeRate();
            string flavor = (bellLine != null && bellLine.Length > 0) ? bellLine : "The kami stir";
            SetStatus("鈴 " + flavor + " — blessing ×" + bellMultiplier.ToString("0.#") + " for " + (int)bellDuration + "s");
        }

        public void _DrawOmikuji()
        {
            if (!loaded) return;
            if (omikujiDrawnThisSession)
            {
                SetStatus("You have already drawn your fortune this visit.");
                return;
            }
            omikujiDrawnThisSession = true;

            float roll = UnityEngine.Random.value;
            string fortune;
            if (roll < 0.10f) { fortune = "大吉 Great Blessing"; omikujiMult = 3.0; }
            else if (roll < 0.35f) { fortune = "中吉 Middle Blessing"; omikujiMult = 2.0; }
            else if (roll < 0.65f) { fortune = "小吉 Small Blessing"; omikujiMult = 1.5; }
            else if (roll < 0.90f) { fortune = "吉 Blessing"; omikujiMult = 1.2; }
            else { fortune = "凶 Misfortune — tie it to the rack to leave it behind"; omikujiMult = 0.8; }

            omikujiEndTime = Time.realtimeSinceStartup + omikujiDuration;
            RecomputeRate();
            SetStatus("おみくじ: " + fortune);
        }

        public void _Purify()
        {
            if (!loaded) return;
            int gainedFaith = (int)Math.Floor(Math.Sqrt(earnedThisRun / faithDivisor));
            if (gainedFaith < 1)
            {
                SetStatus("The shrine is not yet ready for 祓 purification.");
                return;
            }

            faith += gainedFaith;
            reiryoku = startingReiryoku;
            earnedThisRun = 0.0;
            for (int i = 0; i < levels.Length; i++) levels[i] = 0;
            for (int d = 0; d < 5; d++) greaterTier[d] = 0;

            RecomputeRate();
            UpdateAllVisuals();
            dirty = true;
            Save();
            SetStatus("祓 Purified. +" + gainedFaith + " 信仰 faith");
        }

        // ======================================================
        // Economy
        // ======================================================

        private double GetCost(int index)
        {
            return baseCost[index] * Math.Pow(costGrowth, levels[index]);
        }

        private void RecomputeRate()
        {
            for (int d = 0; d < 5; d++)
                domainMult[d] = Math.Pow(2.0, greaterTier[d]);

            double faithMult = 1.0 + faithPerMultiplier * faith;
            double buffMult = omikujiMult;
            if (Time.realtimeSinceStartup < blessingEndTime) buffMult *= bellMultiplier;

            double total = 0.0;
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] == 0) continue;
                total += baseRate[i] * levels[i] * domainMult[kamiDomain[i]];
            }
            cachedRate = total * faithMult * buffMult;
        }

        private void CheckGreaterKami(int domain)
        {
            int minLevel = int.MaxValue;
            bool any = false;
            for (int i = 0; i < levels.Length; i++)
            {
                if (kamiDomain[i] != domain) continue;
                any = true;
                if (levels[i] < minLevel) minLevel = levels[i];
            }
            if (!any) return;

            int tier = 0;
            if (minLevel >= greaterThreshold3) tier = 3;
            else if (minLevel >= greaterThreshold2) tier = 2;
            else if (minLevel >= greaterThreshold1) tier = 1;

            if (tier == greaterTier[domain]) return;
            bool firstManifest = greaterTier[domain] == 0 && tier > 0;
            greaterTier[domain] = tier;
            UpdateGreaterVisual(domain);

            if (firstManifest)
            {
                string line = GetLine(greaterManifestLines, domain);
                SetStatus(line != null ? line : "The greater kami of " + domainNames[domain] + " manifests!");
            }
        }

        private void ApplyOfflineProgress(VRCPlayerApi player)
        {
            long savedTicks;
            if (!PlayerData.TryGetLong(player, KEY_TIME, out savedTicks) || savedTicks <= 0) return;

            double seconds = (DateTime.UtcNow.Ticks - savedTicks) / (double)TimeSpan.TicksPerSecond;
            if (seconds <= 1.0) return;
            if (seconds > offlineCapSeconds) seconds = offlineCapSeconds;

            double gain = cachedRate * seconds * offlineEfficiency;
            if (gain <= 0.0) return;

            reiryoku += gain;
            earnedThisRun += gain;
            dirty = true;
            SetStatus("While you were away, the kami gathered " + FormatJp(gain) + " 霊力");
        }

        // ======================================================
        // Persistence
        // ======================================================

        private void Save()
        {
            if (!loaded) return;
            saveTimer = 0f;
            dirty = false;

            // Levels packed as 4 bytes each (one PlayerData write instead of N).
            byte[] packed = new byte[levels.Length * 4];
            for (int i = 0; i < levels.Length; i++)
            {
                int v = levels[i];
                int o = i * 4;
                packed[o] = (byte)(v & 0xFF);
                packed[o + 1] = (byte)((v >> 8) & 0xFF);
                packed[o + 2] = (byte)((v >> 16) & 0xFF);
                packed[o + 3] = (byte)((v >> 24) & 0xFF);
            }

            PlayerData.SetDouble(KEY_REIRYOKU, reiryoku);
            PlayerData.SetDouble(KEY_EARNED, earnedThisRun);
            PlayerData.SetInt(KEY_FAITH, faith);
            PlayerData.SetBytes(KEY_LEVELS, packed);
            PlayerData.SetLong(KEY_TIME, DateTime.UtcNow.Ticks);
        }

        private void LoadFromPlayerData(VRCPlayerApi player)
        {
            double d;
            if (PlayerData.TryGetDouble(player, KEY_REIRYOKU, out d)) reiryoku = d;
            if (PlayerData.TryGetDouble(player, KEY_EARNED, out d)) earnedThisRun = d;

            int f;
            if (PlayerData.TryGetInt(player, KEY_FAITH, out f)) faith = f;

            byte[] packed;
            if (PlayerData.TryGetBytes(player, KEY_LEVELS, out packed) && packed != null)
            {
                int count = Mathf.Min(levels.Length, packed.Length / 4);
                for (int i = 0; i < count; i++)
                {
                    int o = i * 4;
                    levels[i] = packed[o] | (packed[o + 1] << 8) | (packed[o + 2] << 16) | (packed[o + 3] << 24);
                }
            }

            for (int dmn = 0; dmn < 5; dmn++) CheckGreaterKami(dmn);
        }

        // ======================================================
        // Visuals / UI
        // ======================================================

        private void UpdateAllVisuals()
        {
            for (int i = 0; i < levels.Length; i++) UpdateKamiVisual(i);
            for (int d = 0; d < 5; d++) UpdateGreaterVisual(d);
            UpdateCounterUI();
            if (faithText != null) faithText.text = "信仰 " + faith + "  (×" + (1.0 + faithPerMultiplier * faith).ToString("0.0#") + ")";
        }

        private void UpdateKamiVisual(int i)
        {
            int lv = levels[i];
            int tier = lv <= 0 ? 0 : lv < 10 ? 1 : lv < 25 ? 2 : lv < 50 ? 3 : 4;
            if (tier == lastAwakeTier[i]) return;
            lastAwakeTier[i] = tier;
            if (kamiAnimators != null && i < kamiAnimators.Length && kamiAnimators[i] != null)
                kamiAnimators[i].SetInteger("AwakeTier", tier);
        }

        private void UpdateGreaterVisual(int d)
        {
            if (greaterAnimators == null || d >= greaterAnimators.Length || greaterAnimators[d] == null) return;
            greaterAnimators[d].SetBool("Manifested", greaterTier[d] > 0);
        }

        private void UpdateCounterUI()
        {
            if (reiryokuText != null) reiryokuText.text = FormatJp(reiryoku) + " 霊力";
            if (rateText != null) rateText.text = "+" + FormatJp(cachedRate) + " /s";
        }

        private void SetStatus(string msg)
        {
            if (statusText != null) statusText.text = msg;
        }

        /// <summary>Returns lines[i], or null when the array is missing/short or the entry is empty.</summary>
        private string GetLine(string[] lines, int i)
        {
            if (lines == null || i < 0 || i >= lines.Length) return null;
            string s = lines[i];
            return (s == null || s.Length == 0) ? null : s;
        }

        /// <summary>Formats with Japanese myriad units: 1.23万, 4.56億, 7.89兆 …</summary>
        private string FormatJp(double v)
        {
            if (v < 10000.0) return v < 100.0 ? v.ToString("0.#") : Math.Floor(v).ToString("0");

            int unit = 0;
            while (v >= 10000.0 && unit < JpUnits.Length - 1)
            {
                v /= 10000.0;
                unit++;
            }
            return v.ToString("0.##") + JpUnits[unit];
        }
    }
}
