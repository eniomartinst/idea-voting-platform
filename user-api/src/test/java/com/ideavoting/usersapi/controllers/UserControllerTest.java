package com.ideavoting.usersapi.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.ideavoting.usersapi.exceptions.InvalidUserDataException;
import com.ideavoting.usersapi.exceptions.UserNotFoundException;
import com.ideavoting.usersapi.models.User;
import com.ideavoting.usersapi.models.UserCreateDto;
import com.ideavoting.usersapi.models.UserMapper;
import com.ideavoting.usersapi.models.UserResponseDto;
import com.ideavoting.usersapi.models.UserUpdateDto;
import com.ideavoting.usersapi.services.UserService;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.http.MediaType;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.MvcResult;

import java.util.List;
import java.util.concurrent.CompletableFuture;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(UserController.class)
class UserControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private UserService userService;

    @MockitoBean
    private UserMapper userMapper;

    @Autowired
    private ObjectMapper objectMapper;

    // --- Query Tests ---
    @Test
    void shouldReturnAllUsers() throws Exception {
        User user = User.builder().id("1").name("Test User").build();
        when(userService.getUsers()).thenReturn(CompletableFuture.completedFuture(List.of(user)));

        UserResponseDto dto = new UserResponseDto();
        dto.setId("1");
        dto.setName("Test User");
        when(userMapper.toResponseDtoEntity(user)).thenReturn(dto);

        MvcResult result = mockMvc.perform(get("/api/v1/users")).andReturn();

        mockMvc.perform(asyncDispatch(result))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].name").value("Test User"));
    }

    @Test
    void shouldReturnUserById() throws Exception {
        User user = User.builder().id("1").name("enioteste").build();
        when(userService.getUser("1")).thenReturn(CompletableFuture.completedFuture(user));

        UserResponseDto dto = new UserResponseDto();
        dto.setId("1");
        dto.setName("enioteste");
        when(userMapper.toResponseDtoEntity(user)).thenReturn(dto);

        MvcResult result = mockMvc.perform(get("/api/v1/users/1")).andReturn();

        mockMvc.perform(asyncDispatch(result))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.name").value("enioteste"));
    }


    @Test
    void shouldRespond201WhenCreateIsSuccessful() throws Exception {
        UserCreateDto dto = new UserCreateDto();
        dto.setName("Izac");
        dto.setLogin("izac_login");
        dto.setPassword("pwd123");

        User mappedUser = User.builder().build();
        when(userMapper.toDomainEntity(any(UserCreateDto.class))).thenReturn(mappedUser);

        User createdUser = User.builder().id("1").name("Izac").build();
        when(userService.createUser(mappedUser)).thenReturn(CompletableFuture.completedFuture(createdUser));

        UserResponseDto responseDto = new UserResponseDto();
        responseDto.setId("1");
        when(userMapper.toResponseDtoEntity(createdUser)).thenReturn(responseDto);

        MvcResult result = mockMvc.perform(post("/api/v1/users")
                .contentType(MediaType.APPLICATION_JSON)
                .content(objectMapper.writeValueAsString(dto)))
                .andReturn();

        mockMvc.perform(asyncDispatch(result)).andExpect(status().isCreated());
    }

    @Test
    void shouldRespond200WhenUpdateIsSuccessful() throws Exception {
        User dummyUser = User.builder().id("1").build();
        
        when(userMapper.toDomainEntity(anyString(), any(UserUpdateDto.class))).thenReturn(dummyUser);
        when(userService.updateUser(any(User.class))).thenReturn(CompletableFuture.completedFuture(dummyUser));
        
        UserResponseDto responseDto = new UserResponseDto();
        responseDto.setId("1");
        when(userMapper.toResponseDtoEntity(any(User.class))).thenReturn(responseDto);

        UserUpdateDto dto = new UserUpdateDto();
        dto.setName("New Name");

        MvcResult result = mockMvc.perform(patch("/api/v1/users/1")
                .contentType(MediaType.APPLICATION_JSON)
                .content(objectMapper.writeValueAsString(dto)))
                .andReturn();

        mockMvc.perform(asyncDispatch(result)).andExpect(status().isOk());
    }

    @Test
    void shouldRespond404WhenUserNotFoundForUpdate() throws Exception {
        when(userMapper.toDomainEntity(anyString(), any(UserUpdateDto.class))).thenReturn(User.builder().build());
        
        CompletableFuture<User> future = new CompletableFuture<>();
        future.completeExceptionally(new UserNotFoundException("User not found"));
        when(userService.updateUser(any(User.class))).thenReturn(future);

        UserUpdateDto dto = new UserUpdateDto();
        dto.setName("Some Name");

        MvcResult result = mockMvc.perform(patch("/api/v1/users/non-existent")
                .contentType(MediaType.APPLICATION_JSON)
                .content(objectMapper.writeValueAsString(dto)))
                .andReturn();

        mockMvc.perform(asyncDispatch(result)).andExpect(status().isNotFound());
    }

    @Test
    void shouldRespond400WhenServiceThrowsInvalidUserData() throws Exception {
        CompletableFuture<User> future = new CompletableFuture<>();
        future.completeExceptionally(new InvalidUserDataException("Invalid name"));
        when(userService.updateUser(any())).thenReturn(future);
        
        UserUpdateDto dto = new UserUpdateDto();
        dto.setName(""); 
        
        MvcResult result = mockMvc.perform(patch("/api/v1/users/id")
               .contentType(MediaType.APPLICATION_JSON)
               .content(objectMapper.writeValueAsString(dto)))
               .andReturn();
        
        mockMvc.perform(asyncDispatch(result)).andExpect(status().isBadRequest());
    }

    @Test
    void shouldRespond204WhenDeleteIsSuccessful() throws Exception {
        when(userService.deleteUser("1")).thenReturn(CompletableFuture.completedFuture(null));

        MvcResult result = mockMvc.perform(delete("/api/v1/users/1")).andReturn();

        mockMvc.perform(asyncDispatch(result)).andExpect(status().isNoContent());
    }
}