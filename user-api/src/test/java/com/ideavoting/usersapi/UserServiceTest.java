package com.ideavoting.usersapi.services;

import com.ideavoting.usersapi.exceptions.InvalidUserDataException;
import com.ideavoting.usersapi.exceptions.UserNotFoundException;
import com.ideavoting.usersapi.models.User;
import com.ideavoting.usersapi.repositories.UserRepository;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.util.List;
import java.util.Optional;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.CompletionException;
import java.util.concurrent.ExecutionException;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UserServiceTest {

    @Mock
    private UserRepository userRepository;

    @InjectMocks
    private UserService userService;

    // --- Create ---
    @Test
    void shouldCreateUserSuccessfully() throws Exception {
        User input = User.builder().name("Javier").login("jroca").build();
        User saved = User.builder().id("1").name("Javier").login("jroca").build();

        when(userRepository.save(any(User.class))).thenReturn(saved);

        CompletableFuture<User> resultFuture = userService.createUser(input);
        User result = resultFuture.get();

        assertNotNull(result);
        assertEquals("1", result.getId());
        verify(userRepository).save(input);
    }

    // --- Read ---
    @Test
    void shouldReturnUsers() throws Exception {
        List<User> users = List.of(User.builder().id("1").name("Javier").build());
        when(userRepository.findAll()).thenReturn(users);

        CompletableFuture<List<User>> result = userService.getUsers();
        assertEquals(1, result.get().size());
    }

    @Test
    void shouldReturnUserById() throws Exception {
        User user = User.builder().id("1").name("Javier").build();
        when(userRepository.findById("1")).thenReturn(Optional.of(user));

        CompletableFuture<User> result = userService.getUser("1");
        assertEquals("Javier", result.get().getName());
    }

    @Test
    void shouldReturnUserByLogin() throws Exception {
        User user = User.builder().id("1").login("jroca").build();
        when(userRepository.findByLogin("jroca")).thenReturn(Optional.of(user));

        CompletableFuture<User> result = userService.getUserByLogin("jroca");
        assertEquals("jroca", result.get().getLogin());
    }

    @Test
    void shouldThrowExceptionWhenUserNotFoundById() {
        when(userRepository.findById("2")).thenReturn(Optional.empty());

        CompletableFuture<User> result = userService.getUser("2");
        CompletionException exception = assertThrows(CompletionException.class, result::join);
        assertInstanceOf(UserNotFoundException.class, exception.getCause());
    }

    // --- Update ---
    @Test
    void shouldUpdateUserWhenUserExists() throws Exception {
        String userId = "existing-id";
        User existingUserInDb = User.builder().id(userId).name("Old Name").login("old_login").password("old_pass").build();
        when(userRepository.findById(userId)).thenReturn(Optional.of(existingUserInDb));

        User inputUser = User.builder().id(userId).name("New Name").login("new_login").password("new_pass").build();
        when(userRepository.save(any(User.class))).thenAnswer(i -> i.getArguments()[0]);

        CompletableFuture<User> resultFuture = userService.updateUser(inputUser);
        User result = resultFuture.get();

        assertEquals("New Name", result.getName());
        assertEquals("new_login", result.getLogin());
        assertEquals("new_pass", result.getPassword());
        verify(userRepository, times(1)).save(any(User.class));
    }

    @Test
    void shouldThrowExceptionWhenUpdateUserDoesNotExist() {
        String userId = "missing-id";
        when(userRepository.findById(userId)).thenReturn(Optional.empty());

        User inputUser = User.builder().id(userId).build();

        CompletableFuture<User> future = userService.updateUser(inputUser);
        ExecutionException ex = assertThrows(ExecutionException.class, future::get);
        assertInstanceOf(UserNotFoundException.class, ex.getCause());

        verify(userRepository, never()).save(any());
    }

    @Test
    void shouldThrowExceptionWhenPatchIsEmpty() {
        when(userRepository.findById("existing-id")).thenReturn(Optional.of(new User()));

        CompletableFuture<User> future = userService.updateUser(User.builder().id("existing-id").build());
        ExecutionException ex = assertThrows(ExecutionException.class, future::get);
        assertInstanceOf(InvalidUserDataException.class, ex.getCause());
    }

    @Test
    void shouldThrowExceptionWhenFieldIsBlankOnUpdate() {
        String userId = "1";
        User existing = User.builder().id(userId).name("Name").build();
        when(userRepository.findById(userId)).thenReturn(Optional.of(existing));

        User input = User.builder().id(userId).name("   ").build();

        CompletableFuture<User> future = userService.updateUser(input);
        ExecutionException ex = assertThrows(ExecutionException.class, future::get);
        assertInstanceOf(InvalidUserDataException.class, ex.getCause());
    }

    // --- Delete ---
    @Test
    void shouldDeleteUserSuccessfully() {
        String userId = "1";
        when(userRepository.existsById(userId)).thenReturn(true);

        CompletableFuture<Void> result = userService.deleteUser(userId);
        result.join();

        verify(userRepository, times(1)).deleteById(userId);
    }

    @Test
    void shouldThrowExceptionWhenDeletingNonExistentUser() {
        String userId = "999";
        when(userRepository.existsById(userId)).thenReturn(false);

        CompletableFuture<Void> result = userService.deleteUser(userId);

        CompletionException exception = assertThrows(CompletionException.class, result::join);
        assertInstanceOf(UserNotFoundException.class, exception.getCause());

        verify(userRepository, never()).deleteById(anyString());
    }
}