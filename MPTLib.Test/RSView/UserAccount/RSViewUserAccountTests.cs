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
        Encoding utf16 = Encoding.Unicode;
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
            var str_decode = utf16.GetString(str_code);
            Assert.AreEqual(str_decode, str);
        }

        [TestMethod()]
        public void DeserealizeTest()
        {
            var defaultAccount =
                RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\Default.bin"));
            Assert.AreEqual(defaultAccount.AccountId, "DEFAULT");

            {
            var account =
                RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\LoginMacro.bin"));
            Assert.AreEqual(account.AccountId, "MACRO1");
            Assert.AreEqual(account.LoginMacro, "MACRO");
            Assert.IsNull(account.LogoutMacro);
            }

            {
                var account =
                    RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\LogoutMacro.bin"));
                Assert.AreEqual(account.AccountId, "MACRO2");
                Assert.IsNull(account.LoginMacro);
                Assert.AreEqual(account.LogoutMacro, "MACRO");
            }

            {
                var account =
                    RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\Password.bin"));
                Assert.AreEqual(account.AccountId, "PASSWORD");
                Assert.IsNull(account.LoginMacro, null);
                Assert.AreEqual(account.LogoutMacro, null);
            }
            /*
            {
                var account =
                    RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\АБВГДЕЖЗ.bin"));
                Assert.AreEqual(account.AccountId, "АБВГДЕЖЗ");
                Assert.AreEqual(account.LoginMacro, "");
                Assert.AreEqual(account.LogoutMacro, "");
            }
            */
        }

        [TestMethod]
        public void PasswordTest()
        {
            var account1x4 = RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\Passwords\1111.bin"));
            var account1x8 = RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\Passwords\11111111.bin"));
            var account1234 = RSViewUserAccount.Deserealize(File.ReadAllBytes(@"RSView\UserAccount\Data\Users\Passwords\1111222233334444.bin"));


        }
    }
}