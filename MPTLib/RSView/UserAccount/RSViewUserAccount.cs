using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MPT.RSView.UserAccount
{
    public class RSViewUserAccount
    {
        public string AccountId { get; set; }

        public ushort PasswordLength { get; set; }
        public byte[] PasswordHash { get; set; }
        public List<byte[]> PasswordHashView
        {
            get
            {
                return new List<byte[]>()
                {
                    PasswordHash.Skip(8*0).Take(8).ToArray(),
                    PasswordHash.Skip(8*1).Take(8).ToArray(),
                    PasswordHash.Skip(8*2).Take(8).ToArray(),
                    PasswordHash.Skip(8*3).Take(8).ToArray(),
                };
            }
        }

        public string Password { get; set; }
        public string[] PasswordView
        {
            get
            {
                return PasswordHashView.Select(x => BitConverter.ToString(x).Replace('-',' ')).ToArray();
            }
        }

        public string LoginMacro { get; set; }
        public string LogoutMacro { get; set; }
        public BitArray SecirityCodes { get; set; }

        public static RSViewUserAccount Deserealize(byte[] bytes)
        {

            using (var reader = new BinaryReader(new MemoryStream(bytes), Encoding.Unicode))
            {
                var account = new RSViewUserAccount();
                var idLen = reader.ReadUInt16();
                var id = reader.ReadChars(idLen);
                account.AccountId = new string(id);

                account.PasswordLength = reader.ReadUInt16();
                account.PasswordHash = reader.ReadBytes(32);


                var loginMacroLen = reader.ReadUInt16();
                if (loginMacroLen > 0)
                {
                    var loginMacro = reader.ReadChars(loginMacroLen);
                    account.LoginMacro = new string(loginMacro);
                }

                var logoutMacroLen = reader.ReadUInt16();
                if (logoutMacroLen > 0)
                {
                    var logoutMacro = reader.ReadChars(logoutMacroLen);
                    account.LogoutMacro = new string(logoutMacro);
                }

                account.SecirityCodes = new BitArray(reader.ReadBytes(2));

                var dump2 = reader.ReadUInt16();
                if (dump2 != 0)
                    throw new Exception("Parce error: DUMP2 after id");

                return account;
            }
        }
    }

    public class PassHashPair
    {
        public readonly static byte[] EmptyHash = { 0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69 };

        public string Password;
        public byte[] Hash;

        public readonly static List<PassHashPair> testPassHashList = new List<PassHashPair>()
        {
            new PassHashPair {
                Password = "",
                Hash = new byte[]
                {
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69
                } },

            new PassHashPair {
                Password = "1",
                Hash = new byte[]
                {
                        0xdc, 0xe8, 0xc8, 0x1e, 0x64, 0x94, 0x9f, 0x4e,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69
                } },

            new PassHashPair {
                Password = "2",
                Hash = new byte[]
                {
                        0x32, 0x51, 0xc6, 0x21, 0xd3, 0x1a, 0xfe, 0xdd,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                } },

            new PassHashPair {
                Password = "1111",
                Hash = new byte[]
                {
                        0x5f, 0x7d, 0x04, 0xcf, 0x53, 0x7c, 0x20, 0xbb,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69
                } },

            new PassHashPair {
                Password  = "11111",
                Hash = new byte[]
                {
                        0x5f, 0x7d, 0x04, 0xcf, 0x53, 0x7c, 0x20, 0xbb,
                        0xdc, 0xe8, 0xc8, 0x1e, 0x64, 0x94, 0x9f, 0x4e,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                } },
        };
    }
}