package com.ideavoting.usersapi.controllers;

import jakarta.validation.Valid;
import com.ideavoting.usersapi.models.User;
import com.ideavoting.usersapi.models.UserCreateDto;
import com.ideavoting.usersapi.models.UserResponseDto;
import com.ideavoting.usersapi.models.UserUpdateDto;
import com.ideavoting.usersapi.models.UserMapper;
import com.ideavoting.usersapi.services.UserService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.concurrent.CompletableFuture;

@RestController
@RequestMapping("/api/v1/users")
@RequiredArgsConstructor
public class UserController {

    private final UserService userService;
    private final UserMapper userMapper;

    @PostMapping
    public CompletableFuture<ResponseEntity<UserResponseDto>> createUser(
            @Valid @RequestBody UserCreateDto request) {
        User user = userMapper.toDomainEntity(request);
        
        return userService.createUser(user)
                .thenApply(createdUser -> {
                    UserResponseDto response = userMapper.toResponseDtoEntity(createdUser);
                    return ResponseEntity.status(HttpStatus.CREATED).body(response);
                });
    }

    @GetMapping
    public CompletableFuture<ResponseEntity<List<UserResponseDto>>> getUsers() {
        return userService.getUsers().thenApply(users -> {
            List<UserResponseDto> response = users.stream()
                    .map(userMapper::toResponseDtoEntity)
                    .toList();
            return ResponseEntity.ok(response);
        });
    }

    @GetMapping("/{id}")
    public CompletableFuture<ResponseEntity<UserResponseDto>> getUser(@PathVariable String id) {
        return userService.getUser(id).thenApply(user -> {
            UserResponseDto response = userMapper.toResponseDtoEntity(user);
            return ResponseEntity.ok(response);
        });
    }

    @PatchMapping("/{id}")
    public CompletableFuture<ResponseEntity<UserResponseDto>> updateUser(
            @PathVariable String id, 
            @Valid @RequestBody UserUpdateDto request) {
        User user = userMapper.toDomainEntity(id, request);
        
        return userService.updateUser(user)
                .thenApply(updatedUser -> {
                    if (updatedUser == null) {
                        return ResponseEntity.notFound().build();
                    }
                    
                    UserResponseDto response = userMapper.toResponseDtoEntity(updatedUser);
                    return ResponseEntity.ok(response);
                });
    }

    @DeleteMapping("/{id}")
    public CompletableFuture<ResponseEntity<Void>> deleteUser(@PathVariable String id) {
        return userService.deleteUser(id)
                .thenApply(v -> ResponseEntity.noContent().build());
    }
}