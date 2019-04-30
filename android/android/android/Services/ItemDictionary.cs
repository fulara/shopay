using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace android.Services
{
    public class WordLookup
    {
        private static Object mutex = new object();
        private static WordLookup lookup;
        private List<Definition> database = new List<Definition>();
        public class Definition
        {
            public String Name;
            public String Category;
        }

        private static WordLookup Lookup
        {
            get
            {
                if (lookup != null)
                {
                    return lookup;
                }
                lock (mutex)
                {
                    lookup = new WordLookup();
                    lookup.init();

                    return lookup;
                }
            }
        }


        private void Add(string name, string category)
        {
            database.Add(new Definition{ Name = name, Category =  category});
        }

        public static List<Definition> Get(String name)
        {
            return Lookup.database.FindAll(e => e.Name.Contains(name)).ToList();
        }

        private void init()
        {
            Add("baleron", "meat"); // gammon
            Add("baranina", "meat"); // mutton
            Add("befsztyk", "meat"); // beefsteak
            Add("bekon", "meat"); // bacon
            Add("cielęcina", "meat"); // veal
            Add("cynaderki", "meat"); // kidneys
            Add("drób", "meat"); // poultry
            Add("dziczyzna", "meat"); // venison
            Add("frankfurterka", "meat"); //	
            Add("gęś", "meat"); // goose
            Add("golonka", "meat"); // leg
            Add("indyk", "meat"); // turkey
            Add("jagnięcina", "meat"); // lamb
            Add("kabanos", "meat"); // sausage
            Add("kaczka", "meat"); // duck
            Add("karkówka", "meat"); // neck",
            Add("kaszanka", "meat"); // pudding
            Add("kiełbasa", "meat"); // sausage
            Add("bulion", "meat"); // stock
            Add("kotlet", "meat"); // burger
            Add("koźlina", "meat"); // chevon
            Add("królik", "meat"); // rabbit
            Add("kura", "meat"); // hen
            Add("koźlina", "meat"); // chevon
            Add("kurczak", "meat"); // chicken
            Add("mortadela", "meat"); // mortadella
            Add("ozór", "meat"); // tongue
            Add("parówka", "meat"); // frankfurter
            Add("polędwica", "meat"); // loin
            Add("przepiórka", "meat"); // quail
            Add("rolada", "meat"); // roulade
            Add("rostbef", "meat"); // beef
            Add("salami", "meat"); // salami
            Add("salceson", "meat"); // brawn
            Add("słonina", "meat"); // fat
            Add("szynka", "meat"); // ham
            Add("sznycel", "meat"); // schnitzel
            Add("ślimak", "meat"); // helix
            Add("ślimak", "meat"); // snail
            Add("tatar", "meat"); // tatar
            Add("wątrobianka", "meat"); // pate
            Add("wątróbka", "meat"); // liver
            Add("wieprzowina", "meat"); // pork
            Add("wołowina", "meat"); // beef
            Add("żeberka", "meat"); // ribs
            Add("nabial", "nabial"); //
            Add("śmietana", "nabial"); // cream
            Add("ser", "nabial"); // cheese
            Add("twaróg", "nabial"); // cottage cheese
            Add("maślanka", "nabial"); // buttermilk
            Add("lody", "nabial"); // ice cream
            Add("jogurt", "nabial"); // yoghurt
            Add("mleko", "nabial"); // milk
            Add("kefir", "nabial"); // kefir
            Add("białe wino", "alkohol"); // white wine
            Add("cydr", "alkohol"); // cider
            Add("lemoniada", "woda"); // lemonade
            Add("sok", "woda"); // juice
            Add("herbata", "suche"); // tea
            Add("kawa", "suche"); // coffee
            Add("piwo", "alkohol"); // beer
            Add("wino", "alkohol"); // wine
            Add("czerwone wino", "alkohol"); // red wine
            Add("szampan", "alkohol"); // champagne
            Add("koniak", "alkohol"); // cognac
            Add("likier", "alkohol"); // liqueur
            Add("rum", "alkohol"); // rum
            Add("woda", "woda"); // water
            Add("wódka", "alkohol"); // vodka
            Add("orzech", "suche"); // nut
            Add("orzech wloski", "suche"); // walnut
            Add("orzech laskowy", "suche"); // hazelnut
            Add("migdał", "suche"); // almond
            Add("orzechy migdalowe", "suche"); //almond
            Add("kokos", "suche"); // coconut
            Add("pistacja", "suche"); // pistachio
            Add("orzech nerkowca", "suche"); //  cashew nut
            Add("rodzynka", "suche"); // raisin
            Add("śliwka", "suche"); // prunes
            Add("chleb", "pieczywo"); // bread
            Add("chleb razowy", "pieczywo"); //  wholemeal bread
            Add("bułka", "pieczywo"); // bun
            Add("rogal", "pieczywo"); // croissant
            Add("bagietka", "pieczywo"); // baguette
            Add("ryż", "suche"); // rice
            Add("pita", "pieczywo"); // pita
            Add("sucharek", "suche"); // rusk
            Add("obwarzanek", "pieczywo"); // bagel
            Add("tost", "pieczywo"); // toast
            Add("kasza", "suche"); // grits
            Add("płatki owsiane", "suche"); // oatmeal
            Add("mąka", "suche"); // flour
            Add("makaron", "suche"); // pasta
            Add("makaron", "suche"); // noodles
            Add("sól", "przyprawy"); // salt
            Add("czarny pieprz", "przyprawy"); // black pepper
            Add("chilli", "przyprawy"); // chilli
            Add("imbir", "warzywa"); // ginger
            Add("gałka muszkatołowa", "przyprawy"); // nutmeg
            Add("curry", "przyprawy"); // curry
            Add("cynamon", "przyprawy"); // cinnamon
            Add("kardamon", "przyprawy"); // cardamom
            Add("kminek", "przyprawy"); // caraway seed
            Add("kmin rzymski", "przyprawy"); // cumin
            Add("szafran", "przyprawy"); // saffron
            Add("kolendra", "przyprawy"); // coriander seeds
            Add("goździk", "przyprawy"); // clove
            Add("wanilia", "przyprawy"); // vanilla
            Add("kurkuma", "przyprawy"); // turmeric
            Add("musztarda", "sosy"); // mustard
            Add("keczup", "sosy"); // ketchup
            Add("majonez", "sosy"); // mayonnaise
            Add("ocet", "sosy"); // vinegar
            Add("dorsz", "morskie"); // cod
            Add("flądra", "morskie"); // flounder
            Add("halibut", "morskie"); // halibut
            Add("homar", "morskie"); // lobster
            Add("kalmar", "morskie"); // squid
            Add("karp", "morskie"); // carp
            Add("kawior", "morskie"); // caviar	
            Add("krab", "morskie"); // crab
            Add("krewetka", "morskie"); // shrimp
            Add("langusta", "morskie"); // spiny lobster
            Add("łosoś", "morskie"); // salmon
            Add("makrela", "morskie"); // mackerel
            Add("małż", "morskie"); // mussel
            Add("mintaj", "morskie"); // walleye pollock
            Add("ostryga", "morskie"); // oyster
            Add("pstrąg", "morskie"); // trout
            Add("rak", "morskie"); // crawfish
            Add("ryba maślana", "morskie"); // butterfish
            Add("sandacz", "morskie"); // zander
            Add("sardynka", "morskie"); // sardine
            Add("sieja", "morskie"); // powan
            Add("sola", "morskie"); // sole
            Add("strzykwa", "morskie"); // sea cucumber
            Add("surimi", "morskie"); // 	surimi
            Add("szczupak", "morskie"); // pike
            Add("szprotka", "morskie"); // sprat
            Add("śledź", "morskie"); // herring
            Add("tuńczyk", "morskie"); // tuna
            Add("trepang", "morskie"); // strzykwa 		
            Add("węgorz", "morskie"); // eel
            Add("przegrzebek bagienny", "morskie"); // scallop
            Add("cukier", "suche"); // sugar
            Add("miód", "suche"); // honey
            Add("dżem", "suche"); // jam
            Add("guma do żucia", "slodycze"); // chewing gum
            Add("marmolada", "suche"); // marmalade
            Add("galaretka", "suche"); // jelly
            Add("lizak", "slodycze"); // lollipop
            Add("herbatnik", "slodycze"); // biscuit
            Add("ciasto", "slodycze"); // cake
            Add("szarlotka", "slodycze"); // apple pie
            Add("sernik", "slodycze"); // cheesecake
            Add("piernik", "slodycze"); // gingerbread
            Add("pączek", "pieczywo"); // doughnut
            Add("czekolada", "slodycze"); // chocolate
            Add("bita śmietana", "nabial"); // whipped cream
            Add("masło", "nabial"); // butter
            Add("margaryna", "nabial"); // margarine
            Add("olej", "olej"); // oil
            Add("oliwa", "olej"); // olive oil
            Add("smalec", "nabial"); // lard
            Add("marchew", "warzywa"); // carrot
            Add("ziemniak", "warzywa"); // potato
            Add("burak", "warzywa"); // beet
            Add("koper", "warzywa"); // dill
            Add("rzeżucha", "warzywa"); // cress
            Add("szczypior", "warzywa"); // chives
            Add("kapusta", "warzywa"); // cabbage
            Add("brukselka", "warzywa"); // Brussels sprout
            Add("kalafior", "warzywa"); // cauliflower
            Add("brokuł", "warzywa"); // broccoli
            Add("cebula", "warzywa"); // onion
            Add("czosnek", "warzywa"); // garlic
            Add("por", "warzywa"); // leek
            Add("seler", "warzywa"); // celery
            Add("pietruszka", "warzywa"); // parsley
            Add("pomidor", "warzywa"); // tomato
            Add("papryka", "warzywa"); // pepper
            Add("ogórek", "warzywa"); // cucumber
            Add("cukinia", "warzywa"); // courgette
            Add("kabaczek", "warzywa"); //
            Add("dynia", "warzywa"); // pumpkin
            Add("bakłażan", "warzywa"); // eggplant
            Add("szpinak", "nabial"); // spinach
            Add("sałata", "warzywa"); // lettuce
            Add("szczaw", "warzywa"); // sorrel
            Add("rzodkiew", "warzywa"); // radish
            Add("rzepa", "warzywa"); // turnip
            Add("fasola", "warzywa"); // beans
            Add("fasolka szparagowa", "warzywa"); // string bean
            Add("groszek zielony", "warzywa"); // pea
            Add("bób", "warzywa"); // broad bean
            Add("soczewica", "warzywa"); // lentil
            Add("soja", "warzywa"); // soya
            Add("szparagi", "warzywa"); // asparagus
            Add("karczoch", "warzywa"); // artichoke
            Add("kukurydza", "warzywa"); // corn
            Add("chrzan", "warzywa"); // horseradish
            Add("rabarbar", "warzywa"); // rhubarb
            Add("rzeżucha", "warzywa"); // garden cress
            Add("szczypiorek", "warzywa"); // chives
            Add("koper", "warzywa"); // dill
            Add("pietruszka", "warzywa"); // parsley
            Add("bazylia", "przyprawy"); // basil
            Add("majeranek", "przyprawy"); // marjoram
            Add("szałwia", "przyprawy"); // sage
            Add("rozmaryn", "przyprawy"); // rosemary
            Add("tymianek", "przyprawy"); // thyme
            Add("mięta", "przyprawy"); // mint
            Add("estragon", "przyprawy"); // tarragon
            Add("kminek", "przyprawy"); // caraway seed
            Add("kolendra", "przyprawy"); // coriander
            Add("cząber", "przyprawy"); // savory
        }
    }
}
