using Microsoft.EntityFrameworkCore;
using RuxGymAPI.Context;
using RuxGymAPI.Models;
using RuxGymAPI.VMModels;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace RuxGymAPI.Repository
{
    public class EfRepository : IRepository
    {
        private RuxGymDBcontext _context;
        private static string key = "ruxgameearn1milliondolarin2024on";
        private WorldTimeAPI timeAPI = new WorldTimeAPI();
        private string code = null;
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
           
        }
        public IQueryable<Player> Players => _context.Players;

        public async Task AddPlayerAsync(CreatePlayerVM data)
        {
            DateTime time = await timeAPI.GetCurrentDateTimeInTimeZone("Europe/London");
            string stringTime = time.ToString("dd.MM.yyyy HH:mm:ss");
            Guid id = Guid.NewGuid();
            await _context.Players.AddAsync(new()
            {
                Id = id,
                Email = data.Email,
                UserName = data.UserName,
                Password = EncryptStringToBytes_Aes(data.Password, key),
                CreatedDate = stringTime,
                LastConnectionDate = null,
                IsOnline = false,


            });
            await _context.PlayerStats.AddAsync(new()
            {
                ID = Guid.NewGuid(),
                ALlPower = 50,
                ArmPower = 10,
                SixpackPower = 10,
                BackPower = 10,
                LegPower = 10,
                ChestPower = 10,
                ProteinItem = 0,
                CreatinItem = 0,
                EnergyItem = 0,
                PlayerDiamond = 5,
                PlayerCash = 5000,
                PlayerSpinCount = 5,
                PlayerGoldTicket = 0,
                OlimpiaWin = 0,
                IsOlimpia = false,
                UserId = id,




            });
            await _context.PlayerGymItems.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                DumbbellPressItem = 0,
                AbsItem = 0,
                SquatItem = 0,
                DeadLiftItem = 0,
                BenchPressItem = 0,
                UserID = id

            });
            await _context.PlayerEnergies.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                PlayerCurrentEnergy = 100,
                StartEnergyTime = stringTime,
                EndEnergyTime = stringTime,
                PlayerId = id

            });
            await _context.SpinDateTimes.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                CreatedSpinTime = stringTime,
                PlayerId = id

            });
            await _context.PlayerPremia.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                isPremium = false,
                EndPremiumDay = null,
                UserId = id

            });

            await _context.SaveChangesAsync();
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
        public async Task<bool> SendEmailCodeAsync(string email)
        {
            var user = await _context.Players.FirstOrDefaultAsync(w => w.Email.Equals(email));
            if (user != null)
            {
                _context.PasswordCodes.Add(new PasswordCode { Id = Guid.NewGuid(), UserID = user.Id.ToString(), CodeKey = getCode() });
                await _context.SaveChangesAsync();
                string text = "<h3>Password Code: </h3>" + getCode() + " ";
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



                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> ChangePasswordAsync(ResetPasswordVM data)
        {
            var passwordCode = await _context.PasswordCodes.FirstOrDefaultAsync(x => x.CodeKey.Equals(data.CodeKey));
            if (passwordCode != null)
            {
                var user = await _context.Players.FirstOrDefaultAsync(c => c.Id.Equals(Guid.Parse(passwordCode.UserID)));
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


