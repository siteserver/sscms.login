﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Enums;
using SSCMS.Models;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class RegisterController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] User request)
        {
            var config = await _configRepository.GetAsync();
            var (user, errorMessage) = await _userRepository.InsertAsync(request, request.Password, config.IsUserRegistrationChecked, PageUtils.GetIpAddress(Request));
            if (user == null)
            {
                return this.Error($"用户注册失败：{errorMessage}");
            }

            await _statRepository.AddCountAsync(StatType.UserRegister);

            return new BoolResult
            {
                Value = user.Checked
            };
        }
    }
}
