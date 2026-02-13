//var builder = WebApplication.CreateBuilder(args);
//// Add services to the container.
//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//var app = builder.Build();
//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}
//app.UseAuthorization();
//app.MapControllers();
//app.Run();


/*
                                                                                    ASP.Net Core Rules

ASP .Net Core nədir?
ASP.NET Core, .NET istifadə edərək müasir veb tətbiqləri qurmaq üçün çarpaz platformalı, yüksək performanslı, açıq mənbəli bir çərçivədir . 
Çərçivə genişmiqyaslı tətbiq inkişafı üçün hazırlanmışdır və istənilən ölçülü iş yükünü idarə edə bilir, bu da onu müəssisə səviyyəli tətbiqlər 
üçün etibarlı bir seçim halına gətirir.

Vahid çərçivə : ASP.NET Core, bütün veb inkişaf ehtiyaclarınızı ödəmək üçün daxili istehsala hazır komponentlərə 
malik tam və tam inteqrasiya olunmuş veb çərçivəsidir.

Tam yığım məhsuldarlığı : Tək bir inkişaf çərçivəsindən istifadə edərək, 
komandanızın ön hissədən arxa hissəyə qədər tam yığımda işləməsini təmin etməklə daha sürətli daha çox tətbiq yaradın.

Dizaynla Təhlükəsiz : ASP.NET Core, təhlükəsizlik baxımından əsas məsələ olaraq qurulmuşdur və identifikasiya, 
avtorizasiya və məlumatların qorunması üçün daxili dəstəyi özündə birləşdirir.

Buluda hazır : İstər öz məlumat mərkəzlərinizə, istərsə də buluda yerləşdirməyinizdən asılı olmayaraq, 
ASP.NET Core yerləşdirməni, monitorinqi və konfiqurasiyanı sadələşdirir.

Performans və ölçeklenebilirlik : ASP.NET Core-un sənayedə aparıcı performansı ilə ən tələbkar iş yüklərinin öhdəsindən gəlin.

Etibarlı və yetkin : ASP.NET Core, Bing, Xbox, Microsoft 365 və Azure da daxil olmaqla dünyanın ən böyük 
xidmətlərindən bəziləri tərəfindən hipermiqyasda istifadə olunur və sınaqdan keçirilir.


ASP .Net Core əsas yanaşmaları

ASP .Net Core 3 əsas yanaşma varki bunlar 
1. Web Forms - ASP.NET Web Forms, sürətli veb tətbiqetmə inkişafı üçün vizual komponent əsaslı yanaşma təqdim edir.  Amma bu yanaşma müasir veb inkişafında az istifadə olunur.
Bu yanaşmaya həmdə Code behind yanaşması deyilir. Yəni biz HTML kodlarımızı .aspx faylında yazırıq, və onun arxasında yerləşən .aspx.cs faylında isə C# kodlarımızı yazırıq. Bu yanaşma çox mənasızdır çünki kodların idarə olunması çətinləşir.
Düşünün ki biz sadəcə bir düyməyə kliklədikdə nə qədər kod yazmalıyıq, həmçinin böyük layihələrdə bu yanaşma kodların qarışmasına səbəb olur.

2. Web App(Razor Pages) - ASP.NET Razor Pages, sadə və məhsuldar veb tətbiqləri yaratmaq üçün səhifə əsaslı yanaşma təqdim edir. 
Bu yanaşmada project yaranan zaman belə bir structure yaranır:

- Connected Services - Burda layihəyə əlavə edilən xidmətlər yerləşir.

- Dependencies - Burda layihənin asılılıqları yerləşir. Yəni layihədə istifadə olunan NuGet paketləri və digər asılılıqlar.

- Properties - Burda layihənin xüsusiyyətləri yerləşir. Məsələn layihənin adı, versiyası və s.

- wwwroot - Burda layihənin statik faylları yerləşir. Məsələn CSS, JavaScript, şəkillər və s.

- Pages - Burda Razor Pages faylları yerləşir. Hər bir Razor Page iki fayldan ibarətdir: .cshtml və .cshtml.cs. .cshtml faylında HTML və Razor sintaksisi ilə istifadəçi interfeysi yaradılır, .cshtml.cs faylında isə C# kodu yazılır.
Buda code behind yanaşmasına bənzəyir amma burada kodlar səhifə səviyyəsində idarə olunur və kodların qarışması olmur.

- appsettings.json - Burda layihənin konfiqurasiya parametrləri yerləşir. Məsələn, məlumat bazası bağlantı sətirləri, tətbiq parametrləri və s.

- Program.cs - Burda tətbiqin başlanğıc nöqtəsi yerləşir. Burada tətbiqin konfiqurasiyası və xidmətlərin qeydiyyatı həyata keçirilir.

Program.cs-də əsasən aşağıdakı kod olur:

Codumuzda Builder Pattern istifadə olunur. Yəni əvvəlcə bir Builder yaradılır, sonra ona xidmətlər əlavə olunur, sonra isə tətbiq qurulur və işə salınır.
Həmçinin CoR(Chain of Responsibility) pattern istifadə olunur. Yəni HTTP sorğuları bir sıra middleware-lərdən keçir və hər bir middleware sorğunu emal edə bilər və ya növbəti middleware-ə ötürə bilər.
Əgər hər hansı bir middleware sorğunu emal etməzsə, növbəti middleware-ə ötürülmür və cavab dərhal müştəriyə göndərilir.

var builder = WebApplication.CreateBuilder(args); - Bu sətr yeni bir WebApplicationBuilder nümunəsi yaradır.
// Add services to the container.
builder.Services.AddRazorPages();- Bu sətr Razor Pages xidmətini tətbiqə əlavə edir.
var app = builder.Build();- Bu sətr tətbiqi qurur.
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())- Bu blok inkişaf mühiti yoxdursa, səhv idarəetmə və HSTS istifadə edir.
{
    app.UseExceptionHandler("/Error");- Bu sətr səhv idarəetmə səhifəsini təyin edir.
    app.UseHsts();- Bu sətr HSTS istifadə edir. HSTS - HTTP Strict Transport Security deməkdir və təhlükəsiz əlaqələri təmin edir.
}
app.UseHttpsRedirection(); - Bu sətr HTTP sorğularını HTTPS-ə yönləndirir.
app.UseStaticFiles();- Bu sətr statik faylların xidmət edilməsini təmin edir.
app.UseRouting();- Bu sətr marşrutlaşdırmanı aktivləşdirir.
app.UseAuthorization();- Bu sətr avtorizasiyanı aktivləşdirir.
app.MapRazorPages();- Bu sətr Razor Pages marşrutlarını xəritələndirir.
app.Run();- Bu sətr tətbiqi işə salır.




2. MVC (Model-View-Controller) - ASP.NET MVC, tətbiqləri modellər, görünüşlər və nəzarətçilər kimi komponentlərə ayıran güclü bir dizayn naxışı təqdim edir.
Burada Model - məlumatları təmsil edir, View - istifadəçi interfeysini təmsil edir, Controller isə istifadəçi sorğularını idarə edir və uyğun cavabları qaytarır.
MVC Web Formsdan fərqi odurki burada səhifə fayllarının arxasında kod yazılmır, əvəzinə nəzarətçi siniflərində kod yazılır.

Bu yanaşma böyük və mürəkkəb tətbiqlər üçün uyğundur və kodun yenidən istifadəsini və test edilməsini asanlaşdırır.
Qovluqlar structure belədir:
- Controllers - Burda nəzarətçi sinifləri yerləşir. Hər bir nəzarətçi HTTP sorğularını qəbul edir, onları emal edir və uyğun cavabları qaytarır.

- Models - Burda model sinifləri yerləşir. Modellər tətbiqin məlumat strukturlarını və biznes məntiqini təmsil edir.

- Views - Burda görünüş faylları yerləşir. Hər bir görünüş HTML və Razor sintaksisi ilə istifadəçi interfeysini yaradır.

- wwwroot - Burda layihənin statik faylları yerləşir. Məsələn CSS, JavaScript, şəkillər və s.

- appsettings.json - Burda layihənin konfiqurasiya parametrləri yerləşir. Məsələn, məlumat bazası bağlantı sətirləri, tətbiq parametrləri və s.

- Program.cs - Burda tətbiqin başlanğıc nöqtəsi yerləşir. Burada tətbiqin konfiqurasiyası və xidmətlərin qeydiyyatı həyata keçirilir.

Program.cs-də əsasən aşağıdakı kod olur:

var builder = WebApplication.CreateBuilder(args); - Bu sətr yeni bir WebApplicationBuilder nümunəsi yaradır.
// Add services to the container.
builder.Services.AddControllersWithViews();- Bu sətr MVC xidmətini tətbiqə əlavə edir.
var app = builder.Build();- Bu sətr tətbiqi qurur.
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) - Bu blok inkişaf mühiti yoxdursa, səhv idarəetmə və HSTS istifadə edir.
{
    app.UseExceptionHandler("/Home/Error");- Bu sətr səhv idarəetmə səhifəsini təyin edir.
    app.UseHsts();
}
{
    app.UseExceptionHandler("/Home/Error");- Bu sətr səhv idarəetmə səhifəsini təyin edir.
    app.UseHsts();- Bu sətr HSTS istifadə edir. HSTS - HTTP Strict Transport Security deməkdir və təhlükəsiz əlaqələri təmin edir.
}
app.UseHttpsRedirection(); - Bu sətr HTTP sorğularını HTTPS-ə yönləndirir.
app.UseStaticFiles();- Bu sətr statik faylların xidmət edilməsini təmin edir.
app.UseRouting();- Bu sətr marşrutlaşdırmanı aktivləşdirir.
app.UseAuthorization();- Bu sətr avtorizasiyanı aktivləşdirir.
app.MapControllerRoute(  - Bu sətr MVC nəzarətçi marşrutlarını xəritələndirir.
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();- Bu sətr tətbiqi işə salır.

3. Web API - ASP.NET Core Web API, RESTful xidmətlər yaratmaq üçün optimallaşdırılmışdır və JSON və XML kimi müxtəlif formatlarda məlumat mübadiləsini dəstəkləyir.
Bu yanaşma əsasən server və müştəri arasında məlumat mübadiləsi üçün istifadə olunur və mobil tətbiqlər, tək səhifə tətbiqləri (SPA) və digər veb xidmətləri üçün idealdır.
Burada əsasən nəzarətçi sinifləri yaradılır və onlar HTTP sorğularını qəbul edir, emal edir və uyğun cavabları qaytarır.
Bu yanaşma daha düzgün və yüngül çərçivə təqdim edir, çünki burada istifadəçi interfeysi komponentləri yoxdur. Burada artıq html və ya Razor page yazmaq lazım deyil.
Bu hissə sadəcə dataları JSON və ya XML formatında göndərmək və qəbul etmək üçündür.
Qovluqlar structure belədir:
- Controllers - Burda nəzarətçi sinifləri yerləşir. Hər bir nəzarətçi HTTP sorğularını qəbul edir, onları emal edir və uyğun cavabları qaytarır.
- Models - Burda model sinifləri yerləşir. Modellər tətbiqin məlumat strukturlarını və biznes məntiqini təmsil edir.
- wwwroot - Burda layihənin statik faylları yerləşir. Məsələn CSS, JavaScript, şəkillər və s.
- appsettings.json - Burda layihənin konfiqurasiya parametrləri yerləşir. Məsələn, məlumat bazası bağlantı sətirləri, tətbiq parametrləri və s.
- Program.cs - Burda tətbiqin başlanğıc nöqtəsi yerləşir. Burada tətbiqin konfiqurasiyası və xidmətlərin qeydiyyatı həyata keçirilir.
Program.cs-də əsasən aşağıdakı kod olur:
var builder = WebApplication.CreateBuilder(args); - Bu sətr yeni bir WebApplicationBuilder nümunəsi yaradır.
// Add services to the container.
builder.Services.AddControllers();- Bu sətr Web API xidmətini tətbiqə əlavə edir.
var app = builder.Build();- Bu sətr tətbiqi qurur.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())- Bu blok inkişaf mühiti varsa, OpenAPI istifadə edir.
{
    app.MapOpenApi();- Bu sətr OpenAPI marşrutlarını xəritələndirir.
}
app.UseAuthorization();- Bu sətr avtorizasiyanı aktivləşdirir.
app.MapControllers();- Bu sətr Web API nəzarətçi marşrutlarını xəritələndirir.
app.Run();- Bu sətr tətbiqi işə salır.


                                                                                    
                                                                                Code View 
1. İlk olaraq gəlin bir kiçik HTTP server yaradaq ASP.Net Core istifadə edərək.
Ilk öncə bir class yaradaq adı WebHost olsun. Bu class-da biz serverin port nömrəsini və pathBase-i saxlayacağıq və HttpListener obyektini yaradacağıq.

using System.Net;

class WebHost
{
    int port;
    string pathBase = @"..\..\..\";
    HttpListener listener;
    public WebHost(int port)
    {
        this.port = port;
    }
    public void Run()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        listener.Start();
        Console.WriteLine($"Server is running on {port}/");

        while (true)
        {
            var context = listener.GetContext();
            Task.Run(() => HandleRequest(context));
        }
    }

    private void HandleRequest(HttpListenerContext context)
    {
        var url = context.Request.RawUrl;
        var path = $@"{pathBase}{url.Split("/").Last()}";
        var response = context.Response;
        StreamWriter writer = new StreamWriter(response.OutputStream);
        try
        {
            var src = File.ReadAllText(path);
            writer.WriteLine(src);
        }
        catch (Exception)
        {
            var src = File.ReadAllText($@"{pathBase}404.html");
            throw;
        }
        finally
        {
            writer.Close();
        }
    }
}

İndi bir neçə html səhifəsi yaradırıq ki burada məsələn index.html, about.html, contact.html və 404.html səhifələri.

index.html
<!DOCTYPE html>
<html>
<head>
    <title>Home Page</title>
</head>
<body>
    <h1>Welcome to the Home Page</h1>
    <a href="/about.html">About Us</a><br>
    <a href="/contact.html">Contact Us</a>
</body>
</html>

about.html
<!DOCTYPE html>
<html>
<head>
    <title>About Us</title>
</head>
<body>
    <h1>About Our Company</h1>
    <p>We are a leading company in our industry.</p>
    <a href="/index.html">Home</a><br>
    <a href="/contact.html">Contact Us</a>
</body>
</html>

contact.html
<!DOCTYPE html>
<html>
<head>
    <title>Contact Us</title>
</head>
<body>
    <h1>Contact Information</h1>
    <p>Email: dsas@example.com</p>
    <a href="/index.html">Home</a><br>
    <a href="/about.html">About Us</a>
</body>
</html>

404.html
<!DOCTYPE html>
<html>
<head>
    <title>404 Not Found</title>
</head>
<body>
    <h1>404 - Page Not Found</h1>
    <p>The page you are looking for does not exist.</p>
    <a href="/index.html">Home</a>
</body>
</html>
İndi isə əsas proqramımızı yazırıq ki burada WebHost obyektini yaradıb işə salaq.



new WebHost(27001).Run();


Bu bizim sadə HTTP serverimizdir ki, ASP.Net Core istifadə edir. Bu server localhost üzərindən 27001 portunda işləyəcək və
müxtəlif html səhifələrini xidmət edəcək.

Amma bizim ASP projectlərində gördüyümüz program.cs-də olan kodlar bizim yazdığımız serverin işləmə prinsipi ilə eynidir.
Sadəcə ASP daha kompleks və geniş imkanlar təqdim edir ki, bu da böyük və mürəkkəb veb tətbiqləri yaratmaq üçün uyğundur.
Burada COR(Chain of Responsibility) pattern istifadə olunur ki, bu da HTTP sorğularının bir sıra middleware-lərdən keçməsini təmin edir.
Hər bir middleware sorğunu emal edə bilər və ya növbəti middleware-ə ötürə bilər. Və hər birinin öz müəyyən işi var ki məsələn bir 
mal sifarişi zamanı authorization olmazsa add to Basket işi işləməz buda bizə daha təhlükəsiz və strukturlaşdırılmış kod yazmağa imkan verir.


İndi gəlin mini COR nümunəsi yazaq ki, istifadəçi qeydiyyatı zamanı müxtəlif yoxlamalar aparaq.



interface IChecker // Bu interface Chain of Responsibility pattern üçün istifadə olunur.
{
    public IChecker Next { get; set; }
    bool Check(object obj);
}

class UsernameChecker : BaseChecker // Bu class istifadəçi adını yoxlayır.
{
    public override bool Check(object obj)
    {
        var user = obj as User;
        if (string.IsNullOrEmpty(user.Username) || user.Username.Length < 3)
        {
            Console.WriteLine("Username is invalid.");
            return false;
        }
        if (Next != null)
        {
            return Next.Check(obj);
        }
        return true;
    }
}

class PasswordChecker : BaseChecker // Bu class şifrəni yoxlayır.
{
    public override bool Check(object obj)
    {
        var user = obj as User;
        if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 6)
        {
            Console.WriteLine("Password is invalid.");
            return false;
        }
        if (Next != null)
        {
            return Next.Check(obj);
        }
        return true;
    }
}

class EmailChecker : BaseChecker // Bu class email-i yoxlayır.
{
    public override bool Check(object obj)
    {
        var user = obj as User;
        if (string.IsNullOrEmpty(user.Email) || !user.Email.Contains("@"))
        {
            Console.WriteLine("Email is invalid.");
            return false;
        }
        if (Next != null)
        {
            return Next.Check(obj);
        }
        return true;
    }
}

using ASP.Net_Rules.Concrete;

User user = new() { Username = "salam", Password = "Salam12345", Email = "salam@salam.com" };
var director = new CheckDirector();
Console.WriteLine(director.MakeUserChecker(user));






İndi gəlin bir mini ASP quraq ki, sadə bir HTTP server işləsin və müxtəlif səhifələr göstərsin.
*/








