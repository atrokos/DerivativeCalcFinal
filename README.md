# Kalkulačka derivací
**Jan Pijálek**, letní semestr 2021/2022


# Úvod
Tento program je součástí zápočtu z předmětu Programování 2. Dal jsem si za úkol vytvořit kalkulačku derivací, která (na požádání uživatele) umí vypsat přibližné kroky, jak zadaný výraz zderivovat.

## Využití programu
Program lze použít k výpočtu derivací, jak složitých, tak jednoduchých. I přesto, že umí zobrazit kroky, je vhodný hlavně na osvěžení metod derivování. (Protože způsob, jakým jsou kroky zobrazovány, není úplně vhodný pro začátečníky.)

# Uživatelský manuál
Po zapnutí se zobrazí okno programu, které obsahuje následující:
* Vstupní pole
* Tlačítko ```Počítej!```
* Zaškrtávací box s textem ```Postup```
* Rozkliknutelné menu obsahující ```x```
* Šedé šipky vlevo a vpravo
* Stavové pole s nápisem ```Připraven```

*Pozn.: V celém manuálu je písmeno "x" bráno jako derivační proměnná.*

## Zadání výrazu a jeho výpočet
Do vstupního pole uživatel zadá výraz, který chce zderivovat.
Musí však být ve správném formátu, tj.:
* Musí být pouze v infixní notaci
* Pro sčítání, odečítání, násobení, dělení a mocninu jsou popořadě platné pouze tyto znaky:
	* ```+, -, *, /, ^```
* Závorky musí být spárované a užity dle zavedených konvencí.
* Matematické funkce jsou platné pouze takto: ```<Zkratka funkce>(<argumenty>)```.
* Uživatel může vynechat znaménko násobení mezi číslicí a závorkou/proměnnou.
	* např. ```2x``` je to samé jako ```2*x```.
	* To samé platí pro zápornou hodnotu: stačí ```-x``` místo ```(-1)*x```.
	* Toto neplatí pro násobení funkce či proměnné konstantou:
		* např. ```asin(bx)``` musí být napsáno jako ```a*sin(b*x)```
* Nelze zadat rovnici ani nerovnici, pouze samostatný výraz.
* Mezery jsou ignorovány, stejně tak velikost znaků.
	* tj. ```SiN(x)  X``` bude vyhodnoceno jako ```sin(x)*x```
	* Výjimkou jsou funkce, ty **musí** být napsány v celku (i se závorkou).
* Lze zadávat pouze celá čísla (tj. ne ```1,234```).
	* Ostatní doporučuji nahradit písmennými konstantami.
* Případné nečíselné konstanty musí být jednopísmenné.
	* Výjimkou jsou matematické konstanty (viz níže).
	* Písmeno ```e``` bude vždy bráno jako Eulerovo číslo.



Chce-li se uživatel ujistit, že zadal to, co chtěl, může si to ověřit pod vstupním polem, kde se mu zobrazuje náhled výrazu v "papírovém" stylu.
* Pokud náhled nevypadá tak, jak si uživatel představuje, doporučuji výraz lépe uzávorkovat.

Poté stačí stisknout tlačítko `Počítej!`. Dole je stav výpočtu.
Po dokončení derivace se pod textem `Výsledek:` zobrazí derivace zadaného výrazu.

### Zobrazení postupu
Pro zobrazení postupu, jak zadaný výraz zderivovat, stačí zaškrtnout box s nápisem `Postup` a kliknout na `Počítej!`. Po dokončení derivování se pod výsledkem zobrazí dodatečné pole s vygenerovaným postupem.
Před vypočtením derivace je výraz zjednodušen, např. `25x + 10 - 11` se zjednoduší na `25x - 1`.

#### Formát postupu
Postup má následující formát:
```
[<Právě derivovaný výraz>]'
<Text popisující derivaci tohoto výrazu>
<Výraz přepsaný do podoby popisované textem>
	<(Dle derivace) další kroky>
	.
	.
	.
Po úpravě:
<Výraz po dokončení derivování v tomto kroce, zjednodušený>
```

Obsahuje-li derivace další derivaci (např. násobení), je popis této další derivace odsazen více vpravo a má jinou barvu pro usnadnění orientace v textu.

Výjimkou jsou pouze konstanty a derivační proměnná, jejich kroky obsahují pouze 1 řádek.

### Volba derivační proměnné
Derivační proměnnou lze zvolit v rozklikávacím menu vpravo. Jsou možná všechna písmena standardní abecedy kromě `e`. To je rezervováno pro Eulerovo číslo.
Výchozí volbou je `x`.

