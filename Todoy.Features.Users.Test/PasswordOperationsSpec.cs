using NSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todoy.Features.Users.Test
{
    public class PasswordOperationsSpec : nspec
    {
        void it_can_generate_a_salt()
        {
            it["creates a non-empty salt"] =
                () =>
                {
                    string salt = PasswordOperations.GenerateSalt();

                    salt.should_not_be_empty();
                };

            it["should not produce the same salt when calling generate twice"] =
                () =>
                {
                    string[] hundredSalts =
                        Enumerable
                            .Range(0, 100)
                            .Select(x => PasswordOperations.GenerateSalt())
                            .Distinct()
                            .ToArray();

                    hundredSalts.Length.should_be(100);
                };
        }

        void it_is_unable_to_produce_a_password_hash()
        {
            it["because the password is empty"] =
                () =>
                {
                    expect<InvalidOperationException>(() => PasswordOperations.GenerateHash("", "someSalt"));
                };

            it["because the salt is empty"] =
                () =>
                {
                    expect<InvalidOperationException>(() => PasswordOperations.GenerateHash("somePassword", ""));
                };
        }


        void it_can_generate_a_password_hash()
        {
            string salt = PasswordOperations.GenerateSalt();
            string password = "super-secret";


            it["can generate a verifable password hash"] =
                () =>
                {
                    string hashedPassword = PasswordOperations.GenerateHash(password, salt);

                    string rehashedPassword = PasswordOperations.GenerateHash(password, salt);

                    rehashedPassword.should_be(hashedPassword);
                };

            it["doesn't produce the same hash using the same password but a different salt"] =
                () =>
                {
                    string hashedPassword = PasswordOperations.GenerateHash(password, salt);

                    string samePasswordHashedWithDifferentSalt = PasswordOperations.GenerateHash(password, PasswordOperations.GenerateSalt());

                    samePasswordHashedWithDifferentSalt.should_not_be(hashedPassword);
                };

            it["doesn't produce the same hash using the same salt but a different password"] =
                () =>
                {
                    string hashedPassword = PasswordOperations.GenerateHash(password, salt);

                    string sameSaltHashedWithDifferentPassword = PasswordOperations.GenerateHash(password + "!", salt);

                    sameSaltHashedWithDifferentPassword.should_not_be(hashedPassword);
                };
        }
    }
}
