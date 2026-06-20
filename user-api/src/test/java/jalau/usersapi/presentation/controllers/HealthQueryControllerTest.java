package jalau.usersapi.presentation.controllers;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.web.servlet.MockMvc;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(HealthQueryController.class)
class HealthQueryControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private jalau.usersapi.infrastructure.mysql.mybatis.UserMyBatisMapper userMyBatisMapper;

    @Test
    void shouldReturn200Ok() throws Exception {
        mockMvc.perform(get("/api/v1/health"))
                .andExpect(status().isOk());
    }
}