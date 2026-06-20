package jalau.usersapi.presentation.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IUserCommandService;
import jalau.usersapi.core.exception.InvalidUserDataException;
import jalau.usersapi.core.exception.UserNotFoundException;
import jalau.usersapi.infrastructure.mysql.mybatis.UserMyBatisMapper;
import jalau.usersapi.presentation.dtos.UserCreateDto;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.dtos.UserUpdateDto;
import jalau.usersapi.presentation.mappers.UserMapper;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.http.MediaType;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.MvcResult;

import java.util.concurrent.CompletableFuture;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(UserCommandController.class)
class UserCommandControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private UserMyBatisMapper userMyBatisMapper;

    @MockitoBean
    private IUserCommandService userCommandService;

    @MockitoBean
    private UserMapper userMapper;

    @Autowired
    private ObjectMapper objectMapper;

    @Test
    void shouldRespond404WhenUserNotFound() throws Exception {
        User dummyUser = new User();
        dummyUser.setId("non-existent");
        
        when(userMapper.toDomainEntity(any(String.class), any(UserUpdateDto.class)))
            .thenReturn(dummyUser);
        
        CompletableFuture<User> future = new CompletableFuture<>();
        future.completeExceptionally(new UserNotFoundException("User not found"));
        
        when(userCommandService.updateUser(any(User.class)))
            .thenReturn(future);

        UserUpdateDto dto = new UserUpdateDto();
        dto.setName("Some Name");
        dto.setLogin("slogin");
        dto.setPassword("pwd123");

        MvcResult result = mockMvc.perform(patch("/api/v1/users/non-existent")
                .contentType(MediaType.APPLICATION_JSON)
                .content(objectMapper.writeValueAsString(dto)))
                .andReturn();

        mockMvc.perform(asyncDispatch(result)).andExpect(status().isNotFound());
    }
    
    @Test
    void shouldRespond400WhenServiceThrowsInvalidUserData() throws Exception {
        // Arrange
        CompletableFuture<User> future = new CompletableFuture<>();
        future.completeExceptionally(new InvalidUserDataException("Invalid name"));
        
        when(userCommandService.updateUser(any())).thenReturn(future);
        
        UserUpdateDto dto = new UserUpdateDto();
        dto.setName(""); // invalid
        
        // Act
        MvcResult result = mockMvc.perform(patch("/api/v1/users/id")
               .contentType(MediaType.APPLICATION_JSON)
               .content(objectMapper.writeValueAsString(dto)))
               .andReturn();
        
        // Assert
        mockMvc.perform(asyncDispatch(result)).andExpect(status().isBadRequest());
    }

    @Test
    void shouldRespond400WhenPatchIsEmpty() throws Exception {
        CompletableFuture<User> future = new CompletableFuture<>();
        future.completeExceptionally(
            new InvalidUserDataException("At least one field must be provided for update")
        );
        
        when(userCommandService.updateUser(any())).thenReturn(future);
        
        UserUpdateDto dto = new UserUpdateDto(); // empty
        
        MvcResult result = mockMvc.perform(patch("/api/v1/users/id")
               .contentType(MediaType.APPLICATION_JSON)
               .content(objectMapper.writeValueAsString(dto)))
               .andReturn();
        
        mockMvc.perform(asyncDispatch(result)).andExpect(status().isBadRequest());
    }
    
    @Test
    void shouldRespond200WhenUpdateIsSuccessful() throws Exception {
        User dummyUser = new User();
        dummyUser.setId("1");
        
        when(userMapper.toDomainEntity(any(String.class), any(UserUpdateDto.class)))
            .thenReturn(dummyUser);
        
        when(userCommandService.updateUser(any(User.class)))
            .thenReturn(CompletableFuture.completedFuture(dummyUser));
        
        when(userMapper.toResponseDtoEntity(any(User.class)))
            .thenReturn(new UserResponseDto() {{
                setId("1");
                setName("Name");
                setLogin("login");
            }});

        UserUpdateDto dto = new UserUpdateDto();
        dto.setName("New Name");
        dto.setLogin("new_login");
        dto.setPassword("pwd123");

        MvcResult result = mockMvc.perform(patch("/api/v1/users/1")
                .contentType(MediaType.APPLICATION_JSON)
                .content(objectMapper.writeValueAsString(dto)))
                .andReturn();

        mockMvc.perform(asyncDispatch(result)).andExpect(status().isOk());
    }

    @Test
    void shouldRespond201WhenCreateIsSuccessful() throws Exception {
        UserCreateDto dto = new UserCreateDto();
        dto.setName("Izac");
        dto.setLogin("izac");
        dto.setPassword("pwd123");

        User mappedUser = new User();
        when(userMapper.toDomainEntity(any(UserCreateDto.class))).thenReturn(mappedUser);

        User createdUser = new User("1", "Izac", "izac", "pwd123");
        when(userCommandService.createUser(mappedUser)).thenReturn(CompletableFuture.completedFuture(createdUser));

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
    void shouldRespond204WhenDeleteIsSuccessful() throws Exception {
        when(userCommandService.deleteUser("1")).thenReturn(CompletableFuture.completedFuture(null));

        MvcResult result = mockMvc.perform(delete("/api/v1/users/1")).andReturn();

        mockMvc.perform(asyncDispatch(result)).andExpect(status().isNoContent());
    }

}
