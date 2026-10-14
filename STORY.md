# Kamigakure Shrine — Story Bible (物語)

The story, cast, and lore behind the idle loop in [README.md](README.md). Everything here is built to sit on
top of the existing systems — awakening statues, greater kami, the bell, omikuji, and 祓 purification — so the
mechanics *are* the story rather than a layer pasted over it.

---

## The premise

**神隠れ — *kamigakure*, "the hiding of the kami."**

Every year in the tenth month, *Kannazuki* (神無月, "the month without gods"), the kami of Japan leave their
shrines and travel to Izumo for the great gathering. The patron of this little mountain shrine went too.

When it came home, nobody had lit the lanterns. The path was overgrown, the bell rope had rotted, and the
village below had stopped climbing the steps. The patron could not find its way back — or perhaps it found the
shrine and could not bear what it saw. Either way, it hid itself behind the great mirror, and its household of
lesser kami fell asleep in their stone statues, one by one.

That was a long time ago. The village has forgotten the patron's name.

**The player** is a visitor who climbs the steps and hears a bell that no one rang. A small, tired fox is
waiting at the top.

### The shape of the story

Like the gods outside Amaterasu's cave, the player cannot *summon* a hidden kami. They can only make the world
outside worth coming back to — clean it, feed it, fill it with light, music, and laughter — until the kami
chooses to look.

| Act | What the player does | What the story does |
|---|---|---|
| **I. The sleeping shrine** | First offerings, first statues wake | Kohaku the fox explains what happened; Earth and Fire stir |
| **II. Five domains** | Restore all of Godai, greater kami manifest | Each domain has a wound to heal and a greater kami with its own story |
| **III. The rough wind** | Wind domain | The Storm Tengu is the patron's *rough soul* gone astray — appeased, not beaten |
| **IV. The mirror** | Void domain, Celestial Mirror manifests | The mirror shows the patron its own shrine, restored. It steps out. |
| **∞. Every year again** | 祓 purification | The shrine's year turns; the village remembers a little more |

---

## Lore pillars

The legends that drive the world. Each pillar names the mechanic it supports.

| Pillar | In legend | In the game |
|---|---|---|
| **Purity (清浄)** | Kami are repelled by *kegare* — the pollution of death, filth, and neglect. Rites cleanse. | 祓 Harae purification; each domain's "wound" is a kind of kegare |
| **Reciprocity** | People offer food and festivals; the kami protects in return. | Offerings → Reiryoku; the core loop itself |
| **Two souls (和魂・荒魂)** | Every kami has a gentle *nigimitama* and a rough *aramitama*. Neglect wakes the rough one; worship soothes it. | The Storm Tengu arc; 凶 omikuji flavor; awakening dialogue |
| **Joy (神遊び)** | Amaterasu was lured from her cave by dance and laughter. Festivals entertain the kami. | Matsuri multiplier, festival nights, communal clapping |
| **Sincerity (誠)** | An honest heart matters more than a grand gesture. | Kashiwade and the bell are free — attention, not spending |
| **Place** | Kami dwell in mountains, rivers, rocks, and great trees. | Godai domains; visual restoration of the grounds |
| **Binding (結び)** | *Musubi* — the force behind growth, harvest, and connection. | Greater kami link a domain's lesser kami; social layer |
| **Old things have souls (付喪神)** | Objects cared for a hundred years gain a spirit. | Lantern, mirror, and bell spirits; ema boards |
| **Wrongs set right** | Angry spirits (like Sugawara no Michizane → Tenjin) are enshrined and honored, not destroyed. | No enemies. Every conflict ends in a kami being honored. |

---

## Main cast

### 隠れ大神 Kakure-no-Ōkami — the Hidden Patron

The shrine's own kami. Never seen until the end — only heard: a bell with no ringer, wind that answers a clap,
a warmth on the steps at dusk. Its gentle soul is patient and fond of small, sincere things (a single rice
ball, a swept step). It is not angry. It is ashamed — of hiding, of being forgotten, of leaving its household
asleep.

- **Cares about:** being remembered by name; the village; sincerity over spectacle.
- **Voice:** few words, all of them kind. Speaks through the status line in italics only after Act II.
- **Hook:** the player learns its true name piece by piece from the keeper's ema boards. Saying the name at the
  mirror is the final step (*kotodama* — words have spirit).

### 琥珀 Kohaku — the One-Tailed Fox (guide)

The **Fox Lord** of the Fire domain, shrunk to a scruffy one-tailed fox after centuries without offerings. He
doesn't admit this at first; he claims to be "a messenger." Sly, vain, secretly devoted. He demands fried tofu
(*aburaage*) for every piece of advice.

