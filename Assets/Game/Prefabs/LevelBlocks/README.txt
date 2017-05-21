Käyttöohjeet level blockeille (/Assets/Game/Prefabs/LevelBlocks):

	- PlayerCharacter
		- Kiinnostavimmat jutu löytyy Character2DUserControllerista, josta voi säätää nopeutta,
		hyppjen korkeutta yms...

	- CameraRig
		- Poistakaa aina default kamera ja käyttäkää tätä. Katsokaa lapsena olevan FollowCameran
		asetuksia jos haluatte vaihtaa.

	- BasicBlock
		- Perusblokki jonka päälle voi hypätä ja jossa voi kävellä, ts. siihen voi törmätä.
		Tässä on kiinni Sprite Renderer -komponentti, josta käytettävän spriten (kuvan) voi
		vaihtaa haluamakseen.

	- LockedDoor
		- Ovi joka avautuu kun siihen kytketty avain on löydetty. Tällä hetkellä tukee vain
		3x1 rakennelmaa joka on oletuksena kiinni.

	- Key
		- Avaimessa on Key Controller -skripti. Skriptiin raahataan se se LockedDoor objekti,
		joka avaimella pitäisi pystyä avaamaan.

	- Light
		- Eri kokoisia valoja (atm. 48 ja 64 pikseliä korkeat), joissa kiinni Light Source
		Controller. Täältä voi säätää voiko valo heilua ja vilkkua. Siinä on seuraavat
		muuttujat:
			- Can Shake - voiko heilua
			- Can Flicker - voiko vilkkua
			- Shake Amount - kuinka paljon valo heiluu aloituspisteensä ympärillä
			- Shake Speed - kuinka nopeasti valo heiluu
			- Time Until Flick - keskiarvo aika siitä kuinka kauan valo on päällä ennenkuin
			vilkkuu
			- Flick Random Factor - kuinka paljon (sekunneissa) Time Until Flick arvolla
			on satunnaista vaihtelua
			- Flick Time - kuinka kauan valo on vilkkumisvaiheessa pois päältä
			- Flick Random Factor - taas kuinka paljon on satunnaisuutta
	- MovingTile
		- Kiinni Platform Movement Controller, joka liikuttaa blokkia ympäriinsä
		waypointtien ympärillä. Waypointit asetetaan Waypoints -taulukkoon.

	- Piikit
		- Peruspiikit, ei mitään ihmeellistä.

	- OnewayPlatform
		- yksisuuntainen platformi, josta voi säätää onko yksisuuntainen vain ghostille.

	- Enemy
		- Meidän perusvihu, josa siis liikkuu laidalta laidalle. Tärkeimmät asiat huomata
		ovat Enemy AI Controller -skriptissä, jossa voi vaihtaa vihollisen perusnopeutta,
		sekä nopeutta pelaajan lähellä.

	- Liekkivalokeila
		- Valokeila joka liikkuu sini-aallon mukaisesti. Saatte itse kokeilla miten siihen
		liitetty Sin Wave Movement -skripti toimii :D.

Muuta

	- Tehkää levelit mielellään jonkut parent objektin lapseksi (Esim environment objektin alle). 
	Laittakaa myös tämä parentobjeti origoon (0,0,0).

	- Jos haluatte tehdä piilotetun huoneen, niin laittakaa blokit, jonkin uuden gameobjektin lapseksi,
	jolle laitatte skriptin HiddenRoomController. Lisäksi laitatte siihen komponentin BoxCollider2D
	ja tikkaatte is trigger -boksin. Säädätte BoxColliderin tarpeeksi haluamanne kokoiseksi. Tämän jälkeen,
	kun pelaaja tulee triggeriin, menevät kaikki lapsiobjektit pois päältä. Kun pelaaja taas lähtee triggeristä,
	menevät lapset taas päälle.

	- Jos haluatte, että jokin näkyy vain valossa laittakaa sille raahatkaa siihen MT_ShowOnlyInLight materiaali.
	Mikäli objekti on valonlähde laittakaa sille MT_Light materiaali. Materiaalit löytyvät /Assets/Game/Materials
	kansiosta. Mikäli haluattekin, että objekti toimii taas normaalisti (ei ole valonlähde ja näyttäytyy myös ei
	muuallakin kuin valossa), laittakaa MT_DefaultSprite -materiaali. 

	- Mikäli jokin objekti ei näy:
		- Katsokaa onko objekti piirtojärjestyksessä liian alhaalla. Tämän näkee SpriteRendererin Order in Layer
		kohdasta. Voi koittaa nostaa. 
		- Katsokaa onko em. materiaali kiinni objektissa.

	