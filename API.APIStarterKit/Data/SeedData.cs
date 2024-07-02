using Core.Data.Helper;
using Core.Data.Models;
using Core.Data.Models.UserProfileViewModel;

namespace API.APIStarterKit.Data
{
    public class SeedData
    {
        public IEnumerable<UserProfileCRUDViewModel> GetUserProfileList()
        {
            return new List<UserProfileCRUDViewModel>
            {
                new UserProfileCRUDViewModel { FirstName = "Employee 5", LastName = "User", Email = "Employee5@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U1.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 4", LastName = "User", Email = "Employee4@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U2.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 3", LastName = "User", Email = "Employee3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U3.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 2", LastName = "User", Email = "Employee2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U4.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 1", LastName = "User", Email = "Employee1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U5.png", Address = "California", Country = "USA", },

                new UserProfileCRUDViewModel { FirstName = "Regular", LastName = "User", Email = "regular@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U6.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Technology", LastName = "User", Email = "tech@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U7.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Finance", LastName = "User", Email = "finance@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U8.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "HR", LastName = "User", Email = "hr@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U9.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Accountants", LastName = "User", Email = "accountants@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U10.png", Address = "California", Country = "USA", },
            };
        }
        public IEnumerable<EmailConfig> GetEmailConfigList()
        {
            return new List<EmailConfig>
            {
                new EmailConfig { Email = "devmlbd@gmail.com", Password = "", Hostname = "smtp.gmail.com", Port = 587, SSLEnabled = true, IsDefault = true },
                new EmailConfig { Email = "admin@myinvoicemanager.co.uk", Password = "A7Ga4gTRLQg9ASV@123", Hostname = "www.myinvoicemanager.co.uk", Port = 587, SSLEnabled = false, IsDefault = false },
                new EmailConfig { Email = "exmapl2@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587, SSLEnabled = false, IsDefault = false },
                new EmailConfig { Email = "exmapl3@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587, SSLEnabled = false, IsDefault = false },
            };
        }
        public IEnumerable<Department> GetDepartmentList()
        {
            return new List<Department>
            {
                new Department { Name = "IT", Description = "IT Department"},
                new Department { Name = "HR", Description = "HR Department"},
                new Department { Name = "Finance", Description = "Finance Department"},
                new Department { Name = "Procurement", Description = "Procurement Department"},
                new Department { Name = "Legal", Description = "Procurement Department"},
            };
        }
        public IEnumerable<SubDepartment> GetSubDepartmentList()
        {
            return new List<SubDepartment>
            {
                new SubDepartment { DepartmentId = 1, Name = "QA", Description = "QA Department"},
                new SubDepartment { DepartmentId = 1, Name = "Software Development", Description = "Software Development Department"},
                new SubDepartment { DepartmentId = 1, Name = "Operation", Description = "Operation Department"},
                new SubDepartment { DepartmentId = 1, Name = "PM", Description = "Project Management Department"},
                new SubDepartment { DepartmentId = 2, Name = "Recruitment", Description = "Recruitment Department"},
            };
        }
        public IEnumerable<Designation> GetDesignationList()
        {
            return new List<Designation>
            {
                new Designation { Name = "Project Manager", Description = "Employee Job Designation"},
                new Designation { Name = "Software Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "Head Of Engineering", Description = "Employee Job Designation"},
                new Designation { Name = "Software Architect", Description = "Employee Job Designation"},
                new Designation { Name = "QA Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "DevOps Engineer", Description = "Employee Job Designation"},
            };
        }
        public CompanyInfo GetCompanyInfo()
        {
            return new CompanyInfo
            {
                Name = "XYZ Company Limited",
                Logo = "/upload/company_logo.png",
                InvoiceNoPrefix = "INV",
                QuoteNoPrefix = "QTO",
                Address = "Washington DC, USA",
                City = "Washington DC",
                Country = "USA",
                Phone = "132546789",
                Fax = "123",
                Website = "www.wyx.com",

                ShopNo = "S123",
                StreetName = "ST234",
                PostCode = "P123",
                Office = "123456789",
                Mobile = "123456789",

                Password = "123456789",
                HostName = "123456789",
                TermsAndCondition = "Terms and Conditions – General Site Usage Last Revised: December 16, 2013 Welcome to www.lorem-ipsum.info. This site is provided as a service to our visitors and may be used for informational purposes only. Because the Terms and Conditions contain legal obligations, please read them carefully. 1. YOUR AGREEMENT By using this Site, you agree to be bound by, and to comply with, these Terms and Conditions. If you do not agree to these Terms and Conditions, please do not use this site. PLEASE NOTE: We reserve the right, at our sole discretion, to change, modify or otherwise alter these Terms and Conditions at any time. Unless otherwise indicated, amendments will become effective immediately. Please review these Terms and Conditions periodically. Your continued use of the Site following the posting of changes and/or modifications will constitute your acceptance of the revised Terms and Conditions and the reasonableness of these standards for notice of changes. For your information, this page was last updated as of the date at the top of these terms and conditions. 2. PRIVACY Please review our Privacy Policy, which also governs your visit to this Site, to understand our practices. 3. LINKED SITES This Site may contain links to other independent third-party Web sites ('Linked Sites'). These Linked Sites are provided solely as a convenience to our visitors. Such Linked Sites are not under our control, and we are not responsible for and does not endorse the content of such Linked Sites, including any information or materials contained on such Linked Sites. You will need to make your own independent judgment regarding your interaction with these Linked Sites.",
                TermsAndConditionItemSale = "TBD",
                CompanyNumber = "N123456",

                VatNumber = "V123456",
                CardPercentage = "C123456",
                IsVat = true,
                WhitelistIP = "182.168.0.1",
            };
        }


        //LMS
        public IEnumerable<EmployeeType> GetEmployeeTypeList()
        {
            return new List<EmployeeType>
            {
                new EmployeeType { Name = "Parmanent ", Description = "Parmanent"},
                new EmployeeType { Name = "Contactual ", Description = "Contactual"},
                new EmployeeType { Name = "Full Time", Description = "Full Time"},
                new EmployeeType { Name = "Part Time ", Description = "Part Time"},
                new EmployeeType { Name = "Temporary  ", Description = "Temporary "},

                new EmployeeType { Name = "Leased", Description = "Leased"},
                new EmployeeType { Name = "Other", Description = "Other"},
            };
        }
        public IEnumerable<ManageUserRoles> GetManageRoleList()
        {
            return new List<ManageUserRoles>
            {
                new ManageUserRoles { Name = "Admin", Description = "User Role: New"},
                new ManageUserRoles { Name = "General", Description = "User Role: General"},
            };
        }
    }
}
