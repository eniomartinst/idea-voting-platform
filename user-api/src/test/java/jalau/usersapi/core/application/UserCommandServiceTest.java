package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.core.exception.InvalidUserDataException;
import jalau.usersapi.core.exception.UserNotFoundException;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.util.concurrent.CompletableFuture;
import java.util.concurrent.CompletionException;
import java.util.concurrent.ExecutionException;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

/**
 * Unit tests for {@link UserCommandService}.
 * <p>
 * Verifies that user creation is correctly delegated to the repository
 * and executed asynchronously.
 */
@ExtendWith(MockitoExtension.class)
class UserCommandServiceTest {
    
    /** Mocked repository dependency */
    @Mock
    private IUserRepository userRepository;
    
    /** Service under test */
    @InjectMocks
    private UserCommandService userCommandService;
    
    /**
     * Should create a user successfully and return it asynchronously.
     */
    @Test
    void shouldCreateUserSuccessfully() throws Exception {
        User input = new User();
        input.setName("Javier");
        
        User saved = new User();
        saved.setId("1");
        saved.setName("Javier");
        
        when(userRepository.createUser(any())).thenReturn(saved);
        
        CompletableFuture<User> resultFuture = userCommandService.createUser(input);
        User result = resultFuture.get();
        
        assertNotNull(result);
        assertEquals("1", result.getId());
        verify(userRepository).createUser(input);
    }
    
    @Test
    void shouldUpdateUserWhenUserExists() throws Exception {
        // Arrange
        String userId = "existing-id";
        User existingUserInDb = new User(userId, "Old Name", "old_login", "old_pass");
        when(userRepository.getUser(userId)).thenReturn(existingUserInDb);
        
        User inputUser = new User();
        inputUser.setId(userId);
        inputUser.setName("New Name");
        inputUser.setLogin("new_login");
        inputUser.setPassword("new_pass");

        when(userRepository.updateUser(any(User.class))).thenAnswer(i -> i.getArguments()[0]);

        // Act
        CompletableFuture<User> resultFuture = userCommandService.updateUser(inputUser);
        User result = resultFuture.get();

        // Assert
        assertEquals("New Name", result.getName());
        assertEquals("new_login", result.getLogin());
        assertEquals("new_pass", result.getPassword());
        verify(userRepository, times(1)).updateUser(any(User.class));
    }
    
    @Test
    void shouldThrowExceptionWhenUserDoesNotExist() {
        String userId = "missing-id";
        when(userRepository.getUser(userId)).thenReturn(null);
        
        User inputUser = new User();
        inputUser.setId(userId);
        
        CompletableFuture<User> future = userCommandService.updateUser(inputUser);
        
        ExecutionException ex = assertThrows(ExecutionException.class, future::get);
		assertInstanceOf(UserNotFoundException.class, ex.getCause());
        
        verify(userRepository, never()).updateUser(any());
    }
    
    @Test
    void shouldThrowExceptionWhenPatchIsEmpty() {
        User user = new User();
        user.setId("existing-id");
        when(userRepository.getUser("existing-id")).thenReturn(new User());
        
        CompletableFuture<User> future = userCommandService.updateUser(user);
        
        ExecutionException ex = assertThrows(ExecutionException.class, future::get);
		assertInstanceOf(InvalidUserDataException.class, ex.getCause());
    }
    
    @Test
    void shouldThrowExceptionWhenFieldIsBlank() {
        String userId = "1";
        User existing = new User(userId, "Name", "login", "pass");
        when(userRepository.getUser(userId)).thenReturn(existing);
        
        User input = new User();
        input.setId(userId);
        input.setName("   ");
        
        CompletableFuture<User> future = userCommandService.updateUser(input);
        
        ExecutionException ex = assertThrows(ExecutionException.class, future::get);
        assertInstanceOf(InvalidUserDataException.class, ex.getCause());
    }
    
    @Test
    void shouldIgnoreNullFields() throws Exception {
        String userId = "1";
        User existing = new User(userId, "Name", "login", "pass");
        when(userRepository.getUser(userId)).thenReturn(existing);
        
        User input = new User();
        input.setId(userId);
        input.setName(null);
        input.setLogin("new_login");
        when(userRepository.updateUser(any())).thenAnswer(i -> i.getArguments()[0]);
        
        User result = userCommandService.updateUser(input).get();
        assertEquals("Name", result.getName()); // not changed
    }

    @Test
    void shouldDeleteUserSuccessfully() {
        // Arrange (Prepara um usuário falso)
        String userId = "1";
        User mockUser = new User(userId, "Test Name", "testlogin", "123");
        when(userRepository.getUser(userId)).thenReturn(mockUser);

        // Act (Executa o método)
        CompletableFuture<Void> result = userCommandService.deleteUser(userId);
        result.join(); // Espera a thread assíncrona terminar

        // Assert (Verifica se o repositório foi chamado para deletar)
        verify(userRepository, times(1)).deleteUser(userId);
    }

    @Test
    void shouldThrowExceptionWhenDeletingNonExistentUser() {
        // Arrange (Simula que o banco não achou o usuário)
        String userId = "999";
        when(userRepository.getUser(userId)).thenReturn(null);

        // Act (Executa o método)
        CompletableFuture<Void> result = userCommandService.deleteUser(userId);

        // Assert (Verifica se a exceção de Not Found foi lançada dentro da thread)
        CompletionException exception = assertThrows(CompletionException.class, result::join);
		assertInstanceOf(UserNotFoundException.class, exception.getCause());

        // Garante que o método delete NUNCA foi chamado no banco
        verify(userRepository, never()).deleteUser(anyString());
    }
}
