// <copyright file="UserPresenter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Helpers;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.API.Presenters;

public static class UserPresenter
{
    public static UserResponseDto ToHttp(User user)
    {
        return new UserResponseDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }

    public static PaginationResponseDto<UserResponseDto> ToHttp(IEnumerable<User> users, int count, int page, int itemsPerPage)
    {
        var usersResponse = users.Select(ToHttp);

        return PaginationHelper.FormatResponse(usersResponse, count, page, itemsPerPage);
    }
}