/*





                                                                                MVC

MVC (Model-View-Controller) dizayn naxışı, proqram təminatının inkişafında geniş istifadə olunan bir nümunədir.
Burada Model - məlumatları təmsil edir, View - istifadəçi interfeysini təmsil edir, Controller isə istifadəçi sorğularını idarə edir və uyğun cavabları qaytarır.












                                                                                    Web API Introduction

ASP Web API - ASP.NET Core Web API — datanı bazadan götürüb, onu standart bir formata (JSON) salan və internet vasitəsilə digər proqramlara təqdim edən bir "ötürücü" texnologiyadır.
Yəni sadə dildə desək bu bütün öroqramların və işlətim sistemlərinin (MacOS,Linux,Windows) bir-birilə qarşılıqlı əlaqəsini təmin edir. JSON formatını dəstəkləyir. Kross-Platformdur yəni bizim yazdığımız kod 
(MacOS,Linux,Windows) serverlərinin hər birində işləyir. Olduqca yüksək sürətlidir. JWT (JSON Web Tokens) və OAuth kimi güclü təhlükəsizlik standartlarını asanlıqla tətbiq etmək olur.










                                                                                    DTO
DTO (Data Transfer Object) nümunəsi, proqram təminatında məlumatların bir hissədən digərinə ötürülməsi üçün istifadə olunan sadə obyektlərdir.
DTO bizə giriş və çıxış zamanı maximum dərəcədə lazımlı məlumatların istifadə olunmasına imkan verir.
Məsələn bizim bir istifadəçi məlumatlarımız var və bu dataların daxilində başqa digər fərqli objectlərdə saxlanılır ki, bu zaman biz sadəcə istifadəçini yaradan zaman,göndərən zaman və ya qaytaran zaman bizə əlavə məlumatlar lazım deyil.
Bu zaman isə bizə problemli dataları ötürmək üçün DTO-lar kömək edir. DTO-lar sadəcə lazım olan məlumatları saxlayır və digər əlavə məlumatları saxlamır.
Yəni DTO transfer üçün lazım olan dataları saxlayır və digər əlavə məlumatları saxlamır. Bu da bizim proqramımızın performansını artırır və təhlükəsizliyini təmin edir.





                                                                                    Mapper

Mapper classları, proqram təminatında məlumatların bir formatdan digərinə çevrilməsi üçün istifadə olunan vasitələrdir.
Məsələn bizim əvvəlki sadəcə dto ilə yazdığımız versiyada biz service daxilində həm data type conversion həmdə dataların emalını bir yerdə istifadə etmişdik.
Amma bizim service classlarımızın əsas məqsədi Serverdən gələn sorğuda olan dataların düzgün şəkildə emal olunmasıdır. 
Yəni bizim service classlarımızın əsas məqsədi dataların emalını həyata keçirməkdir, data type conversion isə bizim mapper classlarımızın əsas məqsədidir.

Biz İlk növbədə Mapper classımızı yaradırıq və burada AutoMapper kitabxanasından istifadə edirik ki, bu da bizim üçün dataların bir formatdan digərinə çevrilməsini asanlaşdırır.
və sonra isə bizim service classımızda Mapper classımızı istifadə edirik ki, bu da bizim dataların düzgün şəkildə emal olunmasını təmin edir.
daxilində bir constructor olurki buda bizə Mapper classımızı istifadə etməyə imkan verir. Mapper classımızda isə biz dataların bir formatdan digərinə çevrilməsini həyata keçiririk.
Bir şeyi də nəzərə almalıyıq ki burada iki class bir birinə çevrilən zaman eyni adda olan property-ləri yazmağa ehtiyac yoxdur, AutoMapper kitabxanası bunu avtomatik olaraq edir. Amma əgər property-lərin adları fərqlidirsə, o zaman bizə bu property-ləri manuel olaraq yazmaq lazım gəlir ki, bu da bizim MappingProfile classımızda olur.
Amma bizə məsələn mənbədə olan bir collection var və biz daxildə onun sayını bir property kimi təqdim etmək istəyirik, bu zaman isə bizə AutoMapper kitabxanasının ForMember metodundan istifadə etmək lazım gəlir ki, bu da bizim MappingProfile classımızda olur.


public class MappingProfile : Profile  // Bu class AutoMapper kitabxanasından istifadə edərək dataların bir formatdan digərinə çevrilməsini təmin edir.
{
    public MappingProfile()
    {
        CreateMap<Invoice, InvoiceResponseDTO>() // Bu sətr Invoice modelini InvoiceResponseDTO modelinə çevirmək üçün bir xəritə yaradır.
            .ForMember(dest => dest.InvoiceRowsCount, opt => opt.MapFrom(src => src.Rows.Count())); // Bu sətr isə InvoiceResponseDTO modelindəki InvoiceRowsCount sahəsini Invoice modelindəki Rows sayına uyğun olaraq doldurur.
    }
}



                                                                                Documentation
Documentation, proqram təminatının inkişafında və istifadəsində vacib bir rol oynayan bir prosesdir.
Yaxşı sənədləşdirilmiş bir proqram təminatı, istifadəçilərə və inkişafçılara proqramın necə işlədiyini, necə istifadə ediləcəyini və necə inkişaf etdiriləcəyini anlamağa kömək edir.
Məsələn Swagger documentation-da biz project haqqında məlumat, contact, license və digər başqa məlumatları göstərə bilirik.
Classlarımızda isə biz bunu summary və param kimi atributlardan istifadə edərək göstərə bilərik ki, bu da bizim API-larımızın daha yaxşı sənədləşdirilməsinə kömək edir.
Məsələn bizim bir API endpointimiz var və biz bu endpoint haqqında məlumat vermək istəyirik, bu zaman biz bu endpointin nə iş gördüyünü, hansı parametrləri qəbul etdiyini və hansı cavabları qaytardığını göstərə bilərik ki, bu da bizim API-larımızın daha yaxşı sənədləşdirilməsinə kömək edir.

class InvoiceController : ControllerBase
{
    /// <summary> // burada summary atributu istifadə edərək bu endpoint haqqında məlumat veririk.
    /// Bu endpoint yeni bir invoice yaradır.
    /// </summary>
    /// <param name="request">Bu parametrlə yeni invoice haqqında məlumat verilir.</param> // burada param atributu istifadə edərək bu endpointin hansı parametrləri qəbul etdiyini göstəririk.
    /// <returns>Bu endpoint yeni yaradılan invoice haqqında məlumat qaytarır.</returns> // burada returns atributu istifadə edərək bu endpointin hansı cavabları qaytardığını göstəririk.
    /// <example> // burada example atributu istifadə edərək bu endpointin necə istifadə ediləcəyini göstəririk.
    [HttpPost]
    public IActionResult CreateInvoice(InvoiceRequestDTO request)
    {
        // Endpointin iş loqikası burada olacaq.
    }
}





                                                                        Pagination Filtering Ordering

Pagination - Pagination, böyük məlumat dəstlərini daha kiçik, idarə edilə bilən hissələrə bölmək üçün istifadə olunan 
bir texnikadır. Bu, istifadəçilərə məlumatları daha asanlıqla gəzməyə və tapmağa imkan verir. 
Və bu həmçinin serverin yükünü azaltmağa kömək edir, çünki o, yalnız tələb olunan məlumatları göndərir.

Filtering - Filtering, məlumat dəstlərini müəyyən meyarlara əsasən süzmək üçün istifadə olunan bir texnikadır.
Bu, istifadəçilərə yalnız maraqlandıqları məlumatları görməyə imkan verir. 
Məsələn, bir məhsul kataloqunda istifadəçilər yalnız müəyyən bir kateqoriyaya aid məhsulları görmək istəyə bilərlər.

Ordering - Ordering, məlumat dəstlərini müəyyən bir sıraya görə sıralamaq üçün istifadə olunan bir texnikadır.
Bu, istifadəçilərə məlumatları istədikləri qaydada görməyə imkan verir.





                                                                    

                                                                      Validation Global Exception Handling

Validation, proqram təminatında istifadəçi girişlərinin və digər məlumatların düzgünlüyünü təmin etmək üçün istifadə olunan bir prosesdir.
Daha açıq desək inputların doğruluğunu yoxlamaq üçün istifadə olunur. Məsələn, bir istifadəçi qeydiyyatı formunda istifadəçi adı, şifrə 
və email kimi məlumatlar daxil edilir.

Bu məlumatların doğruluğunu təmin etmək üçün validation istifadə olunur. Məsələn, 
istifadəçi adı minimum 3 simvol olmalıdır, şifrə minimum 6 simvol olmalıdır və email düzgün formatda olmalıdır.

Biz bu inputları həm Front hissədə həmdə Back hissədə validasiya edə bilərik amma bəs bunların hansı daha yaxşıdır.
Burada cavab hər iki tərəfdə mütləq olmasıdır. Çünki biz həmişə birbaşa Front ilə müraciət etmirik. məsələn biz Postman kimi bir vasitə ilə birbaşa Back-ə müraciət edə bilərik və
bu zaman əgər biz yalnız Front hissədə validasiya etmişiksə, o zaman bizim Back-ə gələn məlumatların doğruluğunu təmin edə bilmərik.
Və ya sadəcə Back hissədə Validation etmişiksə bu zaman istifadəçi passvord yerində 100 dəfə "12345" Back hissəyə sorğu göndərər hər sorğudada bizim 
serverimizə çox böyük yük düşər və bu da bizim serverimizin performansını azaldar. Və ya hətta serverimizin çöküşünə səbəb ola bilər.


Validasiyanın bir neçə növü var ki bunlar:
Data Annotation Validation - Bu növ validation, model siniflərində atributlar istifadə edərək doğrulama qaydalarını təyin etməyə imkan verir. 
Məsələn, [Required], [StringLength], [EmailAddress] kimi atributlar istifadə edilərək doğrulama qaydaları təyin edilə bilər.
Ama bu növ validation yalnız sadə doğrulama qaydaları üçün uyğundur və daha kompleks doğrulama qaydaları üçün kifayət qədər güclü deyil.
Islində biz öz Attributumuzu da yarada bilərik ki, bu da bizim daha kompleks doğrulama qaydalarını təyin etməyə imkan verir. 
Məsələn, bir istifadəçi qeydiyyatı formunda istifadəçi adının unikal olmasını təmin etmək üçün bir custom validation attribute yarada bilərik.
Amma biz hər bir fərqli yoxlama üçün bir class yaratsaq bu zaman code-muz şişəcək və bu da bizim proqramımızın performansını azaldacaq. 
Bu zaman isə bizə Fluent Validation kitabxanası kömək edir ki, bu da bizim doğrulama qaydalarını daha strukturlaşdırılmış və təmiz bir şəkildə təyin etməyə imkan verir.


                                                                 
*/