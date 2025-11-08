using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Common.Mappers;

public static class UserMappers
{
    public static GetUserResult ToVewUserDto(this User user)
    {
        return new GetUserResult(user.Id, user.Name, user.Email);
    }
}