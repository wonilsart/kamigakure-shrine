# Kamigakure Shrine (神隠れ神社)

An idle / incremental game for a VRChat Japanese shrine world. Small stone statues hold sleeping kami.
Offerings awaken them, awakened kami generate **Reiryoku (霊力 — spiritual mana)**, and enough
devotion to lesser kami calls down a **greater kami** that empowers its whole domain.

Story, characters, and lore: see **[STORY.md](STORY.md)**.

Target: Unity **2022.3.22f1**, VRChat SDK3 Worlds (with Persistence), UdonSharp.

---

## Core loop

1. Spend Reiryoku on offerings to awaken / level **lesser kami** (statues).
2. Lesser kami produce Reiryoku passively — per tick and while offline.
3. When every lesser kami in a domain reaches the threshold level, that domain's **greater kami** manifests (domain multiplier).
4. **Harae (祓 — Purification)** resets the shrine for **Shinkō (信仰 — faith)**, a permanent multiplier.

## Domains — Godai (五大)

| Domain | Lesser kami (statues) | Greater kami | Visual flair |
|---|---|---|---|
| Earth 地 | Jizō, tanuki, stone frog | Mountain Guardian | Moss spreads, stone lanterns ignite |
| Water 水 | Kappa, koi, rain toad | River Dragon | Pond fills, mist rolls in |
| Fire 火 | Kitsune, ember komainu | Fox Lord (tails unlock one by one) | Foxfire wisps orbit |
| Wind 風 | Tengu mask, crane, bell spirit | Storm Tengu | Shimenawa streamers sway, petals drift |
| Void 空 | Moon rabbit, mirror, lantern spirit | Celestial Mirror | Starfield appears at night |

## Numbers

- **Production:** `baseRate × level × domainMult × faithMult × blessingMult`
- **Cost:** `baseCost × 1.15^level`
- **Greater kami:** ×2 to its domain when all lesser kami in the domain reach level 25; ×2 more at level 50 and 100.
- **Faith on purify:** `floor(sqrt(totalEarnedThisRun / 1e6))`; faith multiplier `1 + 0.1 × faith`.
- **Offline:** 50% efficiency, capped at 8 hours.
- **Display:** Japanese large-number units — 万 億 兆 京 垓 秭 穣 溝 澗 正 載 極. (Use a TMP font asset with kanji glyphs, e.g. Noto Sans JP.)

## Mystical / ritual mechanics

- **Suzu bell** — ring for a 30 s ×2 blessing (cooldown). *Implemented.*
- **Omikuji (おみくじ)** — one fortune per session: 大吉 … 凶, each a timed multiplier. *Implemented.*
- **Kashiwade (柏手)** — clap twice at the haiden for an active-income burst (hand collider or Interact). *Planned.*
- **Ema board** — long-term goals that grant rewards. *Planned.*
- **Day / night & seasons** — Void and Fire stronger at night; seasonal kami. *Planned.*

## Social layer (planned)

- Personal progress is **local + Persistence (PlayerData)** — no sync needed.
- **Communal prayer:** one small synced counter; when several players clap together everyone gets a *Matsuri* multiplier.
- **Festival nights** when enough players are present.

---

## Udon performance design

- **No `Update()`.** Ticks once per second via `SendCustomEventDelayedSeconds`; each tick is one multiply-add.
- **Rate is cached** and recomputed only when something changes (purchase, buff start/end).
- **Offline catch-up** is one calculation from a saved UTC timestamp — no loops.
- **All kami data in parallel arrays** inside one manager behaviour. Statues hold only an index and forward `Interact` (rare event → cross-behaviour call is fine).
- **Visuals are Animators / particle systems.** Udon only sets an int (`AwakeTier`) / bool (`Manifested`) when the value changes.
- **Saves are throttled** (every 30 s while dirty) to respect PlayerData rate limits.
- Public methods are limited to event entry points and are `_`-prefixed so they can't be triggered as network events.

## Scripts

| File | Purpose |
|---|---|
| `KamiShrineManager.cs` | Economy, tick, offline progress, greater kami, blessings, omikuji, purification, save/load, UI |
| `KamiStatue.cs` | Put on each lesser kami statue; Interact → buy a level |
| `ShrineInteractable.cs` | Generic Interact → calls a named event on the manager (bell, omikuji, purify) |

## Scene setup

1. Add an empty `ShrineManager` object with **KamiShrineManager**. Fill the per-kami arrays (same length):
   `kamiNames`, `kamiDomain` (0–4), `baseRate`, `baseCost`, `kamiAnimators`, `kamiAwakenLines`.
   `kamiAwakenLines` is the first-awakening dialogue shown as `{name}: {line}`; it follows your `kamiNames`
   order, so it has no code default — copy the lines from STORY.md → *Draft status lines*. An empty or missing
   entry falls back to `"{name} awakens!"`.
2. Fill per-domain arrays (length 5, order Earth, Water, Fire, Wind, Void): `domainNames`, `greaterAnimators`,
   `greaterManifestLines`. `greaterManifestLines` ships pre-filled with the STORY.md manifest lines; clear an
   entry to fall back to `"The greater kami of {domain} manifests!"`. The bell's Kohaku remark is `bellLine`.
3. Each statue: collider + **KamiStatue** with `manager` and `kamiIndex` set.
4. Bell / omikuji box / purification altar: collider + **ShrineInteractable** with `eventName` = `_RingBell`, `_DrawOmikuji`, or `_Purify`.
5. Statue Animator: int parameter `AwakeTier` (0 asleep, 1 awake, 2 lv10+, 3 lv25+, 4 lv50+).
   Greater kami Animator: bool `Manifested`.
6. Assign TextMeshPro fields for the Reiryoku total, rate, status line, and faith (all optional).

## MVP scope

Fire + Earth domains, ~6 lesser kami, 2 greater kami, bell, omikuji, persistence, offline progress.
Add the remaining domains and prestige polish once the feel is right.
