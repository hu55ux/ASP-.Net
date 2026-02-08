namespace ASP.Net_Rules.Concrete

{
    public class CheckDirector
    {
        public bool MakeUserChecker(User user)
        {
            UsernameChecker userNameChecker = new(); // Birinci zincir elemanı
            PasswordChecker passwordChecker = new(); // İkinci zincir elemanı
            EmailChecker emailChecker = new(); // Üçüncü zincir elemanı
            userNameChecker.Next = passwordChecker; // Burada biz sıralamanı düzəldirik ki yoxlama sırası belə olsun: istifadəçi adı -> şifrə -> email
            passwordChecker.Next = emailChecker; // Burada biz sıralamanı düzəldirik
            return userNameChecker.Check(user);// Artıq yoxlama prosesini ilk zincir elemanından başlatırıq
        }
    }
}
