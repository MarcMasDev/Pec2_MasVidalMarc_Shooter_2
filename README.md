# 🎮 PEC2 – First Person Shooter
### Marc Mas Vidal, Programació en Unity 3D, 03/05/2026

> Joc de trets en primera persona desenvolupat en **Unity 6 (6000.0.38f1 LTS)** amb **HDRP** (High Definition Render Pipeline). El jugador ha de superar dos zones de joc ben diferenciades: una fàbrica abandonada que conté interiors y exteriors, enfrontant-se a enemics, i sobrevivint amb un sistema de vida i escut. L'objectiu es matar a tots els enemics del nivell.

---

## Taula de continguts

1. [Instruccions per executar el projecte](#1-instruccions-per-executar-el-projecte)
2. [Controls del joc](#2-controls-del-joc)
3. [Vídeo de gameplay](#3-vídeo-de-gameplay)
4. [Estructura de dades](#4-estructura-de-dades)
5. [Arquitectura del codi — Scripts propis](#5-arquitectura-del-codi--scripts-propis)
6. [Descripció del treball realitzat](#6-descripció-del-treball-realitzat)

---

## 1. Instruccions per executar el projecte

### Requisits previs

Requisit: Versió
Unity Editor: 6000.0.38f1 LTS
Render Pipeline: HDRP (High Definition Render Pipeline) 
Visual Studio / Rider: Qualsevol versió compatible amb Unity 6 
Git LFS: Recomanat (el projecte conté assets pesats)


### Passos per clonar i obrir el projecte

# 1. Clona el repositori
git clone https://github.com/MarcMasDev/Pec2_MasVidalMarc_Shooter_2.git

# 2. Obre Unity Hub
# 3. Fes clic a "Add project from disk" i selecciona la carpeta clonada
# 4. Selecciona la versió Unity 6000.0.38f1 LTS
# 5. Espera que Unity importi tots els assets


### Obrir l'escena principal
1. A la finestra **Project**, navega fins a `Assets/Scenes/`
2. Fes doble clic sobre l'escena **`OutdoorsScene`**
3. Prem el botó **Play** per iniciar el joc

### Notes importants

- Si apareix un avís de **"Bake NavMesh"**, ves a `Window → AI → Navigation` i fes clic a **Bake** per regenerar-lo.
- Si els materials es veuen negres o rosats, ves a `Edit → Rendering → Materials → Convert All Built-in Materials to HDRP`. Però si el projecte s'ha obert en HDRP, els materials han d'estar bé.

---

## 2. Controls del joc

### Moviment i càmera

| Tecla / Input | Acció |
|---|---|
| `W A S D` | Moure el jugador (endavant, esquerra, enrere, dreta) |
| `Ratolí` | Orientar la càmera (mirar al voltant) |
| `Espai` | Saltar |
| `Left Shift` | Córrer (sprint) |
| `Left Ctrl` | Ajupir-se (crouch) |

### Armes

| Tecla / Input | Acció |
|---|---|
| `Botó esquerre del ratolí` | Disparar / Atacar |
| `Botó dret del ratolí` | Apuntar |
| `R` | Recarregar l'arma |
| `Roda del ratolí` | Canviar d'arma, també es pot canviar utilitzant els números (1,2,3,4) |

### Interacció

| Tecla / Input | Acció |
|---|---|
| `E` | Interactuar (recollir claus, obrir portes, activar interruptors) |
| `F` | Activar/desactivar llanterna (sistema legacy creat amb l'idea de donar gameplay diferent a un survival horror, al final descartat per les dinàmiques no desitjades que crea, pero útil per veure els enemics) |

### HUD – Informació en pantalla

El HUD mostra permanentment:
- **Barra de vida** (vermell): punts de vida restants
- **Barra d'escut** (blau): el dany va primer a l'escut (x%), quan arriba a 0, impacta al 100% sobre la vida
- **Arma equipada**: nom de l'arma actual
- **Munició**: [bales al carregador] / [bales de reserva]

---

## 3. Vídeo de gameplay

> **[LINK AL VÍDEO DE GAMEPLAY]**


El vídeo demostra:
- Navegació per l'escenari exterior muntanyenc i l'interior de l'edifici
- Sistema d'armes: pistola, fusell automàtic, fusell de ràfagas, destral
- Sistema de vida i escut rebent dany
- Recollida d'ítems (vida, escut, munició, claus)
- Comportament dels enemics (patrulla, detecció, persecució, dispar/atac)
- Enemics voladors (EnemyFlying)
- Enemics amb diferents tipus d'atac utilitzant el sistema d'armes.
- Sistema de portes i claus
- Plataformes mòbils
- Dead zones (zona de lava)
- Sistema HDRP: boira volumètrica, oclusió ambiental, il·luminació global
- Pantalla de Game Over i reinici de nivell

---

## 4. Estructura de dades

### Filosofia de disseny

El projecte aplica el **patró Blackboard** (pissarra de dades compartida) com a nucli de la comunicació entre components. Cada personatge (jugador o enemic) disposa d'un `CharacterBlackboard`.

### Classes de dades principals

#### `WeaponClass` (ScriptableObject)
Tots els paràmetres de les armes es defineixen com a **ScriptableObjects**, la qual cosa permet configurar-les des de l'Inspector sense modificar codi.

#### `EntityHealth` (Component compartit)
Tant el jugador com els enemics utilitzen el **mateix component** `EntityHealth`, garantint consistència en la mecànica de dany.


#### Estructura d'un enemic

Cada enemic combina múltiples components desacoblats que es comuniquen via el `CharacterBlackboard`:

```
**EnemyPrefab**
CharacterBlackboard       > dades compartides
CharacterAnimationManager > llegeix blackboard, controla Animator
EntityHealth              > gestiona vida (reutilitzat del jugador)
EnemySensors              > detecció (vista i oïda)
EnemyInput                > decisions de l'agent
EnemyFSM                  > màquina d'estats (Patrulla / Alerta / Atac / Mort)
WeaponRanged              > lògica de dispar (reutilitzat del jugador) o d'atac (cos a cos)
DamageDealer              > aplica dany en projectils o melee
NavMeshAgent              > navegació Unity (R=0.6, H=2)
Rigidbody                 > física
```

### Estructura de l'escena

```
OutdoorsScene
Terrain                   > terreny HDRP amb 6 capes de textura
AudioManager              > sistema d'àudio centralitzat
CustomPassVolumes         > efectes especials (visió tèrmica) i FIX per a que les armes no  les parets
Player
	FPSController         > controlador principal del jugador
		FirstPersonCharacter         	> Càmera
			Hands_Gun02      	> pistola
			Hands_Automatic_Rifle 	> fusell automàtic
			Hands_Tommy_gun   	> metralleta
			Hands_Axe         	> arma cos a cos

	PlayerCanvas          > HUD (PlayerStatusUI, DamageOverlay, Screens)

NavMeshSurface            > malla de navegació dels enemics
Enemies
	EnemyFlying	> enemics aeris atac a distància (arma automàtica) amb waypoints
	EnemyMelee	> enemics terrestres cos a cos amb waypoints
	EnemyRanged	> enemics terrestres atac a distància (arma semiautomàtica) amb waypoints

Scenario                  > geometria de l'escenari
Items                     > ítems de vida, escut, munició
	Doors                     > portes amb sistema de claus, algunes claus estan al mapa, d'altres les deixen els enemics al morir
	Platforms                 > plataformes mòbils

NightLight
	Global Volume         > HDRP: sky, núvols, fog, SSGI, SSAO...
	Directional Light     > llum direccional
	Local Volumetric Fog  > boira local
	Reflection Probe      > reflexos de l'entorn
	Post Processing Thermal Vision > efecte visual extra
```

---

## 5. Descripció del treball realitzat
 
---
 
### Basic 1: Escenari (terreny muntanyós i entorn tancat)
 
L'escena `OutdoorsScene` integra dos entorns clarament diferenciats dins d'un flux de joc continu.
 
**Exterior muntanyós:** El terreny es construeix amb l'eina nativa `Terrain` d'Unity, amb 6 capes de textura (gespa, roca, terra, paviment...) pintades manualment perquè hi hagi variació visual natural. Les muntanyes envolten la zona de joc i actuen com a límit natural. La zona exterior inclou construccions urbanes (una fàbrica abandonada) i una zona de lava vermella que serveix com a dead zone natural.
 
**Interior tancat:** La zona interior és un edifici industrial amb passadissos.
 
---
 
### Basic 2: Tipus d'armes
 
S'implementen dues armes principals tal com demana l'enunciat, més armes addicionals (vegeu Opcional 2). Totes estan implementades amb el component `WeaponRanged.cs` i configurades via `WeaponClass` ScriptableObjects, la qual cosa permet modificar paràmetres des de l'Inspector sense tocar el codi.
 
| Arma | Prefab | Tipus | Mecànica |
|---|---|---|---|
| Pistola | `Hands_Gun02` | Semi-automàtica | Un tret per clic, precisa, cadència lenta |
| Fusell automàtic | `Hands_Automatic_Rifle02` | Ràfega de 3 dispars | Foc continu mentre es prem el botó |
| Tommy Gun | `Hands_Tommy_gun` | Automàtic | Alta cadència |
| Destral | `Hands_Axe` | Melee (`WeaponMelee`) | Atac cos a cos, infinita |
 
Les armes de foc comparteixen el mateix script `WeaponRanged.cs` però amb configuració independent: cadència, mida del carregador, munició de reserva, temps de recàrrega i mode semi/automàtic/ràfaga. L'Animator de cada arma té estats propis (Shot, Aim_Shot, Reload, pistol_melee) però tots llegits des del `CharacterAnimationManager`, que és el mateix per a totes.
 
---
 
### Basic 3: Sistema de vida i escut
 
Implementat al component `EntityHealth.cs`, compartit entre jugador i tots els enemics. La mecànica compleix exactament l'enunciat:
 
- Si l'escut és major que 0: el 75% del dany va a l'escut i el 25% a la vida directament.
- Si l'escut arriba a 0: el 100% del dany restant va a la vida.
Quan la vida arriba a 0 s'invoca l'event `OnDeath`, que cada objecte escolta de manera independent (el jugador mostra la pantalla de Game Over, l'enemic activa la mort i el loot drop). La separació via UnityEvent evita qualsevol acoblament directe entre `EntityHealth` i la lògica específica de cada personatge.
 
---
 
### Basic 4: HUD (vida, escut, armes, munició)
 
El component `PlayerStatusUI.cs` llegeix el `CharacterBlackboard` del jugador cada frame i actualitza els elements visuals del canvas:
 
- **Barra de vida** (vermell): s'actualitza en temps real amb el valor de `currentHealth / maxHealth`.
- **Barra d'escut** (blau): s'actualitza amb `currentShield / maxShield`.
- **Arma equipada**: mostra el nom de l'arma activa llegit del `WeaponClass` ScriptableObject.
- **Munició**: format `[carregador] / [reserva]` actualitzat en disparar i recarregar.
El `DamageOverlay.cs` afegeix un feedback visual d'impacte (overlay vermell semitransparent) quan el jugador rep dany, que s'esvaeix gradualment. A més, el `CustomPassVolume` s'utilitza per assegurar que les mans i armes del jugador es renderitzen sempre al davant de qualsevol geometria, evitant que les armes travessin les parets.
 
---
 
### Basic 5: Plataformes mòbils
 
El script `PlatformMover.cs` implementa plataformes amb moviment configurable. Des de l'Inspector es pot definir la llista de punts de la trajectòria, la velocitat, el temps d'espera a cada punt i si el moviment és lineal o cíclic.
 
Les plataformes porten el jugador correctament gràcies al sistema de `parent transform`: quan el jugador col·lisiona amb la plataforma, passa a ser fill temporal d'aquesta, heretant el seu moviment sense necessitat de físiques complexes. Hi ha plataformes verticals (ascensors) i horitzontals al llarg del nivell.
 
---
 
### Basic 6: Portes i claus
 
El sistema de portes i claus genera la progressió natural del nivell. Cada porta té un component `Door.cs` que comprova si el jugador, en prémer `E`, porta la clau corresponent a l'inventari (`CharacterBlackboard.keyInventory`).
 
Quan la porta s'obre, les filles `LeftDoor` i `RightDoor` es mouen cap a les posicions `LeftOpenPos` / `RightOpenPos` predefinides. La porta inclou un component `NavMeshObstacle` (amb Carve activat) que actualitza la malla de navegació dinàmicament, de manera que els enemics adapten la seva ruta un cop la porta és oberta.
 
Les claus es troben distribuïdes de dues formes al nivell: algunes estan en objectes `KeyPickup` a l'escenari, i d'altres les deixen caure els enemics en morir via `LootDrop.cs`.
 
---
 
### Basic 7: Enemics (patrulla i reacció)
 
El comportament dels enemics es gestiona mitjançant tres scripts: `EnemySensors.cs`, `EnemyInput.cs` i `EnemyFSM.cs`, que segueixen el principi de responsabilitat única.
 
**Patrulla:** L'`EnemyFSM` navega entre una llista de waypoints predefinits usant el `NavMeshAgent` (tipus Humanoid, R=0.6, H=2). El NavMesh està sobre tota la geometria del nivell i s'actualitza dinàmicament per les portes (nav mesh obstacle).
 
**Detecció per vista:** `EnemySensors` llança un RayCast des dels ulls de l'enemic dins d'un con de visió (`visionAngle`, `visionDistance`). Si impacta a la capa `Player` sense obstacle, s'activa l'estat de persecució.
 
**Detecció per oïda:** Una esfera de radi `hearingRadius` detecta si el jugador és a prop. Si és dins però fora de la línia de visió, l'enemic activa l'estat `Search`: gira 360° buscant el jugador i, si no el troba, reprèn la patrulla.
 
**Reacció i atac:** Un cop detectat el jugador, l'enemic el persegueix (`Chase`) i quan entra al rang d'atac (`Attack`) dispara o ataca. Si el jugador s'allunya prou, torna a `Chase`.
 
---
 
### Basic 8: Ítems (vida, escut, munició)
 
`ItemPickup.cs` gestiona tots els ítems "recollibles". Cada ítem té un tipus (vida, escut, munició) i un valor configurable. En col·lisionar amb el jugador i prémer `E`, aplica l'efecte corresponent al `CharacterBlackboard` i es destrueix.
 
Els ítems estan distribuïts per l'escenari en posicions estratègiques. A més, `LootDrop.cs` (component present en alguns enemics) genera ítems i claus a la posició de l'enemic quan mor, amb probabilitats configurables per Inspector. Això incentiva el jugador a eliminar enemics per obtenir recursos.
 
---
 
### Basic 9: Pantalla de Game Over i reinici
 
Quan `EntityHealth.currentHealth` arriba a 0 al jugador, s'invoca `OnDeath`, que activa `GameOverScreen.cs`. Aquesta mostra la pantalla de Game Over (inclosa en el `PlayerCanvas > Screens`) i ofereix l'opció de reiniciar el nivell via `SceneManager.LoadScene(SceneManager.GetActiveScene().name)`. Tots els elements de la pantalla segueixen l'estètica HDRP del joc (fons fosc, tipografia adequada).
Quan el jugador ha matat a tots els enemics, activa a `GameOverScreen.cs` la pantalla de victoria.
 
---
 
### Opcional 1: Més nivells
 
S'ha implementat un nivell de tipus survival. On hi ha un spawner que invoca a enemics.
Tot i que no s'ha fet una geometria nova o nous espais, s'ha incorporat una mecànica nova que ha requerit modificacions a la script `EnemyFSM.cs`
 
---
 
### Opcional 2: Noves armes
 
Com s'ha vist a l'apartat de les armes dels punts bàsics, s'ha afegit una destral i un arma de ràfega. A més totes (expecta la cos a cos) disposen d'un sistema d'apuntat.
 
---
 
### Opcional 3: So i efectes de so
 
`AudioManager.cs` implementa un Singleton que centralitza tota la gestió d'àudio del joc.
 
Sons implementats: disparar cada arma, recàrrega pas a pas (múltiples clips encadenats utilitzant animator events), recollida d'ítems, obertura de portes, passos del jugador, salt, rebre dany, morir, enemics (dispar, recàrrega, pasos).
 
---
 
### Opcional 4: Puzzles (portes amb interruptors)
 
No implementat.
 
---
 
### Opcional 5: Diferents tipus d'enemics
 
S'implementen tres tipus d'enemics ben diferenciats, tots compartint el mateix sistema `EnemyFSM` + `EnemySensors` + `EnemyInput`, però amb components d'atac i animacions propis:
 
**EnemyFlying (×3):** Enemic aeri. Usa `WeaponRanged` en mode automàtic (`AttackAut`). Pot moure's en l'eix vertical, la qual cosa el fa difícil d'esquivar.
 
**Enemy_melee (×4):** Enemic terrestre cos a cos. Usa `WeaponMelee` amb l'animació `ClawsAttack`. Molt ràpid en proximitat però sense atac a distància, forçant el jugador a mantenir distància.
 
**EnemyRanged (×5):** Enemic terrestre a distància basat en el model. Usa `WeaponRanged` en mode semiautomàtic.

 
---
 
### Opcional 6: Efectes de desaparició d'enemics
 
La desaparició gradual dels enemics en morir s'implementa via **Shader Graph**: `SG_Enemy`. 
`EnemyFadeDeath.cs` activa l'efecte quan `OnDeath` es dispara: modifica progressivament un paràmetre del Shader Graph (dissolució / opacity) al llarg d'un temps definit fins que l'enemic desapareix completament i el GameObject es destrueix.
  
---
 
### Opcional 7: Dead zones
 
S'implementen dos tipus de zones perilloses:
 
**DeadZone (lava):** La gran zona de lava de l'exterior aplica mort instantània al jugador en entrar-hi. Implementat via `DeadZone.cs`, que crida `TakeDamage` amb un valor molt elevat en detectar col·lisió amb el jugador.
 
**DamageZone (tòxic):** Zones de dany continu que apliquen dany periòdic via `DamageZone.cs` mentre el jugador hi és dins. El dany per segon és configurable per Inspector. Aquestes zones forcen el jugador a prendre decisions ràpides.
 
---
 
### Opcional 8: Checkpoints
 
No implementat.
 
---
 
### Opcional 9: HDRP (High Definition Render Pipeline)
 
El projecte usa HDRP de forma exhaustiva, aprofitant les seves capacitats:
 
**Llum i cel:** `Directional Light` amb temperatura de color, `Physically Based Sky` i `Volumetric Clouds` al Global Volume, creant un cel nocturn realista.
 
**Post-processat:** `Screen Space Global Illumination (SSGI)` per a rebots de llum indirecta, `Screen Space Ambient Occlusion (SSAO)` per a ombres de contacte, `Exposure` automàtica, `Vignette` i `Color Adjustments` per a la grading de color fosca i de suspens.
 
**Boira volumètrica:** `Local Volumetric Fog` (200×2.4×200 unitats) a l'interior amb mode Additive, donant un ambient dens i claustrofòbic. Boira global via el Global Volume per a l'exterior nocturn.
 
**Reflexos:** `Reflection Probe` per a reflexos correctes als materials metàl·lics i brillants de l'interior.
 
 
**Armes sense clip:** Un segon `CustomPassVolume` assegura que les mans i armes del jugador es renderitzen en una passada separada, sempre al davant de qualsevol geometria del món, eliminant l'artefacte de clipping d'armes a través de les parets.
 
---

### EXTRA: 
**Visió tèrmica:** Un `CustomPassVolume` amb un Custom Pass de Shader Graph implementa un efecte de visió tèrmica activable, que remapeja els colors de la imatge. Activat/desactivat en joc com a habilitat del jugador.
Apretar F per activar.

 
---

## 6. Bibliografia
 
---
Per realitzar el projecte, s'han utilitzat els següents "assets" tots adquirits de forma legal mitjançant la ASSET STORE, que garanteix disposar de les llicències necessàries pel seu ús. També alguns sons de "free sounds" tots amb llicència Creative Common 0:

## Unity Asset Store

### Abandoned Factory Buildings - Day/Night Scene
**Autor:** ScansFactory
**Versió/Data:** 1.6 (18 de novembre, 2025)
**Descripció:** Escenari industrial postapocalíptic d'alt detall basat en fotogrametria. Inclou escenes de dia i nit.

### Animated Hands with Weapons Pack
**Autor:** Maksim Bugrimov
**Versió/Data:** 1.2 (15 de juliol, 2025)
**Descripció:** Paquet de mans animades per a FPS amb 18 tipus d'armes diferents i múltiples textures.

### MONSTER FULL PACK VOL 2
**Autor:** PROTOFACTOR, INC
**Versió/Data:** 1.01 (20 de juny, 2024)
**Descripció:** Col·lecció de 30 monstres amb animacions completes.

### Game Makers Sound Effects Kit Part 1
**Autor:** Epic Sounds and FX
**Versió/Data:** 1.0 (26 de juliol, 2024)
**Descripció:** Més de 3000 actius d'àudio (SFX) d'alta qualitat en format .WAV.

### Particle Pack
**Autor:** Unity Technologies
**Versió/Data:** 3.1 (7 d'abril, 2026)
**Descripció:** Recull d'efectes de partícules (foc, explosions, gel) proporcionat per Unity.

### Blood Gush
**Autor:** RRFreelance
**Versió/Data:** 1.01 (7 de febrer, 2019)
**Descripció:** Sistema de partícules per a efectes de sang.

### NaughtyAttributes
**Autor:** Denis Rizov
**Versió/Data:** 2.1.5 (9 d'abril, 2026)
**Descripció:** Extensió d'atributs per a l'inspector d'Unity que facilita la serialització i el flux de treball.

### FPS Engine
**Autor:** Cowsins
**Versió/Data:** 2.1.5 (3 de Maig, 2026)
**Descripció:** Només s'ha fet ús d'àudios i de VFX. Tot el còdig que inclou ha estat descartat y no ha estat usat al projecte.


## Recursos d'Àudio (Freesound)

| Asset | Autor | Enllaç |
| :--- | :--- | :--- |
| **dark-dungeon-ambience** (516566) | Kinoton | [Visitar Freesound](https://freesound.org/people/Kinoton/sounds/516566/) |
| **hurt-5** (840220) | kreha | [Visitar Freesound](https://freesound.org/people/kreha/sounds/840220/) |

---

Marc Mas Vidal, Programació en Unity 3D, 03/05/2026