- **Cares about:** the patron (he was its first messenger); his dignity; aburaage.
- **Arc:** each Fire milestone restores a tail (matches the README's "tails unlock one by one"). At nine tails
  he finally tells the player the truth about Kannazuki — he was the one who was supposed to light the lanterns.
- **Role:** tutorial voice, status-line commentary, comic relief, emotional core of Act I–II.

### おばば Grandmother Fumi — the Last Keeper (absent)

The shrine's last *miko*/keeper, long passed. She kept the shrine alone for decades after the village stopped
coming. She never appears; she speaks through **ema boards** — wooden wish plaques she hung over the years.

- **Cares about:** the patron, the village children she used to teach, her stone frog.
- **Role:** drives the **Ema board** long-term goals. Each board is a wish she never saw fulfilled ("I wish
  the koi pond were full again"). Fulfilling it reveals a fragment of the patron's name and her story.

### The Visitor — the player

Deliberately undefined (VRChat players bring their own avatars). Treated by the kami as a guest, then a
friend, then — after the first purification — as the shrine's new keeper.

---

## The five domains — 五大 Godai

Each domain has a **wound** (its kegare), a **greater kami** with its own story, and three **lesser kami**.
Lesser kami speak a line when they first wake (`AwakeTier` 1) and change mood at later tiers.

### 地 Earth — *the forgotten path*

**Wound:** the stone steps are buried in moss and roots; the village can't find the way up.
**Greater kami — 山守 the Mountain Guardian.** Old as the mountain, slow as stone. Speaks once per manifestation
tier. Remembers every footstep ever taken on the path and grieves the silence. When manifested, the lanterns
along the steps relight — the first sign to the village below.

| Lesser kami | Personality | Cares about |
|---|---|---|
| Jizō (地蔵) | Gentle protector of travelers and children; wears a red bib Fumi knitted | Children, safe roads, the bib |
| Tanuki (狸) | Jolly trickster, belly-drummer, terrible liar | Sake, a good party, being thanked |
| Stone frog (石蛙) | Fumi's favorite; its name puns on *kaeru* — "return home" | That travelers come back safely |

> The frog is the story's quiet thesis: *kaeru*, to return. It's the first statue the player wakes.

### 火 Fire — *the unlit lanterns*

**Wound:** the hearth and every lantern are cold. No one lit the way home at Kannazuki.
**Greater kami — 狐王 the Fox Lord.** Kohaku, restored. Each Fire milestone returns a tail; at full power he is
regal, warm, and still demands aburaage.

| Lesser kami | Personality | Cares about |
|---|---|---|
| Kitsune (狐) | Young foxes; Kohaku's nieces and nephews; giggly, quick | Kohaku's approval, foxfire games |
| Ember komainu (火狛犬) | Guardian lion-dog pair, one mouth open (*a*), one closed (*un*) — they finish each other's sentences | Guarding the gate; never being split up |

*(MVP has two Fire statues; a third can be a hearth spirit 竈神 if needed.)*

### 水 Water — *the poisoned spring*

**Wound:** something upstream fouled the shrine's spring; the purification basin is dry and the pond is mud.
**Greater kami — 川龍 the River Dragon.** Proud and fastidious; refuses to manifest in dirty water. Her story is
pure kegare: the spring was fouled when someone dumped old shrine objects upstream instead of retiring them
properly. Restoring Water reveals what was dumped — the Void domain's mirror and lantern.

| Lesser kami | Personality | Cares about |
|---|---|---|
| Kappa (河童) | Mischievous, polite to a fault — bows back and spills its head-water | Cucumbers, sumo, being bowed to |
| Koi (鯉) | Dreams of climbing the waterfall to become a dragon | Effort, persistence (Lv milestones = "climbing") |
| Rain toad (雨蛙) | Weather-caller, croaks before every rain | The rice fields below; seasons |

### 風 Wind — *the stolen voice*

**Wound:** the shrine bell was taken down; the wind has no voice and has turned bitter.
**Greater kami — 嵐天狗 the Storm Tengu.** The heart of **Act III**. The tengu is the patron's **rough soul
(aramitama)**, torn loose when the patron hid. Proud, wounded, furious at the village for forgetting. It
sends gusts that scatter offerings (flavor only — no punishment mechanics).

The player doesn't defeat it. Following the Tenjin pattern, they **hear its grievance and honor it**: re-hang
the bell, give it its own small sub-shrine. Manifesting the Storm Tengu is the moment it accepts — and the
patron, gentle and rough souls reunited, is now whole enough to be found.

| Lesser kami | Personality | Cares about |
|---|---|---|
| Tengu mask (天狗面) | Haughty, quotes old martial wisdom badly | Respect, discipline, being right |
| Crane (鶴) | Elegant, long-memoried; one of the "thousand cranes" | Longevity, wishes, grace |
| Bell spirit (鈴の精) | A *tsukumogami* — the old bell's soul, voice lost | Being rung by a sincere hand |

> The **suzu bell** mechanic already exists — in story terms, ringing it is what wakes Kohaku in the prologue.

### 空 Void — *the clouded mirror*

**Wound:** the great mirror (the patron's *shintai*, the object it dwells in) is fogged and cracked; the patron is
hidden behind it.
**Greater kami — 天鏡 the Celestial Mirror.** Not a character so much as a threshold. It does not speak; it
*shows*. When it manifests, it reflects the shrine back — fully restored, lanterns lit, people on the steps —
and the patron, seeing that it is wanted, steps out. This is the climax, at night under the starfield.

| Lesser kami | Personality | Cares about |
|---|---|---|
| Moon rabbit (月兎) | Pounds mochi on the moon; dreamy, sleepy, sweet | The full moon, sharing mochi |
| Mirror spirit (鏡の精) | A shard-tsukumogami; can only repeat what you say | Truth; the patron's name |
| Lantern spirit (提灯お化け) | One-eyed paper lantern, long tongue, terrified of the dark | Being lit; never being thrown away again |

---

## Purification — 祓 as the turning year

Prestige is the shrine's **Ōharae (大祓)**, the great purification held at the turn of each half-year. It is not
"starting over" — it is a new ritual year.

- Statues return to sleep, but the kami **remember** the visitor (dialogue changes after the first purify).
- **Shinkō (faith)** is framed as *the village remembering*. Status lines show villagers returning: one lantern
  lit below, then a few, then a festival.
- First purify after the patron appears: Kohaku names the player the shrine's **new keeper**.
- Late-game flavor: each purify can mark a season (spring → summer → autumn → Kannazuki → …), feeding the
  planned day/night & seasons system.

---

## Mapping to existing systems

| System (README) | Story meaning | Suggested text hook |
|---|---|---|
| Reiryoku 霊力 | The kami's strength returning | — |
| Statue awaken (`AwakeTier` 1) | A lesser kami wakes and speaks | `"{name} awakens!"` → per-kami first line |
| `AwakeTier` 2–4 | Mood: shy → warm → radiant | Optional per-tier line |
| Greater kami manifest | Domain's wound healed | `"The greater kami of {domain} manifests!"` → per-domain line (below) |
| Suzu bell | Calling the kami's attention | Kohaku: *"Not so loud. …Again, though."* |
| Omikuji | The patron speaking through fortune | 凶: the rough soul stirs; tie it to the rack |
| Kashiwade (planned) | Sincerity — two claps, a bow | Wind answers the clap after Act III |
| Ema board (planned) | Fumi's unfulfilled wishes | Each completion reveals a name fragment |
| Offline progress | The kami tending the shrine for you | `"While you were away, the kami gathered…"` (already fits) |
| Matsuri / communal prayer (planned) | Joy draws the kami out — Amaterasu's cave | Festival night = the patron is visibly present |

### Draft status lines

Short, drop-in replacements for the generic strings in `KamiShrineManager.cs`.

**First awakenings**
- Stone frog — *"Ribbit. …You came back. Fumi said someone would."*
- Jizō — *"Watch your step, traveler. The stones are loose."*
- Tanuki — *"A guest! Quick, someone find the sake. Someone? …I'll find the sake."*
- Kitsune — *"Uncle Kohaku! There's a human! Can we keep it?"*
- Komainu — *"A—" / "—un. Welcome."*
- Kappa — *(bows; water spills)* *"Ah. Excuse me. Do you have a cucumber?"*
- Koi — *"One day I'll climb the falls. Today I'll just… swim."*
- Rain toad — *"Rain tomorrow. Good rain."*
- Tengu mask — *"Hmph. The wind remembers who forgot it."*
- Crane — *"I have waited a thousand years. I can wait a moment more."*
- Bell spirit — *(a soft, cracked chime)*
- Moon rabbit — *"Mochi? …Mochi."*
- Mirror spirit — *"…came back. …came back."*
- Lantern spirit — *"Is it dark? Don't let it be dark. Oh — it's light. Thank you."*

**Greater kami manifest**
- Mountain Guardian — *"The path is open. Let them climb."*
- Fox Lord — *"Nine tails. Nine lanterns. I remember my duty now."*
- River Dragon — *"Clean. Finally. You may look at me."*
- Storm Tengu — *"…You re-hung the bell. Then I will ring it."*
- Celestial Mirror — *(no words — the patron steps out)*

---

## Respect notes

Shinto is a living religion with real, active shrines. The game should honor it:

- **No combat against kami.** Conflicts end in appeasement and enshrinement, never destruction.
- **Lesser kami are a mix**, as real shrine grounds are: Jizō is Buddhist (a bodhisattva — shrines and temples
  were intertwined for a millennium, *shinbutsu-shūgō*), and tanuki, kappa, and tengu are folk *yōkai*.
  Calling them "lesser kami" is a game liberty; dialogue can let them joke about it ("I'm not technically a kami,
  but Fumi fed me anyway").
- **The patron is original**, not a real named deity. Keep real kami (Amaterasu, Inari, Tenjin) as myths told
  *about*, not characters you level up.
- **Rituals simplified, not mocked.** Bell → clap → bow, omikuji tied to the rack, purification — these are real
  practices; keep them earnest even when the characters around them are silly.
