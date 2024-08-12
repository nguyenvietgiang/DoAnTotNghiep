using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using System;

namespace DoAnTotNghiep.Common
{
    public class SeedData
    {
        private readonly  DataContext _context;

        public SeedData(DataContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Accounts.Any())
            {
                string hashedPassword = Encrypt.MD5Hash("AdminPassword123");
                _context.Accounts.AddRange(
                    new Account {
                        UserID = Guid.NewGuid(),
                        Email = "admin@example.com", 
                        Password = hashedPassword, 
                        AccountRole = AccountRole.Admin, 
                        Status = true,
                    }
                );

                _context.SaveChanges();
            }
        }
    }
}
