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

Gəlin baxaq API-da yazılan hissəyə məsələn get sorğusuna necə cavab verilir:













                                                                 
*/