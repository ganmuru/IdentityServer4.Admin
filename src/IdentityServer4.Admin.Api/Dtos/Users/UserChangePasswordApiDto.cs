﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Admin.Api.Dtos.Users
{
    public class UserChangePasswordApiDto<TUserDtoKey>
    {
        public TUserDtoKey UserId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}