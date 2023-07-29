using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Models;
using RuxGymAPI.VMModels;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RuxGymAPI.Repository
{
    public class EfRepository : IRepository
    {
        private RuxGymDBcontext _context;
        private static string key = "ruxgameearn1milliondolarin2024on";
        private WorldTimeAPI timeAPI = new WorldTimeAPI();
        private string code = null;
        private Timer _timer;
        private string getCode()
        {
            if (code == null)
            {

                Random rand = new Random();
                code = "";
                for (int i = 0; i < 6; i++)
                {
                    char tmp = Convert.ToChar(rand.Next(48, 58));
                    code += tmp;
                }
            }
            return code;
        }
        public EfRepository(RuxGymDBcontext context)
        {
            _context = context;
            _timer = new Timer(60000); // 60 saniye (60000 milisaniye)
            _timer.Elapsed += async (sender, e) => await TimerElapsed(sender, e);
            _timer.AutoReset = true;


        }
        public IQueryable<Player> Players => _context.Players;


        public async Task AddPlayerAsync(CreatePlayerVM data)
        {
            DateTime time = await timeAPI.GetCurrentDateTimeInTimeZone("Europe/London");
            string stringTime = time.ToString("dd.MM.yyyy HH:mm:ss");
            var player = new Player
            {
                Id = Guid.NewGuid(),
                Email = data.Email,
                UserName = data.UserName,
                Password = EncryptStringToBytes_Aes(data.Password, key),
                CreatedDate = stringTime,
                LastConnectionDate = null,
                IsOnline = false,
                IsGuest = data.IsGuest,
                IsFacebookUser = data.IsFacebookUser,
                FacebookId = data.FacebookId,

            };
            var playerStat = new PlayerStat()
            {
                PlayerId = player.Id,
            };
            var playerEnergy = new PlayerEnergy()
            {
                StartEnergyTime = stringTime,
                EndEnergyTime = stringTime,
                PlayerId = player.Id
            };
            var playerSpinTime = new PlayerSpinTime()
            {

                CreatedSpinTime = stringTime,
                PlayerId = player.Id
            };
            var playerGymItem = new PlayerGymItem()
            {
                PlayerId = player.Id,
            };
            var playerPremium = new PlayerPremium()
            {
                PlayerId = player.Id
            };
            var playerBoxing = new PlayerBoxing()
            {
                PlayerId = player.Id
            };
            player.PlayerStat = playerStat;
            player.PlayerEnergy = playerEnergy;
            player.PlayerSpinTime = playerSpinTime;
            player.PlayerGymItem = playerGymItem;
            player.PlayerPremium = playerPremium;
            player.PlayerBoxing = playerBoxing;




            await _context.AddAsync(player);
            await _context.SaveChangesAsync();



        }
        public async Task<Player> GetPlayerData(string id)
        {
            Player? player = await _context.Players
                .Include(q => q.PlayerStat)
                .Include(q => q.PlayerEnergy)
                .Include(w => w.PlayerSpinTime)
                .Include(e => e.PlayerGymItem)
                .Include(c => c.PlayerPremium)
                .Include(x => x.PlayerBoxing).FirstOrDefaultAsync(v => v.Id.Equals(Guid.Parse(id)));
            return player;
        }

       
        public async Task<bool> UpdatePlayerOnlineAsync(string id, bool isOnline)
        {
            Player? currentPlayer = await _context.Players.FirstOrDefaultAsync(data => data.Id.Equals(Guid.Parse(id)));
            DateTime time = await timeAPI.GetCurrentDateTimeInTimeZone("Europe/London");
            string stringTime = time.ToString("dd.MM.yyyy HH:mm:ss");
            if (currentPlayer == null)
            {
                return false;
            }
            else
            {

                currentPlayer.IsOnline = isOnline;
                currentPlayer.LastConnectionDate = stringTime.ToString();
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task MergeGuestUser(string id, CreatePlayerVM data)
        {
            Player? player = await _context.Players.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));
            player.IsGuest = false;
            player.Email = data.Email;
            player.Password = EncryptStringToBytes_Aes(data.Password, key);
            player.UserName = data.UserName;
            await _context.SaveChangesAsync();

        }
        public async Task<bool> MergeFacebookUser(string id, string facebookId)
        {
            Player? player = await _context.Players.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));

            if (player == null)
            {
                return false;

            }
            else
            {
                player.IsGuest = false;
                player.IsFacebookUser = true;
                player.FacebookId = facebookId;
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<Player> GetFacebookUser(string facebookId)
        {
            Player? user = await _context.Players.FirstOrDefaultAsync(z => z.FacebookId.Equals(facebookId));
            if (user == null)
            {
                return user;
            }
            else
            {
                return user;
            }
        }
        public async Task<bool> SendEmailCodeAsync(string email)
        {
            var user = await _context.Players.FirstOrDefaultAsync(w => w.Email.Equals(email));
            if (user != null)
            {
                var code = getCode();
                var passwordCode = new PasswordCode
                {
                    Id = Guid.NewGuid(),
                    PlayerId = user.Id.ToString(),
                    CodeKey = code,
                    CreatedTime = DateTime.Now // Şifrenin oluşturulma zamanını ayarla
                };
                _context.PasswordCodes.Add(passwordCode);
                await _context.SaveChangesAsync();

                string text = "<h3>Password Code: </h3>" + code + " ";
                string subject = "Rux Gym Reset Password Code";
                string fromMail = "ruxgames.info@gmail.com";
                string fromPassword = "bplhoktwnarxoqvq";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = subject;
                message.To.Add(new MailAddress(email));
                message.Body = text;
                message.IsBodyHtml = true;
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                };

                smtpClient.Send(message);

                _timer.Start(); // Zamanlayıcıyı başlat

                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task DeleteCodes()
        {
            var expiredCodes = await _context.PasswordCodes
         .Where(code => code.CreatedTime.AddSeconds(60) <= DateTime.Now)
         .ToListAsync();

            _context.PasswordCodes.RemoveRange(expiredCodes);
            await _context.SaveChangesAsync();
        }
        private async Task TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string apiUrl = "http://104.40.246.13/RuxGym/api/Players/code"; // API'nin URL'i
          //  string apiUrl = "http://localhost:5210/api/Players/code"; // API'nin URL'i

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("DELETE isteği başarılı bir şekilde gönderildi.");
                }
                else
                {
                    Console.WriteLine($"DELETE isteği başarısız oldu. Hata kodu: {response.StatusCode}");
                }
            }

            _timer.Stop();


        }
        public async Task<string> ChangePasswordAsync(ResetPasswordVM data)
        {
            var passwordCode = await _context.PasswordCodes.FirstOrDefaultAsync(x => x.CodeKey.Equals(data.CodeKey));
            if (passwordCode != null)
            {
                var user = await _context.Players.FirstOrDefaultAsync(c => c.Id.Equals(Guid.Parse(passwordCode.PlayerId)));
                var newPassword = EncryptStringToBytes_Aes(data.NewPassword, key);
                if (user.Email.Equals(data.UserEmail))
                {
                    user.Password = newPassword;
                    _context.Players.Update(user);
                    _context.PasswordCodes.Remove(passwordCode);
                    await _context.SaveChangesAsync();
                    return "true";
                }
                else
                {
                    return "Wrong Email";
                }

            }
            else
            {
                return "false";
            }

        }
        public async Task<bool> ChangeUserName(string id, string userName)
        {
            Player? player = await _context.Players.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));

            if (player == null)
            {
                return false;

            }
            else
            {

                player.UserName = userName;
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<GameVersion> GetGameVersion()
        {
            var data = await _context.GameVersions.FirstOrDefaultAsync(z => z.Id == 1);
            return data;
        }
        public static string DecryptStringFromBytes_Aes(byte[] cipherText, string key)
        {
            byte[] iv = new byte[16];
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static byte[] EncryptStringToBytes_Aes(string plainText, string key)
        {
            byte[] iv = new byte[16];
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        
    }
}


