using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MPT.RSView.UserAccount;


namespace MPT.RSView.Password.Tests
{
    [TestClass()]
    public class RSViewUserAccountTests
    {
        static List<RSViewUserAccount.PassHashPair> testPassHashList = new List<RSViewUserAccount.PassHashPair>()
        {
            new RSViewUserAccount.PassHashPair {
                Password = "",
                Hash = new byte[]
                {
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69
                } },
            new RSViewUserAccount.PassHashPair {
                Password = "1",
                Hash = new byte[]
                {
                        0xdc, 0xe8, 0xc8, 0x1e, 0x64, 0x94, 0x9f, 0x4e,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69
                } },
            new RSViewUserAccount.PassHashPair {
                Password = "2",
                Hash = new byte[]
                {
                        0x32, 0x51, 0xc6, 0x21, 0xd3, 0x1a, 0xfe, 0xdd,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                } },
            new RSViewUserAccount.PassHashPair {
                Password = "1111",
                Hash = new byte[]
                {
                        0x5f, 0x7d, 0x04, 0xcf, 0x53, 0x7c, 0x20, 0xbb,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69
                } },
            new RSViewUserAccount.PassHashPair {
                Password  = "11111",
                Hash = new byte[]
                {
                        0x5f, 0x7d, 0x04, 0xcf, 0x53, 0x7c, 0x20, 0xbb,
                        0xdc, 0xe8, 0xc8, 0x1e, 0x64, 0x94, 0x9f, 0x4e,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                        0x44, 0x94, 0x34, 0x2b, 0x7c, 0x8f, 0x27, 0x69,
                } },
        };


        [TestMethod()]
        public void UTF16Test()
        {
            var str = "DEFAULT";
            var str_code = new byte[] {
                0x44, 0,
                0x45, 0,
                0x46, 0,
                0x41, 0,
                0x55, 0,
                0x4C, 0,
                0x54, 0,};
            var str_decode = Encoding.Unicode.GetString(str_code);
            Assert.AreEqual(str_decode, str);
        }


        [TestMethod()]
        public void DeserealizeTest()
        {
            var defaultAccount =
                RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\Default.bin"));
            Assert.AreEqual(defaultAccount.AccountId, "DEFAULT");

            {
            var account =
                RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\LoginMacro.bin"));
            Assert.AreEqual(account.AccountId, "MACRO1");
            Assert.AreEqual(account.LoginMacro, "MACRO");
            Assert.IsNull(account.LogoutMacro);
            }

            {
                var account =
                    RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\LogoutMacro.bin"));
                Assert.AreEqual(account.AccountId, "MACRO2");
                Assert.IsNull(account.LoginMacro);
                Assert.AreEqual(account.LogoutMacro, "MACRO");
            }

            {
                var account =
                    RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\Password.bin"));
                Assert.AreEqual(account.AccountId, "PASSWORD");
                Assert.IsNull(account.LoginMacro, null);
                Assert.AreEqual(account.LogoutMacro, null);
            }
            /*
            {
                var account =
                    RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\АБВГДЕЖЗ.bin"));
                Assert.AreEqual(account.AccountId, "АБВГДЕЖЗ");
                Assert.AreEqual(account.LoginMacro, "");
                Assert.AreEqual(account.LogoutMacro, "");
            }
            */
        }


        [TestMethod]
        public void PasswordTest()
        {
            var account1x4 = RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\Passwords\1111.bin"));
            var account1x8 = RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\Passwords\11111111.bin"));
            var account1234 = RSViewUserAccount.Deserealize(File.ReadAllBytes(@"_TestData\RSView\Users\Passwords\1111222233334444.bin"));


        }
    }
}