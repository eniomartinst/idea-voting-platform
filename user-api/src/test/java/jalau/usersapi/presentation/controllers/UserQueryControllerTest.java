package jalau.usersapi.presentation.controllers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IUserQueryService;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.mappers.UserMapper;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.MvcResult;

import java.util.List;
import java.util.concurrent.CompletableFuture;

import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.asyncDispatch;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(UserQueryController.class)
class UserQueryControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private jalau.usersapi.infrastructure.mysql.mybatis.UserMyBatisMapper userMyBatisMapper;

    @MockitoBean
    private IUserQueryService userQueryService;

    @MockitoBean
    private UserMapper userMapper;

    @Test
    void shouldReturnAllUsers() throws Exception {
        // Usando dados de teste genéricos
        User user = new User("1", "Test User", "testlogin", "pwd123");
        when(userQueryService.getUsers()).thenReturn(CompletableFuture.completedFuture(List.of(user)));

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
        // Usando dados de teste genéricos
        User user = new User("1", "enioteste", "eniologin", "pwd123");
        when(userQueryService.getUser("1")).thenReturn(CompletableFuture.completedFuture(user));

        UserResponseDto dto = new UserResponseDto();
        dto.setId("1");
        dto.setName("enioteste");
        when(userMapper.toResponseDtoEntity(user)).thenReturn(dto);

        MvcResult result = mockMvc.perform(get("/api/v1/users/1")).andReturn();

        mockMvc.perform(asyncDispatch(result))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.name").value("enioteste"));
    }
}