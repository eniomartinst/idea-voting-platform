package jalau.usersapi.infrastructure.mongodb.repositories;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mongodb.entities.UserDocument;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.data.mongodb.core.query.Query;

import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UserRepositoryTest {

    @Mock
    private MongoTemplate mongoTemplate;

    @InjectMocks
    private UserRepository userRepository;

    private User userDomain;
    private UserDocument userDocument;

    @BeforeEach
    void setUp() {
        // Objeto de Domínio simulado
        userDomain = new User();
        userDomain.setId("660c1234");
        userDomain.setName("Naruto");
        userDomain.setLogin("naruto");
        userDomain.setPassword("123456");

        // Documento do MongoDB simulado
        userDocument = UserDocument.builder()
                .id("660c1234")
                .name("Naruto")
                .login("naruto")
                .password("123456")
                .build();
    }

    @Test
    void createUser_ShouldSaveAndReturnMappedUser() {
        when(mongoTemplate.save(any(UserDocument.class))).thenReturn(userDocument);

        User result = userRepository.createUser(userDomain);

        assertNotNull(result);
        assertEquals("660c1234", result.getId());
        assertEquals("Naruto", result.getName());
        verify(mongoTemplate, times(1)).save(any(UserDocument.class));
    }

    @Test
    void updateUser_ShouldSaveAndReturnSameUser() {
        when(mongoTemplate.save(any(UserDocument.class))).thenReturn(userDocument);

        User result = userRepository.updateUser(userDomain);

        assertNotNull(result);
        assertEquals("660c1234", result.getId());
        verify(mongoTemplate, times(1)).save(any(UserDocument.class));
    }

    @Test
    void deleteUser_WhenUserExists_ShouldRemoveDocument() {
        when(mongoTemplate.findById("660c1234", UserDocument.class)).thenReturn(userDocument);

        userRepository.deleteUser("660c1234");

        verify(mongoTemplate, times(1)).findById("660c1234", UserDocument.class);
        verify(mongoTemplate, times(1)).remove(userDocument);
    }

    @Test
    void deleteUser_WhenUserDoesNotExist_ShouldNotRemove() {
        when(mongoTemplate.findById("9999", UserDocument.class)).thenReturn(null);

        userRepository.deleteUser("9999");

        verify(mongoTemplate, times(1)).findById("9999", UserDocument.class);
        verify(mongoTemplate, never()).remove(any());
    }

    @Test
    void getUsers_ShouldReturnListOfMappedUsers() {
        when(mongoTemplate.findAll(UserDocument.class)).thenReturn(List.of(userDocument));

        List<User> result = userRepository.getUsers();

        assertFalse(result.isEmpty());
        assertEquals(1, result.size());
        assertEquals("Naruto", result.get(0).getName());
        verify(mongoTemplate, times(1)).findAll(UserDocument.class);
    }

    @Test
    void getUser_WhenUserExists_ShouldReturnMappedUser() {
        when(mongoTemplate.findById("660c1234", UserDocument.class)).thenReturn(userDocument);

        User result = userRepository.getUser("660c1234");

        assertNotNull(result);
        assertEquals("660c1234", result.getId());
        verify(mongoTemplate, times(1)).findById("660c1234", UserDocument.class);
    }

    @Test
    void getUser_WhenUserDoesNotExist_ShouldReturnNull() {
        when(mongoTemplate.findById("9999", UserDocument.class)).thenReturn(null);

        User result = userRepository.getUser("9999");

        assertNull(result);
    }

    @Test
    void getUserByLogin_WhenExists_ShouldReturnMappedUser() {
        when(mongoTemplate.findOne(any(Query.class), eq(UserDocument.class))).thenReturn(userDocument);

        User result = userRepository.getUserByLogin("naruto");

        assertNotNull(result);
        assertEquals("naruto", result.getLogin());
        verify(mongoTemplate, times(1)).findOne(any(Query.class), eq(UserDocument.class));
    }

    @Test
    void getUserByLogin_WhenDoesNotExist_ShouldReturnNull() {
        when(mongoTemplate.findOne(any(Query.class), eq(UserDocument.class))).thenReturn(null);

        User result = userRepository.getUserByLogin("unknown");

        assertNull(result);
    }
}