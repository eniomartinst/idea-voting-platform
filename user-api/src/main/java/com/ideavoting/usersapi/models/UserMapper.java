package com.ideavoting.usersapi.models;

import org.springframework.stereotype.Component;

@Component
public class UserMapper {

    public UserResponseDto toResponseDtoEntity(User user) {
        if (user == null) return null;
        UserResponseDto dto = new UserResponseDto();
        dto.setId(user.getId());
        dto.setName(user.getName());
        dto.setLogin(user.getLogin());
        return dto;
    }

    public User toDomainEntity(UserCreateDto dto) {
        if (dto == null) return null;
        return User.builder()
                .name(dto.getName())
                .login(dto.getLogin())
                .password(dto.getPassword())
                .build();
    }

    public User toDomainEntity(String id, UserUpdateDto dto) {
        if (dto == null) return null;
        return User.builder()
                .id(id)
                .name(dto.getName())
                .login(dto.getLogin())
                .password(dto.getPassword())
                .build();
    }
}