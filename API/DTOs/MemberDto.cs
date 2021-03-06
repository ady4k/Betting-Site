using System;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public int Money { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public PhotoDto Avatar { get; set; }
    }
}