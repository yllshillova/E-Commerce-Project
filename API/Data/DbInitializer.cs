using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(StoreContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "bob",
                    Email = "bob@test.com"
                };
                // nuk e kem perdor savechangesAsync se vet kto dy metoda merren me at pun
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");


                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new [] {"Member","Admin"});

            }





            // code if there is any product 
            if (context.Products.Any()) return;
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Apple iPhone 14 Pro Max",
                    Description =
                        "Dizajni, qëndrueshmëria, funksionet dhe teknologjia e klasit të parë, e gjithë kjo dhe shumë më tepër fshihen në modelin Apple iPhone 14 Pro Max. Dynamic Island është një përvojë interaktive dhe tërheqëse e iPhone për të gjitha njoftimet, sinjalizimet dhe aktivitetet. Dynamic Island shfaq informacione të rëndësishme, ju tregon se çfarë muzike po luan, thirrjet hyrëse të FaceTime dhe më shumë, të gjitha duke mos ndërhyrë në atë që po bëni. Imazhi i lartë i iPhone 14 Pro Max sigurohet nga një ekran 6.7 OLED Super Retina XDR me rezolucion 2796x1290 piksel në 460 ppi dhe me mbështetje për të gjithë gamën e teknologjive. Ka teknologjinë True Tone, e cila përshtat ekranin me kushtet e dritës së ambientit , duke shpëtuar sytë tuaj. ",
                    Price = 164900,
                    PictureUrl = "/images/products/iphone-14promax.jpg",
                    Brand = "Apple",
                    Type = "Phone",
                    QuantityInStock = 100
                },
                new Product
                {
                    Name = "Apple iPhone 14 Pro",
                    Description =
                        "Dizajni, qëndrueshmëria, funksionet dhe teknologjia e klasit të parë, e gjithë kjo dhe shumë më tepër fshihen në modelin Apple iPhone 14 Pro. Dynamic Island është një përvojë interaktive dhe tërheqëse e iPhone për të gjitha njoftimet, sinjalizimet dhe aktivitetet. Dynamic Island shfaq informacione të rëndësishme, ju tregon se çfarë muzike po luan, thirrjet hyrëse të FaceTime dhe më shumë, të gjitha duke mos ndërhyrë në atë që po bëni. Ekrani 6.1 OLED Super Retina XDR me mbështetje për të gjithë gamën e teknologjive kujdeset për imazhin e sipërm të iPhone 14 Pro. Mund të përmendim, për shembull, teknologjinë True Tone, e cila përshtat ekranin me kushtet e dritës së ambientit. Duke kursyer sytë tuaj. Ekrani ka rezolucion 2556x1179 piksel në 460 ppi.",
                    Price = 144900,
                    PictureUrl = "/images/products/iphone-14pro.jpg",
                    Brand = "Apple",
                    Type = "Phone",
                    QuantityInStock = 10
                },
                new Product
                {
                    Name = "Apple iPhone 11",
                    Description =
                        "IPhone 11 i ri bazohet në modelet e shkëlqyera të gjeneratës së mëparshme iPhone X dhe sjell një numër risish që mund të gjenden edhe në modelet e fundit 11 Pro. Mbi të gjitha, është performanca e pakompromistë e çipit A13 Bionic, karakteristika të shkëlqyera kamerash për fotografi, video dhe selfie, jetëgjatësi të zgjatur të baterisë, dhe më shumë. IPhone 11 vjen me një kamerë të dyfishtë të përmirësuar, në të cilin lentja XS telephoto është zëvendësuar nga një lente ultra-wide-angle. Me një këndvështrim të gjerë, ju mund të kapni gjithçka në fotografinë tuaj. Kamerat mund të punojnë së bashku, dhe me inteligjencën artificiale të procesorit A13 Bionic, ato gjithmonë krijojnë pamjen më të mirë. Shijoni edhe më shumë argëtim me kamerën e përmirësuar selfie TrueDepth. Provoni selfie Full HD slow-motion në 120 fps dhe argëtohuni",
                    Price = 64900,
                    PictureUrl = "/images/products/iphone-11.jpg",
                    Brand = "Apple",
                    Type = "Phone",
                    QuantityInStock = 100
                },
                new Product
                {
                    Name = "Apple iPhone 14",
                    Description =
                        "Dizajni, cilësia dhe qëndrueshmëria, e gjithë kjo dhe shumë më tepër fshihen në modelin Apple iPhone 14 Plus. Ekrani 6.7 OLED Super Retina XDR me mbështetje për teknologjinë True Tone, i cili përshtat ekranin me kushtet e dritës së ambientit, kujdeset për imazhin e sipërm të iPhone 14 Plus, duke ju kursyer sytë. Ekrani ka rezolucion 2778x1284 piksel në 458 ppi. iPhone 14 Plus krenohet me sistemin operativ iOS 16 dhe çipin e fuqishëm A15 Bionic. Falë këtij çifti, të gjitha aktivitetet e telefonit zhvillohen në harmoni të përsosur - shpejt dhe pa probleme.",
                    Price = 103900,
                    PictureUrl = "/images/products/iphone-14.jpg",
                    Brand = "Apple",
                    Type = "Phone",
                    QuantityInStock = 32
                },
                new Product
                {
                    Name = "Lenovo IdeaCentre Gaming 5",
                    Description =
                        "Me një procesor si AMD Ryzen™ 7, grafika NVIDIA® GeForce RTX™ dhe memorie 32 GB, ky kompjuter mund t'ju çojë gjithashtu në nivele të reja kreativiteti dhe produktiviteti. Shasia e gjerë 13.6L në një ngjyrë të dallueshme blu është e mbushur me porte dhe mund të përmirësohet për nevojat e ardhshme. Lojë si kurrë më parë - Procesorët e serisë AMD Ryzen™ 7 e çojnë lojën në një nivel të ri. Falë reagimit jashtëzakonisht të shpejtë të kompjuterit ndaj komandave tuaja, ju mund të lëvizni më shpejt, të synoni më saktë dhe të bëni përshtypje me të gjitha aftësitë tuaja.",
                    Price = 96700,
                    PictureUrl = "/images/products/Lenovo-IdeaCentre-Gaming-5.jpg",
                    Brand = "Lenovo",
                    Type = "Computer",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "ASUS ROG Strix G10DK",
                    Description =
                        "Kompjuteri i lojës ASUS ROG Strix G10DK përdor një procesor AMD Ryzen 5 5600X me 6 bërthama, i cili ka teknologjinë SMT për kryerjen më të lehtë të shumë detyrave me aftësinë për të përpunuar deri në 2 procese njëkohësisht në një bërthamë dhe është i frekuentuar në 3.7 GHz ose deri në 4.6 GHz kur overclocking në modalitetin turbo. Kompleti vjen me 16 GB memorie DDR4 (2 module / 4 slote). Kartela grafike NVIDIA GeForce GTX 1660 Ti me 6 GB memorie grafike GDDR6 kujdeset për daljen e videos. Hard disku 512 GB SSD M.2 PCIe NVMe është projektuar për sistemin, të dhënat dhe aplikacionet.",
                    Price = 79700,
                    PictureUrl = "/images/products/ASUS-ROG-Strix-G10DK.png",
                    Brand = "ASUS",
                    Type = "Computer",
                    QuantityInStock = 33
                },
                new Product
                {
                    Name = "HP Desktop M01-F2055nc",
                    Description =
                        "Me një procesor si AMD Ryzen™ 7, grafika NVIDIA® GeForce RTX™ dhe memorie 32 GB, ky kompjuter mund t'ju çojë gjithashtu në nivele të reja kreativiteti dhe produktiviteti. Shasia e gjerë 13.6L në një ngjyrë të dallueshme blu është e mbushur me porte dhe mund të përmirësohet për nevojat e ardhshme. Lojë si kurrë më parë - Procesorët e serisë AMD Ryzen™ 7 e çojnë lojën në një nivel të ri. Falë reagimit jashtëzakonisht të shpejtë të kompjuterit ndaj komandave tuaja, ju mund të lëvizni më shpejt, të synoni më saktë dhe të bëni përshtypje me të gjitha aftësitë tuaja.",
                    Price = 88900,
                    PictureUrl = "/images/products/HP-Desktop-M01-F2055nc.jpg",
                    Brand = "HP",
                    Type = "Computer",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Acer Nitro N50-640",
                    Description =
                        "Kompjuter lojrash i serisë së lojërave Nitro për argëtim të shkëlqyeshëm në definicion të lartë. Procesor me 6 bërthama Intel Core i5-12400F (2.5 GHz, TB 4.4 GHz, HyperThreading), memorie operative 16 GB DDR4, grafikë NVIDIA GeForce RTX 3060 Ti 8 GB GDDR6, 1TB SSD M.2 GPCIe, disk WiFi, pa disk, WiFi, Bluetooth 5.0, USB Type-A 3.1/3.2 Gen 2 dhe Type-C 3.2 Gen 2x2, DTS:X Ultra sound, tastierë dhe maus USB, OS Windows 11 Home.",
                    Price = 128200,
                    PictureUrl = "/images/products/Acer-Nitro-N50-640.png",
                    Brand = "Acer",
                    Type = "Computer",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "ASUS TUF Dash F15 (2022)",
                    Description =
                        "Laptopi ASUS TUF Dash F15 (FX517ZM) mahnit me dizajnin dhe qëndrueshmërinë e tij dhe është i pajisur me fuqinë për të zgjidhur çdo situatë të lojës. Performanca e shkëlqyer ofrohet nga procesori më i fundit Intel Core i gjeneratës së 12-të në kombinim me grafikën NVIDIA GeForce RTX 3060, e cila është një zgjidhje e përballueshme për entuziastët e lojërave. Laptopi është i pajisur me memorie operative 16 GB DDR5 (1x 16 GB, keni gjithsej 2 slote SO-DIMM). Hard disku i shpejtë 1 TB SSD M.2 PCIe NVMe ofron kapacitet të mjaftueshëm për sistemin, lojërat dhe aplikacionet. Ai ofron një total prej 2 slote M.2 PCIe NVMe, kështu që ju mund të zgjeroni kapacitetin e ruajtjes në të ardhmen.",
                    Price = 124900,
                    PictureUrl = "/images/products/ASUS-TUF-Dash-F15-(2022).jpg",
                    Brand = "ASUS",
                    Type = "Laptop",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Lenovo IdeaPad Gaming 3 15ACH6",
                    Description =
                        "Laptopi është i pajisur me procesor të fuqishëm AMD Ryzen 5 5600H me 6 bërthama. Memoria RAM ka kapacitet 16GB ndërsa për të dhëna ka 512 GB SSD. Ekrani 15.6'' ofron rezolucion Full HD në kombinim me kartën grafike NVIDIA GeForce RTX 3060. Ofron porte të ndryshme për lidhje dhe nuk ka sistem operativ.",
                    Price = 107900,
                    PictureUrl = "/images/products/Lenovo-IdeaPad-Gaming-3-15ACH6.png",
                    Brand = "Lenovo",
                    Type = "Laptop",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Victus by HP 16-d1993nc",
                    Description =
                        "Një laptop i mrekullueshëm 16.1 inç nga HP ka gjithçka që prisni për nevojat tuaja të lojërave. Mundësuar nga një procesor Intel Core i gjeneratës së 12-të dhe grafika fantastike për lojëra nga familja NVIDIA GeForce RTX, do t'ju japë një avantazh ndaj rivalëve tuaj. Laptopi është i pajisur me memorie 16 GB DDR5 (2 x 8 GB, keni gjithsej 2 slote SO-DIMM). Shpejtësia e transferimit të të dhënave deri në 4800 MT/s. Hard disku i shpejtë 1 TB SSD M.2 PCIe 4.0 NVMe ofron përgjigje shtesë të shkathëta për sistemin, lojërat dhe aplikacionet.",
                    Price = 77900,
                    PictureUrl = "/images/products/Victus-by-HP-16-d1993nc.jpg",
                    Brand = "HP",
                    Type = "Laptop",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Apple Watch SE2 GPS 44mm",
                    Description =
                        "E mençur, elegante dhe plot funksione - kjo është Apple Watch SE 2022.Ora është rezistente ndaj ujit deri në 50 metra, kështu që nuk e e ka problem një dush në shtëpi, shi i madh apo disa temperatura në pishinë. Kur notoni, ora do t'ju tregojë përsëri kohën e saktë të ndarjes dhe numrin e pishinave",
                    Price = 34900,
                    PictureUrl = "/images/products/Apple-Watch-SE2-GPS-44mm.jpg",
                    Brand = "Apple",
                    Type = "Watch",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Xiaomi Watch Imilab W12 (037617)",
                    Description =
                        "Monitorimi i ngopjes së gjakut me oksigjen në çdo situatë për të kapur ndryshimet alarmante. Niveli i SpO2 matet automatikisht dhe frekuenca e testimit mund të zgjidhet në aplikacion. Fytyrat e ndryshme të orës mund të shkarkohen dhe ngarkohen nga aplikacioni, të shtohen dhe ndryshohen lirisht për t'iu përshtatur situatës përkatëse. Ekzistojnë 34 fytyra të animuara të orës të disponueshme në aplikacionin e pronarit W12. Është e mundur të krijohet një screensaver individual, në mënyrë që të gjithë të zgjedhin diçka sipas shijes së tyre. Ora ka një ekran 1.32 me rezolucion 360x320 piksel. Ka mbrojtje IP68.",
                    Price = 6200,
                    PictureUrl = "/images/products/Xiaomi-Imilab-W12-(037617).jpg",
                    Brand = "Xiaomi",
                    Type = "Watch",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Apple Watch S8 GPS 41mm",
                    Description =
                        "pple Watch Series 8 është krijuar për të përballuar dhe kapërcyer shumë pengesa të përditshme. Ato janë të papërshkueshme nga pluhuri, me mbrojtje IP6X dhe janë gjithashtu të papërshkueshmenga uji deri në 50 metra, kështu që mund të përdoren, për shembull, kur notoni në pishinë apo edhe në det. Ora nuk është vetëm trajneri juaj i fitnesit, por funksionon edhe pak si vëllai juaj shëndetësor. Falë gamës së informacionit që ora mund të marrë përmes një numri sensorësh të avancuar shëndetësorë, ju do të merrni një gamë të plotë informacioni për shëndetin tuaj. ",
                    Price = 49900,
                    PictureUrl = "/images/products/Apple-Watch-S8-GPS-41mm.jpg",
                    Brand = "Apple",
                    Type = "Watch",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Xiaomi Mi Band 7",
                    Description =
                        "Matësi është i pajisur me një ekran të madh 1.62' AMOLED, në të cilin mund të lexoni lehtësisht edhe një mesazh më të shkurtër SMS ose të shihni menjëherë se kush po ju telefonon. Ekrani mbrohet nga xham mbrojtës 2.5D, kështu që zgjat me të vërtetë. Meqenëse ekrani është gjëja e parë që dikush vëren në matës, ju keni një shumëllojshmëri të gjerë të faqeve të ndryshme të orës për të zgjedhur! Mund ta modifikoni lehtësisht matësin dhe ta përshtatni saktësisht sipas shijes tuaj. Falë një algoritmi të lartë, matësi regjistron dhe gjurmon çdo lëvizje tuajën me saktësinë maksimale të mundshme. ",
                    Price = 5900,
                    PictureUrl = "/images/products/Xiaomi-Mi-Band-7.jpg",
                    Brand = "Xiaomi",
                    Type = "Watch",
                    QuantityInStock = 15
                },
                
            };

            foreach (var product in products)
            {
                // part where we list the products
                context.Products.Add(product);
            }
            // Saves all the changes made on this context
            context.SaveChanges();
        }
    }
}