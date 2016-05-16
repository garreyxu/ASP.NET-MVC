namespace PoliceServeSystem.Models
{
    public class Users
    {
        public string Userid { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public int? UserRoleLevel { get; set; }
        public string UserName { get; set; }
        public int? PExpire { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string WorkPhone { get; set; }
        public string Division { get; set; }
        public string AgencyName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Zip { get; set; }
        public string BadgeNo { get; set; }
        public int? UserActive { get; set; }
        public int? AgencyActive { get; set; }
        public int IsAgencyExpired { get; set; }
        public string Agency { get; set; }
        public int? AccessLevel { get; set; }
        public string NotificationType { get; set; }
    }
}