### Další / předchozí derivace
Kalkulačka umožňuje zobrazit další, popř. předchozí derivaci právě zobrazené derivace. Po dokončení výpočtu lze kliknout na `>`, což zobrazí druhou (poté třetí, čtvrtou, atd.) derivaci. Obdobně pro předchozí derivace.

**Poznámky**
* Nelze zobrazit předchozí derivaci právě zadaného výrazu (tj. vlastně její intergál).
* Je-li n-tá derivace rovná 0, tlačítko `>` se deaktivuje.
* Chce-li uživatel vidět postup výpočtu i dalších stupňů derivace, bude vidět pouze poslední viděný stupeň. Nelze se vrátit k předchozím.

## Podporované funkce
Tato kalkulačka podporuje následující matematické funkce:

Název | Zkratka | Poznámka
------- | ------ | -------
Sinus | sin |
Kosinus | cos |
Tangens | tan |
Kotangens | cotan |
Arkus sinus | arcsin |
Arkus kosinus | arccos |
Arkus tangens | arctan |
Arkus kotangens | arccotan |
Přiroz. logaritmus | ln |
Absolutní hodnota | abs | Nelze použít \|x\|
Druhá odmocnina | sqrt | Lze použít ^(1/2)
Třetí odmocnina | cbrt | Lze použít ^(1/3)

## Podporované konstanty
Kalkulačka podporuje pouze dvě konstanty, a sice:

Název | Zkratka | Přibližný rozvoj
------- | ------ | -------
Pí | pi | 3.141592654
Eulerovo číslo | e | 2.718281828

## Ostatní
* Pokud by se vzorce výrazů nevešly na obrazovku, tak se zobrazí posuvník, kterým je možné vidět zbytek výrazu.
* Výpočet složitějších vzorců může trvat déle, zejména jejich zjednodušení a případné generování kroků.


# Technická část
## Využité knihovny
Tento program využívá 2 externí knihovny:
* AngouriMath - převedení rovnice na LaTeXový ekvivalent
* WPFMath - grafické zobrazení rovnic z LaTeXu

Na uživatelské prostředí jsem použil framework WPF (Windows Presentation Foundation).

## Obecný princip
Přišlo mi vhodné popsat princip této kalkulačky obecně, aby se dala pochopit hlavní myšlenka fungování:
1. Zadaná rovnice je zkontrolována, zda je syntakticky správně a případně jsou doplněna znaménka násobení tam, kde chybí.
2. Poté se celá rovnice převede z infixové formy na prefixovou.
3. Z prefixové formy se vytvoří aritmetický strom (AS).
4. AS provede derivaci.
5. AS je převeden zpět na infixovou notaci.
6. Infixová notace je předána knihovně AngouriMath, která ji převede na LaTeXovou formu.
7. Tato forma je předána knihovně WpfMath, která z ní vysází rovnici.

Pokud uživatel chce vidět derivační kroky, je celý postup pozměněn:
* Na začátku se vstup zjednoduší.
* Derivační kroky se během kroku 4 ukládají do třídy Storage pomocí klonování.
* Třída StepGenerator mezi kroky 5 a 6 vygeneruje příkazy podle obsahu Storage.
* Metoda GenerateSteps sestaví z příkazů UI prvky, které následovně předá WPF k zobrazení.

V případě nezobrazování kroků je tedy celý proces podstatně rychlejší.

## Aritmetický strom
Tento aritmetický strom je nejdůležitější součást celé kalkulačky - zde se provádí celé derivování.
Pravidla tohoto stromu jsou jednoduchá:
1. Kořenem je vždy tzv. hlava, která obsahuje odkaz na zbytek rovnice. Slouží jen na předávání příkazů jako `Differentiate()` nebo `SelfCheck()`.
2. Všechny uzly, které nejsou hlava, mají právě jednoho rodiče.
3. Pouze hlava, uzly operátorů a uzly funkcí mají popořadě právě 1, 2 a 1 dětí.
4. Číselné/písmenné konstanty a derivační proměnné jsou vždy listy.
5. Strom je vždy sestavován "odspoda", tj. začíná se listy a končí se hlavou.
6. Všechny matematické operace na tomto stromu jsou rekurzivní.

Každý uzel si pamatuje, jakým způsobem se derivuje. Například uzel pro násobení ví, že si musí naklonovat obě děti, pak obě kopie zderivovat a pronásobit svými protějšky. Nakonec toto vrátí jako součet:
\[F\*G\]' = \[F\]'\*G + F\*\[G\]'

Také se každý uzel umí sám zkontrolovat (tzv. SelfCheck). Jde o velice jednoduché zkontrolování, zda uzel není zbytečný. Například:
* Uzel sčítání si všimne, že jeho pravé dítě je 0. Pak stačí, aby sebe nahradil levým dítětem.
* Uzel násobení má jako levé dítě 0. Pak vytvoří konstantu s číslem 0, kterou dá na své místo.

## ON Aritmetického stromu
Všechny uzly při vytváření musí mít (případné) děti přiřazené již v konstruktoru. Ty lze pouze vyměňovat za jiné. Nemůže tedy nastat situace, že by uzlu chybělo dítě.

Snažil jsem se, aby tento ON byl co nejvíce flexibilní. Může fungovat sám o sobě, není těžké do něj přidávat nové funkce a (dle mého názoru) ani psaní derivačního postupu není složité.

Pro správnou spolupráci s Parserem stačí jen novou funkci/operátor přidat do podporovaných a přidat `case` bloky s názvy přidaných funkcí/operátorů pro správný parsing.

### INode
INode je stěžejní prvek celého AS. Všechny objekty splňující toto rozhraní se umí derivovat, zjednodušovat, klonovat, měnit rodiče a generovat derivační kroky.

### IParent
Objekty s tímto rozhraním umí pracovat děti (tj. odebrat, přidat, změnit, apod.).

### ConstNode : INode
Jde o abstraktní třídu, která mimo INode obsahuje pouze odkaz na rodiče.
Jsou od ní odvozeny tyto uzly:
**Constant**
Obsahuje pouze číslo (double).
**DiffVariable**
Představuje derivační proměnnou.
**LetterConstant**
Obsahuje písmeno (string).

### ActionNode : INode, IParent
ActionNode je abstraktní třída, od které jsou odvozeny všechny třídy, které musí obsahovat děti, tj. operátory a funkce.  Všechny metody požadované rozhraním INode jsou abstraktní. Metoda z IParent je definována, ale může být přetížena.
Obsahuje proměnnou pro rodiče a jedno dítě.

### Function : ActionNode
Jde o abstraktní třídu, která od níž jsou odvozeny všechny funkce (popsané v *Podporované funkce*). Přetěžuje pouze metodu  `SelfCheck()`.

### OPNode : ActionNode
Jde o abstraktní třídu, která přidává ještě druhé dítě (a dle toho přetěžuje funkci na přidání dítěte.) Jsou od ní odvozeny všechny uzly na operátory.

## Storage
Toto je jediná statická třída v celém programu. Slouží k uchovávání případných derivačních kroků a k jednoduchému předávání těchto kroků ostatním třídám.

## ON ostatních objektů
### MathExpression
Tato třída se chová jako samostatný matematický výraz. Při vytváření dostane výraz jako string, dále derivační proměnnou a zda je potřeba generovat kroky.
Obsahuje výraz jako string i jako strom.
Úzce spolupracuje s Parserem.

### Parser
Tato třída provádí veškeré práce s parsování vstupu:
* Kontroluje správnost výrazu a případně doplňuje znaménka.
* Převádí matematický výraz na strom a zpátky.

Obsahuje množinu všech podporovaných operátorů, funkcí a konstant.

### StepGenerator
Jak název napovídá, tato třída připravuje kroky po derivaci. Z třídy Storage si vezme seznam kroků a poté vygeneruje seznam příkazů pro WPF klienta.
Mezi příkazy patří:
* `/margin [číslo]` - odsazení zleva
* `/color [číslo]` - jakou barvu pozadí má mít generovaný UI prvek. Dle čísla se použije barva ze seznamu barev.
* `/math [LaTeXový string]` - vytvoření vysázeného LaTeXového výrazu pomocí WPFMath.
* Ostatní text bez příkazu je převeden na textový blok obsahující tento text.

## Ostatní
Ostatní metody, které se nikam nevešly.

### Metoda GenerateSteps
Tato metoda pracuje úzce s WPF. Dle příkazů získaných od StepGenerator vytvoří příslušné UI prvky, nebo změní barvu/odsazení.

# Závěr
Díky tomuto programu jsem lépe pochopil rozhraní, abstraktní třídy a celkově dědění. Zejména to dědění zde bylo velice potřeba, aby samotný strom byl nějak logicky uspořádaný.
Myslím, že jsem se i zlepšil v objektovém návrhu, protože jsem ho předělával zhruba 4x, než jsem s ním byl nějakým způsobem spokojený.

Osobně mě překvapilo, že samotná implementace derivací nebyla tak složitá, zatímco nejhorší pro mě bylo napsání parseru a celková kontrola vstupu.
